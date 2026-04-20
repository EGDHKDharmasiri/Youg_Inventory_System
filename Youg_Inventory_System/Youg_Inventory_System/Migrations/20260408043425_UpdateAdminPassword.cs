using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Youg_Inventory_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Color", "ImageData", "LowStockLevel", "Name", "Price", "ProductCode", "Size", "StockQuantity" },
                values: new object[] { 1, "N/A", null, 0, "System Admin Account", 0m, "ADMIN", "N/A", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
