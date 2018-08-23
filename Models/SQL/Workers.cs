using System;
using System.Collections.Generic;

namespace tasktServer.Models.SQL
{
    public partial class Workers
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public int? AccountStatus { get; set; }
        public string LastExecutionStatus { get; set; }
        public DateTime? LastCommunicationReceived { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
