using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class workerpools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkerPoolPoolID",
                table: "Workers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkerPools",
                columns: table => new
                {
                    PoolID = table.Column<Guid>(nullable: false),
                    PoolName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPools", x => x.PoolID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_WorkerPoolPoolID",
                table: "Workers",
                column: "WorkerPoolPoolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_WorkerPools_WorkerPoolPoolID",
                table: "Workers",
                column: "WorkerPoolPoolID",
                principalTable: "WorkerPools",
                principalColumn: "PoolID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkerPools_WorkerPoolPoolID",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "WorkerPools");

            migrationBuilder.DropIndex(
                name: "IX_Workers_WorkerPoolPoolID",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerPoolPoolID",
                table: "Workers");
        }
    }
}
