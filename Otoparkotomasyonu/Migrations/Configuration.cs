namespace Otoparkotomasyonu.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Otoparkotomasyonu.Entity.OtoparkOtomasyonu>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Otoparkotomasyonu.Entity.OtoparkOtomasyonu";
        }

        protected override void Seed(Otoparkotomasyonu.Entity.OtoparkOtomasyonu context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
