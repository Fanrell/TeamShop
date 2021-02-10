using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamShop.Migrations
{
    public partial class ShopingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopingCratItems",
                columns: table => new
                {
                    ItemId = table.Column<string>(nullable: false),
                    CartId = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    DataCreated = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopingCratItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_ShopingCratItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopingCratItems_ProductId",
                table: "ShopingCratItems",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopingCratItems");
        }
    }
}
