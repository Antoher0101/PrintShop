using Microsoft.EntityFrameworkCore;
using PrintShop.models;

namespace PrintShop.core
{
    public partial class ApplicationContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountInfo> DiscountsInfo { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceInfo> ServiceInfos { get; set; }
        public DbSet<TotalService> TotalServices { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies().UseSqlite("Data Source=./db/db.db;");

            
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
