using System;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class History
    {
        public int Id { get; set; }
        public float CurrentAllocation { get; set; }
        public float PreviousAllocation { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Comment { get; set; }
        public string CommentForBren { get; set; }
    }
}
