using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {

        // Users

        modelBuilder.Entity<UserModel>().HasData(
            // Admin 1 
            new UserModel
            {
                UserId = 1,
                UserName = "Hawa",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Email = "hawa@example.com",
                Role = "Admin",
                PhoneNumber = "99990000",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },

            // Admin 2 
            new UserModel
            {
                UserId = 2,
                UserName = "Mohammed",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Email = "mohammed@example.com",
                Role = "Admin",
                PhoneNumber = "99999900",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 2)
            },

            // Customer 1 
            new UserModel
            {
                UserId = 3,
                UserName = "Yousef",
                Password = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                Email = "yousef@example.com",
                Role = "Customer",
                PhoneNumber = "91111111",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 3)
            },

            // Customer 2 
            new UserModel
            {
                UserId = 4,
                UserName = "Maria",
                Password = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                Email = "maria@example.com",
                Role = "Customer",
                PhoneNumber = "92222222",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 4)
            },

            // Customer 3 
            new UserModel
            {
                UserId = 5,
                UserName = "Bassam",
                Password = BCrypt.Net.BCrypt.HashPassword("Customer@123"),
                Email = "bassam@example.com",
                Role = "Customer",
                PhoneNumber = "93333333",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 5)
            },
            
            // Mechanic 1
            new UserModel
            {
                UserId = 6,
                UserName = "Ahmed",
                Password = BCrypt.Net.BCrypt.HashPassword("Mechanic@123"),
                Email = "ahmed@example.com",
                Role = "Mechanic",
                PhoneNumber = "94444444",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 6)
            },

            // Mechanic 2
            new UserModel
            {
                UserId = 7,
                UserName = "Khalid",
                Password = BCrypt.Net.BCrypt.HashPassword("Mechanic@123"),
                Email = "khalid@example.com",
                Role = "Mechanic",
                PhoneNumber = "95555555",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 7)
            }
        );
        
        // Branches

        modelBuilder.Entity<BranchModel>().HasData(
            new BranchModel
            {
                BranchId = 1,
                BranchName = "Main Branch",
                Address = "Muscat, Oman",
                PhoneNumber = "99995555",
                Email = "main@vehicleservice.com",
                OpeningTime = new TimeSpan(8, 0, 0),
                ClosingTime = new TimeSpan(18, 0, 0),
                IsActive = true
            },
            new BranchModel
            {
                BranchId = 2,
                BranchName = "Barka Branch",
                Address = "Barka, Oman",
                PhoneNumber = "26891234",
                Email = "barka@vehicleservice.com",
                OpeningTime = new TimeSpan(8, 0, 0),
                ClosingTime = new TimeSpan(18, 0, 0),
                IsActive = true
            }
        );
        
        // Customer Profiles

        modelBuilder.Entity<CustomerProfileModel>().HasData(
            new CustomerProfileModel
            {
                CustomerProfileId = 1,
                UserId = 3,
                Address = "Barka, Oman",
                DateOfBirth = new DateOnly(2001, 5, 10),
                CreatedAt = new DateTime(2026, 1, 3)
            },

            new CustomerProfileModel
            {
                CustomerProfileId = 2,
                UserId = 4,
                Address = "Muscat, Oman",
                DateOfBirth = new DateOnly(2000, 8, 15),
                CreatedAt = new DateTime(2026, 1, 4)
            },

            new CustomerProfileModel
            {
                CustomerProfileId = 3,
                UserId = 5,
                Address = "Barka, Oman",
                DateOfBirth = new DateOnly(1999, 3, 20),
                CreatedAt = new DateTime(2026, 1, 5)
            }
            
        );
        
        // Mechanic Profiles

        modelBuilder.Entity<MechanicProfileModel>().HasData(
            new MechanicProfileModel
            {
                MechanicProfileId = 1,
                UserId = 6,
                BranchId = 1,
                Specialization = "Engine Repair",
                ExperienceYears = 7,
                HireDate = new DateOnly(2022, 1, 10),
                IsAvailable = true
            },

            new MechanicProfileModel
            {
                MechanicProfileId = 2,
                UserId = 7,
                BranchId = 2,
                Specialization = "Electrical Systems",
                ExperienceYears = 5,
                HireDate = new DateOnly(2023, 3, 15),
                IsAvailable = true
            }
        );
        
        // Vehicles

        modelBuilder.Entity<VehicleModel>().HasData(
            new VehicleModel
            {
                VehicleId = 1,
                CustomerProfileId = 1,
                PlateNumber = "12345",
                VIN = "OMVIN000000000001",
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Color = "White",
                Mileage = 45000,
                CreatedAt = new DateTime(2026, 1, 12)
            },

            new VehicleModel
            {
                VehicleId = 2,
                CustomerProfileId = 2,
                PlateNumber = "23456",
                VIN = "OMVIN000000000002",
                Make = "Nissan",
                Model = "Altima",
                Year = 2021,
                Color = "Black",
                Mileage = 62000,
                CreatedAt = new DateTime(2026, 1, 13)
            },

            new VehicleModel
            {
                VehicleId = 3,
                CustomerProfileId = 3,
                PlateNumber = "34567",
                VIN = "OMVIN000000000003",
                Make = "Honda",
                Model = "Accord",
                Year = 2023,
                Color = "Silver",
                Mileage = 28000,
                CreatedAt = new DateTime(2026, 1, 14)
            }
        );
        
        // Service Types

        modelBuilder.Entity<ServiceTypeModel>().HasData(
            new ServiceTypeModel
            {
                ServiceTypeId = 1,
                Name = "Oil Change",
                Description = "Engine oil and oil filter replacement",
                BasePrice = 25.000m,
                EstimatedDurationMinutes = 45,
                IsActive = true
            },

            new ServiceTypeModel
            {
                ServiceTypeId = 2,
                Name = "Brake Inspection",
                Description = "Complete brake system inspection",
                BasePrice = 30.000m,
                EstimatedDurationMinutes = 60,
                IsActive = true
            },

            new ServiceTypeModel
            {
                ServiceTypeId = 3,
                Name = "Engine Diagnostic",
                Description = "Computerized engine diagnostic service",
                BasePrice = 40.000m,
                EstimatedDurationMinutes = 60,
                IsActive = true
            },

            new ServiceTypeModel
            {
                ServiceTypeId = 4,
                Name = "Battery Replacement",
                Description = "Vehicle battery inspection and replacement",
                BasePrice = 15.000m,
                EstimatedDurationMinutes = 30,
                IsActive = true
            }
        );
        
        
        // Spare Parts

        modelBuilder.Entity<SparePartModel>().HasData(
            new SparePartModel
            {
                SparePartId = 1,
                BranchId = 1,
                PartName = "Oil Filter",
                PartNumber = "OF-001",
                Description = "Standard engine oil filter",
                UnitPrice = 8.500m,
                StockQuantity = 50,
                ReorderLevel = 10,
                IsAvailable = true
            },

            new SparePartModel
            {
                SparePartId = 2,
                BranchId = 1,
                PartName = "Brake Pads",
                PartNumber = "BP-001",
                Description = "Front brake pads",
                UnitPrice = 35.000m,
                StockQuantity = 20,
                ReorderLevel = 5,
                IsAvailable = true
            },

            new SparePartModel
            {
                SparePartId = 3,
                BranchId = 2,
                PartName = "Car Battery",
                PartNumber = "BAT-001",
                Description = "12V vehicle battery",
                UnitPrice = 45.000m,
                StockQuantity = 15,
                ReorderLevel = 3,
                IsAvailable = true
            },

            new SparePartModel
            {
                SparePartId = 4,
                BranchId = 2,
                PartName = "Air Filter",
                PartNumber = "AF-001",
                Description = "Engine air filter",
                UnitPrice = 12.000m,
                StockQuantity = 30,
                ReorderLevel = 5,
                IsAvailable = true
            }
        );
        
        // Appointments

        modelBuilder.Entity<AppointmentModel>().HasData(
            new AppointmentModel
            {
                AppointmentId = 1,
                CustomerProfileId = 1,
                VehicleId = 1,
                ServiceTypeId = 1,
                MechanicProfileId = 1,
                BranchId = 1,
                AppointmentDate = new DateTime(2026, 8, 10, 10, 0, 0),
                Status = "Confirmed",
                Notes = "Regular maintenance and oil change",
                CreatedAt = new DateTime(2026, 8, 8, 10, 0, 0)
            },

            new AppointmentModel
            {
                AppointmentId = 2,
                CustomerProfileId = 2,
                VehicleId = 2,
                ServiceTypeId = 2,
                MechanicProfileId = 1,
                BranchId = 1,
                AppointmentDate = new DateTime(2026, 8, 11, 11, 0, 0),
                Status = "Pending",
                Notes = "Brake inspection",
                CreatedAt = new DateTime(2026, 8, 8, 11, 0, 0)
            },

            new AppointmentModel
            {
                AppointmentId = 3,
                CustomerProfileId = 3,
                VehicleId = 3,
                ServiceTypeId = 3,
                MechanicProfileId = 2,
                BranchId = 2,
                AppointmentDate = new DateTime(2026, 8, 12, 9, 0, 0),
                Status = "Confirmed",
                Notes = "Check engine warning light",
                CreatedAt = new DateTime(2026, 8, 8, 12, 0, 0)
            }
        );
        
        // Service Orders

        modelBuilder.Entity<ServiceOrderModel>().HasData(
            new ServiceOrderModel
            {
                ServiceOrderId = 1,
                AppointmentId = 1,
                CustomerProfileId = 1,
                VehicleId = 1,
                MechanicProfileId = 1,
                BranchId = 1,
                OrderDate = new DateTime(2026, 8, 10, 10, 0, 0),
                CompletionDate = new DateTime(2026, 8, 10, 12, 0, 0),
                Status = "Completed",
                CustomerComplaint = "Vehicle needs regular maintenance",
                Diagnosis = "Oil and filter replacement required",
                TotalAmount = 33.500m,
                CreatedAt = new DateTime(2026, 8, 10, 10, 0, 0)
            },

            new ServiceOrderModel
            {
                ServiceOrderId = 2,
                AppointmentId = 2,
                CustomerProfileId = 2,
                VehicleId = 2,
                MechanicProfileId = 1,
                BranchId = 1,
                OrderDate = new DateTime(2026, 8, 11, 11, 0, 0),
                CompletionDate = null,
                Status = "In Progress",
                CustomerComplaint = "Customer requested brake inspection",
                Diagnosis = "Brake system inspection in progress",
                TotalAmount = 65.000m,
                CreatedAt = new DateTime(2026, 8, 11, 11, 0, 0)
            },

            new ServiceOrderModel
            {
                ServiceOrderId = 3,
                AppointmentId = 3,
                CustomerProfileId = 3,
                VehicleId = 3,
                MechanicProfileId = 2,
                BranchId = 2,
                OrderDate = new DateTime(2026, 8, 12, 9, 0, 0),
                CompletionDate = null,
                Status = "Pending",
                CustomerComplaint = "Engine warning light is on",
                Diagnosis = "Diagnostic inspection required",
                TotalAmount = 40.000m,
                CreatedAt = new DateTime(2026, 8, 12, 9, 0, 0)
            }
        );
        
        
        // Service Order Items

    modelBuilder.Entity<ServiceOrderItemModel>().HasData(
        // Order 1 - Oil Change Service
        new ServiceOrderItemModel
        {
            ServiceOrderItemId = 1,
            ServiceOrderId = 1,
            ServiceTypeId = 1,
            SparePartId = null,
            ItemType = "Service",
            Description = "Oil change service",
            Quantity = 1,
            UnitPrice = 25.000m,
            LaborHours = 0.75m,
            Subtotal = 25.000m
        },

        // Order 1 - Oil Filter
        new ServiceOrderItemModel
        {
            ServiceOrderItemId = 2,
            ServiceOrderId = 1,
            ServiceTypeId = null,
            SparePartId = 1,
            ItemType = "Part",
            Description = "Oil filter replacement",
            Quantity = 1,
            UnitPrice = 8.500m,
            LaborHours = null,
            Subtotal = 8.500m
        },

        // Order 2 - Brake Inspection
        new ServiceOrderItemModel
        {
            ServiceOrderItemId = 3,
            ServiceOrderId = 2,
            ServiceTypeId = 2,
            SparePartId = null,
            ItemType = "Service",
            Description = "Brake inspection service",
            Quantity = 1,
            UnitPrice = 30.000m,
            LaborHours = 1.00m,
            Subtotal = 30.000m
        },

        // Order 2 - Brake Pads
        new ServiceOrderItemModel
        {
            ServiceOrderItemId = 4,
            ServiceOrderId = 2,
            ServiceTypeId = null,
            SparePartId = 2,
            ItemType = "Part",
            Description = "Front brake pads",
            Quantity = 1,
            UnitPrice = 35.000m,
            LaborHours = null,
            Subtotal = 35.000m
        },

        // Order 3 - Engine Diagnostic
        new ServiceOrderItemModel
        {
            ServiceOrderItemId = 5,
            ServiceOrderId = 3,
            ServiceTypeId = 3,
            SparePartId = null,
            ItemType = "Service",
            Description = "Engine diagnostic service",
            Quantity = 1,
            UnitPrice = 40.000m,
            LaborHours = 1.00m,
            Subtotal = 40.000m
        }
    );
    
    // Invoices

    modelBuilder.Entity<InvoiceModel>().HasData(
        // Invoice for completed order
        new InvoiceModel
        {
            InvoiceId = 1,
            ServiceOrderId = 1,
            InvoiceNumber = "INV-2026-0001",
            IssueDate = new DateTime(2026, 8, 10),
            DueDate = new DateTime(2026, 8, 10),
            Subtotal = 33.500m,
            TaxAmount = 0.000m,
            DiscountAmount = 0.000m,
            TotalAmount = 33.500m,
            Status = "Paid",
            Notes = "Paid in full"
        },

        // Invoice for in-progress order
        new InvoiceModel
        {
            InvoiceId = 2,
            ServiceOrderId = 2,
            InvoiceNumber = "INV-2026-0002",
            IssueDate = new DateTime(2026, 8, 11),
            DueDate = new DateTime(2026, 8, 18),
            Subtotal = 65.000m,
            TaxAmount = 0.000m,
            DiscountAmount = 0.000m,
            TotalAmount = 65.000m,
            Status = "Unpaid",
            Notes = "Payment pending"
        },

        // Invoice for pending order
        new InvoiceModel
        {
            InvoiceId = 3,
            ServiceOrderId = 3,
            InvoiceNumber = "INV-2026-0003",
            IssueDate = new DateTime(2026, 8, 12),
            DueDate = new DateTime(2026, 8, 19),
            Subtotal = 40.000m,
            TaxAmount = 0.000m,
            DiscountAmount = 0.000m,
            TotalAmount = 40.000m,
            Status = "Unpaid",
            Notes = "Payment pending"
        }
    );
    
    // Payments

    modelBuilder.Entity<PaymentModel>().HasData(
        new PaymentModel
        {
            PaymentId = 1,
            InvoiceId = 1,
            Amount = 33.500m,
            PaymentDate = new DateTime(2026, 8, 10, 12, 30, 0),
            PaymentMethod = "Card",
            TransactionReference = "TXN-2026-0001",
            Status = "Completed",
            Notes = "Full payment"
        },

        new PaymentModel
        {
            PaymentId = 2,
            InvoiceId = 2,
            Amount = 30.000m,
            PaymentDate = new DateTime(2026, 8, 11, 14, 0, 0),
            PaymentMethod = "Cash",
            TransactionReference = "TXN-2026-0002",
            Status = "Completed",
            Notes = "Partial payment"
        }
    );
    
        
        
    }
}

