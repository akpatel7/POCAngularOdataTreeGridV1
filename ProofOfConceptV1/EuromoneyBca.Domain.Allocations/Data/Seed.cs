﻿using System;
using System.Collections.Generic;
using System.Linq;
using EuromoneyBca.Domain.Allocations.Poco;

namespace EuromoneyBca.Domain.Allocations.Data
{
    public static class Seed
    {
        public static void SeedData(EuromoneyBca.Domain.Allocations.Data.BcaAllocationsCodeFirstContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE [History]");
            context.Database.ExecuteSqlCommand("DELETE [Allocation]");
            context.Database.ExecuteSqlCommand("DELETE [Portfolio]");

            var portfolioCount = 0;

            var benchmarks = new List<Benchmark>
                {
                    context.Benchmarks.Single(b => b.Id == 1),
                    context.Benchmarks.Single(b => b.Id == 2)
                };

            var durations = new List<DurationType>
                {
                    context.DurationTypes.Single(d => d.Id == 1),
                    context.DurationTypes.Single(d => d.Id == 2),
                    context.DurationTypes.Single(d => d.Id == 3)
                };

            var statuses = new List<Status>();
            for (var i = 1; i <= 4; i++)
            {
                statuses.Add(context.Status.Single(s => s.Id == i));
            }

            var portfolioTypes = new List<PortfolioType>();
            for (var i = 1; i <= 2; i++)
            {
                portfolioTypes.Add(context.PortfolioTypes.Single(s => s.Id == i));
            }

            foreach (var serviceString in new List<string> { "CIS", "GAA", "GFIS", "GIS" })
            {
                var service = context.Services.Single(s => s.service_code == serviceString);

                foreach (var portfolioName in new List<string>
                    {
                        "Model Low Risk Portfolio",
                        "Model Medium Risk Portfolio",
                        "Model High Risk Portfolio",
                        "Model medium risk Portfolio (40-60% Equities)",
                        "Fixed Income Sector Performance"
                    })
                {

                    var portfolio = new Portfolio
                        {
                            Service = service,
                            Name = portfolioName,
                            LastUpdated = DateTime.Now,
                            Comments = "Auto generated",
                            PerformanceModel = "Ahmad ?",
                            Benchmark = benchmarks[portfolioCount % 2],
                            Duration = durations[portfolioCount % 3],
                            Status = statuses[portfolioCount % 4],
                            Type = portfolioTypes[portfolioCount % 2],
                            History = CreateHistories(portfolioName)
                        };

                    context.Portfolios.Add(portfolio);
                    context.SaveChanges();

                    for (var i = 1; i <= 1; i++)
                    {
                        AddAllocationsToPortfolio(context, portfolio, i);
                        context.SaveChanges();
                    }

                    portfolioCount++;

                }
            }
        }

        private static ICollection<History> CreateHistories(string header)
        {
            var histories = new List<History>();

            for (var i = 1; i <= 5; i++)
            {
                histories.Add(
                    new History
                        {
                            Comment = header + " - History comment " + i,
                            CurrentAllocation = i,
                            PreviousAllocation = i - 1,
                            Date = new DateTime(2013, 01, 01).AddDays(i)
                        });
            }
            return histories;
        }

        private static void AddAllocationsToPortfolio(BcaAllocationsCodeFirstContext context, Portfolio portfolio, int iteration)
        {
            var cashAllocation = AddRootAllocation(context, portfolio, "CASH", iteration, 2.5, 0, 5, -1);
            var equitiesAllocation = AddRootAllocation(context, portfolio, "EQUITIES", iteration, 50, 40, 60, -1);
            var bondAllocation = AddRootAllocation(context, portfolio, "BOND", iteration, 47.5, 35, 60, -1);

            AddChildAllocation(context, portfolio, equitiesAllocation, "US", 54, 49, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "EMU", 11, 11, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "EMU", 12, 14, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "JAPAN", 9, 9, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "UK", 9, 9, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "CANADA", 3, 5, -1);
            AddChildAllocation(context, portfolio, equitiesAllocation, "AUSTRALIA", 1, 3, -1);

            AddChildAllocation(context, portfolio, bondAllocation, "GOVERNMENT", 68.5, 73, -1);
            AddChildAllocation(context, portfolio, bondAllocation, "INVESTMENT GRADE", 26.5, 22, -1);
            AddChildAllocation(context, portfolio, bondAllocation, "HIGH-YIELD", 5, 5, -1);

        }

        private static Allocation AddChildAllocation(BcaAllocationsCodeFirstContext context, Portfolio portfolio,
            Allocation parentAllocation, string instrumentName, double currentAllocation,
                                        double currentBenchmark, double absolutePerformance)
        {
            var randomGenerator = new Random(DateTime.Now.Millisecond);
            var childAllocation = new Allocation
                {
                    ParentAllocation = parentAllocation,
                    Portfolio = portfolio,
                    Instrument = context.Tradable_Thing.Single(t => t.tradable_thing_label.ToUpper() == instrumentName),
                    CurrentAllocation = (float)(randomGenerator.NextDouble() * 100),
                    CurrentBenchmark = (float)(randomGenerator.NextDouble() * 100),
                    CurrentBenchmarkMin = (float)(randomGenerator.NextDouble() * 100),
                    CurrentBenchmarkMax = (float)(randomGenerator.NextDouble() * 100),
                    PreviousBenchmark = (float)(randomGenerator.NextDouble() * 100),
                    PreviousBenchmarkMin = (float)(randomGenerator.NextDouble() * 100),
                    PreviousBenchmarkMax = (float)(randomGenerator.NextDouble() * 100),
                    AbsolutePerformance = (float)(randomGenerator.NextDouble() * 100),
                    Comments = string.Format("{0}_{1}", "Comment on", instrumentName ),
                    History = CreateHistories(instrumentName)
                };

            context.Allocations.Add(childAllocation);

            return childAllocation;

        }

        private static Allocation AddRootAllocation(BcaAllocationsCodeFirstContext context,
            Portfolio portfolio, string instrumentName, int iteration, double currentAllocation,
                                        double currentBenchmarkMin, double currentBenchmarkMax, double absolutePerformance)
        {
            var randomGenerator = new Random(DateTime.Now.Millisecond);
            var rootAllocation = new Allocation
            {
                Portfolio = portfolio,
                Instrument = context.Tradable_Thing.Single(t => t.tradable_thing_label.ToUpper() == instrumentName),
                //CurrentAllocation = (float)currentAllocation,
                //CurrentBenchmarkMin = (float)currentBenchmarkMin,
                //CurrentBenchmarkMax = (float)currentBenchmarkMax,
                //AbsolutePerformance = (float)absolutePerformance,
                CurrentAllocation = (float) (randomGenerator.NextDouble() * 100),
                CurrentBenchmark = (float)(randomGenerator.NextDouble() * 100),
                CurrentBenchmarkMin = (float)(randomGenerator.NextDouble() * 100),
                CurrentBenchmarkMax = (float)(randomGenerator.NextDouble() * 100),
                PreviousBenchmark = (float)(randomGenerator.NextDouble() * 100),
                PreviousBenchmarkMin = (float)(randomGenerator.NextDouble() * 100),
                PreviousBenchmarkMax = (float)(randomGenerator.NextDouble() * 100),
                AbsolutePerformance = (float)(randomGenerator.NextDouble() * 100),

                Comments = string.Format("{0}_{1}", instrumentName, iteration),
                History = CreateHistories(instrumentName)
            };

            context.Allocations.Add(rootAllocation);

            return rootAllocation;

        }

        public static string GetReferenceDataSql()
        {
            const string sql = @"
SET IDENTITY_INSERT [dbo].[Location] ON
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (1, N'http://data.emii.com/locations/aa', N'AA', N'Australasia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (2, N'http://data.emii.com/locations/ae', N'AE', N'Advanced Economies')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (3, N'http://data.emii.com/locations/aex', N'AEX', N'Advanced Economies excl. G7 & Euro Area')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (4, N'http://data.emii.com/locations/afr', N'AFR', N'Africa')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (5, N'http://data.emii.com/locations/ago', N'AGO', N'Angola')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (6, N'http://data.emii.com/locations/amer', N'AMER', N'Americas')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (7, N'http://data.emii.com/locations/ara', N'ARA', N'Arabian Markets')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (8, N'http://data.emii.com/locations/are', N'ARE', N'United Arab Emirates')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (9, N'http://data.emii.com/locations/arg', N'ARG', N'Argentina')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (10, N'http://data.emii.com/locations/asi', N'ASI', N'Asia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (11, N'http://data.emii.com/locations/asid', N'ASID', N'Asia Developing')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (12, N'http://data.emii.com/locations/asini', N'ASINI', N'Asian Economies Newly Industrlialzed')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (13, N'http://data.emii.com/locations/asip', N'ASIP', N'Asia Pacific')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (14, N'http://data.emii.com/locations/asixj', N'ASIXJ', N'Asia Excluding Japan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (15, N'http://data.emii.com/locations/aus', N'AUS', N'Australia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (16, N'http://data.emii.com/locations/aut', N'AUT', N'Austria')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (17, N'http://data.emii.com/locations/aze', N'AZE', N'Azerbaijan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (18, N'http://data.emii.com/locations/bel', N'BEL', N'Belgium')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (19, N'http://data.emii.com/locations/belx', N'BELX', N'Belgium-Luxembourg')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (20, N'http://data.emii.com/locations/bgd', N'BGD', N'Bangladesh')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (21, N'http://data.emii.com/locations/bgr', N'BGR', N'Bulgaria')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (22, N'http://data.emii.com/locations/bhr', N'BHR', N'Bahrain')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (23, N'http://data.emii.com/locations/blr', N'BLR', N'Belarus')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (24, N'http://data.emii.com/locations/bmu', N'BMU', N'Bermuda')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (25, N'http://data.emii.com/locations/bol', N'BOL', N'Bolivia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (26, N'http://data.emii.com/locations/bra', N'BRA', N'Brazil')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (27, N'http://data.emii.com/locations/bric', N'BRIC', N'BRIC')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (28, N'http://data.emii.com/locations/can', N'CAN', N'Canada')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (29, N'http://data.emii.com/locations/ceeur', N'CEEUR', N'Central & Eastern Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (30, N'http://data.emii.com/locations/che', N'CHE', N'Switzerland')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (31, N'http://data.emii.com/locations/chl', N'CHL', N'Chile')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (32, N'http://data.emii.com/locations/chn', N'CHN', N'China')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (33, N'http://data.emii.com/locations/col', N'COL', N'Colombia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (34, N'http://data.emii.com/locations/cyp', N'CYP', N'Cyprus')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (35, N'http://data.emii.com/locations/cze', N'CZE', N'Czech')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (36, N'http://data.emii.com/locations/dbl', N'DBL', N'Dollar Block')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (37, N'http://data.emii.com/locations/deu', N'DEU', N'Germany')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (38, N'http://data.emii.com/locations/dev', N'DEV', N'Developing Countries')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (39, N'http://data.emii.com/locations/dnk', N'DNK', N'Denmark')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (40, N'http://data.emii.com/locations/dom', N'DOM', N'Dominican Repbulic')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (41, N'http://data.emii.com/locations/dza', N'DZA', N'Algeria')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (42, N'http://data.emii.com/locations/eafe', N'EAFE', N'Europe, Asia, Far East')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (43, N'http://data.emii.com/locations/ecu', N'ECU', N'Ecuador')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (44, N'http://data.emii.com/locations/eeur', N'EEUR', N'Eastern Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (45, N'http://data.emii.com/locations/egy', N'EGY', N'Egypt')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (46, N'http://data.emii.com/locations/em', N'EM', N'Emerging Markets')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (47, N'http://data.emii.com/locations/ema', N'EMA', N'Emerging Asia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (48, N'http://data.emii.com/locations/emea', N'EMEA', N'Europe, Middle East & Africa')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (49, N'http://data.emii.com/locations/ememea', N'EMEMEA', N'Emerging Market Europe, Middle East & Africa')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (50, N'http://data.emii.com/locations/esp', N'ESP', N'Spain')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (51, N'http://data.emii.com/locations/est', N'EST', N'Estonia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (52, N'http://data.emii.com/locations/eup', N'EUP', N'European peripheral')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (53, N'http://data.emii.com/locations/europe', N'EUROPE', N'Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (54, N'http://data.emii.com/locations/eurx', N'EURX', N'Europe Ex EMU')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (55, N'http://data.emii.com/locations/eurzn', N'EURZN', N'Eurozone')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (56, N'http://data.emii.com/locations/eurznc', N'EURZNC', N'Euro zone Core')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (57, N'http://data.emii.com/locations/exux', N'EXUX', N'Europe excluding UK')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (58, N'http://data.emii.com/locations/far', N'FAR', N'Far East')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (59, N'http://data.emii.com/locations/fin', N'FIN', N'Finland')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (60, N'http://data.emii.com/locations/fm', N'FM', N'Frontier Markets')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (61, N'http://data.emii.com/locations/fra', N'FRA', N'France')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (62, N'http://data.emii.com/locations/gbr', N'GBR', N'United Kingdom')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (63, N'http://data.emii.com/locations/geo', N'GEO', N'Georgia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (64, N'http://data.emii.com/locations/grc', N'GRC', N'Greece')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (65, N'http://data.emii.com/locations/hkg', N'HKG', N'Hong Kong')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (66, N'http://data.emii.com/locations/hrv', N'HRV', N'Croatia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (67, N'http://data.emii.com/locations/hun', N'HUN', N'Hungary')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (68, N'http://data.emii.com/locations/idn', N'IDN', N'Indonesia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (69, N'http://data.emii.com/locations/inc', N'INC', N'Industrial Countries')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (70, N'http://data.emii.com/locations/ind', N'IND', N'India')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (71, N'http://data.emii.com/locations/irl', N'IRL', N'Ireland')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (72, N'http://data.emii.com/locations/irn', N'IRN', N'Iran')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (73, N'http://data.emii.com/locations/irq', N'IRQ', N'Iraq')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (74, N'http://data.emii.com/locations/isl', N'ISL', N'Iceland')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (75, N'http://data.emii.com/locations/isr', N'ISR', N'Israel')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (76, N'http://data.emii.com/locations/ita', N'ITA', N'Italy')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (77, N'http://data.emii.com/locations/jem', N'JEM', N'Jordon, Egypt & Morocco')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (78, N'http://data.emii.com/locations/jor', N'JOR', N'Jordan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (79, N'http://data.emii.com/locations/jpn', N'JPN', N'Japan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (80, N'http://data.emii.com/locations/kaz', N'KAZ', N'Kazakhstan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (81, N'http://data.emii.com/locations/ken', N'KEN', N'Kenya')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (82, N'http://data.emii.com/locations/kor', N'KOR', N'Republic of Korea')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (83, N'http://data.emii.com/locations/kwt', N'KWT', N'Kuwait')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (84, N'http://data.emii.com/locations/la', N'LA', N'Latin America')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (85, N'http://data.emii.com/locations/lbn', N'LBN', N'Lebanon')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (86, N'http://data.emii.com/locations/lby', N'LBY', N'Libya')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (87, N'http://data.emii.com/locations/lka', N'LKA', N'Sri Lanka')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (88, N'http://data.emii.com/locations/ltu', N'LTU', N'Lithuania')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (89, N'http://data.emii.com/locations/lux', N'LUX', N'Luxembourg')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (90, N'http://data.emii.com/locations/lva', N'LVA', N'Latvia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (91, N'http://data.emii.com/locations/mac', N'MAC', N'Macau')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (92, N'http://data.emii.com/locations/maj', N'MAJ', N'Major Economies')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (93, N'http://data.emii.com/locations/mar', N'MAR', N'Morocco')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (94, N'http://data.emii.com/locations/mea', N'MEA', N'Middle East & Africa')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (95, N'http://data.emii.com/locations/mex', N'MEX', N'Mexico')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (96, N'http://data.emii.com/locations/mid', N'MID', N'Middle East')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (97, N'http://data.emii.com/locations/mlt', N'MLT', N'Malta')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (98, N'http://data.emii.com/locations/mmr', N'MMR', N'Myanmar')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (99, N'http://data.emii.com/locations/mng', N'MNG', N'Mongolia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (100, N'http://data.emii.com/locations/mys', N'MYS', N'Malaysia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (101, N'http://data.emii.com/locations/na', N'NA', N'North America')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (102, N'http://data.emii.com/locations/ncl', N'NCL', N'New Caledonia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (103, N'http://data.emii.com/locations/neec', N'NEEC', N'Non-EEC Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (104, N'http://data.emii.com/locations/nga', N'NGA', N'Nigeria')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (105, N'http://data.emii.com/locations/nld', N'NLD', N'Netherlands')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (106, N'http://data.emii.com/locations/nold', N'NOLD', N'Non-Oil-Less-Developed')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (107, N'http://data.emii.com/locations/nopc', N'NOPC', N'Non-OPEC')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (108, N'http://data.emii.com/locations/nor', N'NOR', N'Norway')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (109, N'http://data.emii.com/locations/nzl', N'NZL', N'New Zealand')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (110, N'http://data.emii.com/locations/omn', N'OMN', N'Oman')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (111, N'http://data.emii.com/locations/pac', N'PAC', N'Pacific Rim')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (112, N'http://data.emii.com/locations/pak', N'PAK', N'Pakistan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (113, N'http://data.emii.com/locations/pan', N'PAN', N'Panama')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (114, N'http://data.emii.com/locations/per', N'PER', N'Peru')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (115, N'http://data.emii.com/locations/pers', N'PERS', N'Persian Gulf Nations')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (116, N'http://data.emii.com/locations/phl', N'PHL', N'Philippines')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (117, N'http://data.emii.com/locations/png', N'PNG', N'New Guinea Papua')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (118, N'http://data.emii.com/locations/pol', N'POL', N'Poland')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (119, N'http://data.emii.com/locations/prt', N'PRT', N'Portugal')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (120, N'http://data.emii.com/locations/qat', N'QAT', N'Qatar')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (121, N'http://data.emii.com/locations/rou', N'ROU', N'Romania')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (122, N'http://data.emii.com/locations/rus', N'RUS', N'Russia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (123, N'http://data.emii.com/locations/sasi', N'SASI', N'South East Asia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (124, N'http://data.emii.com/locations/sau', N'SAU', N'Saudi Arabia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (125, N'http://data.emii.com/locations/scan', N'SCAN', N'Scandinavia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (126, N'http://data.emii.com/locations/seur', N'SEUR', N'Southern Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (127, N'http://data.emii.com/locations/sgp', N'SGP', N'Singapore')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (128, N'http://data.emii.com/locations/sle', N'SLE', N'Sierra Leone')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (129, N'http://data.emii.com/locations/slv', N'SLV', N'El Salvador')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (130, N'http://data.emii.com/locations/srb', N'SRB', N'Serbia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (131, N'http://data.emii.com/locations/svk', N'SVK', N'Slovakia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (132, N'http://data.emii.com/locations/svn', N'SVN', N'Slovenia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (133, N'http://data.emii.com/locations/swe', N'SWE', N'Sweden')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (134, N'http://data.emii.com/locations/syr', N'SYR', N'Syria')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (135, N'http://data.emii.com/locations/tha', N'THA', N'Thailand')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (136, N'http://data.emii.com/locations/tun', N'TUN', N'Tunisia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (137, N'http://data.emii.com/locations/tur', N'TUR', N'Turkey')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (138, N'http://data.emii.com/locations/twn', N'TWN', N'Taiwan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (139, N'http://data.emii.com/locations/ukr', N'UKR', N'Ukraine')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (140, N'http://data.emii.com/locations/ury', N'URY', N'Uruguay')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (141, N'http://data.emii.com/locations/usa', N'USA', N'United States')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (142, N'http://data.emii.com/locations/uzb', N'UZB', N'Uzbekistan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (143, N'http://data.emii.com/locations/ven', N'VEN', N'Venezuela')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (144, N'http://data.emii.com/locations/vnm', N'VNM', N'Vietnam')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (145, N'http://data.emii.com/locations/weur', N'WEUR', N'Western Europe')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (146, N'http://data.emii.com/locations/whm', N'WHM', N'Western Hemisphere')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (147, N'http://data.emii.com/locations/wld', N'WLD', N'Global')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (148, N'http://data.emii.com/locations/wxja', N'WXJA', N'Global Excluding Japan')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (149, N'http://data.emii.com/locations/wxus', N'WXUS', N'Global Excluding USA')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (150, N'http://data.emii.com/locations/yem', N'YEM', N'Yemen')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (151, N'http://data.emii.com/locations/zaf', N'ZAF', N'South Africa')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (152, N'http://data.emii.com/locations/zmb', N'ZMB', N'Zambia')
INSERT [dbo].[Location] ([Id], [location_uri], [location_code], [location_label]) VALUES (153, N'http://data.emii.com/locations/zwe', N'ZWE', N'Zimbabwe')
SET IDENTITY_INSERT [dbo].[Location] OFF

SET IDENTITY_INSERT [dbo].[Tradable_Thing_Class] ON
INSERT [dbo].[Tradable_Thing_Class] ([Id], [tradable_thing_class_uri], [tradable_thing_class_label], [tradable_thing_class_editorial_label]) VALUES (1, N'<http://data.emii.com/ontologies/economy/FixedIncomeMarket>', N'FixedIncomeMarket', N'Fixed Income')
INSERT [dbo].[Tradable_Thing_Class] ([Id], [tradable_thing_class_uri], [tradable_thing_class_label], [tradable_thing_class_editorial_label]) VALUES (2, N'<http://data.emii.com/ontologies/economy/CommodityMarket>', N'CommodityMarket', N'Commodity')
INSERT [dbo].[Tradable_Thing_Class] ([Id], [tradable_thing_class_uri], [tradable_thing_class_label], [tradable_thing_class_editorial_label]) VALUES (4, N'<http://data.emii.com/ontologies/economy/CurrencyMarket>', N'CurrencyMarket', N'Currency')
INSERT [dbo].[Tradable_Thing_Class] ([Id], [tradable_thing_class_uri], [tradable_thing_class_label], [tradable_thing_class_editorial_label]) VALUES (5, N'<http://data.emii.com/ontologies/economy/EquityMarket>', N'EquityMarket', N'Equity')
INSERT [dbo].[Tradable_Thing_Class] ([Id], [tradable_thing_class_uri], [tradable_thing_class_label], [tradable_thing_class_editorial_label]) VALUES (6, N'<http://data.emii.com/ontologies/economy/RealEstateMarket>', N'RealEstateMarket', N'Real Estate')
SET IDENTITY_INSERT [dbo].[Tradable_Thing_Class] OFF

SET IDENTITY_INSERT [dbo].[Tradable_Thing] ON
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (1, N'<http://data.emii.com/commodities-markets/silver>', 2, 147, NULL, N'Silver')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (2, N'<http://data.emii.com/commodities-markets/gold>', 2, 147, NULL, N'Gold')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (3, NULL, 1, 76, NULL, N'Italy 10-years')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (4, NULL, 1, 76, NULL, N'Italy 2-years')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (5, NULL, 1, 37, NULL, N'Germany 10-years')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (6, NULL, 1, 37, NULL, N'Germany 2-years')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (7, N'<http://data.emii.com/currency-pairs/mxn-usd>', 4, NULL, N'MXN', N'MXN/USD')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (8, N'<http://data.emii.com/currency-pairs/usd-gbp>', 4, NULL, N'USD', N'USD/GBP')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (9, N'<http://data.emii.com/currency-pairs/usd-eur>', 4, NULL, N'USD/EUR', N'USD/EUR')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (10, N'<http://data.emii.com/equity-markets/irl/industrials>', 5, 71, NULL, N'Irish Industrials Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (11, N'<http://data.emii.com/equity-markets/kor/automotive-retail>', 5, 82, NULL, N'Korean Automotive Retail Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (12, N'<http://data.emii.com/equity-markets/kor/banks>', 5, 82, NULL, N'Korean Banks Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (13, N'<http://data.emii.com/equity-markets/chn/semiconductors>', 5, 32, NULL, N'Chinese Semiconductors Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (14, N'<http://data.emii.com/equity-markets/chn/oil-gas-drilling>', 5, 32, NULL, N'Chinese Oil & Gas Drilling Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (15, N'<http://data.emii.com/equity-markets/mid/large-cap>', 5, 96, NULL, N'Middle East Large Cap Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (16, N'<http://data.emii.com/equity-markets/mid/leisure-facilities>', 5, 96, NULL, N'Middle East Leisure Facilities Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (17, N'<http://data.emii.com/equity-markets/fra/health-care-equipment>', 5, 61, NULL, N'French Health Care Equipment Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (18, N'<http://data.emii.com/equity-markets/fra/gas-utilities>', 5, 61, NULL, N'French Gas Utilities Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (19, N'<http://data.emii.com/fixed-income/esp-gov>', 1, 50, NULL, N'Spanish Government Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (20, N'<http://data.emii.com/fixed-income/esp-gov-abs>', 1, 50, NULL, N'Spanish Government ABS Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (21, N'<http://data.emii.com/fixed-income/esp-hkd-abs>', 1, 50, NULL, N'HKD Spanish ABS Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (22, N'<http://data.emii.com/fixed-income/usa-gov>', 1, 141, NULL, N'U.S. Government Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (23, N'<http://data.emii.com/fixed-income/usa-gov-abs>', 1, 141, NULL, N'U.S. Government ABS Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (24, N'<http://data.emii.com/fixed-income/usa-crp-hy>', 1, 141, NULL, N'U.S. Corporate High Yield Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (25, N'<http://data.emii.com/fixed-income/usa-eur-age>', 1, 141, NULL, N'EUR U.S. Agency Bonds')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (26, N'<http://data.emii.com/commodities-markets/copper>', 2, NULL, NULL, N'Copper')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (27, N'<http://data.emii.com/commodities-markets/oil>', 2, NULL, NULL, N'Oil')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (28, N'<http://data.emii.com/commodities-markets/precious-metals>', 2, NULL, NULL, N'Precious-Metals')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (29, N'<http://data.emii.com/currency-pairs/aud-cny>', 4, NULL, N'AUD/CNY', N'AUD/CNY')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (30, N'<http://data.emii.com/currency-pairs/nzd-mxn>', 4, NULL, NULL, N'NZD/MXN')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (31, N'<http://data.emii.com/currency-pairs/cash>', 1, NULL, NULL, N'CASH')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (32, N'<http://data.emii.com/currency-pairs/equities>', 1, NULL, NULL, N'Equities')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (33, N'<http://data.emii.com/currency-pairs/us>', 1, NULL, NULL, N'US')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (34, N'<http://data.emii.com/currency-pairs/emu>', 1, NULL, NULL, N'EMU')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (35, N'<http://data.emii.com/currency-pairs/japan>', 1, NULL, NULL, N'JAPAN')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (36, N'<http://data.emii.com/currency-pairs/uk>', 1, NULL, NULL, N'UK')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (37, N'<http://data.emii.com/currency-pairs/canada>', 1, NULL, NULL, N'CANADA')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (38, N'<http://data.emii.com/currency-pairs/australia>', 1, NULL, NULL, N'AUSTRALIA')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (39, N'<http://data.emii.com/currency-pairs/bond>', 1, NULL, NULL, N'Bond')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (40, N'<http://data.emii.com/currency-pairs/government>', 1, NULL, NULL, N'GOVERNMENT')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (41, N'<http://data.emii.com/currency-pairs/investment-grade>', 1, NULL, NULL, N'INVESTMENT GRADE')
INSERT [dbo].[Tradable_Thing] ([Id], [tradable_thing_uri], [tradable_thing_class_id], [location_id], [tradable_thing_code], [tradable_thing_label]) VALUES (42, N'<http://data.emii.com/currency-pairs/high-yield>', 1, NULL, NULL, N'HIGH-YIELD')
SET IDENTITY_INSERT [dbo].[Tradable_Thing] OFF

SET IDENTITY_INSERT [dbo].[Service] ON
INSERT [dbo].[Service] ([Id], [service_uri], [service_code], [service_label]) VALUES (1, N'<http://data.emii.com/ontologies/service/CIS>', N'CIS', N'CIS')
INSERT [dbo].[Service] ([Id], [service_uri], [service_code], [service_label]) VALUES (2, N'<http://data.emii.com/ontologies/service/GAA>', N'GAA', N'GAA')
INSERT [dbo].[Service] ([Id], [service_uri], [service_code], [service_label]) VALUES (3, N'<http://data.emii.com/ontologies/service/GIS>', N'GIS', N'GIS')
INSERT [dbo].[Service] ([Id], [service_uri], [service_code], [service_label]) VALUES (4, N'<http://data.emii.com/ontologies/service/GFIS>', N'GFIS', N'GFIS')
SET IDENTITY_INSERT [dbo].[Service] OFF

SET IDENTITY_INSERT [dbo].[Currency] ON
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (1, N'http://data.emii.com/currencies/aed', N'AED', NULL, N'United Arab Emirates Dirham')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (2, N'http://data.emii.com/currencies/afn', N'AFN', NULL, N'Afghanistan Afghani')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (3, N'http://data.emii.com/currencies/all', N'ALL', NULL, N'Albania Lek')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (4, N'http://data.emii.com/currencies/amd', N'AMD', NULL, N'Armenia Dram')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (5, N'http://data.emii.com/currencies/ang', N'ANG', NULL, N'Netherlands Antilles Guilder')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (6, N'http://data.emii.com/currencies/aoa', N'AOA', NULL, N'Angola Kwanza')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (7, N'http://data.emii.com/currencies/ars', N'ARS', NULL, N'Argentina Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (8, N'http://data.emii.com/currencies/asc', N'ASC', NULL, N'Asian Currencies')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (9, N'http://data.emii.com/currencies/aud', N'AUD', NULL, N'Australia Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (10, N'http://data.emii.com/currencies/awg', N'AWG', NULL, N'Aruba Guilder')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (11, N'http://data.emii.com/currencies/azn', N'AZN', NULL, N'Azerbaijan New Manat')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (12, N'http://data.emii.com/currencies/bam', N'BAM', NULL, N'Bosnia and Herzegovina Convertible Marka')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (13, N'http://data.emii.com/currencies/bbd', N'BBD', NULL, N'Barbados Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (14, N'http://data.emii.com/currencies/bdt', N'BDT', NULL, N'Bangladesh Taka')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (15, N'http://data.emii.com/currencies/bgl', N'BGL', NULL, N'Bulgaria Lev')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (16, N'http://data.emii.com/currencies/bhd', N'BHD', NULL, N'Bahrain Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (17, N'http://data.emii.com/currencies/bif', N'BIF', NULL, N'Burundi Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (18, N'http://data.emii.com/currencies/bmd', N'BMD', NULL, N'Bermuda Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (19, N'http://data.emii.com/currencies/bnd', N'BND', NULL, N'Brunei Darussalam Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (20, N'http://data.emii.com/currencies/bob', N'BOB', NULL, N'Bolivia Boliviano')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (21, N'http://data.emii.com/currencies/brl', N'BRL', NULL, N'Brazil Real')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (22, N'http://data.emii.com/currencies/bsd', N'BSD', NULL, N'Bahamas Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (23, N'http://data.emii.com/currencies/btn', N'BTN', NULL, N'Bhutan Ngultrum')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (24, N'http://data.emii.com/currencies/bwp', N'BWP', NULL, N'Botswana Pula')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (25, N'http://data.emii.com/currencies/byr', N'BYR', NULL, N'Belarus Ruble')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (26, N'http://data.emii.com/currencies/bzd', N'BZD', NULL, N'Belize Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (27, N'http://data.emii.com/currencies/cad', N'CAD', NULL, N'Canada Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (28, N'http://data.emii.com/currencies/cdf', N'CDF', NULL, N'Congo Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (29, N'http://data.emii.com/currencies/chf', N'CHF', NULL, N'Switzerland Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (30, N'http://data.emii.com/currencies/clp', N'CLP', NULL, N'Chile Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (31, N'http://data.emii.com/currencies/cmc', N'CMC', NULL, N'Commodity Currencies')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (32, N'http://data.emii.com/currencies/cny', N'CNY', NULL, N'China Yuan Renminbi')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (33, N'http://data.emii.com/currencies/cop', N'COP', NULL, N'Colombia Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (34, N'http://data.emii.com/currencies/crc', N'CRC', NULL, N'Costa Rica Colon')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (35, N'http://data.emii.com/currencies/cuc', N'CUC', NULL, N'Cuba Convertible Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (36, N'http://data.emii.com/currencies/cup', N'CUP', NULL, N'Cuba Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (37, N'http://data.emii.com/currencies/cve', N'CVE', NULL, N'Cape Verde Escudo')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (38, N'http://data.emii.com/currencies/czk', N'CZK', NULL, N'Czech Republic Koruna')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (39, N'http://data.emii.com/currencies/djf', N'DJF', NULL, N'Djibouti Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (40, N'http://data.emii.com/currencies/dkk', N'DKK', NULL, N'Denmark Krone')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (41, N'http://data.emii.com/currencies/dop', N'DOP', NULL, N'Dominican Republic Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (42, N'http://data.emii.com/currencies/dzd', N'DZD', NULL, N'Algeria Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (43, N'http://data.emii.com/currencies/egp', N'EGP', NULL, N'Egypt Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (44, N'http://data.emii.com/currencies/emc', N'EMC', NULL, N'Emerging Markets Currencies')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (45, N'http://data.emii.com/currencies/ern', N'ERN', NULL, N'Eritrea Nakfa')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (46, N'http://data.emii.com/currencies/etb', N'ETB', NULL, N'Ethiopia Birr')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (47, N'http://data.emii.com/currencies/eur', N'EUR', NULL, N'Euro')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (48, N'http://data.emii.com/currencies/fjd', N'FJD', NULL, N'Fiji Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (49, N'http://data.emii.com/currencies/fkp', N'FKP', NULL, N'Falkland Islands Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (50, N'http://data.emii.com/currencies/gbp', N'GBP', NULL, N'United Kingdom Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (51, N'http://data.emii.com/currencies/gel', N'GEL', NULL, N'Georgia Lari')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (52, N'http://data.emii.com/currencies/ggp', N'GGP', NULL, N'Guernsey Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (53, N'http://data.emii.com/currencies/ghs', N'GHS', NULL, N'Ghana Cedi')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (54, N'http://data.emii.com/currencies/gip', N'GIP', NULL, N'Gibraltar Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (55, N'http://data.emii.com/currencies/gmd', N'GMD', NULL, N'Gambia Dalasi')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (56, N'http://data.emii.com/currencies/gnf', N'GNF', NULL, N'Guinea Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (57, N'http://data.emii.com/currencies/gtq', N'GTQ', NULL, N'Guatemala Quetzal')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (58, N'http://data.emii.com/currencies/gyd', N'GYD', NULL, N'Guyana Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (59, N'http://data.emii.com/currencies/hkd', N'HKD', NULL, N'Hong Kong Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (60, N'http://data.emii.com/currencies/hnl', N'HNL', NULL, N'Honduras Lempira')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (61, N'http://data.emii.com/currencies/hrk', N'HRK', NULL, N'Croatia Kuna')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (62, N'http://data.emii.com/currencies/htg', N'HTG', NULL, N'Haiti Gourde')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (63, N'http://data.emii.com/currencies/huf', N'HUF', NULL, N'Hungary Forint')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (64, N'http://data.emii.com/currencies/idr', N'IDR', NULL, N'Indonesia Rupiah')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (65, N'http://data.emii.com/currencies/ils', N'ILS', NULL, N'Israel Shekel')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (66, N'http://data.emii.com/currencies/imp', N'IMP', NULL, N'Isle of Man Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (67, N'http://data.emii.com/currencies/inr', N'INR', NULL, N'India Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (68, N'http://data.emii.com/currencies/iqd', N'IQD', NULL, N'Iraq Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (69, N'http://data.emii.com/currencies/irr', N'IRR', NULL, N'Iran Rial')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (70, N'http://data.emii.com/currencies/isk', N'ISK', NULL, N'Iceland Krona')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (71, N'http://data.emii.com/currencies/jep', N'JEP', NULL, N'Jersey Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (72, N'http://data.emii.com/currencies/jmd', N'JMD', NULL, N'Jamaica Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (73, N'http://data.emii.com/currencies/jod', N'JOD', NULL, N'Jordan Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (74, N'http://data.emii.com/currencies/jpy', N'JPY', NULL, N'Japan Yen')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (75, N'http://data.emii.com/currencies/kes', N'KES', NULL, N'Kenya Shilling')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (76, N'http://data.emii.com/currencies/kgs', N'KGS', NULL, N'Kyrgyzstan Som')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (77, N'http://data.emii.com/currencies/khr', N'KHR', NULL, N'Cambodia Riel')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (78, N'http://data.emii.com/currencies/kmf', N'KMF', NULL, N'Comoros Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (79, N'http://data.emii.com/currencies/kpw', N'KPW', NULL, N'Korea Won')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (80, N'http://data.emii.com/currencies/krw', N'KRW', NULL, N'Korea Won')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (81, N'http://data.emii.com/currencies/kwd', N'KWD', NULL, N'Kuwait Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (82, N'http://data.emii.com/currencies/kyd', N'KYD', NULL, N'Cayman Islands Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (83, N'http://data.emii.com/currencies/kzt', N'KZT', NULL, N'Kazakhstan Tenge')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (84, N'http://data.emii.com/currencies/lak', N'LAK', NULL, N'Laos Kip')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (85, N'http://data.emii.com/currencies/lbp', N'LBP', NULL, N'Lebanon Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (86, N'http://data.emii.com/currencies/lkr', N'LKR', NULL, N'Sri Lanka Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (87, N'http://data.emii.com/currencies/lrd', N'LRD', NULL, N'Liberia Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (88, N'http://data.emii.com/currencies/lsl', N'LSL', NULL, N'Lesotho Loti')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (89, N'http://data.emii.com/currencies/ltl', N'LTL', NULL, N'Lithuania Litas')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (90, N'http://data.emii.com/currencies/lvl', N'LVL', NULL, N'Latvia Lat')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (91, N'http://data.emii.com/currencies/lyd', N'LYD', NULL, N'Libya Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (92, N'http://data.emii.com/currencies/mad', N'MAD', NULL, N'Morocco Dirham')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (93, N'http://data.emii.com/currencies/mdl', N'MDL', NULL, N'Moldova Leu')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (94, N'http://data.emii.com/currencies/mga', N'MGA', NULL, N'Madagascar Ariary')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (95, N'http://data.emii.com/currencies/mkd', N'MKD', NULL, N'Macedonia Denar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (96, N'http://data.emii.com/currencies/mmk', N'MMK', NULL, N'Myanmar (Burma) Kyat')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (97, N'http://data.emii.com/currencies/mnt', N'MNT', NULL, N'Mongolia Tughrik')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (98, N'http://data.emii.com/currencies/mop', N'MOP', NULL, N'Macau Pataca')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (99, N'http://data.emii.com/currencies/mro', N'MRO', NULL, N'Mauritania Ouguiya')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (100, N'http://data.emii.com/currencies/mur', N'MUR', NULL, N'Mauritius Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (101, N'http://data.emii.com/currencies/mvr', N'MVR', NULL, N'Maldives Rufiyaa')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (102, N'http://data.emii.com/currencies/mwk', N'MWK', NULL, N'Malawi Kwacha')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (103, N'http://data.emii.com/currencies/mxn', N'MXN', NULL, N'Mexico Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (104, N'http://data.emii.com/currencies/myr', N'MYR', NULL, N'Malaysia Ringgit')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (105, N'http://data.emii.com/currencies/mzn', N'MZN', NULL, N'Mozambique Metical')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (106, N'http://data.emii.com/currencies/nad', N'NAD', NULL, N'Namibia Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (107, N'http://data.emii.com/currencies/ngn', N'NGN', NULL, N'Nigeria Naira')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (108, N'http://data.emii.com/currencies/nio', N'NIO', NULL, N'Nicaragua Cordoba')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (109, N'http://data.emii.com/currencies/nok', N'NOK', NULL, N'Norway Krone')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (110, N'http://data.emii.com/currencies/npr', N'NPR', NULL, N'Nepal Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (111, N'http://data.emii.com/currencies/nzd', N'NZD', NULL, N'New Zealand Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (112, N'http://data.emii.com/currencies/omr', N'OMR', NULL, N'Oman Rial')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (113, N'http://data.emii.com/currencies/pab', N'PAB', NULL, N'Panama Balboa')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (114, N'http://data.emii.com/currencies/pen', N'PEN', NULL, N'Peru Nuevo Sol')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (115, N'http://data.emii.com/currencies/pgk', N'PGK', NULL, N'Papua New Guinea Kina')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (116, N'http://data.emii.com/currencies/php', N'PHP', NULL, N'Philippines Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (117, N'http://data.emii.com/currencies/pkr', N'PKR', NULL, N'Pakistan Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (118, N'http://data.emii.com/currencies/pln', N'PLN', NULL, N'Poland Zloty')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (119, N'http://data.emii.com/currencies/pyg', N'PYG', NULL, N'Paraguay Guarani')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (120, N'http://data.emii.com/currencies/qar', N'QAR', NULL, N'Qatar Riyal')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (121, N'http://data.emii.com/currencies/ron', N'RON', NULL, N'Romania New Leu')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (122, N'http://data.emii.com/currencies/rsd', N'RSD', NULL, N'Serbia Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (123, N'http://data.emii.com/currencies/rub', N'RUB', NULL, N'Russia Ruble')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (124, N'http://data.emii.com/currencies/rwf', N'RWF', NULL, N'Rwanda Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (125, N'http://data.emii.com/currencies/sar', N'SAR', NULL, N'Saudi Arabia Riyal')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (126, N'http://data.emii.com/currencies/sbd', N'SBD', NULL, N'Solomon Islands Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (127, N'http://data.emii.com/currencies/scr', N'SCR', NULL, N'Seychelles Rupee')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (128, N'http://data.emii.com/currencies/sdg', N'SDG', NULL, N'Sudan Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (129, N'http://data.emii.com/currencies/sek', N'SEK', NULL, N'Sweden Krona')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (130, N'http://data.emii.com/currencies/sgd', N'SGD', NULL, N'Singapore Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (131, N'http://data.emii.com/currencies/shp', N'SHP', NULL, N'Saint Helena Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (132, N'http://data.emii.com/currencies/sll', N'SLL', NULL, N'Sierra Leone Leone')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (133, N'http://data.emii.com/currencies/sos', N'SOS', NULL, N'Somalia Shilling')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (134, N'http://data.emii.com/currencies/spl*', N'SPL', NULL, N'Seborga Luigino')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (135, N'http://data.emii.com/currencies/srd', N'SRD', NULL, N'Suriname Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (136, N'http://data.emii.com/currencies/std', N'STD', NULL, N'São Tomé and Príncipe Dobra')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (137, N'http://data.emii.com/currencies/svc', N'SVC', NULL, N'El Salvador Colon')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (138, N'http://data.emii.com/currencies/syp', N'SYP', NULL, N'Syria Pound')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (139, N'http://data.emii.com/currencies/szl', N'SZL', NULL, N'Swaziland Lilangeni')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (140, N'http://data.emii.com/currencies/thb', N'THB', NULL, N'Thailand Baht')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (141, N'http://data.emii.com/currencies/tjs', N'TJS', NULL, N'Tajikistan Somoni')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (142, N'http://data.emii.com/currencies/tmt', N'TMT', NULL, N'Turkmenistan Manat')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (143, N'http://data.emii.com/currencies/tnd', N'TND', NULL, N'Tunisia Dinar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (144, N'http://data.emii.com/currencies/top', N'TOP', NULL, N'Tonga Pa''anga')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (145, N'http://data.emii.com/currencies/try', N'TRY', NULL, N'Turkey Lira')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (146, N'http://data.emii.com/currencies/ttd', N'TTD', NULL, N'Trinidad and Tobago Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (147, N'http://data.emii.com/currencies/tvd', N'TVD', NULL, N'Tuvalu Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (148, N'http://data.emii.com/currencies/twd', N'TWD', NULL, N'Taiwan New Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (149, N'http://data.emii.com/currencies/tzs', N'TZS', NULL, N'Tanzania Shilling')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (150, N'http://data.emii.com/currencies/uah', N'UAH', NULL, N'Ukraine Hryvna')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (151, N'http://data.emii.com/currencies/ugx', N'UGX', NULL, N'Uganda Shilling')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (152, N'http://data.emii.com/currencies/usd', N'USD', NULL, N'US Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (153, N'http://data.emii.com/currencies/uyu', N'UYU', NULL, N'Uruguay Peso')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (154, N'http://data.emii.com/currencies/uzs', N'UZS', NULL, N'Uzbekistan Som')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (155, N'http://data.emii.com/currencies/vef', N'VEF', NULL, N'Venezuela Bolivar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (156, N'http://data.emii.com/currencies/vnd', N'VND', NULL, N'Viet Nam Dong')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (157, N'http://data.emii.com/currencies/vuv', N'VUV', NULL, N'Vanuatu Vatu')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (158, N'http://data.emii.com/currencies/wst', N'WST', NULL, N'Samoa Tala')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (159, N'http://data.emii.com/currencies/xaf', N'XAF', NULL, N'Communauté Financière Africaine (BEAC) CFA Franc BEAC')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (160, N'http://data.emii.com/currencies/xcd', N'XCD', NULL, N'East Caribbean Dollar')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (161, N'http://data.emii.com/currencies/xdr', N'XDR', NULL, N'International Monetary Fund (IMF) Special Drawing Rights')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (162, N'http://data.emii.com/currencies/xof', N'XOF', NULL, N'Communauté Financière Africaine (BCEAO) Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (163, N'http://data.emii.com/currencies/xpf', N'XPF', NULL, N'Comptoirs Français du Pacifique (CFP) Franc')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (164, N'http://data.emii.com/currencies/yer', N'YER', NULL, N'Yemen Rial')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (165, N'http://data.emii.com/currencies/zar', N'ZAR', NULL, N'South Africa Rand')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (166, N'http://data.emii.com/currencies/zmk', N'ZMK', NULL, N'Zambia Kwacha')
INSERT [dbo].[Currency] ([Id], [currency_uri], [currency_code], [currency_symbol], [currency_label]) VALUES (167, N'http://data.emii.com/currencies/zwd', N'ZWD', NULL, N'Zimbabwe Dollar')
SET IDENTITY_INSERT [dbo].[Currency] OFF

SET IDENTITY_INSERT [dbo].[Benchmark] ON
INSERT [dbo].[Benchmark] ([Id], [benchmark_uri], [benchmark_code], [benchmark_label]) VALUES (1, N'<http://data.emii.com/indexes/fyzvvktdtgdm>', N'SPX', N'S&P 500')
INSERT [dbo].[Benchmark] ([Id], [benchmark_uri], [benchmark_code], [benchmark_label]) VALUES (2, NULL, NULL, N'Germany 10-year Bonds')
SET IDENTITY_INSERT [dbo].[Benchmark] OFF

SET IDENTITY_INSERT [dbo].[WeightingDescription] ON
INSERT [dbo].[WeightingDescription] ([Id], [Description]) VALUES (1, N'Underweight')
INSERT [dbo].[WeightingDescription] ([Id], [Description]) VALUES (2, N'Overweight')
SET IDENTITY_INSERT [dbo].[WeightingDescription] OFF

SET IDENTITY_INSERT [dbo].[Measure_Type] ON
INSERT [dbo].[Measure_Type] ([Id], [Description]) VALUES (1, N'BPS')
INSERT [dbo].[Measure_Type] ([Id], [Description]) VALUES (2, N'Currency')
INSERT [dbo].[Measure_Type] ([Id], [Description]) VALUES (3, N'Percent')
SET IDENTITY_INSERT [dbo].[Measure_Type] OFF

SET IDENTITY_INSERT [dbo].[DurationType] ON
INSERT [dbo].[DurationType] ([Id], [Description]) VALUES (1, N'Below Average')
INSERT [dbo].[DurationType] ([Id], [Description]) VALUES (2, N'Average')
INSERT [dbo].[DurationType] ([Id], [Description]) VALUES (3, N'Above Average')
SET IDENTITY_INSERT [dbo].[DurationType] OFF

SET IDENTITY_INSERT [dbo].[Status] ON
INSERT [dbo].[Status] ([Id], [Description]) VALUES (1, N'Published')
INSERT [dbo].[Status] ([Id], [Description]) VALUES (2, N'Unpublished')
INSERT [dbo].[Status] ([Id], [Description]) VALUES (3, N'Deleted')
INSERT [dbo].[Status] ([Id], [Description]) VALUES (4, N'Ready for Publish')
SET IDENTITY_INSERT [dbo].[Status] OFF

SET IDENTITY_INSERT [dbo].[PortfolioType] ON
INSERT [dbo].[PortfolioType] ([Id], [Description]) VALUES (1, N'Absolute')
INSERT [dbo].[PortfolioType] ([Id], [Description]) VALUES (2, N'Need more examples Ahmad')
SET IDENTITY_INSERT [dbo].[PortfolioType] OFF

";
            return sql;
        }
    }
}