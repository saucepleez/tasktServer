using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class CheckInResponse
    {
        public Task ScheduledTask { get; set; }
        public Worker Worker { get; set; }
    }
}
