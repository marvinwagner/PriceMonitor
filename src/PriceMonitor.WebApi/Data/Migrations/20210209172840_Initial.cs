using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceMonitor.WebApi.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(300)", nullable: false),
                    Url = table.Column<string>(type: "varchar(300)", nullable: false),
                    CurrentInCashValue = table.Column<decimal>(type: "money", nullable: false),
                    CurrentNormalValue = table.Column<decimal>(type: "money", nullable: false),
                    CurrentFullValue = table.Column<decimal>(type: "money", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InCashValue = table.Column<decimal>(type: "money", nullable: false),
                    NormalValue = table.Column<decimal>(type: "money", nullable: false),
                    FullValue = table.Column<decimal>(type: "money", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_History_ItemId",
                table: "History",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
