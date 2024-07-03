using Microsoft.EntityFrameworkCore;
using RATSP.Common.Models;

namespace RATSP.API;

public class AppDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}