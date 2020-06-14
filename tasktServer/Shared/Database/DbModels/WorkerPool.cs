using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared.Database.DbModels
{
    public class WorkerPool
    {
        [Key]
        public Guid WorkerPoolID { get; set; }
        public string WorkerPoolName { get; set; }
        public List<AssignedPoolWorker> PoolWorkers { get; set; }
    }
    public class AssignedPoolWorker 
    {
        [Key]
        public Guid AssignedPoolWorkerItemID { get; set; }

        [ForeignKey("WorkerID")]
        public Guid WorkerID { get; set; }
    }
}
