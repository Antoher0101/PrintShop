using Microsoft.EntityFrameworkCore;


namespace PrintShop.core
{
    public class ApplicationContext : DbContext
    {
        public string connectionString;
        public ApplicationContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }

    }
}
