using System;
using System.Collections.Generic;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class Allocation
    {
        public Allocation()
        {
            History = new HashSet<History>();
        }

        public int Id { get; set; }

        public Allocation ParentAllocation { get; set; }
        public Portfolio Portfolio { get; set; }

        public virtual Tradable_Thing Instrument { get; set; }

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
        public virtual Measure_Type AbsolutePerformanceMeasure { get; set; }
        public Currency AbsolutePerformanceCurrency { get; set; }

        public DateTime? LastUpdated { get; set; }

        public virtual ICollection<History> History { get; set; }

      
    }
}
