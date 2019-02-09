using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tasktServer.Migrations
{
    public partial class botstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotStore");
        }
    }
}
