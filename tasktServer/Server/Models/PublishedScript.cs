using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class PublishedScript
    {
        [Key]
        public Guid PublishedScriptID { get; set; }
        public Guid WorkerID { get; set; }
        public DateTime PublishedOn { get; set; }
        public PublishType ScriptType { get; set; }
        public string FriendlyName { get; set; }
        public string ScriptData { get; set; }

        [NotMapped]
        public string WorkerName { get; set; }
        [NotMapped]
        public string MachineName { get; set; }
        [NotMapped]
        public bool OverwriteExisting { get; set; }

        public enum PublishType
        {
            ClientReference,
            ServerReference,
        }
    }

}
