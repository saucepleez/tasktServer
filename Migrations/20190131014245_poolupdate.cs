using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class poolupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkerPools_WorkerPoolPoolID",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_WorkerPoolPoolID",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerPoolPoolID",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "PoolID",
                table: "WorkerPools",
                newName: "WorkerPoolID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "WorkerPoolID",
                table: "WorkerPools",
                newName: "PoolID");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerPoolPoolID",
                table: "Workers",
                nullable: true);

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
    }
}
