using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using tasktServer.Shared.Database.DbModels;

namespace tasktServer.Shared.Database
{
    public class tasktDbContext : DbContext
    {
      protected override void OnConfiguring(DbContextOptionsBuilder contextBuilder)
        {
            if (!contextBuilder.IsConfigured)
            {

                var connection = DatabaseConfiguration.ConnectionString;
                //temp override
                connection = "Server=(localdb)\\mssqllocaldb;Database=taskt;Trusted_Connection=True;ConnectRetryCount=0";
                contextBuilder.UseSqlServer(connection);
            }
        }
        public tasktDbContext(DbContextOptions<tasktDbContext> options)
            : base(options)
        { }
        public tasktDbContext()
        { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<WorkerPool> WorkerPools { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<PublishedScript> PublishedScripts { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<BotStoreModel> BotStore { get; set; }
        public DbSet<UserProfile> LoginProfiles { get; set; }
    }

    public static class DatabaseConfiguration
    {
        public static string ConnectionString { get; set; }
    }



}
