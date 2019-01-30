using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class WorkerPool
    {
        [Key]
        public Guid PoolID { get; set; }
        public string PoolName { get; set; }
        public List<Worker> AssignedWorkers { get; set; }
    }
}
