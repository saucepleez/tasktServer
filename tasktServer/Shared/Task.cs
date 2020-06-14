using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared
{
    public class Task
    {
        public Guid TaskID { get; set; }
        public Guid WorkerID { get; set; }
        public string WorkerType { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string ExecutionType { get; set; }
        public string Script { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime TaskStarted { get; set; }
        public DateTime TaskFinished { get; set; }

    }
}
