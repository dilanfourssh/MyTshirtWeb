namespace Tshirt.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tshirt.Context.tshirtContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Tshirt.Context.tshirtContext";
        }

        protected override void Seed(Tshirt.Context.tshirtContext context)
        {
           
        }
    }
}
