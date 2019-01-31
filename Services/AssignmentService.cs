using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using tasktServer.Models;

namespace tasktServer.Services
{
    /// <summary>
    /// This services scans assignments and creates tasks on a schedule
    /// </summary>
    public class AssignmentService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public AssignmentService(ILogger<AssignmentService> logger)
        {
            _logger = logger;
        }

        public System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(15));

            return System.Threading.Tasks.Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            tasktDatabaseContext dbContext = new tasktDatabaseContext();
           var assignments = dbContext.Assignments.Where(f => f.Enabled);


            foreach (var assn in assignments)
            {
                if (assn.NewTaskDue <= DateTime.Now)
                {

                    //create task
                    var newTask = new Models.Task();
                    newTask.WorkerID = assn.AssignedWorker;
                    newTask.TaskStarted = DateTime.Now;
                    newTask.Status = "Scheduled";
                    newTask.ExecutionType = "Assignment";
                    newTask.Script = assn.PublishedScriptID.ToString();
                    newTask.Remark = "Scheduled by tasktServer";

                    dbContext.Tasks.Add(newTask);
                    
                    //update database
                    switch (assn.Interval)
                    {
                        case Assignment.TimeInterval.Seconds:
                            assn.NewTaskDue = DateTime.Now.AddSeconds(assn.Frequency);
                            break;
                        case Assignment.TimeInterval.Minutes:
                            assn.NewTaskDue = DateTime.Now.AddMinutes(assn.Frequency);
                            break;
                        case Assignment.TimeInterval.Days:
                            assn.NewTaskDue = DateTime.Now.AddDays(assn.Frequency);
                            break;
                        case Assignment.TimeInterval.Months:
                            assn.NewTaskDue = DateTime.Now.AddMonths(assn.Frequency);
                            break;
                        default:
                            break;
                    }






                }

            }


            dbContext.SaveChanges();


            _logger.LogInformation("Timed Background Service is working.");
        }

        public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
