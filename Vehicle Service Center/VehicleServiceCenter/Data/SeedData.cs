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
        
        
        
    }
}

