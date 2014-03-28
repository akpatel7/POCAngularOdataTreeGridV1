using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyBca.Domain.Allocations.Data
{
    public class BcaAllocationsCodeFirstContext : DbContext
    {
        public BcaAllocationsCodeFirstContext()
            : base("name=BCATradeAllocationEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // These POCOs are for views, so we don't want code first to generate and DB assets.
            modelBuilder.Ignore<AllocationSummary>();
            modelBuilder.Ignore<PortfolioSummary>();
        }


        public DbSet<AllocationSummary> AllocationSummaries { get; set; }
        public DbSet<PortfolioSummary> PortfolioSummaries { get; set; }

        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<Benchmark> Benchmarks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<DurationType> DurationTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Measure_Type> Measure_Type { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioType> PortfolioTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Tradable_Thing> Tradable_Thing { get; set; }
        public DbSet<Tradable_Thing_Class> Tradable_Thing_Class { get; set; }
        public DbSet<WeightingDescription> WeightingDescriptions { get; set; }
        public DbSet<History> History { get; set; }
    }
}