using System;
using System.Collections.Generic;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class Portfolio
    {
        public Portfolio()
        {
            History = new HashSet<History>();
        }

        public int Id { get; set; }

        public virtual Service Service { get; set; }

        public virtual PortfolioType Type { get; set; }
        
        public string Name { get; set; }

        public virtual Benchmark Benchmark { get; set; }

        public virtual DurationType Duration { get; set; }
        
        public DateTime? LastUpdated { get; set; }

        public string Comments { get; set; }

        public string PerformanceModel { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<History> History { get; set; }


    }
}
