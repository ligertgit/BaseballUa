using BaseballUa.Models;
using Microsoft.EntityFrameworkCore;

namespace BaseballUa.Data
{
    public class BaseballUaDbContext : DbContext
    {
		public BaseballUaDbContext(DbContextOptions<BaseballUaDbContext> options) : base(options)
		{ 
		
		}

		public DbSet<Category> Category { get; set; }
	}
}
