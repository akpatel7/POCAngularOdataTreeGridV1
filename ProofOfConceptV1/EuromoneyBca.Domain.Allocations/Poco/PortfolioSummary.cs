using System;
using System.Collections.Generic;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public  class PortfolioSummary
    {
        public PortfolioSummary()
        {
            AllocationSummaries = new HashSet<AllocationSummary>();
        }
        public string Service { get; set; }
        public string Type { get; set; }
        public string benchmark_label { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string Comments { get; set; }
        public string PerformanceModel { get; set; }

        public virtual ICollection<AllocationSummary> AllocationSummaries { get; set; }
    }
}
