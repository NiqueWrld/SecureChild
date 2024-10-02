using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureChild.Areas.Identity.Data;
using SecureChild.Models;
using System.Security;

namespace SecureChild.Data;

public class SecureChildContext : IdentityDbContext<SecureChildUser>
{
    public SecureChildContext()
    {
    }

    public SecureChildContext(DbContextOptions<SecureChildContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<SecurityStaff> SecurityStaff { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Exit> Exits { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<AlertSettings> AlertSettings { get; set; }
    public object Logs { get; internal set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<SecureChild.Models.SchoolTime> SchoolTime { get; set; } = default!;
}
