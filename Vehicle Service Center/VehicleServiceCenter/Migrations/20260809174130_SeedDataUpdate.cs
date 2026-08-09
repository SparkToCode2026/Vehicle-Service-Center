using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleServiceCenter.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[,]
                {
                    { 23333, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohammed@example.com", true, "Admin@123", "99999900", "Admin", "Mohammed" },
                    { 122222, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hawa@example.com", true, "Admin@123", "99990000", "Admin", "Hawa" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 23333);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 122222);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hawa@example.com", true, "Admin@123", "99990000", "Admin", "Hawa" },
                    { 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohammed@example.com", true, "Admin@123", "99999900", "Admin", "Mohammed" }
                });
        }
    }
}
