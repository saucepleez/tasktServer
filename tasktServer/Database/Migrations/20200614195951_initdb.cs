using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Database.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentID = table.Column<Guid>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    AssignmentName = table.Column<string>(nullable: true),
                    PublishedScriptID = table.Column<Guid>(nullable: false),
                    AssignedWorker = table.Column<Guid>(nullable: false),
                    Frequency = table.Column<int>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    NewTaskDue = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentID);
                });

            migrationBuilder.CreateTable(
                name: "BotStore",
                columns: table => new
                {
                    StoreID = table.Column<Guid>(nullable: false),
                    BotStoreName = table.Column<string>(nullable: true),
                    BotStoreValue = table.Column<string>(nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotStore", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "LoginProfiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    LastSuccessfulLogin = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginProfiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PublishedScripts",
                columns: table => new
                {
                    PublishedScriptID = table.Column<Guid>(nullable: false),
                    WorkerID = table.Column<Guid>(nullable: false),
                    PublishedOn = table.Column<DateTime>(nullable: false),
                    ScriptType = table.Column<int>(nullable: false),
                    FriendlyName = table.Column<string>(nullable: true),
                    ScriptData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishedScripts", x => x.PublishedScriptID);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<Guid>(nullable: false),
                    WorkerID = table.Column<Guid>(nullable: false),
                    WorkerType = table.Column<string>(nullable: true),
                    MachineName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    ExecutionType = table.Column<string>(nullable: true),
                    Script = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    TaskStarted = table.Column<DateTime>(nullable: false),
                    TaskFinished = table.Column<DateTime>(nullable: false),
                    TotalSeconds = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPools",
                columns: table => new
                {
                    WorkerPoolID = table.Column<Guid>(nullable: false),
                    WorkerPoolName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPools", x => x.WorkerPoolID);
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

            migrationBuilder.CreateTable(
                name: "AssignedPoolWorker",
                columns: table => new
                {
                    AssignedPoolWorkerItemID = table.Column<Guid>(nullable: false),
                    WorkerID = table.Column<Guid>(nullable: false),
                    WorkerPoolID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedPoolWorker", x => x.AssignedPoolWorkerItemID);
                    table.ForeignKey(
                        name: "FK_AssignedPoolWorker_WorkerPools_WorkerPoolID",
                        column: x => x.WorkerPoolID,
                        principalTable: "WorkerPools",
                        principalColumn: "WorkerPoolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedPoolWorker_WorkerPoolID",
                table: "AssignedPoolWorker",
                column: "WorkerPoolID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedPoolWorker");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "BotStore");

            migrationBuilder.DropTable(
                name: "LoginProfiles");

            migrationBuilder.DropTable(
                name: "PublishedScripts");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "WorkerPools");
        }
    }
}
