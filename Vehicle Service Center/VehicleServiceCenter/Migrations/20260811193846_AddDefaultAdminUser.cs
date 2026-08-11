using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleServiceCenter.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { 300001, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "$2a$11$dYf86bU3YfogdeiSjQy93eNG/ytmFJtHfDgNuSpP8mOMIzmenlq.K", null, "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 300001);
        }
    }
}
