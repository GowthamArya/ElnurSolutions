using ElnurSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace ElnurSolutions.DataBase
{
	public class ElnurDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public ElnurDbContext(DbContextOptions<ElnurDbContext> options)
			  : base(options)
		{
		}
	}
}