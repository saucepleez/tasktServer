using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class NewTaskRequest
    {
        public Guid workerID { get; set; }
        public Guid publishedScriptID { get; set; }
    }
}
