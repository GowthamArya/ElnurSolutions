using Microsoft.EntityFrameworkCore;

namespace ElnurSolutions.DataBase
{
	public class ElnurDbContext : DbContext
	{
		public ElnurDbContext(DbContextOptions<ElnurDbContext> options)
			  : base(options)
		{
		}
	}
}