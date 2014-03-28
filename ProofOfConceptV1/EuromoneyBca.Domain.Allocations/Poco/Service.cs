using System.Collections.Generic;

namespace EuromoneyBca.Domain.Allocations.Poco
{
    public class Service
    {
        public int Id { get; set; }
        public string service_uri { get; set; }
        public string service_code { get; set; }
        public string service_label { get; set; }
    }
}