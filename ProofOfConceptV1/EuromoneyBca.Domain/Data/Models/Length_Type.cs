//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EuromoneyBca.Domain.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Length_Type
    {
        public Length_Type()
        {
            this.Trades = new HashSet<Trade>();
        }
    
        public int length_type_id { get; set; }
        public string length_type_label { get; set; }
    
        public virtual ICollection<Trade> Trades { get; set; }
    }
}