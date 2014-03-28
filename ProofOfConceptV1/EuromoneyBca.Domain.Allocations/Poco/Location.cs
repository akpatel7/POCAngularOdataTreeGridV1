using System.Collections.Generic;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class Location
    {
        public int Id { get; set; }
        public string location_uri { get; set; }
        public string location_code { get; set; }
        public string location_label { get; set; }
        
    }
}
