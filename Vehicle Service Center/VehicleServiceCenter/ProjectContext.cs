using Microsoft.EntityFrameworkCore;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter
{
    public class ProjectContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CustomerProfileModel> CustomerProfiles { get; set; }
        public DbSet<MechanicProfileModel> MechanicProfiles { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }
    }
}
