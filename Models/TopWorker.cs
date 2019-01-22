using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class TopWorker
    {
        public Guid WorkerID { get; set; }
        public string UserName { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int RunningTasks { get; set; }
        public int ClosedTasks { get; set; }
        public int ErrorTasks { get; set; }
    }
}
