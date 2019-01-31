using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class pooldbupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPools_Workers_WorkerID",
                table: "WorkerPools");

            migrationBuilder.DropIndex(
                name: "IX_WorkerPools_WorkerID",
                table: "WorkerPools");

            migrationBuilder.DropColumn(
                name: "WorkerID",
                table: "WorkerPools");

            migrationBuilder.RenameColumn(
                name: "PoolName",
                table: "WorkerPools",
                newName: "WorkerPoolName");

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

            migrationBuilder.RenameColumn(
                name: "WorkerPoolName",
                table: "WorkerPools",
                newName: "PoolName");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerID",
                table: "WorkerPools",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPools_WorkerID",
                table: "WorkerPools",
                column: "WorkerID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPools_Workers_WorkerID",
                table: "WorkerPools",
                column: "WorkerID",
                principalTable: "Workers",
                principalColumn: "WorkerID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
