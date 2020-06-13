using System.Security.Claims;
using System.Threading.Tasks;
using Core.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Core.EF.DataCtx
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class DataCtx :  IdentityDbContext<ApplicationUser>
    {
       
        public DataCtx()
            : base("name=DataCtx", throwIfV1Schema: false)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<User> Users { get; set; }

        public static DataCtx Create()
        {
            return new DataCtx();
        }
    }
}