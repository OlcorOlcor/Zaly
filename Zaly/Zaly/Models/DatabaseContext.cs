using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace Zaly.Models {
	public class DatabaseContext : DbContext {
		public DbSet<User> User { get; set; }
		public DbSet<Admin> Admin { get; set; }
		public DbSet<Team> Team { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseMySQL("Server=a055um.forpsi.com;Database=f170685;Uid=f170685;Pwd=Rnp2CxTP");
		}
	}
}
