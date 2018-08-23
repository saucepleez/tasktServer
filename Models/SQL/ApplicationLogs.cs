using System;
using System.Collections.Generic;

namespace tasktServer.Models.SQL
{
    public partial class ApplicationLogs
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string LoggedBy { get; set; }
        public DateTime? LoggedOn { get; set; }
    }
}
