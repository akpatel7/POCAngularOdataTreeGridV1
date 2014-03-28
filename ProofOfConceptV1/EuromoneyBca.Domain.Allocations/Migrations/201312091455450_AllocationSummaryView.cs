namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllocationSummaryView : DbMigration
    {
        public override void Up()
        {
            var sql = @"
CREATE VIEW [dbo].[AllocationSummary]
AS
SELECT dbo.Allocation.Id, dbo.Allocation.CurrentAllocation, dbo.Allocation.PreviousAllocation, dbo.Allocation.CurrentBenchmark, dbo.Allocation.CurrentBenchmarkMin, 
                  dbo.Allocation.CurrentBenchmarkMax, dbo.Allocation.PreviousBenchmark, dbo.Allocation.PreviousBenchmarkMin, dbo.Allocation.PreviousBenchmarkMax, 
                  dbo.Allocation.Comments, dbo.Allocation.AbsolutePerformance, dbo.Allocation.LastUpdated, dbo.Allocation.ParentAllocation_Id, dbo.Allocation.Portfolio_Id, 
                  dbo.Measure_Type.Description AS AbsolutePerformanceMeasure, dbo.Currency.currency_label AS Currency, 
                  dbo.Tradable_Thing.tradable_thing_label AS Instrument
FROM     dbo.Allocation LEFT OUTER JOIN
                  dbo.Tradable_Thing ON dbo.Allocation.Instrument_Id = dbo.Tradable_Thing.Id LEFT OUTER JOIN
                  dbo.Measure_Type ON dbo.Allocation.AbsolutePerformanceMeasure_Id = dbo.Measure_Type.Id LEFT OUTER JOIN
                  dbo.Currency ON dbo.Allocation.AbsolutePerformanceCurrency_Id = dbo.Currency.Id
";

            Sql(sql);

        }
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[AllocationSummary]");
        }
    }
}
