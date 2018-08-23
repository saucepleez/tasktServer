using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace tasktServer.Models.SQL
{
    public partial class tasktserverContext : DbContext
    {
        public tasktserverContext()
        {
        }

        public tasktserverContext(DbContextOptions<tasktserverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationLogs> ApplicationLogs { get; set; }
        public virtual DbSet<Workers> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=tasktserver;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationLogs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .IsUnicode(false);

                entity.Property(e => e.LoggedBy).IsUnicode(false);

                entity.Property(e => e.LoggedOn).HasColumnType("datetime");

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.Type).IsUnicode(false);
            });

            modelBuilder.Entity<Workers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastCommunicationReceived).HasColumnType("datetime");

                entity.Property(e => e.LastExecutionStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MachineName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
