using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyBca.Domain.Allocations.Data
{
    public class BcaAllocationsDataService : DbContext, IBcaAllocationsDataService
    {
        public BcaAllocationsDataService()
            : base("name=BCATradeAllocationEntities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException(); 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // We need this to support this:
            // odata/PortfolioSummaries?$expand=AllocationSummaries
            modelBuilder.Entity<AllocationSummary>().HasRequired(a => a.PortfolioSummary)
            .WithMany(p => p.AllocationSummaries)
            .HasForeignKey(p => p.Portfolio_Id);
        
        }

        // We only need the Root level entites exposed through Odata
        public DbSet<AllocationSummary> AllocationSummaries { get; set; }
        public DbSet<PortfolioSummary> PortfolioSummaries { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<History> Histories { get; set; }
       
        //public DbSet<Benchmark> Benchmarks { get; set; }
        //public DbSet<Currency> Currencies { get; set; }
        //public DbSet<DurationType> DurationTypes { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<Measure_Type> Measure_Type { get; set; }
       
        //public DbSet<PortfolioType> PortfolioTypes { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<Status> Status { get; set; }
        //public DbSet<Tradable_Thing> Tradable_Thing { get; set; }
        //public DbSet<Tradable_Thing_Class> Tradable_Thing_Class { get; set; }
        //public DbSet<WeightingDescription> WeightingDescriptions { get; set; }

        IQueryable<T> IBcaAllocationsDataService.Query<T>()
        {
            return Set<T>();
        }

        void IBcaAllocationsDataService.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IBcaAllocationsDataService.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void IBcaAllocationsDataService.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void IBcaAllocationsDataService.SaveChanges()
        {
            SaveChanges();
        }
    }
}
