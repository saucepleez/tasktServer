using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<Guid>(nullable: false),
                    WorkerID = table.Column<Guid>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    TaskName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TaskStarted = table.Column<DateTime>(nullable: false),
                    TaskFinished = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerID = table.Column<Guid>(nullable: false),
                    MachineName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    LastCheckIn = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
