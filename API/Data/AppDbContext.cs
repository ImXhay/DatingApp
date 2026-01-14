using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
   public required DbSet<AppUser> Users { get; set; }
   public DbSet<Members> Members { get; set; }
   public DbSet<Photo> Photos { get; set; }



}