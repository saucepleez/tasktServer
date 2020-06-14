using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Shared.Database.DbModels
{
    public class BotStoreModel
    {
        [Key]
        public Guid StoreID { get; set; }
        public string BotStoreName { get; set; }
        public string BotStoreValue { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public Guid LastUpdatedBy { get; set; }
    }
}
