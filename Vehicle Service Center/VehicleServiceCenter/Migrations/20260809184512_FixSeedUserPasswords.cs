using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleServiceCenter.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 23333,
                column: "PasswordHash",
                value: "$2a$11$njiRWwm346PHbIdUUNQ9q.zTnQ60S7s81iFMnb.i0oY/MUWj2UOCe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 122222,
                column: "PasswordHash",
                value: "$2a$11$ddvzOew6X6eRPWFL05eUHepg3eEwIUcR3PRS0QE64qxIgGaecTkZS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 23333,
                column: "PasswordHash",
                value: "Admin@123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 122222,
                column: "PasswordHash",
                value: "Admin@123");
        }
    }
}
