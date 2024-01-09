using Microsoft.EntityFrameworkCore;
using Project1.Model;

namespace Project1.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

public DbSet<Project1.Model.Plant> Plant { get; set; } = default!;
public DbSet<UserData> UserData { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.UserData)
            .WithOne(ud => ud.ApplicationUser)
            .HasForeignKey<UserData>(ud => ud.UserID);
    }
}
