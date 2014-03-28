using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using EuromoneyBca.Domain.Allocations.Poco;
using Microsoft.Data.Edm;

namespace EuromoneyArchitecturePoc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapODataRoute("odata", "odata", GetImplicitEdm());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();
        }
        private static IEdmModel GetImplicitEdm()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            // Here we need all the navigable entites for odata
            builder.EntitySet<History>("Histories");
       
            builder.EntitySet<Portfolio>("Portfolios");
            builder.EntitySet<Allocation>("Allocations");
            builder.EntitySet<PortfolioSummary>("PortfolioSummaries");
            builder.EntitySet<AllocationSummary>("AllocationSummaries");

            builder.EntitySet<Service>("Services");
            builder.EntitySet<PortfolioType>("PortfolioTypes");
            builder.EntitySet<Benchmark>("Benchmarks");
            builder.EntitySet<DurationType>("DurationTypes");
            builder.EntitySet<Status>("Statuses");

            builder.EntitySet<Tradable_Thing>("Tradable_Things");
            builder.EntitySet<Measure_Type>("Measure_Types");
            builder.EntitySet<Currency>("Currencies");
            builder.EntitySet<Location>("Locations");
            builder.EntitySet<Tradable_Thing_Class>("Tradable_Thing_Classes");

            return builder.GetEdmModel(); // magic happens here
        }
    }
}
