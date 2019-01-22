using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tasktServer.Controllers
{
    [Produces("application/json")]
    public class ApiController : Controller
    {

        #region Test API for Workers

        [HttpGet("/api/Test")]
        public IActionResult TestConnection()
        {
            return Ok("Hello World!");
        }

        #endregion

        #region Metrics API for Workers

        [HttpGet("/api/Workers/Metrics/Authorized")]
        public IActionResult GetAuthorizedWorkers()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var workers = context.Workers.Count();
                return Ok(workers + " known worker(s)");
            }

        }

        #endregion

        #region Data API for Workers

        [HttpGet("/api/Workers/All")]
        public IActionResult GetAllWorkers()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var workers = context.Workers.ToList();
                return Ok(workers);
            }

        }

        [HttpGet("/api/Workers/Top")]
        public IActionResult GetTopWorkers()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var topWorkerList = new List<Models.TopWorker>();

                var groupedWorker = context.Tasks.Where(f => f.TaskStarted >= DateTime.Now.AddDays(-1)).GroupBy(f => f.WorkerID).OrderByDescending(f => f.Count());

                foreach (var wrkr in groupedWorker)
                {

                    var workerInfo = context.Workers.Where(f => f.WorkerID == wrkr.Key).FirstOrDefault();

                    string userName = "Unknown";
                    if (!(workerInfo is null))
                    {
                        userName = workerInfo.UserName + " (" + workerInfo.MachineName + ")";

                    }


                    topWorkerList.Add(new Models.TopWorker
                    {
                        WorkerID = wrkr.Key,
                        UserName = userName,
                        TotalTasks = wrkr.Count(),
                        RunningTasks = wrkr.Where(f => f.Status == "Running").Count(),
                        CompletedTasks = wrkr.Where(f => f.Status == "Completed").Count(),
                        ClosedTasks = wrkr.Where(f => f.Status == "Closed").Count(),
                        ErrorTasks = wrkr.Where(f => f.Status == "Error").Count()
                    });
                }



                return Ok(topWorkerList);
            }

        }

        [HttpGet("/api/Workers/New")]
        public IActionResult AddNewWorker(string userName, string machineName)
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var newWorker = new Models.Worker();
                newWorker.UserName = userName;
                newWorker.MachineName = machineName;
                newWorker.LastCheckIn = DateTime.Now;
                newWorker.Status = Models.WorkerStatus.Pending;
                context.Workers.Add(newWorker);
                context.SaveChanges();
                return Ok(newWorker);
            }

        }

        [HttpGet("/api/Workers/CheckIn")]
        public IActionResult CheckInWorker(Guid workerID)
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var targetWorker = context.Workers.Where(f => f.WorkerID == workerID).FirstOrDefault();

                if (targetWorker is null)
                {
                    return BadRequest();
                }
                else
                {
                    targetWorker.LastCheckIn = DateTime.Now;
                    context.SaveChanges();
                    return Ok(targetWorker);
                }

            }

        }

        #endregion

        //Coming soon...
        //[HttpGet("/api/Workers/Pending")]
        //public IActionResult GetPendingWorkers()
        //{
        //    using (var context = new Models.tasktDatabaseContext())
        //    {
        //        //Todo: Change to workers table
        //        var workers = context.Workers.Where(f => f.Status == Models.WorkerStatus.Pending).Count();
        //        return Ok(workers + " pending workers");
        //    }

        //}

        //Coming soon...
        //[HttpGet("/api/Workers/Revoked")]
        //public IActionResult GetRevokedWorkers()
        //{
        //    using (var context = new Models.tasktDatabaseContext())
        //    {
        //        //Todo: Change to workers table
        //        var workers = context.Workers.Where(f => f.Status == Models.WorkerStatus.Revoked).Count();
        //        return Ok(workers + " revoked workers");
        //    }

        //}

        #region Metrics API for Tasks

        [HttpGet("/api/Tasks/Metrics/Completed")]
        public IActionResult GetCompletedTasks()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var completedTasks = context.Tasks.Where(f => f.Status == "Completed").Count();
                return Ok(completedTasks + " completed");
            }
        }

        [HttpGet("/api/Tasks/Metrics/Closed")]
        public IActionResult GetClosedTasks()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var completedTasks = context.Tasks.Where(f => f.Status == "Closed").Count();
                return Ok(completedTasks + " closed");
            }
        }

        [HttpGet("/api/Tasks/Metrics/Errored")]
        public IActionResult GetErroredTasks()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var erroredTasks = context.Tasks.Where(f => f.Status == "Error").Count();
                return Ok(erroredTasks + " errored");
            }
        }

        [HttpGet("/api/Tasks/Metrics/Running")]
        public IActionResult GetRunningTasks()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var runningTasks = context.Tasks.Where(f => f.Status == "Running").Count();
                return Ok(runningTasks + " running");
            }


        }

        #endregion

        #region Data API for Tasks

        [HttpGet("/api/Tasks/All")]
        public IActionResult GetAllTasks()
        {
            using (var context = new Models.tasktDatabaseContext())
            {
                var runningTasks = context.Tasks.ToList().OrderByDescending(f => f.TaskStarted);
                return Ok(runningTasks);
            }


        }

        [HttpGet("/api/Tasks/New")]
        public IActionResult NewTask(Guid workerID, string taskName, string userName, string machineName)
        {
            //Todo: Add Auth Check, Change to HTTPPost and validate workerID is valid


            using (var context = new Models.tasktDatabaseContext())
            {
                //var workerExists = context.Workers.Where(f => f.WorkerID == workerID).Count() > 0;

                //if (!workerExists)
                //{
                //    //Todo: Create Alert
                //    return Unauthorized();
                //}

                //close out any stale tasks
                var staleTasks = context.Tasks.Where(f => f.WorkerID == workerID && f.Status == "Running");
                foreach (var task in staleTasks)
                {
                    task.Status = "Closed";
                }

                var newTask = new Models.Task();
                newTask.WorkerID = workerID;
                newTask.UserName = userName;
                newTask.MachineName = machineName;
                newTask.TaskStarted = DateTime.Now;
                newTask.Status = "Running";
                newTask.TaskName = taskName;

                var entry = context.Tasks.Add(newTask);
                context.SaveChanges();
                return Ok(newTask);
            }
        }

        [HttpGet("/api/Tasks/Update")]
        public IActionResult UpdateTask(Guid taskID, string status, Guid workerID)
        {
            //Todo: Needs Testing
            using (var context = new Models.tasktDatabaseContext())
            {
                var taskToUpdate = context.Tasks.Where(f => f.TaskID == taskID && f.WorkerID == workerID).FirstOrDefault();
                taskToUpdate.TaskFinished = DateTime.Now;
                taskToUpdate.Status = status;
                context.SaveChanges();
                return Ok(taskToUpdate);
            }


        }

        #endregion

    }
}
