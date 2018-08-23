using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace tasktServer.Models
{

    public class WorkForce
    {
        List<RobotClient> clientList = new List<RobotClient>();
    }

    public class RobotClient
    {
        public string id { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
        public DateTime LastCommunicationEvent { get; set; }
        public WebSocket ClientSocket { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }

        public RobotClient(string machineName, string userName, string ipAddress, WebSocket clientSocket, ApprovalStatus approvalStatus)
        {
            id = System.Guid.NewGuid().ToString();
            this.MachineName = machineName;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.ClientSocket = clientSocket;
            this.ApprovalStatus = approvalStatus;
            this.LastCommunicationEvent = DateTime.Now;
            var generatedKeys = tasktServer.Cryptography.CreateKeyPair();
            PrivateKey = generatedKeys.Item1;
            PublicKey = generatedKeys.Item2;

        }

    }

    public enum ApprovalStatus
    {
        RequiresApproval,
        Disabled,
        Enabled
    }

    public class SocketPackage
    {
        public string PUBLIC_KEY { get; set; }
        public string MACHINE_NAME { get; set; }
        public string USER_NAME { get; set; }
        public string MESSAGE { get; set; }
    }

}
