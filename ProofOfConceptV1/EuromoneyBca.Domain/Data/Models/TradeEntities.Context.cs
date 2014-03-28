﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BCATradeEntities1 : DbContext
    {
        public BCATradeEntities1()
            : base("name=BCATradeEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Benchmark> Benchmarks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Hedge_Type> Hedge_Type { get; set; }
        public DbSet<Instruction_Type> Instruction_Type { get; set; }
        public DbSet<Length_Type> Length_Type { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Measure_Type> Measure_Type { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Related_Trade> Related_Trade { get; set; }
        public DbSet<Relativity> Relativities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Structure_Type> Structure_Type { get; set; }
        public DbSet<Track_Record> Track_Record { get; set; }
        public DbSet<Track_Record_Type> Track_Record_Type { get; set; }
        public DbSet<Tradable_Thing> Tradable_Thing { get; set; }
        public DbSet<Tradable_Thing_Class> Tradable_Thing_Class { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Trade_Comment> Trade_Comment { get; set; }
        public DbSet<Trade_Instruction> Trade_Instruction { get; set; }
        public DbSet<Trade_Line> Trade_Line { get; set; }
        public DbSet<Trade_Line_Group> Trade_Line_Group { get; set; }
        public DbSet<Trade_Line_Group_Type> Trade_Line_Group_Type { get; set; }
        public DbSet<Trade_Performance> Trade_Performance { get; set; }
    }
}
