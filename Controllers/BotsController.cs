using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using tasktServer.Models.SQL;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace tasktServer.Controllers
{
    public class BotsController : Controller
    {
        private IMemoryCache cache;

        public BotsController(IMemoryCache memoryCache): base()
        {
            this.cache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RetrieveWorkforce()
        {
            //database context
            tasktserverContext dbContext = new tasktserverContext();

            //get workers
            var workers = dbContext.Workers.ToList();

            //return view
            return PartialView("~/Views/Bots/PartialViews/Workforce.cshtml", workers);
        }

        public IActionResult DeleteWorkerEntry(string key)
        {
            //database context
            tasktserverContext dbContext = new tasktserverContext();

            //get selected worker
            var workersDeleted = dbContext.Workers.Where(f => f.PublicKey == key);

            //remove worker
            dbContext.Workers.RemoveRange(workersDeleted);

            //save
            dbContext.SaveChanges();

            //return view
            return PartialView("~/Views/Bots/PartialViews/Workforce.cshtml", dbContext.Workers);


        }

        public string PingWorker(string key)
        {
            //refactor into a better way to ping the client from server
            var clientRequired = ActiveSocketClients.GetClient(key);

            if (clientRequired == null)
            {
                return "Client is currently not connected";
            }

            clientRequired.PingRequest = new Models.PingRequestModel();
            clientRequired.SendMessageAsync("CLIENT_STATUS");
            clientRequired.PingRequest.RequestSent = DateTime.Now;
    
            //wait for the response
            do
            {       
                if ((DateTime.Now > clientRequired.PingRequest.RequestSent.AddSeconds(10)))
                {
                    return "Timed out waiting for client response";
                }
                else
                {
                    Thread.Sleep(500);
                }
            } while ((!clientRequired.PingRequest.ReadyForUIReporting));


          return clientRequired.PingRequest.ClientStatus + " @ " + DateTime.Now.ToString();



        }
      
        public IActionResult RetrieveAssignments()
        {
           
            return PartialView("~/Views/Bots/PartialViews/Assignments.cshtml");
        }

        public IActionResult RetrieveWorkforceSchedule()
        {
         
            return PartialView("~/Views/Bots/PartialViews/Schedule.cshtml");
        }



        [HttpPost("UploadAndExecute")]
        public async Task<string> UploadAndExecute(IFormFile file)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "File has been saved.";

        }

        public string ExecuteTask(string key)
        {
            //get client
            var clientRequired = ActiveSocketClients.GetClient(key);

            //if client is not found then return
            if (clientRequired == null)
            {
                return "Client is currently not connected";
            }


            //get task folder
            var appData = AppDomain.CurrentDomain.GetData("AppData") as string;
            string taskFolder = System.IO.Path.Combine(appData, "Tasks");

            var files = System.IO.Directory.GetFiles(taskFolder);

            foreach (var fil in files)
            {
                var taskData = System.IO.File.ReadAllText(fil);
                clientRequired.SendMessageAsync(taskData);
                break;
            }

            return taskFolder;

        }


          
        }
}
