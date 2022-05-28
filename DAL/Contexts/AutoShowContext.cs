using Microsoft.EntityFrameworkCore;
using DAL.Models;

#nullable disable

namespace DAL.Contexts
{
    public partial class AutoShowContext : DbContext
    {
        public AutoShowContext() { }

        public AutoShowContext(DbContextOptions<AutoShowContext> options)
            : base(options) { }

        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Drive> Drives { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
