using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class ClientModel
    {
        public string DisplayName { get; set; }
        public bool Enabled { get; set; }
        public bool RestrictIP { get; set; }
        public string IPAddress { get; set; }
        public bool RestrictUserName { get; set; }
        public string UserName { get; set; }
        public string EntryType { get; set; }
        public string PublicKeyLicense { get; set; }
        public string PrivateKeyLicense { get; set; }
    }
}
