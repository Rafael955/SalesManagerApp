using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesManagerApp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteraçõesnaBasedeDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORDER_ITEMS_PRODUCT_PRODUCT_ID",
                table: "ORDER_ITEMS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PRODUCT",
                table: "PRODUCT");

            migrationBuilder.RenameTable(
                name: "PRODUCT",
                newName: "PRODUCTS");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ORDER_ITEMS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ORDER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PRODUCTS",
                table: "PRODUCTS",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUSTOMERS_EMAIL",
                table: "CUSTOMERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ORDER_ITEMS_PRODUCTS_PRODUCT_ID",
                table: "ORDER_ITEMS",
                column: "PRODUCT_ID",
                principalTable: "PRODUCTS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ORDER_ITEMS_PRODUCTS_PRODUCT_ID",
                table: "ORDER_ITEMS");

            migrationBuilder.DropIndex(
                name: "IX_CUSTOMERS_EMAIL",
                table: "CUSTOMERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PRODUCTS",
                table: "PRODUCTS");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ORDER_ITEMS");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ORDER");

            migrationBuilder.RenameTable(
                name: "PRODUCTS",
                newName: "PRODUCT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PRODUCT",
                table: "PRODUCT",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ORDER_ITEMS_PRODUCT_PRODUCT_ID",
                table: "ORDER_ITEMS",
                column: "PRODUCT_ID",
                principalTable: "PRODUCT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
