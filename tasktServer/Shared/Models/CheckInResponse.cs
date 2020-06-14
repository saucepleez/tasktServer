using System;
using System.Collections.Generic;
using System.Linq;
using tasktServer.Shared.Database.DbModels;

namespace tasktServer.Shared.Models
{
    public class CheckInResponse
    {
        public Task ScheduledTask { get; set; }
        public PublishedScript PublishedScript { get; set; }
        public Worker Worker { get; set; }
    }
}
