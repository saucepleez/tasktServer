using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using tasktServer.Models;
using tasktServer.Models.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace tasktServer
{

    public class SocketManagement
    {
        private IMemoryCache cache;

        public SocketManagement(IMemoryCache memoryCache)
        {
            this.cache = memoryCache;
        }

        private void LogEvent(ApplicationLogs log)
        {
            using (Models.SQL.tasktserverContext dbContext = new tasktserverContext())
            {
                try
                {
                    log.LoggedOn = DateTime.Now;
                    dbContext.ApplicationLogs.Add(log);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }

        /// <summary>
        /// Processes incoming socket messages from taskt clients
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public async Task ProcessIncomingSocketMessage(HttpContext context, WebSocket webSocket)
        {
            //create dbcontext
            Models.SQL.tasktserverContext dbContext = new tasktserverContext();

            //generate GUID for this connection
            Guid guid;
            guid = Guid.NewGuid();
            string connectionGUID = guid.ToString();


            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {

                //get connection information
                string connectionID = context.Connection.Id;
                string ipAddress = context.Connection.RemoteIpAddress.ToString();

         
                //retrieve value
                var arraySegment = new ArraySegment<byte>(buffer, 0, result.Count);
                var incomingMessage = System.Text.Encoding.Default.GetString(arraySegment.Array);
                incomingMessage = incomingMessage.Substring(0, result.Count);
                System.Console.WriteLine(incomingMessage);

                //convert client data
                var clientData = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketPackage>(incomingMessage);
                var clientInfo = string.Join(", ", "MACHINE: " + clientData.MACHINE_NAME, "USER: " + clientData.USER_NAME, "MESSAGE: " + clientData.MESSAGE, "IP: " + ipAddress);

                
                //determine if we were pinging this client for availability
                if (clientData.MESSAGE.StartsWith("CLIENT_STATUS="))
                {
                    //get client update
                    var clientUpdate = clientData.MESSAGE.Replace("CLIENT_STATUS=", "");

                    //find client socket connection
                    var socketClient = ActiveSocketClients.GetClient(clientData.PUBLIC_KEY);

                    //check if we are awaiting a ping reply from this client
                    if (socketClient.PingRequest.AwaitingPingReply)
                    {
                        //update client object accordingly
                        socketClient.PingRequest.AwaitingPingReply = false;
                        socketClient.PingRequest.ClientStatus = clientUpdate;
                        socketClient.PingRequest.ReadyForUIReporting = true;
                    }

                    //log update from worker
                    LogEvent(new ApplicationLogs() { Type = "WORKER UPDATE", Guid = connectionGUID, Message = clientUpdate, LoggedBy = clientData.PUBLIC_KEY });

                    //get reference to the client
                    var worker = dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault();

                    //update communication
                    worker.LastCommunicationReceived = DateTime.Now;
                    dbContext.SaveChanges();

                    //send acknowledge to client
                    await SendMessageAsync(webSocket, "ACK", CancellationToken.None);
                }


                //log client connection
                LogEvent(new ApplicationLogs() {Guid = connectionGUID, Message = "ROBOT CONNECTED FROM '" + ipAddress + "'", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                LogEvent(new ApplicationLogs() {Guid = connectionGUID, Message = "CLIENT INFO: " + clientInfo + "", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });


                //determine if this client is known
                if ((string.IsNullOrEmpty(clientData.PUBLIC_KEY)) || (dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault() == null))
                {
                    //client is new or not yet configured       

                    //create log
                    LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "ROBOT NOT REGISTERED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });

                    //generate key pair - public key sent to client and private key stored locally
                    var generatedKeys = tasktServer.Cryptography.CreateKeyPair();

                    //add as a new worker to the database
                    var worker = new Workers() { MachineName = clientData.MACHINE_NAME, UserName = clientData.USER_NAME, AccountStatus = (int)ApprovalStatus.RequiresApproval, LastCommunicationReceived = DateTime.Now, PublicKey = generatedKeys.Item2, PrivateKey = generatedKeys.Item1 };
                    dbContext.Workers.Add(worker);

                    //save changes
                    dbContext.SaveChanges();

                    //set client as an active socket client which enables the ability to execute scripts
                    ActiveSocketClients.SetClient(clientData.PUBLIC_KEY, webSocket);

                    //create log
                    LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "ROBOT GIVEN NEW PUBLIC KEY AND AUTHENTICATED AUTOMATICALLY (DANGEROUS!)", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });

                    //send key back to client
                    await SendMessageAsync(webSocket, "ACCEPT_KEY=" + worker.PublicKey, CancellationToken.None);
                }
                else
                {

                   
                    //find client info
                    var knownClient = dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault();

                    //update last communication received
                    knownClient.LastCommunicationReceived = DateTime.Now;

                    //save changes
                    dbContext.SaveChanges();

                    //set client as active and available
                    ActiveSocketClients.SetClient(clientData.PUBLIC_KEY, webSocket);

                    //send message back to client
                    await SendMessageAsync(webSocket, "WORKER_ENABLED", CancellationToken.None);

                    //APPROVAL PROCESS, FUTURE ENHANCEMENT!
                    //switch (worker.ApprovalStatus)
                    //{
                    //    case ApprovalStatus.RequiresApproval:
                    //        LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_AWAITING_APPROVAL", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                    //        await SendMessageAsync(webSocket, "WORKER_AWAITING_APPROVAL", CancellationToken.None);
                    //        break;
                    //    case ApprovalStatus.Disabled:
                    //        LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_DISABLED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                    //        await SendMessageAsync(webSocket, "WORKER_DISABLED", CancellationToken.None);
                    //        break;
                    //    case ApprovalStatus.Enabled:
                    //        LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_ENABLED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                    //        await SendMessageAsync(webSocket, "WORKER_ENABLED", CancellationToken.None);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        /// <summary>
        /// Sends a message through a websocket connection
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="data"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static async Task SendMessageAsync(WebSocket ws, String data, CancellationToken cancellation)
        {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellation);
        }


    }

    public static class ActiveSocketClients
    {
       private static List<Models.SocketConnectionModel> AvailableConnections = new List<SocketConnectionModel>();


        public static void SetClient(string publicKey, WebSocket socket)
        {

            if (AvailableConnections.Count > 0)
            {
                var existingConnection = AvailableConnections.Where(f => f.PublicKey == publicKey).FirstOrDefault();

                if (existingConnection != null)
                {
                    AvailableConnections.Remove(existingConnection);
                }

                AvailableConnections.Add(new SocketConnectionModel { PublicKey = publicKey, WebSocket = socket });
            }
            else
            {
                AvailableConnections.Add(new SocketConnectionModel { PublicKey = publicKey, WebSocket = socket });
            }

          

          

        }
        public static SocketConnectionModel GetClient(string publicKey)
        {
            var requiredConnection = AvailableConnections.Where(f => f.PublicKey == publicKey).FirstOrDefault();

            if (requiredConnection == null)
            {
                return null;
            }
            else
            {
                return requiredConnection;
            }

        }

      


    }

}
