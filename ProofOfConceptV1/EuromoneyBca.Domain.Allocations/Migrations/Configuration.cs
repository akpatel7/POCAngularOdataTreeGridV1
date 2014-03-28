namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<EuromoneyBca.Domain.Allocations.Data.BcaAllocationsCodeFirstContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EuromoneyBca.Domain.Allocations.Data.BcaAllocationsCodeFirstContext context)
        {
            EuromoneyBca.Domain.Allocations.Data.Seed.SeedData(context);
        }
    }
}
