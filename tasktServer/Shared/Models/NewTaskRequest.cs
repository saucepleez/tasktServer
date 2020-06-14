using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared.Models
{
    public class NewTaskRequest
    {
        public Guid workerID { get; set; }
        public Guid publishedScriptID { get; set; }
    }
}
