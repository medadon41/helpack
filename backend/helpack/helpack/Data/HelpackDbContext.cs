using helpack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace helpack.Data;

public class HelpackDbContext : DbContext
{
    public HelpackDbContext(DbContextOptions<HelpackDbContext> context)
       : base(context)
    {
        
    }
    
    public DbSet<Donation> Donations { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }
    
    public DbSet<HelpackUser> Users { get; set; }
}