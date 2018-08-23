using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer
{
    public static class Logging
    {
        private static Models.SQL.tasktserverContext dbContext { get; set; } = new Models.SQL.tasktserverContext();

        public static void WriteLog(Models.SQL.ApplicationLogs log)
        {
            try
            {
                log.LoggedOn = DateTime.Now;
                dbContext.ApplicationLogs.Add(log);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
