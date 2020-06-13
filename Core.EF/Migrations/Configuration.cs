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
           
        }
    }
}
