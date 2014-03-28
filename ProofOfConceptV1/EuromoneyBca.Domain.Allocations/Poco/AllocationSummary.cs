using System;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class AllocationSummary
    {
        public int Id { get; set; }
        public float CurrentAllocation { get; set; }
        public float PreviousAllocation { get; set; }
        public float CurrentBenchmark { get; set; }
        public float CurrentBenchmarkMin { get; set; }
        public float CurrentBenchmarkMax { get; set; }
        public float PreviousBenchmark { get; set; }
        public float PreviousBenchmarkMin { get; set; }
        public float PreviousBenchmarkMax { get; set; }
        public string Comments { get; set; }
        public float AbsolutePerformance { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<int> ParentAllocation_Id { get; set; }
        public Nullable<int> Portfolio_Id { get; set; }
        public string AbsolutePerformanceMeasure { get; set; }
        public string Currency { get; set; }
        public string Instrument { get; set; }

        public virtual PortfolioSummary PortfolioSummary { get; set; }
    }
}
