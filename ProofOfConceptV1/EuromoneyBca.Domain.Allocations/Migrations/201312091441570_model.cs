namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Allocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentAllocation = c.Single(nullable: false),
                        PreviousAllocation = c.Single(nullable: false),
                        CurrentBenchmark = c.Single(nullable: false),
                        CurrentBenchmarkMin = c.Single(nullable: false),
                        CurrentBenchmarkMax = c.Single(nullable: false),
                        PreviousBenchmark = c.Single(nullable: false),
                        PreviousBenchmarkMin = c.Single(nullable: false),
                        PreviousBenchmarkMax = c.Single(nullable: false),
                        Comments = c.String(),
                        AbsolutePerformance = c.Single(nullable: false),
                        LastUpdated = c.DateTime(),
                        AbsolutePerformanceCurrency_Id = c.Int(),
                        AbsolutePerformanceMeasure_Id = c.Int(),
                        Instrument_Id = c.Int(),
                        ParentAllocation_Id = c.Int(),
                        Portfolio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currency", t => t.AbsolutePerformanceCurrency_Id)
                .ForeignKey("dbo.Measure_Type", t => t.AbsolutePerformanceMeasure_Id)
                .ForeignKey("dbo.Tradable_Thing", t => t.Instrument_Id)
                .ForeignKey("dbo.Allocation", t => t.ParentAllocation_Id)
                .ForeignKey("dbo.Portfolio", t => t.Portfolio_Id)
                .Index(t => t.AbsolutePerformanceCurrency_Id)
                .Index(t => t.AbsolutePerformanceMeasure_Id)
                .Index(t => t.Instrument_Id)
                .Index(t => t.ParentAllocation_Id)
                .Index(t => t.Portfolio_Id);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        currency_uri = c.String(),
                        currency_code = c.String(),
                        currency_symbol = c.String(),
                        currency_label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Measure_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tradable_Thing",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tradable_thing_uri = c.String(),
                        tradable_thing_class_id23 = c.Int(),
                        location_id2 = c.Int(),
                        tradable_thing_code = c.String(),
                        tradable_thing_label = c.String(),
                        Location_Id = c.Int(),
                        Tradable_Thing_Class_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.Location_Id)
                .ForeignKey("dbo.Tradable_Thing_Class", t => t.Tradable_Thing_Class_Id)
                .Index(t => t.Location_Id)
                .Index(t => t.Tradable_Thing_Class_Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        location_uri = c.String(),
                        location_code = c.String(),
                        location_label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tradable_Thing_Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tradable_thing_class_uri = c.String(),
                        tradable_thing_class_label = c.String(),
                        tradable_thing_class_editorial_label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Portfolio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastUpdated = c.DateTime(),
                        Comments = c.String(),
                        PerformanceModel = c.String(),
                        Benchmark_Id = c.Int(),
                        Duration_Id = c.Int(),
                        Service_Id = c.Int(),
                        Status_Id = c.Int(),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benchmark", t => t.Benchmark_Id)
                .ForeignKey("dbo.DurationType", t => t.Duration_Id)
                .ForeignKey("dbo.Service", t => t.Service_Id)
                .ForeignKey("dbo.Status", t => t.Status_Id)
                .ForeignKey("dbo.PortfolioType", t => t.Type_Id)
                .Index(t => t.Benchmark_Id)
                .Index(t => t.Duration_Id)
                .Index(t => t.Service_Id)
                .Index(t => t.Status_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Benchmark",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        benchmark_uri = c.String(),
                        benchmark_code = c.String(),
                        benchmark_label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DurationType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Service",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        service_uri = c.String(),
                        service_code = c.String(),
                        service_label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PortfolioType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeightingDescription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Allocation", "Portfolio_Id", "dbo.Portfolio");
            DropForeignKey("dbo.Portfolio", "Type_Id", "dbo.PortfolioType");
            DropForeignKey("dbo.Portfolio", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Portfolio", "Service_Id", "dbo.Service");
            DropForeignKey("dbo.Portfolio", "Duration_Id", "dbo.DurationType");
            DropForeignKey("dbo.Portfolio", "Benchmark_Id", "dbo.Benchmark");
            DropForeignKey("dbo.Allocation", "ParentAllocation_Id", "dbo.Allocation");
            DropForeignKey("dbo.Allocation", "Instrument_Id", "dbo.Tradable_Thing");
            DropForeignKey("dbo.Tradable_Thing", "Tradable_Thing_Class_Id", "dbo.Tradable_Thing_Class");
            DropForeignKey("dbo.Tradable_Thing", "Location_Id", "dbo.Location");
            DropForeignKey("dbo.Allocation", "AbsolutePerformanceMeasure_Id", "dbo.Measure_Type");
            DropForeignKey("dbo.Allocation", "AbsolutePerformanceCurrency_Id", "dbo.Currency");
            DropIndex("dbo.Allocation", new[] { "Portfolio_Id" });
            DropIndex("dbo.Portfolio", new[] { "Type_Id" });
            DropIndex("dbo.Portfolio", new[] { "Status_Id" });
            DropIndex("dbo.Portfolio", new[] { "Service_Id" });
            DropIndex("dbo.Portfolio", new[] { "Duration_Id" });
            DropIndex("dbo.Portfolio", new[] { "Benchmark_Id" });
            DropIndex("dbo.Allocation", new[] { "ParentAllocation_Id" });
            DropIndex("dbo.Allocation", new[] { "Instrument_Id" });
            DropIndex("dbo.Tradable_Thing", new[] { "Tradable_Thing_Class_Id" });
            DropIndex("dbo.Tradable_Thing", new[] { "Location_Id" });
            DropIndex("dbo.Allocation", new[] { "AbsolutePerformanceMeasure_Id" });
            DropIndex("dbo.Allocation", new[] { "AbsolutePerformanceCurrency_Id" });
            DropTable("dbo.WeightingDescription");
            DropTable("dbo.PortfolioType");
            DropTable("dbo.Status");
            DropTable("dbo.Service");
            DropTable("dbo.DurationType");
            DropTable("dbo.Benchmark");
            DropTable("dbo.Portfolio");
            DropTable("dbo.Tradable_Thing_Class");
            DropTable("dbo.Location");
            DropTable("dbo.Tradable_Thing");
            DropTable("dbo.Measure_Type");
            DropTable("dbo.Currency");
            DropTable("dbo.Allocation");
        }
    }
}
