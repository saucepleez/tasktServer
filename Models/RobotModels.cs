using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    //Managing clients by JSON Configuration
    //Not recommended due to file locks, etc.
    //public class tasktWorkForce
    //{
    //   public List<tasktClient> tasktWorkers = new List<tasktClient>();

    //    public tasktWorkForce()
    //    {
    //        //create path to app data and file
    //        var appData = AppDomain.CurrentDomain.GetData("AppData") as string;
    //        var filePath = System.IO.Path.Combine(appData, "tasktWorkers.json");

    //        //check if file exists at path
    //        if (System.IO.File.Exists(filePath))
    //        {
    //            //read data
    //            var json = System.IO.File.ReadAllText(filePath);

    //            //deserialize
    //            tasktWorkers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tasktClient>>(json);
    //        }

    //    }
    //    public void SaveClientData()
    //    {
    //        //create path to app data and file
    //        var appData = AppDomain.CurrentDomain.GetData("AppData") as string;
    //        var filePath = System.IO.Path.Combine(appData, "tasktWorkers.json");

    //        var workerJson = Newtonsoft.Json.JsonConvert.SerializeObject(tasktWorkers);
    //        System.IO.File.WriteAllText(filePath, workerJson);

            
    //    }
    //    public void AddClientData(tasktClient newClient)
    //    {
    //        this.tasktWorkers.Add(newClient);
    //        SaveClientData();
            
    //    }
    //    public void DeleteClientData(string publicKey)
    //    {
    //        var itemsToRemove = this.tasktWorkers.Where(f => f.PublicKey == publicKey).ToList();

    //        foreach (var item in itemsToRemove)
    //        {
    //            this.tasktWorkers.Remove(item);
    //        }

    //        SaveClientData();

    //    }
    //    public tasktClient GetClientData(string publicKey)
    //    {
    //        return this.tasktWorkers.Where(f => f.PublicKey == publicKey).FirstOrDefault();
    //    }
    //}

    public class tasktClient
    {
        public string id { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
        public DateTime LastCommunicationEvent { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }

        public tasktClient(string machineName, string userName, string ipAddress, ApprovalStatus approvalStatus)
        {
            id = System.Guid.NewGuid().ToString();
            this.MachineName = machineName;
            this.UserName = userName;
            this.IPAddress = ipAddress;
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
