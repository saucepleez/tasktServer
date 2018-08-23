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
                    throw;
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

                //convert client data
                var clientData = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketPackage>(incomingMessage);
                var clientInfo = string.Join(", ", "MACHINE: " + clientData.MACHINE_NAME, "USER: " + clientData.USER_NAME, "MESSAGE: " + clientData.MESSAGE, "IP: " + ipAddress);





                if (clientData.MESSAGE.StartsWith("CLIENT_STATUS="))
                {
                    var clientUpdate = clientData.MESSAGE.Replace("CLIENT_STATUS=", "");

                    var socketClient = ActiveSocketClients.GetClient(clientData.PUBLIC_KEY);
                    if (socketClient.PingRequest.AwaitingPingReply)
                    {
                        socketClient.PingRequest.AwaitingPingReply = false;
                        socketClient.PingRequest.ClientStatus = clientUpdate;
                        socketClient.PingRequest.ReadyForUIReporting = true;
                    }

           
                    LogEvent(new ApplicationLogs() { Type = "WORKER UPDATE", Guid = connectionGUID, Message = clientUpdate, LoggedBy = clientData.PUBLIC_KEY });

                    var worker = dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault();
                    if (worker != null)
                    {
                        worker.LastCommunicationReceived = DateTime.Now;
                        worker.LastExecutionStatus = clientUpdate;
                        dbContext.SaveChanges();
                    }

                    await SendMessageAsync(webSocket, "ACK", CancellationToken.None);
                }



                LogEvent(new ApplicationLogs() {Guid = connectionGUID, Message = "ROBOT CONNECTED FROM '" + ipAddress + "'", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                LogEvent(new ApplicationLogs() {Guid = connectionGUID, Message = "CLIENT INFO: " + clientInfo + "", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });


          

                //if public key is null or empty we automatically assign
                if ((string.IsNullOrEmpty(clientData.PUBLIC_KEY)) || (dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault() == null))
                {

                    LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "ROBOT NOT REGISTERED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });

                    var generatedKeys = tasktServer.Cryptography.CreateKeyPair();
                    var worker = new Workers() { MachineName = clientData.MACHINE_NAME, UserName = clientData.USER_NAME, AccountStatus = (int)ApprovalStatus.RequiresApproval, LastCommunicationReceived = DateTime.Now, PublicKey = generatedKeys.Item2, PrivateKey = generatedKeys.Item1 };
                    dbContext.Workers.Add(worker);
                    dbContext.SaveChanges();


                    ActiveSocketClients.SetClient(clientData.PUBLIC_KEY, webSocket);

                    LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "ROBOT GIVEN NEW PUBLIC KEY AND AWAITING AUTHORIZATION", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });

                    await SendMessageAsync(webSocket, "ACCEPT_KEY=" + worker.PublicKey, CancellationToken.None);
                }
                else
                {
                    var knownClient = dbContext.Workers.Where(f => f.PublicKey == clientData.PUBLIC_KEY).FirstOrDefault();
                    knownClient.LastCommunicationReceived = DateTime.Now;
                    dbContext.SaveChanges();


                    ActiveSocketClients.SetClient(clientData.PUBLIC_KEY, webSocket);

                   


                    switch (knownClient.AccountStatus)
                    {
                        case (int)ApprovalStatus.RequiresApproval:
                            LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_AWAITING_APPROVAL", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                            await SendMessageAsync(webSocket, "WORKER_AWAITING_APPROVAL", CancellationToken.None);
                            break;
                        case (int)ApprovalStatus.Disabled:
                            LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_DISABLED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                            await SendMessageAsync(webSocket, "WORKER_DISABLED", CancellationToken.None);
                            break;
                        case (int)ApprovalStatus.Enabled:
                            LogEvent(new ApplicationLogs() { Guid = connectionGUID, Message = "RESPONDED WORKER_ENABLED", LoggedBy = "SYSTEM", Type = "SOCKET REQUEST" });
                            await SendMessageAsync(webSocket, "WORKER_ENABLED", CancellationToken.None);
                            break;
                        default:
                            break;
                    }
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        /// <summary>
        /// Sends an outbound socket message containing script data to be executed
        /// </summary>
        /// <param name="machineName"></param>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static string SendScriptToClient(string machineName, string scriptName)
        {
            List<RobotClient> connectedClients = WorkForceManagement.GetClients();
            RobotClient requiredConn = connectedClients.Where(client => client.MachineName == machineName).FirstOrDefault();

            if (requiredConn != null)
            {

                var rpaScriptsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\sharpRPA\\My Scripts\\";
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(rpaScriptsFolder + scriptName);
                byte[] bytes = System.Text.Encoding.Default.GetBytes(dom.OuterXml);
                var buffer = new ArraySegment<Byte>(bytes, 0, bytes.Length);
                requiredConn.ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                return "Script Sent To Client";

            }
            else
            {
                return "Client Not Found";
            }
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
