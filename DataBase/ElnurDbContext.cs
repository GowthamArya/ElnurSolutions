﻿using ElnurSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace ElnurSolutions.DataBase
{
	public class ElnurDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<TeamCategory> TeamCategories { get; set; }
		public DbSet<TeamMember> TeamMembers { get; set; }
		public DbSet<UploadedFile> UploadedFiles { get; set; }
		public ElnurDbContext(DbContextOptions<ElnurDbContext> options)
			  : base(options)
		{
		}
	}
}