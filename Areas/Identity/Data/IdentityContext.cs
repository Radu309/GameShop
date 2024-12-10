// using GameShop.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
//
// namespace GameShop.Areas.Identity.Data;
//
// public class IdentityContext: IdentityDbContext<AppUser>
// {
//     public IdentityContext(DbContextOptions<IdentityContext> options)
//         : base(options)
//     {
//     }
//     
//     protected override void OnModelCreating(ModelBuilder builder)
//     {
//         base.OnModelCreating(builder);
//         builder.Entity<AppUser>().ToTable("AppUsers");
//         builder.Entity<IdentityRole>().ToTable("Roles");
//         builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
//         builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
//         builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
//         builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
//         builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
//     }
// }
