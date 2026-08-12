using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleServiceCenter.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceAllSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            DisableSeedForeignKeys(migrationBuilder);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 23333);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 122222);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 300001);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1,
                columns: new[] { "AppointmentDate", "CreatedAt", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 8, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled oil-change maintenance", "Completed" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 2,
                columns: new[] { "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status" },
                values: new object[] { new DateTime(2026, 8, 20, 10, 30, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 8, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Air conditioner is not cooling properly", 5, "Confirmed" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 3,
                columns: new[] { "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status" },
                values: new object[] { new DateTime(2026, 8, 13, 11, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 8, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, "Noise while braking", 2, "In Progress" });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status", "VehicleId" },
                values: new object[] { 5, new DateTime(2026, 8, 22, 13, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 8, 12, 16, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Battery is slow during startup", 4, "Pending", 3 });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                columns: new[] { "Address", "BranchName", "Email", "PhoneNumber" },
                values: new object[] { "Al Khuwair, Muscat, Oman", "Muscat Main Branch", "muscat@vehicleservice.com", "24000001" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                columns: new[] { "Address", "BranchName", "Email", "PhoneNumber" },
                values: new object[] { "Al Hail, Seeb, Oman", "Seeb Branch", "seeb@vehicleservice.com", "24000002" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Bawshar, Muscat, Oman", new DateTime(2026, 1, 2, 9, 5, 0, 0, DateTimeKind.Unspecified), new DateOnly(1995, 4, 12), 3 });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Al Amerat, Muscat, Oman", new DateTime(2026, 1, 3, 9, 5, 0, 0, DateTimeKind.Unspecified), new DateOnly(1990, 9, 21), 4 });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 3,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Al Hail, Seeb, Oman", new DateTime(2026, 1, 4, 9, 5, 0, 0, DateTimeKind.Unspecified), new DateOnly(1998, 2, 7), 5 });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 1,
                columns: new[] { "DueDate", "IssueDate", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 36m, 36m });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 2,
                columns: new[] { "DueDate", "IssueDate", "Notes", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 13, 12, 0, 0, 0, DateTimeKind.Unspecified), "Partial payment received", 74m, 74m });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 3,
                columns: new[] { "DueDate", "IssueDate", "Notes", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), "Payment is due after service completion", 45m, 45m });

            migrationBuilder.UpdateData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 1,
                columns: new[] { "ExperienceYears", "HireDate", "Specialization", "UserId" },
                values: new object[] { 8, new DateOnly(2021, 3, 15), "Engine and Brake Systems", 6 });

            migrationBuilder.UpdateData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 2,
                columns: new[] { "ExperienceYears", "HireDate", "Specialization", "UserId" },
                values: new object[] { 6, new DateOnly(2022, 6, 1), "Electrical and Air Conditioning", 7 });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "Amount", "Notes", "PaymentDate", "TransactionReference" },
                values: new object[] { 36m, "Full card payment", new DateTime(2026, 8, 1, 10, 15, 0, 0, DateTimeKind.Unspecified), "PAY-2026-0001" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                columns: new[] { "Notes", "PaymentDate", "TransactionReference" },
                values: new object[] { "Partial cash payment", new DateTime(2026, 8, 13, 13, 0, 0, 0, DateTimeKind.Unspecified), "PAY-2026-0002" });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 1,
                column: "Description",
                value: "Oil-change labor");

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 2,
                columns: new[] { "Description", "ItemType", "Subtotal", "UnitPrice" },
                values: new object[] { "Oil-filter replacement", "SparePart", 11m, 11m });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 3,
                column: "Description",
                value: "Brake inspection labor");

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 4,
                columns: new[] { "Description", "ItemType", "Subtotal", "UnitPrice" },
                values: new object[] { "Front brake-pad set", "SparePart", 44m, 44m });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 5,
                columns: new[] { "Description", "Subtotal", "UnitPrice" },
                values: new object[] { "Engine diagnostic scan", 45m, 45m });

            // AppointmentId is unique. Clear the old one-to-one links before
            // assigning appointments to different seeded service orders.
            migrationBuilder.Sql(
                "UPDATE [ServiceOrders] SET [AppointmentId] = NULL " +
                "WHERE [ServiceOrderId] IN (1, 2, 3);");

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 1,
                columns: new[] { "CompletionDate", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Routine oil-change service", "Oil and oil filter were due for replacement", new DateTime(2026, 8, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), 36m });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 2,
                columns: new[] { "AppointmentId", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[] { 3, new DateTime(2026, 8, 13, 11, 0, 0, 0, DateTimeKind.Unspecified), "Noise and vibration while braking", "Front brake pads require replacement", new DateTime(2026, 8, 13, 11, 0, 0, 0, DateTimeKind.Unspecified), "InProgress", 74m, 3 });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 3,
                columns: new[] { "AppointmentId", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[] { 7, new DateTime(2026, 8, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), "Check-engine warning light is on", "Diagnostic scan is approved and waiting to begin", new DateTime(2026, 8, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), "Approved", 45m, 4 });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 1,
                column: "AppointmentId",
                value: 1);

            migrationBuilder.InsertData(
                table: "ServiceOrders",
                columns: new[] { "ServiceOrderId", "AppointmentId", "BranchId", "CompletionDate", "CreatedAt", "CustomerComplaint", "CustomerProfileId", "Diagnosis", "MechanicProfileId", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[,]
                {
                    { 4, 2, 2, null, new DateTime(2026, 8, 20, 10, 30, 0, 0, DateTimeKind.Unspecified), "Air conditioner is not cooling properly", 1, "Air-conditioning inspection is pending", 2, new DateTime(2026, 8, 20, 10, 30, 0, 0, DateTimeKind.Unspecified), "Pending", 78m, 2 },
                    { 5, null, 1, new DateTime(2026, 8, 12, 9, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 8, 30, 0, 0, DateTimeKind.Unspecified), "Vehicle would not start", 2, "Battery failed its load test and was replaced", 1, new DateTime(2026, 8, 12, 8, 30, 0, 0, DateTimeKind.Unspecified), "Completed", 105m, 3 }
                });

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 1,
                column: "Description",
                value: "Engine oil and oil-filter replacement");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 2,
                column: "Description",
                value: "Complete brake-system inspection");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 3,
                column: "BasePrice",
                value: 45m);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 4,
                columns: new[] { "BasePrice", "Description" },
                values: new object[] { 20m, "Battery testing and replacement service" });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "BasePrice", "Description", "EstimatedDurationMinutes", "IsActive", "Name" },
                values: new object[] { 5, 60m, "Air-conditioning inspection and maintenance", 90, true, "Air Conditioning Service" });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 1,
                columns: new[] { "StockQuantity", "UnitPrice" },
                values: new object[] { 45, 11m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 2,
                columns: new[] { "Description", "PartName", "StockQuantity", "UnitPrice" },
                values: new object[] { "Front axle ceramic brake-pad set", "Front Brake Pads", 18, 44m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 3,
                columns: new[] { "BranchId", "Description", "StockQuantity", "UnitPrice" },
                values: new object[] { 1, "12-volt maintenance-free vehicle battery", 12, 85m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 4,
                columns: new[] { "Description", "PartName", "PartNumber", "ReorderLevel", "StockQuantity", "UnitPrice" },
                values: new object[] { "Cabin air filter for climate-control systems", "Cabin Air Filter", "CAF-001", 6, 25, 18m });

            migrationBuilder.InsertData(
                table: "SpareParts",
                columns: new[] { "SparePartId", "BranchId", "Description", "IsAvailable", "PartName", "PartNumber", "ReorderLevel", "StockQuantity", "UnitPrice" },
                values: new object[,]
                {
                    { 5, 2, "Standard engine intake air filter", true, "Engine Air Filter", "EAF-001", 7, 30, 15m },
                    { 6, 2, "Four-piece iridium spark-plug set", true, "Spark Plug Set", "SP-001", 5, 4, 32m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "$2a$11$GiQ5o4CPh1sr1GQQVxyv2O51hdE2lXCVHbfQJRdR667WwFe0EYR6m", "90000001", "Admin", "Admin User" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), "customer1@gmail.com", true, "$2a$11$WHsBifzPW/23FBBmsDb/pOWH5CFLr7xgXt85f7Y7Q36m51nHJBamC", "91110001", "Customer", "Sara Al Balushi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "customer2@gmail.com", true, "$2a$11$2W08xdjKhmOrb.XzkPvUwuYEPStJe0V.HixHDfa.A04RcdoqlYC1i", "92220002", "Customer", "Omar Al Harthi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), "customer3@gmail.com", true, "$2a$11$PR.SYUMNka84Z8vjRcx89.dWEZieqCKueqHAw1ypZrRsXxD8Z2UAS", "93330003", "Customer", "Lina Al Lawati" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), "mechanic1@gmail.com", true, "$2a$11$vekQUchkuBwnXV1BdoGv3u/2QaQRQhGd.RrDZyyjvM.my4x9S5O.S", "94440004", "Mechanic", "Ahmed Al Rashdi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "mechanic2@gmail.com", true, "$2a$11$/YWclfj9mFqcGGPnoo0p0.X0zZ0jSOgj7nhrrngllAdcSg4Tl1n.m", "95550005", "Mechanic", "Khalid Al Siyabi" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PlateNumber", "VIN" },
                values: new object[] { new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "OM-10001", "OMVIN2026000000001" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[] { new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Lexus", 28500m, "NX 350", "OM-10002", "OMVIN2026000000002", 2023 });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[] { new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Nissan", 62000m, "Altima", "OM-20001", "OMVIN2026000000003", 2021 });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "Color", "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[] { 4, "Blue", new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Honda", 19000m, "Accord", "OM-30001", "OMVIN2026000000004", 2024 });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status", "VehicleId" },
                values: new object[,]
                {
                    { 7, new DateTime(2026, 8, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 8, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Check-engine warning light is on", 3, "Confirmed", 4 },
                    { 6, new DateTime(2026, 8, 12, 15, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 8, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Customer cancelled the appointment", 1, "Cancelled", 4 }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "DiscountAmount", "DueDate", "InvoiceNumber", "IssueDate", "Notes", "ServiceOrderId", "Status", "Subtotal", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 4, 0m, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0004", new DateTime(2026, 8, 20, 11, 0, 0, 0, DateTimeKind.Unspecified), "Pending payment recorded", 4, "Unpaid", 78m, 0m, 78m },
                    { 5, 0m, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0005", new DateTime(2026, 8, 12, 9, 15, 0, 0, DateTimeKind.Unspecified), "Paid in full", 5, "Paid", 105m, 0m, 105m }
                });

            migrationBuilder.InsertData(
                table: "ServiceOrderItems",
                columns: new[] { "ServiceOrderItemId", "Description", "ItemType", "LaborHours", "Quantity", "ServiceOrderId", "ServiceTypeId", "SparePartId", "Subtotal", "UnitPrice" },
                values: new object[,]
                {
                    { 6, "Air-conditioning inspection and service", "Service", 1.5m, 1, 4, 5, null, 60m, 60m },
                    { 7, "Cabin air-filter replacement", "SparePart", null, 1, 4, null, 4, 18m, 18m },
                    { 8, "Battery testing and installation", "Service", 0.5m, 1, 5, 4, null, 20m, 20m },
                    { 9, "12-volt replacement battery", "SparePart", null, 1, 5, null, 3, 85m, 85m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "InvoiceId", "Notes", "PaymentDate", "PaymentMethod", "Status", "TransactionReference" },
                values: new object[,]
                {
                    { 3, 105m, 5, "Full card payment", new DateTime(2026, 8, 12, 9, 30, 0, 0, DateTimeKind.Unspecified), "Card", "Completed", "PAY-2026-0003" },
                    { 4, 20m, 4, "Online payment awaiting confirmation", new DateTime(2026, 8, 20, 11, 30, 0, 0, DateTimeKind.Unspecified), "Online", "Pending", "PAY-2026-0004" }
                });

            EnableSeedForeignKeys(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DisableSeedForeignKeys(migrationBuilder);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "yousef@example.com", true, "$2a$11$hALBJpOvyTrZlLEcJnPFWu4NA7TBLwk3q3huW/keV6c4Bbk6brA9O", "91111111", "Customer", "Yousef" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@example.com", true, "$2a$11$BGK2Ppq9wuPbpnsWk2L6W.tnrlxujROYp9PR2tLzmDAIfswo4zKxK", "92222222", "Customer", "Maria" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "bassam@example.com", true, "$2a$11$ok4EkGbmQVurJmezTbQcL.3Vhxj7n95ThNtJeSXH1rPD1XZMSrYZC", "93333333", "Customer", "Bassam" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed@example.com", true, "$2a$11$nUeJyCFSbAhaazyDrTer5exvyE/ImOZCqkiHG04iNos3CK4Roky12", "94444444", "Mechanic", "Ahmed" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[] { new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "khalid@example.com", true, "$2a$11$Sf7OqYUyOnJmF.kR9vG2/O5rcqbClLV2JmIeNoH5bfOq2YujF4Z3.", "95555555", "Mechanic", "Khalid" });

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1,
                columns: new[] { "AppointmentDate", "CreatedAt", "Notes", "Status" },
                values: new object[] { new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), "Regular maintenance and oil change", "Confirmed" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 2,
                columns: new[] { "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status" },
                values: new object[] { new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 8, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, "Brake inspection", 2, "Pending" });

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 3,
                columns: new[] { "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status" },
                values: new object[] { new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 8, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Check engine warning light", 3, "Confirmed" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                columns: new[] { "Address", "BranchName", "Email", "PhoneNumber" },
                values: new object[] { "Muscat, Oman", "Main Branch", "main@vehicleservice.com", "99995555" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                columns: new[] { "Address", "BranchName", "Email", "PhoneNumber" },
                values: new object[] { "Barka, Oman", "Barka Branch", "barka@vehicleservice.com", "26891234" });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 1,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Barka, Oman", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2001, 5, 10), 3 });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 2,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Muscat, Oman", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2000, 8, 15), 4 });

            migrationBuilder.UpdateData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 3,
                columns: new[] { "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[] { "Barka, Oman", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1999, 3, 20), 5 });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 1,
                columns: new[] { "DueDate", "IssueDate", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 33.500m, 33.500m });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 2,
                columns: new[] { "DueDate", "IssueDate", "Notes", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment pending", 65.000m, 65.000m });

            migrationBuilder.UpdateData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 3,
                columns: new[] { "DueDate", "IssueDate", "Notes", "Subtotal", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment pending", 40.000m, 40.000m });

            migrationBuilder.UpdateData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 1,
                columns: new[] { "ExperienceYears", "HireDate", "Specialization", "UserId" },
                values: new object[] { 7, new DateOnly(2022, 1, 10), "Engine Repair", 6 });

            migrationBuilder.UpdateData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 2,
                columns: new[] { "ExperienceYears", "HireDate", "Specialization", "UserId" },
                values: new object[] { 5, new DateOnly(2023, 3, 15), "Electrical Systems", 7 });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1,
                columns: new[] { "Amount", "Notes", "PaymentDate", "TransactionReference" },
                values: new object[] { 33.500m, "Full payment", new DateTime(2026, 8, 10, 12, 30, 0, 0, DateTimeKind.Unspecified), "TXN-2026-0001" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2,
                columns: new[] { "Notes", "PaymentDate", "TransactionReference" },
                values: new object[] { "Partial payment", new DateTime(2026, 8, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), "TXN-2026-0002" });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 1,
                column: "Description",
                value: "Oil change service");

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 2,
                columns: new[] { "Description", "ItemType", "Subtotal", "UnitPrice" },
                values: new object[] { "Oil filter replacement", "Part", 8.500m, 8.500m });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 3,
                column: "Description",
                value: "Brake inspection service");

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 4,
                columns: new[] { "Description", "ItemType", "Subtotal", "UnitPrice" },
                values: new object[] { "Front brake pads", "Part", 35.000m, 35.000m });

            migrationBuilder.UpdateData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 5,
                columns: new[] { "Description", "Subtotal", "UnitPrice" },
                values: new object[] { "Engine diagnostic service", 40.000m, 40.000m });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 1,
                columns: new[] { "CompletionDate", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "TotalAmount" },
                values: new object[] { new DateTime(2026, 8, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Vehicle needs regular maintenance", "Oil and filter replacement required", new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 33.500m });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 2,
                columns: new[] { "AppointmentId", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[] { 2, new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "Customer requested brake inspection", "Brake system inspection in progress", new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "In Progress", 65.000m, 2 });

            migrationBuilder.UpdateData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 3,
                columns: new[] { "AppointmentId", "CreatedAt", "CustomerComplaint", "Diagnosis", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[] { 3, new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Engine warning light is on", "Diagnostic inspection required", new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Pending", 40.000m, 3 });

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 1,
                column: "Description",
                value: "Engine oil and oil filter replacement");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 2,
                column: "Description",
                value: "Complete brake system inspection");

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 3,
                column: "BasePrice",
                value: 40.000m);

            migrationBuilder.UpdateData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 4,
                columns: new[] { "BasePrice", "Description" },
                values: new object[] { 15.000m, "Vehicle battery inspection and replacement" });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 1,
                columns: new[] { "StockQuantity", "UnitPrice" },
                values: new object[] { 50, 8.500m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 2,
                columns: new[] { "Description", "PartName", "StockQuantity", "UnitPrice" },
                values: new object[] { "Front brake pads", "Brake Pads", 20, 35.000m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 3,
                columns: new[] { "BranchId", "Description", "StockQuantity", "UnitPrice" },
                values: new object[] { 2, "12V vehicle battery", 15, 45.000m });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 4,
                columns: new[] { "Description", "PartName", "PartNumber", "ReorderLevel", "StockQuantity", "UnitPrice" },
                values: new object[] { "Engine air filter", "Air Filter", "AF-001", 5, 30, 12.000m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[,]
                {
                    { 23333, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohammed@example.com", true, "$2a$11$njiRWwm346PHbIdUUNQ9q.zTnQ60S7s81iFMnb.i0oY/MUWj2UOCe", "99999900", "Admin", "Mohammed" },
                    { 122222, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hawa@example.com", true, "$2a$11$ddvzOew6X6eRPWFL05eUHepg3eEwIUcR3PRS0QE64qxIgGaecTkZS", "99990000", "Admin", "Hawa" },
                    { 300001, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "$2a$11$dYf86bU3YfogdeiSjQy93eNG/ytmFJtHfDgNuSpP8mOMIzmenlq.K", null, "Admin", "Admin" }
                });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PlateNumber", "VIN" },
                values: new object[] { new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "12345", "OMVIN000000000001" });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[] { new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Nissan", 62000m, "Altima", "23456", "OMVIN000000000002", 2021 });

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[] { new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Honda", 28000m, "Accord", "34567", "OMVIN000000000003", 2023 });

            EnableSeedForeignKeys(migrationBuilder);
        }

        private static void DisableSeedForeignKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE [CustomerProfiles] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [MechanicProfiles] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [Vehicles] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [Appointments] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [ServiceOrders] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [ServiceOrderItems] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [Invoices] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [Payments] NOCHECK CONSTRAINT ALL;
                ALTER TABLE [SpareParts] NOCHECK CONSTRAINT ALL;
                """);
        }

        private static void EnableSeedForeignKeys(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE [CustomerProfiles] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [MechanicProfiles] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [Vehicles] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [Appointments] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [ServiceOrders] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [ServiceOrderItems] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [Invoices] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [Payments] WITH CHECK CHECK CONSTRAINT ALL;
                ALTER TABLE [SpareParts] WITH CHECK CHECK CONSTRAINT ALL;
                """);
        }
    }
}
