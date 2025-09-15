using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClaimsMVC.Data.Models
{
    public class ClaimsContextDB : IdentityDbContext<User, ApplicationRole, string>
    {
  


        public ClaimsContextDB(DbContextOptions<ClaimsContextDB> options)
            : base(options)
        {
       ;
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimItem> ClaimItems { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
  


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add precision to decimal properties to resolve warnings
            builder.Entity<Claim>(entity =>
            {
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.ApprovedAmount).HasPrecision(18, 2);
            });

            builder.Entity<ClaimItem>(entity =>
            {
                entity.Property(e => e.Cost).HasPrecision(18, 2);
            });

            builder.Entity<Designation>(entity =>
            {
                entity.Property(e => e.ClaimLimit).HasPrecision(18, 2);
            });

            // Map Identity tables to custom names
            builder.Entity<User>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });
        }
    }
}
