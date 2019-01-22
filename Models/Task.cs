using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class Task
    {
        [Key]
        public Guid TaskID { get; set; }
        public Guid WorkerID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
        public DateTime TaskStarted { get; set; }
        public DateTime TaskFinished { get; set; }

    }
}
