using System;

namespace EuromoneyBca.Domain.Allocations.Poco
{
 

    public class Tradable_Thing
    {
        
        public int Id { get; set; }
        public string tradable_thing_uri { get; set; }
        public string tradable_thing_code { get; set; }
        public string tradable_thing_label { get; set; }

        public virtual Location Location { get; set; }
        public virtual Tradable_Thing_Class Tradable_Thing_Class { get; set; }
   
    }
}