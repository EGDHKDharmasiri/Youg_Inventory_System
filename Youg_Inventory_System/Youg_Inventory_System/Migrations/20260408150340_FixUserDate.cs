using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Youg_Inventory_System.Migrations
{
    /// <inheritdoc />
    public partial class FixUserDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 14, 57, 42, 52, DateTimeKind.Utc).AddTicks(5140));
        }
    }
}
