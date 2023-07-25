using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Zaly.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<MultipartAnswer> MultipartAnswer { get; set; }
        public DbSet<UserToQuestion> UserToQuestion  { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
        }
    }
}
