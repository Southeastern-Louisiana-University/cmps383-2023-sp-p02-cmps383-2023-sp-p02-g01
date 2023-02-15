using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Features.Authorization;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.Users;
using static System.Net.Mime.MediaTypeNames;

namespace SP23.P02.Web.Data;

public class DataContext : IdentityDbContext
    <User, Role, int, 
    IdentityUserClaim<int>, 
    UserRole, 
    IdentityUserLogin<int>, 
    IdentityRoleClaim<int>, 
    IdentityUserToken<int>>
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    public DataContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        var userRoleBuilder = modelBuilder.Entity<UserRole>();
        
        userRoleBuilder.HasKey(x => new { x.UserId,x.RoleId });

        userRoleBuilder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId);

        userRoleBuilder.HasOne(x => x.User)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserId);

    }
}