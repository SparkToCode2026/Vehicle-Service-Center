using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedBranches(modelBuilder);
        SeedCustomerProfiles(modelBuilder);
        SeedMechanicProfiles(modelBuilder);
        SeedVehicles(modelBuilder);
        SeedServiceTypes(modelBuilder);
        SeedSpareParts(modelBuilder);
        SeedAppointments(modelBuilder);
        SeedServiceOrders(modelBuilder);
        SeedServiceOrderItems(modelBuilder);
        SeedInvoices(modelBuilder);
        SeedPayments(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasData(
            new UserModel
            {
                UserId = 1,
                UserName = "Admin User",
                Password = "$2a$11$GiQ5o4CPh1sr1GQQVxyv2O51hdE2lXCVHbfQJRdR667WwFe0EYR6m",
                Email = "admin@gmail.com",
                Role = "Admin",
                PhoneNumber = "90000001",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 8, 0, 0)
            },
            new UserModel
            {
                UserId = 3,
                UserName = "Sara Al Balushi",
                Password = "$2a$11$WHsBifzPW/23FBBmsDb/pOWH5CFLr7xgXt85f7Y7Q36m51nHJBamC",
                Email = "customer1@gmail.com",
                Role = "Customer",
                PhoneNumber = "91110001",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 2, 9, 0, 0)
            },
            new UserModel
            {
                UserId = 4,
                UserName = "Omar Al Harthi",
                Password = "$2a$11$2W08xdjKhmOrb.XzkPvUwuYEPStJe0V.HixHDfa.A04RcdoqlYC1i",
                Email = "customer2@gmail.com",
                Role = "Customer",
                PhoneNumber = "92220002",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 3, 9, 0, 0)
            },
            new UserModel
            {
                UserId = 5,
                UserName = "Lina Al Lawati",
                Password = "$2a$11$PR.SYUMNka84Z8vjRcx89.dWEZieqCKueqHAw1ypZrRsXxD8Z2UAS",
                Email = "customer3@gmail.com",
                Role = "Customer",
                PhoneNumber = "93330003",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 4, 9, 0, 0)
            },
            new UserModel
            {
                UserId = 6,
                UserName = "Ahmed Al Rashdi",
                Password = "$2a$11$vekQUchkuBwnXV1BdoGv3u/2QaQRQhGd.RrDZyyjvM.my4x9S5O.S",
                Email = "mechanic1@gmail.com",
                Role = "Mechanic",
                PhoneNumber = "94440004",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 5, 9, 0, 0)
            },
            new UserModel
            {
                UserId = 7,
                UserName = "Khalid Al Siyabi",
                Password = "$2a$11$/YWclfj9mFqcGGPnoo0p0.X0zZ0jSOgj7nhrrngllAdcSg4Tl1n.m",
                Email = "mechanic2@gmail.com",
                Role = "Mechanic",
                PhoneNumber = "95550005",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 6, 9, 0, 0)
            });
    }

    private static void SeedBranches(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchModel>().HasData(
            new BranchModel
            {
                BranchId = 1,
                BranchName = "Muscat Main Branch",
                Address = "Al Khuwair, Muscat, Oman",
                PhoneNumber = "24000001",
                Email = "muscat@vehicleservice.com",
                OpeningTime = new TimeSpan(8, 0, 0),
                ClosingTime = new TimeSpan(18, 0, 0),
                IsActive = true
            },
            new BranchModel
            {
                BranchId = 2,
                BranchName = "Seeb Branch",
                Address = "Al Hail, Seeb, Oman",
                PhoneNumber = "24000002",
                Email = "seeb@vehicleservice.com",
                OpeningTime = new TimeSpan(8, 0, 0),
                ClosingTime = new TimeSpan(18, 0, 0),
                IsActive = true
            });
    }

    private static void SeedCustomerProfiles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerProfileModel>().HasData(
            new CustomerProfileModel
            {
                CustomerProfileId = 1,
                UserId = 3,
                Address = "Bawshar, Muscat, Oman",
                DateOfBirth = new DateOnly(1995, 4, 12),
                CreatedAt = new DateTime(2026, 1, 2, 9, 5, 0)
            },
            new CustomerProfileModel
            {
                CustomerProfileId = 2,
                UserId = 4,
                Address = "Al Amerat, Muscat, Oman",
                DateOfBirth = new DateOnly(1990, 9, 21),
                CreatedAt = new DateTime(2026, 1, 3, 9, 5, 0)
            },
            new CustomerProfileModel
            {
                CustomerProfileId = 3,
                UserId = 5,
                Address = "Al Hail, Seeb, Oman",
                DateOfBirth = new DateOnly(1998, 2, 7),
                CreatedAt = new DateTime(2026, 1, 4, 9, 5, 0)
            });
    }

    private static void SeedMechanicProfiles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MechanicProfileModel>().HasData(
            new MechanicProfileModel
            {
                MechanicProfileId = 1,
                UserId = 6,
                BranchId = 1,
                Specialization = "Engine and Brake Systems",
                ExperienceYears = 8,
                HireDate = new DateOnly(2021, 3, 15),
                IsAvailable = true
            },
            new MechanicProfileModel
            {
                MechanicProfileId = 2,
                UserId = 7,
                BranchId = 2,
                Specialization = "Electrical and Air Conditioning",
                ExperienceYears = 6,
                HireDate = new DateOnly(2022, 6, 1),
                IsAvailable = true
            });
    }

    private static void SeedVehicles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleModel>().HasData(
            new VehicleModel
            {
                VehicleId = 1,
                CustomerProfileId = 1,
                PlateNumber = "OM-10001",
                VIN = "OMVIN2026000000001",
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Color = "White",
                Mileage = 45000m,
                CreatedAt = new DateTime(2026, 1, 10)
            },
            new VehicleModel
            {
                VehicleId = 2,
                CustomerProfileId = 1,
                PlateNumber = "OM-10002",
                VIN = "OMVIN2026000000002",
                Make = "Lexus",
                Model = "NX 350",
                Year = 2023,
                Color = "Black",
                Mileage = 28500m,
                CreatedAt = new DateTime(2026, 2, 5)
            },
            new VehicleModel
            {
                VehicleId = 3,
                CustomerProfileId = 2,
                PlateNumber = "OM-20001",
                VIN = "OMVIN2026000000003",
                Make = "Nissan",
                Model = "Altima",
                Year = 2021,
                Color = "Silver",
                Mileage = 62000m,
                CreatedAt = new DateTime(2026, 1, 12)
            },
            new VehicleModel
            {
                VehicleId = 4,
                CustomerProfileId = 3,
                PlateNumber = "OM-30001",
                VIN = "OMVIN2026000000004",
                Make = "Honda",
                Model = "Accord",
                Year = 2024,
                Color = "Blue",
                Mileage = 19000m,
                CreatedAt = new DateTime(2026, 3, 18)
            });
    }

    private static void SeedServiceTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceTypeModel>().HasData(
            new ServiceTypeModel
            {
                ServiceTypeId = 1,
                Name = "Oil Change",
                Description = "Engine oil and oil-filter replacement",
                BasePrice = 25m,
                EstimatedDurationMinutes = 45,
                IsActive = true
            },
            new ServiceTypeModel
            {
                ServiceTypeId = 2,
                Name = "Brake Inspection",
                Description = "Complete brake-system inspection",
                BasePrice = 30m,
                EstimatedDurationMinutes = 60,
                IsActive = true
            },
            new ServiceTypeModel
            {
                ServiceTypeId = 3,
                Name = "Engine Diagnostic",
                Description = "Computerized engine diagnostic service",
                BasePrice = 45m,
                EstimatedDurationMinutes = 60,
                IsActive = true
            },
            new ServiceTypeModel
            {
                ServiceTypeId = 4,
                Name = "Battery Replacement",
                Description = "Battery testing and replacement service",
                BasePrice = 20m,
                EstimatedDurationMinutes = 30,
                IsActive = true
            },
            new ServiceTypeModel
            {
                ServiceTypeId = 5,
                Name = "Air Conditioning Service",
                Description = "Air-conditioning inspection and maintenance",
                BasePrice = 60m,
                EstimatedDurationMinutes = 90,
                IsActive = true
            });
    }

    private static void SeedSpareParts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SparePartModel>().HasData(
            new SparePartModel
            {
                SparePartId = 1,
                BranchId = 1,
                PartName = "Oil Filter",
                PartNumber = "OF-001",
                Description = "Standard engine oil filter",
                UnitPrice = 11m,
                StockQuantity = 45,
                ReorderLevel = 10,
                IsAvailable = true
            },
            new SparePartModel
            {
                SparePartId = 2,
                BranchId = 1,
                PartName = "Front Brake Pads",
                PartNumber = "BP-001",
                Description = "Front axle ceramic brake-pad set",
                UnitPrice = 44m,
                StockQuantity = 18,
                ReorderLevel = 5,
                IsAvailable = true
            },
            new SparePartModel
            {
                SparePartId = 3,
                BranchId = 1,
                PartName = "Car Battery",
                PartNumber = "BAT-001",
                Description = "12-volt maintenance-free vehicle battery",
                UnitPrice = 85m,
                StockQuantity = 12,
                ReorderLevel = 3,
                IsAvailable = true
            },
            new SparePartModel
            {
                SparePartId = 4,
                BranchId = 2,
                PartName = "Cabin Air Filter",
                PartNumber = "CAF-001",
                Description = "Cabin air filter for climate-control systems",
                UnitPrice = 18m,
                StockQuantity = 25,
                ReorderLevel = 6,
                IsAvailable = true
            },
            new SparePartModel
            {
                SparePartId = 5,
                BranchId = 2,
                PartName = "Engine Air Filter",
                PartNumber = "EAF-001",
                Description = "Standard engine intake air filter",
                UnitPrice = 15m,
                StockQuantity = 30,
                ReorderLevel = 7,
                IsAvailable = true
            },
            new SparePartModel
            {
                SparePartId = 6,
                BranchId = 2,
                PartName = "Spark Plug Set",
                PartNumber = "SP-001",
                Description = "Four-piece iridium spark-plug set",
                UnitPrice = 32m,
                StockQuantity = 4,
                ReorderLevel = 5,
                IsAvailable = true
            });
    }

    private static void SeedAppointments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppointmentModel>().HasData(
            new AppointmentModel
            {
                AppointmentId = 1,
                CustomerProfileId = 1,
                VehicleId = 1,
                ServiceTypeId = 1,
                MechanicProfileId = 1,
                BranchId = 1,
                AppointmentDate = new DateTime(2026, 8, 1, 9, 0, 0),
                Status = "Completed",
                Notes = "Scheduled oil-change maintenance",
                CreatedAt = new DateTime(2026, 7, 28, 10, 0, 0)
            },
            new AppointmentModel
            {
                AppointmentId = 2,
                CustomerProfileId = 1,
                VehicleId = 2,
                ServiceTypeId = 5,
                MechanicProfileId = 2,
                BranchId = 2,
                AppointmentDate = new DateTime(2026, 8, 20, 10, 30, 0),
                Status = "Confirmed",
                Notes = "Air conditioner is not cooling properly",
                CreatedAt = new DateTime(2026, 8, 10, 14, 0, 0)
            },
            new AppointmentModel
            {
                AppointmentId = 3,
                CustomerProfileId = 2,
                VehicleId = 3,
                ServiceTypeId = 2,
                MechanicProfileId = 1,
                BranchId = 1,
                AppointmentDate = new DateTime(2026, 8, 13, 11, 0, 0),
                Status = "In Progress",
                Notes = "Noise while braking",
                CreatedAt = new DateTime(2026, 8, 8, 11, 0, 0)
            },
            new AppointmentModel
            {
                AppointmentId = 7,
                CustomerProfileId = 3,
                VehicleId = 4,
                ServiceTypeId = 3,
                MechanicProfileId = 2,
                BranchId = 2,
                AppointmentDate = new DateTime(2026, 8, 15, 9, 30, 0),
                Status = "Confirmed",
                Notes = "Check-engine warning light is on",
                CreatedAt = new DateTime(2026, 8, 9, 12, 0, 0)
            },
            new AppointmentModel
            {
                AppointmentId = 5,
                CustomerProfileId = 2,
                VehicleId = 3,
                ServiceTypeId = 4,
                MechanicProfileId = null,
                BranchId = 1,
                AppointmentDate = new DateTime(2026, 8, 22, 13, 0, 0),
                Status = "Pending",
                Notes = "Battery is slow during startup",
                CreatedAt = new DateTime(2026, 8, 12, 16, 0, 0)
            },
            new AppointmentModel
            {
                AppointmentId = 6,
                CustomerProfileId = 3,
                VehicleId = 4,
                ServiceTypeId = 1,
                MechanicProfileId = 2,
                BranchId = 2,
                AppointmentDate = new DateTime(2026, 8, 12, 15, 0, 0),
                Status = "Cancelled",
                Notes = "Customer cancelled the appointment",
                CreatedAt = new DateTime(2026, 8, 7, 9, 0, 0)
            });
    }

    private static void SeedServiceOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceOrderModel>().HasData(
            new ServiceOrderModel
            {
                ServiceOrderId = 1,
                AppointmentId = 1,
                CustomerProfileId = 1,
                VehicleId = 1,
                MechanicProfileId = 1,
                BranchId = 1,
                OrderDate = new DateTime(2026, 8, 1, 9, 0, 0),
                CompletionDate = new DateTime(2026, 8, 1, 10, 0, 0),
                Status = "Completed",
                CustomerComplaint = "Routine oil-change service",
                Diagnosis = "Oil and oil filter were due for replacement",
                TotalAmount = 36m,
                CreatedAt = new DateTime(2026, 8, 1, 9, 0, 0)
            },
            new ServiceOrderModel
            {
                ServiceOrderId = 2,
                AppointmentId = 3,
                CustomerProfileId = 2,
                VehicleId = 3,
                MechanicProfileId = 1,
                BranchId = 1,
                OrderDate = new DateTime(2026, 8, 13, 11, 0, 0),
                CompletionDate = null,
                Status = "InProgress",
                CustomerComplaint = "Noise and vibration while braking",
                Diagnosis = "Front brake pads require replacement",
                TotalAmount = 74m,
                CreatedAt = new DateTime(2026, 8, 13, 11, 0, 0)
            },
            new ServiceOrderModel
            {
                ServiceOrderId = 3,
                AppointmentId = 7,
                CustomerProfileId = 3,
                VehicleId = 4,
                MechanicProfileId = 2,
                BranchId = 2,
                OrderDate = new DateTime(2026, 8, 15, 9, 30, 0),
                CompletionDate = null,
                Status = "Approved",
                CustomerComplaint = "Check-engine warning light is on",
                Diagnosis = "Diagnostic scan is approved and waiting to begin",
                TotalAmount = 45m,
                CreatedAt = new DateTime(2026, 8, 15, 9, 30, 0)
            },
            new ServiceOrderModel
            {
                ServiceOrderId = 4,
                AppointmentId = 2,
                CustomerProfileId = 1,
                VehicleId = 2,
                MechanicProfileId = 2,
                BranchId = 2,
                OrderDate = new DateTime(2026, 8, 20, 10, 30, 0),
                CompletionDate = null,
                Status = "Pending",
                CustomerComplaint = "Air conditioner is not cooling properly",
                Diagnosis = "Air-conditioning inspection is pending",
                TotalAmount = 78m,
                CreatedAt = new DateTime(2026, 8, 20, 10, 30, 0)
            },
            new ServiceOrderModel
            {
                ServiceOrderId = 5,
                AppointmentId = null,
                CustomerProfileId = 2,
                VehicleId = 3,
                MechanicProfileId = 1,
                BranchId = 1,
                OrderDate = new DateTime(2026, 8, 12, 8, 30, 0),
                CompletionDate = new DateTime(2026, 8, 12, 9, 15, 0),
                Status = "Completed",
                CustomerComplaint = "Vehicle would not start",
                Diagnosis = "Battery failed its load test and was replaced",
                TotalAmount = 105m,
                CreatedAt = new DateTime(2026, 8, 12, 8, 30, 0)
            });
    }

    private static void SeedServiceOrderItems(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceOrderItemModel>().HasData(
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 1,
                ServiceOrderId = 1,
                ServiceTypeId = 1,
                SparePartId = null,
                ItemType = "Service",
                Description = "Oil-change labor",
                Quantity = 1,
                UnitPrice = 25m,
                LaborHours = 0.75m,
                Subtotal = 25m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 2,
                ServiceOrderId = 1,
                ServiceTypeId = null,
                SparePartId = 1,
                ItemType = "SparePart",
                Description = "Oil-filter replacement",
                Quantity = 1,
                UnitPrice = 11m,
                LaborHours = null,
                Subtotal = 11m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 3,
                ServiceOrderId = 2,
                ServiceTypeId = 2,
                SparePartId = null,
                ItemType = "Service",
                Description = "Brake inspection labor",
                Quantity = 1,
                UnitPrice = 30m,
                LaborHours = 1m,
                Subtotal = 30m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 4,
                ServiceOrderId = 2,
                ServiceTypeId = null,
                SparePartId = 2,
                ItemType = "SparePart",
                Description = "Front brake-pad set",
                Quantity = 1,
                UnitPrice = 44m,
                LaborHours = null,
                Subtotal = 44m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 5,
                ServiceOrderId = 3,
                ServiceTypeId = 3,
                SparePartId = null,
                ItemType = "Service",
                Description = "Engine diagnostic scan",
                Quantity = 1,
                UnitPrice = 45m,
                LaborHours = 1m,
                Subtotal = 45m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 6,
                ServiceOrderId = 4,
                ServiceTypeId = 5,
                SparePartId = null,
                ItemType = "Service",
                Description = "Air-conditioning inspection and service",
                Quantity = 1,
                UnitPrice = 60m,
                LaborHours = 1.5m,
                Subtotal = 60m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 7,
                ServiceOrderId = 4,
                ServiceTypeId = null,
                SparePartId = 4,
                ItemType = "SparePart",
                Description = "Cabin air-filter replacement",
                Quantity = 1,
                UnitPrice = 18m,
                LaborHours = null,
                Subtotal = 18m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 8,
                ServiceOrderId = 5,
                ServiceTypeId = 4,
                SparePartId = null,
                ItemType = "Service",
                Description = "Battery testing and installation",
                Quantity = 1,
                UnitPrice = 20m,
                LaborHours = 0.5m,
                Subtotal = 20m
            },
            new ServiceOrderItemModel
            {
                ServiceOrderItemId = 9,
                ServiceOrderId = 5,
                ServiceTypeId = null,
                SparePartId = 3,
                ItemType = "SparePart",
                Description = "12-volt replacement battery",
                Quantity = 1,
                UnitPrice = 85m,
                LaborHours = null,
                Subtotal = 85m
            });
    }

    private static void SeedInvoices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceModel>().HasData(
            new InvoiceModel
            {
                InvoiceId = 1,
                ServiceOrderId = 1,
                InvoiceNumber = "INV-2026-0001",
                IssueDate = new DateTime(2026, 8, 1, 10, 0, 0),
                DueDate = new DateTime(2026, 8, 1),
                Subtotal = 36m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                TotalAmount = 36m,
                Status = "Paid",
                Notes = "Paid in full"
            },
            new InvoiceModel
            {
                InvoiceId = 2,
                ServiceOrderId = 2,
                InvoiceNumber = "INV-2026-0002",
                IssueDate = new DateTime(2026, 8, 13, 12, 0, 0),
                DueDate = new DateTime(2026, 8, 20),
                Subtotal = 74m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                TotalAmount = 74m,
                Status = "Unpaid",
                Notes = "Partial payment received"
            },
            new InvoiceModel
            {
                InvoiceId = 3,
                ServiceOrderId = 3,
                InvoiceNumber = "INV-2026-0003",
                IssueDate = new DateTime(2026, 8, 15, 10, 0, 0),
                DueDate = new DateTime(2026, 8, 22),
                Subtotal = 45m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                TotalAmount = 45m,
                Status = "Unpaid",
                Notes = "Payment is due after service completion"
            },
            new InvoiceModel
            {
                InvoiceId = 4,
                ServiceOrderId = 4,
                InvoiceNumber = "INV-2026-0004",
                IssueDate = new DateTime(2026, 8, 20, 11, 0, 0),
                DueDate = new DateTime(2026, 8, 27),
                Subtotal = 78m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                TotalAmount = 78m,
                Status = "Unpaid",
                Notes = "Pending payment recorded"
            },
            new InvoiceModel
            {
                InvoiceId = 5,
                ServiceOrderId = 5,
                InvoiceNumber = "INV-2026-0005",
                IssueDate = new DateTime(2026, 8, 12, 9, 15, 0),
                DueDate = new DateTime(2026, 8, 12),
                Subtotal = 105m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                TotalAmount = 105m,
                Status = "Paid",
                Notes = "Paid in full"
            });
    }

    private static void SeedPayments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentModel>().HasData(
            new PaymentModel
            {
                PaymentId = 1,
                InvoiceId = 1,
                Amount = 36m,
                PaymentDate = new DateTime(2026, 8, 1, 10, 15, 0),
                PaymentMethod = "Card",
                TransactionReference = "PAY-2026-0001",
                Status = "Completed",
                Notes = "Full card payment"
            },
            new PaymentModel
            {
                PaymentId = 2,
                InvoiceId = 2,
                Amount = 30m,
                PaymentDate = new DateTime(2026, 8, 13, 13, 0, 0),
                PaymentMethod = "Cash",
                TransactionReference = "PAY-2026-0002",
                Status = "Completed",
                Notes = "Partial cash payment"
            },
            new PaymentModel
            {
                PaymentId = 3,
                InvoiceId = 5,
                Amount = 105m,
                PaymentDate = new DateTime(2026, 8, 12, 9, 30, 0),
                PaymentMethod = "Card",
                TransactionReference = "PAY-2026-0003",
                Status = "Completed",
                Notes = "Full card payment"
            },
            new PaymentModel
            {
                PaymentId = 4,
                InvoiceId = 4,
                Amount = 20m,
                PaymentDate = new DateTime(2026, 8, 20, 11, 30, 0),
                PaymentMethod = "Online",
                TransactionReference = "PAY-2026-0004",
                Status = "Pending",
                Notes = "Online payment awaiting confirmation"
            });
    }
}
