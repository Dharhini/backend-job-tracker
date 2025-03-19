using Microsoft.EntityFrameworkCore;
using server.Models;
using Microsoft.Extensions.Configuration;

namespace server.Data {
    public class DatabaseContext : DbContext {
        private readonly IConfiguration _configuration;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) 
            : base(options) {
            _configuration = configuration;
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    if (!optionsBuilder.IsConfigured) {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString); // ✅ Now using SQL Server
    }
}
    }
}