using Core.Data.Models;
using Core.Data.Types;

namespace Core.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Core.EF.DataCtx.DataCtx>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Core.EF.DataCtx.DataCtx db)
        {
            //db.Users.RemoveRange(db.Users.ToList());
            db.Likes.RemoveRange(db.Likes.ToList());
            db.Posts.RemoveRange(db.Posts.ToList());
            db.Comments.RemoveRange(db.Comments.ToList());
            db.ErrorLogs.RemoveRange(db.ErrorLogs.ToList());

            //var admin = db.Users.Add(new User
            //{
            //    Id = 1,
            //    Name = "Admin",
            //    CreateDate = DateTime.Now,
            //    Gender = Gender.Male,
            //    IsActive = true,
            //    SetNewPassword = true,
            //    Phone = "0965438114",
            //    Role = Role.Admin,
            //    Serial = Guid.NewGuid().ToString(),
            //    Email = "admin@sudasource.com"
            //});
        }
    }
}
