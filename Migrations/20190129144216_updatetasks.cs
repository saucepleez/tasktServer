using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class updatetasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskName",
                table: "Tasks",
                newName: "Script");

            migrationBuilder.AddColumn<string>(
                name: "ExecutionType",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutionType",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Script",
                table: "Tasks",
                newName: "TaskName");
        }
    }
}
