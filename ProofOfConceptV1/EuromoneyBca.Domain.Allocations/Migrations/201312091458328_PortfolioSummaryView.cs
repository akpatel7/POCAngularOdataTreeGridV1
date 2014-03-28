namespace EuromoneyBca.Domain.Allocations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PortfolioSummaryView : DbMigration
    {
        public override void Up()
        {
            var sql = @"
CREATE VIEW [dbo].[PortfolioSummary]
AS
SELECT dbo.Service.service_label AS Service, dbo.PortfolioType.Description AS Type, dbo.Benchmark.benchmark_label, dbo.Status.Description AS Status, 
                  dbo.DurationType.Description AS Duration, dbo.Portfolio.Id, dbo.Portfolio.Name, dbo.Portfolio.LastUpdated, dbo.Portfolio.Comments, 
                  dbo.Portfolio.PerformanceModel
FROM     dbo.Portfolio LEFT OUTER JOIN
                  dbo.Service ON dbo.Portfolio.Service_Id = dbo.Service.Id LEFT OUTER JOIN
                  dbo.PortfolioType ON dbo.Portfolio.Type_Id = dbo.PortfolioType.Id LEFT OUTER JOIN
                  dbo.Benchmark ON dbo.Portfolio.Benchmark_Id = dbo.Benchmark.Id LEFT OUTER JOIN
                  dbo.DurationType ON dbo.Portfolio.Duration_Id = dbo.DurationType.Id LEFT OUTER JOIN
                  dbo.Status ON dbo.Portfolio.Status_Id = dbo.Status.Id
";

            Sql(sql);

        }

        public override void Down()
        {
            Sql("DROP VIEW [dbo].[PortfolioSummary]");
        }
    }
}
