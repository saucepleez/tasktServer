using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class assignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentID = table.Column<Guid>(nullable: false),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
