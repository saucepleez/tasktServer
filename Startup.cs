using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace tasktServer
{


    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            Logging.WriteLog(new Models.SQL.ApplicationLogs() { Type = "APPLICATION", Message = "STARTING APPLICATION", LoggedBy = "SYSTEM" });
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

           

            string baseDir = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("TASK_FOLDER", System.IO.Path.Combine(baseDir, "App_Data", "Tasks"));

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            Logging.WriteLog(new Models.SQL.ApplicationLogs() { Type = "APPLICATION", Message = "CONFIGURING APPLICATION SERVICES", LoggedBy = "SYSTEM" });
            // Add framework services.
            services.AddMvc();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });


            // Add Memory Cache
            services.AddMemoryCache();

            Logging.WriteLog(new Models.SQL.ApplicationLogs() { Type = "APPLICATION", Message = "FINISHED CONFIGURING APPLICATION SERVICES", LoggedBy = "SYSTEM" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IMemoryCache cache)
        {

            Logging.WriteLog(new Models.SQL.ApplicationLogs() { Type = "APPLICATION", Message = "CONFIGURING HTTP PIPELINE", LoggedBy = "SYSTEM" });
            //cache.CreateEntry("SocketConnections");
            //cache.Set("SocketConnections", new List<Models.SocketConnectionModel>());

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

      
            app.UseWebSockets(webSocketOptions);

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        SocketManagement socketManagement = new SocketManagement(cache);
                        await socketManagement.ProcessIncomingSocketMessage(context, webSocket);
                        
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }

            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Bots}/{action=Index}/{id?}");




            });

            Logging.WriteLog(new Models.SQL.ApplicationLogs() { Type = "APPLICATION", Message = "FINISHED CONFIGURING HTTP PIPELINE", LoggedBy = "SYSTEM" });
        }

    }

}
