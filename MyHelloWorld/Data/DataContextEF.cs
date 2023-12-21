using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyHelloWorld.Models;

namespace MyHelloWorld.Data
{
  public class DataContextEF : DbContext
  {

    public DbSet<Computer>? Computer { get; set; }
    private IConfiguration _config;

    public DataContextEF(IConfiguration config){
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.EnableRetryOnFailure());
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("TutorialAppSchema");
      modelBuilder.Entity<Computer>()
                // .HasNoKey()
                .HasKey(c => c.ComputerId);
                //.ToTable("Computer","TutorialAppSchema");

    }
  }
}