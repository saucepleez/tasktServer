using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using tasktServer.Models;
using tasktServer.Models.SQL;

namespace tasktServer.Controllers
{
    public class ApiController
    {
        [HttpPost]
        public HttpResponseMessage WriteLog(string ClientName, string LogData)
        {

            //log to database
            using (Models.SQL.tasktserverContext dbContext = new tasktserverContext())
            {
                try
                {
                    ApplicationLogs log = new ApplicationLogs();
                    log.LoggedBy = ClientName;
                    log.Message = LogData;
                    log.Type = "WORKER LOG";
                    log.LoggedOn = DateTime.Now;
                    dbContext.ApplicationLogs.Add(log);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

            }


            // Save Code will be here
            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        [HttpGet]
        public ActionResult GetLogs(string ClientName)
        {
            //var appData = AppDomain.CurrentDomain.GetData("AppData") as string;
            //var filePath = System.IO.Path.Combine(appData, "EngineLogs", ClientName + ".txt");
            //var logs = System.IO.File.ReadAllLines(filePath);
            Models.SQL.tasktserverContext dbContext = new tasktserverContext();
            var logs = dbContext.ApplicationLogs.Where(f => f.LoggedBy == ClientName).OrderBy(f => f.LoggedOn).ToList();
            return new OkObjectResult(logs);

        }




    }
}

