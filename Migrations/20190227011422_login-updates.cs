using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class loginupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoginPassword",
                table: "LoginProfiles",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "LoginName",
                table: "LoginProfiles",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "LoginID",
                table: "LoginProfiles",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LoginProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "LoginProfiles");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "LoginProfiles",
                newName: "LoginPassword");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "LoginProfiles",
                newName: "LoginName");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "LoginProfiles",
                newName: "LoginID");
        }
    }
}
