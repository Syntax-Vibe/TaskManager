
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Domain;

namespace TaskManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        b.Entity<User>().Property(p=>p.UserName).HasMaxLength(50).IsRequired();

        b.Entity<TaskItem>().Property(t=>t.Title).HasMaxLength(120).IsRequired();
        b.Entity<TaskItem>().HasOne(t=>t.User).WithMany(u=>u.Tasks).HasForeignKey(t=>t.UserId);
    }
}
