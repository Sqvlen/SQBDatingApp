using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public class DataBaseContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, 
        IdentityUserLogin<int>, IdentityRoleClaim<int>, 
        IdentityUserToken<int>>
    {
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public DataBaseContext(DbContextOptions options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(user => user.UserRoles)
                .WithOne(user => user.User)
                .HasForeignKey(user => user.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(user => user.UserRoles)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId)
                .IsRequired();

            builder.Entity<UserLike>().HasKey(key => new { key.SourceUserId, key.TargetUserId });

            builder.Entity<UserLike>().HasOne(source => source.SourceUser).WithMany(liked => liked.LikedUsers)
                .HasForeignKey(source => source.SourceUserId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>().HasOne(target => target.TargetUser).WithMany(liked => liked.LikedByUsers)
                .HasForeignKey(target => target.TargetUserId).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Message>()
                .HasOne(message => message.Recipient)
                .WithMany(user => user.MessagesReceived).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(message => message.Sender)
                .WithMany(user => user.MessagesSent).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
