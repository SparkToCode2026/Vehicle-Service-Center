using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;
using VehicleServiceCenter.Data;

namespace VehicleServiceCenter
{
    
        public class ProjectContext : DbContext
        {
            public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
            {
            }
            public DbSet<UserModel> Users { get; set; }
            public DbSet<CustomerProfileModel> CustomerProfiles { get; set; }
            public DbSet<MechanicProfileModel> MechanicProfiles { get; set; }
            public DbSet<VehicleModel> Vehicles { get; set; } = null!;

            public DbSet<ServiceTypeModel> ServiceTypes { get; set; } = null!;
            
            public DbSet<AppointmentModel> Appointments { get; set; }
            public DbSet<SparePartModel> SpareParts { get; set; }
            
            public DbSet<ServiceOrderModel> ServiceOrders { get; set; }
            public DbSet<ServiceOrderItemModel> ServiceOrderItems { get; set; }
            
            public DbSet<InvoiceModel> Invoices { get; set; }
            public DbSet<PaymentModel> Payments { get; set; }
            public DbSet<BranchModel> Branches { get; set; }
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                SeedData.Seed(modelBuilder);
            }

            
        }
    
}




