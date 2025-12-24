using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesManagerApp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CUSTOMERS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUSTOMERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ROLE = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ORDER",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ORDER_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TOTAL_VALUE = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    CUSTOMER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_CUSTOMERS_CUSTOMER_ID",
                        column: x => x.CUSTOMER_ID,
                        principalTable: "CUSTOMERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORDER_ITEMS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    UNIT_PRICE = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ORDER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_ITEMS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_ITEMS_ORDER_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ORDER_ITEMS_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "USERS",
                columns: new[] { "ID", "EMAIL", "IsActive", "NAME", "PASSWORD", "ROLE" },
                values: new object[] { new Guid("aef750e0-419e-4a7b-9bf3-34ec41a08744"), "admin@admin.com", true, "Admin", "6f2cb9dd8f4b65e24e1c3f3fa5bc57982349237f11abceacd45bbcb74d621c25", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_CUSTOMER_ID",
                table: "ORDER",
                column: "CUSTOMER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEMS_ORDER_ID",
                table: "ORDER_ITEMS",
                column: "ORDER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEMS_PRODUCT_ID",
                table: "ORDER_ITEMS",
                column: "PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDER_ITEMS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "ORDER");

            migrationBuilder.DropTable(
                name: "PRODUCT");

            migrationBuilder.DropTable(
                name: "CUSTOMERS");
        }
    }
}
