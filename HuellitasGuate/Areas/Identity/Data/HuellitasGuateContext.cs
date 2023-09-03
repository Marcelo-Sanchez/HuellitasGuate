using HuellitasGuate.Areas.Identity.Data;
using HuellitasGuate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HuellitasGuate.Data;

public class HuellitasGuateContext : IdentityDbContext<HuellitasGuateUser>
{
    public HuellitasGuateContext(DbContextOptions<HuellitasGuateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
