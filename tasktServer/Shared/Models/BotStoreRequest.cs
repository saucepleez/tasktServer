using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared.Models
{
    public class BotStoreRequest
    {
        public Guid workerID { get; set; }
        public string BotStoreName { get; set; }
        public RequestType requestType { get; set; }
        public enum RequestType
        {
            BotStoreValue,
            BotStoreModel
        }
    }
}
