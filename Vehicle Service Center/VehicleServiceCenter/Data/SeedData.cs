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
                PhoneNumber = "91234567",
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
                PhoneNumber = "92345678",
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
                PhoneNumber = "93456789",
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
                PhoneNumber = "94567890",
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
                PhoneNumber = "95678901",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 5)
            }
        );
    }
}

