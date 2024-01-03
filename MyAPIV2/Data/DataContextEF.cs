using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAPIV2.Models;

namespace MyAPIV2.Data
{
  public class DataContextEF : DbContext
  {
    private readonly IConfiguration _config;

    public DataContextEF(IConfiguration config){
      _config = config;
    }

    public virtual DbSet<Article> Articles {get; set;}
    public virtual DbSet<User> Users {get; set;}
    public virtual DbSet<UserSalary> UserSalary {get; set;}
    public virtual DbSet<UserJobInfo> UserJobInfo {get; set;}

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

      modelBuilder.Entity<Article>()
        .ToTable("Articles", "TutorialAppSchema")
        .HasKey(c => c.ArticleId);
      
      modelBuilder.Entity<User>()
        .ToTable("Users", "TutorialAppSchema")
        .HasKey(c => c.UserId);

      modelBuilder.Entity<UserSalary>()
        .HasKey(c => c.UserId);

      modelBuilder.Entity<UserJobInfo>()
        .HasKey(c => c.UserId);
    }
    
  }
}