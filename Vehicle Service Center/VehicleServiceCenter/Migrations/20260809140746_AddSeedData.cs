using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleServiceCenter.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "BranchName", "ClosingTime", "Email", "IsActive", "OpeningTime", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Muscat, Oman", "Main Branch", new TimeSpan(0, 18, 0, 0, 0), "main@vehicleservice.com", true, new TimeSpan(0, 8, 0, 0, 0), "99995555" },
                    { 2, "Barka, Oman", "Barka Branch", new TimeSpan(0, 18, 0, 0, 0), "barka@vehicleservice.com", true, new TimeSpan(0, 8, 0, 0, 0), "26891234" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "BasePrice", "Description", "EstimatedDurationMinutes", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 25.000m, "Engine oil and oil filter replacement", 45, true, "Oil Change" },
                    { 2, 30.000m, "Complete brake system inspection", 60, true, "Brake Inspection" },
                    { 3, 40.000m, "Computerized engine diagnostic service", 60, true, "Engine Diagnostic" },
                    { 4, 15.000m, "Vehicle battery inspection and replacement", 30, true, "Battery Replacement" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "PasswordHash", "PhoneNumber", "Role", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hawa@example.com", true, "$2a$11$N3sA0Zt34ok9v8JHJdCtbexJ2lXnlBMJL1XpSdW90B7KRs0H3dmn2", "99990000", "Admin", "Hawa" },
                    { 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohammed@example.com", true, "$2a$11$1zioPNrzUW7ROf1GXOhBWOGAPetSd.cyhVD5et.X/Ps6IkK95.h.2", "99999900", "Admin", "Mohammed" },
                    { 3, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "yousef@example.com", true, "$2a$11$hALBJpOvyTrZlLEcJnPFWu4NA7TBLwk3q3huW/keV6c4Bbk6brA9O", "91111111", "Customer", "Yousef" },
                    { 4, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@example.com", true, "$2a$11$BGK2Ppq9wuPbpnsWk2L6W.tnrlxujROYp9PR2tLzmDAIfswo4zKxK", "92222222", "Customer", "Maria" },
                    { 5, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "bassam@example.com", true, "$2a$11$ok4EkGbmQVurJmezTbQcL.3Vhxj7n95ThNtJeSXH1rPD1XZMSrYZC", "93333333", "Customer", "Bassam" },
                    { 6, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed@example.com", true, "$2a$11$nUeJyCFSbAhaazyDrTer5exvyE/ImOZCqkiHG04iNos3CK4Roky12", "94444444", "Mechanic", "Ahmed" },
                    { 7, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "khalid@example.com", true, "$2a$11$Sf7OqYUyOnJmF.kR9vG2/O5rcqbClLV2JmIeNoH5bfOq2YujF4Z3.", "95555555", "Mechanic", "Khalid" }
                });

            migrationBuilder.InsertData(
                table: "CustomerProfiles",
                columns: new[] { "CustomerProfileId", "Address", "CreatedAt", "DateOfBirth", "UserId" },
                values: new object[,]
                {
                    { 1, "Barka, Oman", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2001, 5, 10), 3 },
                    { 2, "Muscat, Oman", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2000, 8, 15), 4 },
                    { 3, "Barka, Oman", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1999, 3, 20), 5 }
                });

            migrationBuilder.InsertData(
                table: "MechanicProfiles",
                columns: new[] { "MechanicProfileId", "BranchId", "ExperienceYears", "HireDate", "IsAvailable", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 7, new DateOnly(2022, 1, 10), true, "Engine Repair", 6 },
                    { 2, 2, 5, new DateOnly(2023, 3, 15), true, "Electrical Systems", 7 }
                });

            migrationBuilder.InsertData(
                table: "SpareParts",
                columns: new[] { "SparePartId", "BranchId", "Description", "IsAvailable", "PartName", "PartNumber", "ReorderLevel", "StockQuantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, "Standard engine oil filter", true, "Oil Filter", "OF-001", 10, 50, 8.500m },
                    { 2, 1, "Front brake pads", true, "Brake Pads", "BP-001", 5, 20, 35.000m },
                    { 3, 2, "12V vehicle battery", true, "Car Battery", "BAT-001", 3, 15, 45.000m },
                    { 4, 2, "Engine air filter", true, "Air Filter", "AF-001", 5, 30, 12.000m }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "Color", "CreatedAt", "CustomerProfileId", "Make", "Mileage", "Model", "PlateNumber", "VIN", "Year" },
                values: new object[,]
                {
                    { 1, "White", new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Toyota", 45000m, "Camry", "12345", "OMVIN000000000001", 2022 },
                    { 2, "Black", new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Nissan", 62000m, "Altima", "23456", "OMVIN000000000002", 2021 },
                    { 3, "Silver", new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Honda", 28000m, "Accord", "34567", "OMVIN000000000003", 2023 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "BranchId", "CreatedAt", "CustomerProfileId", "MechanicProfileId", "Notes", "ServiceTypeId", "Status", "VehicleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 8, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Regular maintenance and oil change", 1, "Confirmed", 1 },
                    { 2, new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 8, 8, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, "Brake inspection", 2, "Pending", 2 },
                    { 3, new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 8, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Check engine warning light", 3, "Confirmed", 3 }
                });

            migrationBuilder.InsertData(
                table: "ServiceOrders",
                columns: new[] { "ServiceOrderId", "AppointmentId", "BranchId", "CompletionDate", "CreatedAt", "CustomerComplaint", "CustomerProfileId", "Diagnosis", "MechanicProfileId", "OrderDate", "Status", "TotalAmount", "VehicleId" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 8, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Vehicle needs regular maintenance", 1, "Oil and filter replacement required", 1, new DateTime(2026, 8, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 33.500m, 1 },
                    { 2, 2, 1, null, new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "Customer requested brake inspection", 2, "Brake system inspection in progress", 1, new DateTime(2026, 8, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), "In Progress", 65.000m, 2 },
                    { 3, 3, 2, null, new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Engine warning light is on", 3, "Diagnostic inspection required", 2, new DateTime(2026, 8, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Pending", 40.000m, 3 }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "DiscountAmount", "DueDate", "InvoiceNumber", "IssueDate", "Notes", "ServiceOrderId", "Status", "Subtotal", "TaxAmount", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 0.000m, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0001", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid in full", 1, "Paid", 33.500m, 0.000m, 33.500m },
                    { 2, 0.000m, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0002", new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment pending", 2, "Unpaid", 65.000m, 0.000m, 65.000m },
                    { 3, 0.000m, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-0003", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment pending", 3, "Unpaid", 40.000m, 0.000m, 40.000m }
                });

            migrationBuilder.InsertData(
                table: "ServiceOrderItems",
                columns: new[] { "ServiceOrderItemId", "Description", "ItemType", "LaborHours", "Quantity", "ServiceOrderId", "ServiceTypeId", "SparePartId", "Subtotal", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "Oil change service", "Service", 0.75m, 1, 1, 1, null, 25.000m, 25.000m },
                    { 2, "Oil filter replacement", "Part", null, 1, 1, null, 1, 8.500m, 8.500m },
                    { 3, "Brake inspection service", "Service", 1.00m, 1, 2, 2, null, 30.000m, 30.000m },
                    { 4, "Front brake pads", "Part", null, 1, 2, null, 2, 35.000m, 35.000m },
                    { 5, "Engine diagnostic service", "Service", 1.00m, 1, 3, 3, null, 40.000m, 40.000m }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "InvoiceId", "Notes", "PaymentDate", "PaymentMethod", "Status", "TransactionReference" },
                values: new object[,]
                {
                    { 1, 33.500m, 1, "Full payment", new DateTime(2026, 8, 10, 12, 30, 0, 0, DateTimeKind.Unspecified), "Card", "Completed", "TXN-2026-0001" },
                    { 2, 30.000m, 2, "Partial payment", new DateTime(2026, 8, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), "Cash", "Completed", "TXN-2026-0002" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceOrderItems",
                keyColumn: "ServiceOrderItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "InvoiceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SpareParts",
                keyColumn: "SparePartId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceOrders",
                keyColumn: "ServiceOrderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MechanicProfiles",
                keyColumn: "MechanicProfileId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomerProfiles",
                keyColumn: "CustomerProfileId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);
        }
    }
}
