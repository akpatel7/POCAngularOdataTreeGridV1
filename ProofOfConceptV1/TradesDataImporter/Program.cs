using System;
using System.IO;
using System.Linq;
using EuromoneyBca.Domain.Data.Models;
using OfficeOpenXml;

namespace TradesDataImporter
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value.Trim());
        }
    }

    internal class Program
    {

        private static void Main(string[] args)
        {
            // -loadTradeData "ImportFiles/FES trade data.xlsx"
            // -loadTradeableThingData "ImportFiles/FromFabio/tradable_20140120.xlsx"

            const string usage = "Usage: TradesDataImport -loadTradeData excelFilename | -loadTradeableThingData excelFilename";
            if (args.Count() != 2)
            {
                Console.WriteLine(usage);
                Console.WriteLine("Press any key to finish...");
            }

            switch (args[0])
            {
                case "-loadTradeData":
                    LoadTradeData(args[1]);
                    break;

                case "-loadTradeableThingData":
                    LoadTradeableThingData(args[1]);
                    break;

                default:
                    Console.WriteLine(usage);
                    Console.WriteLine("Press any key to finish...");
                    break;
            }

        }

        private static void LoadTradeableThingData(string excelFileName)
        {
            var newFile = new FileInfo(excelFileName);

            var pck = new ExcelPackage(newFile);

            var sheetName = "tradable_20140120";

            var sheet = pck.Workbook.Worksheets.FirstOrDefault(s => s.Name.ToLower() == sheetName.ToLower());
            if (sheet == null)
                throw new ArgumentException("Cannot find sheet " + sheetName);

            // Find the extent of the data
            var endCol = sheet.Dimension.End.Column;
            var endRow = sheet.Dimension.End.Row;

            using (var dataContext = new BCATradeEntities1())
            {

                var tradeableThingExtractor = new TradeableThingExtractor(dataContext, sheet.Cells);

                // Start on the second row, first row is the header
                //endRow = 10;
                for (var rowNumber = 3; rowNumber <= endRow; rowNumber++)
                {
                    Console.Write(".");
                    tradeableThingExtractor.ApplyTradeableThingDataFromRow(rowNumber);
                }

                File.WriteAllText("tradeableThing_eventlog.txt", tradeableThingExtractor.ImportEventLog.ToString());
            }

            Console.WriteLine("Done, press any key to finish...");
            Console.ReadLine();
        }

        private static void LoadTradeData(string excelFileName)
        {
            var newFile = new FileInfo(excelFileName);

            var pck = new ExcelPackage(newFile);

            var sheetName = "Sheet1";

            var sheet = pck.Workbook.Worksheets.FirstOrDefault(s => s.Name.ToLower() == sheetName.ToLower());
            if (sheet == null)
                throw new ArgumentException("Cannot find sheet " + sheetName);

            // Find the extent of the data
            var endCol = sheet.Dimension.End.Column;
            var endRow = sheet.Dimension.End.Row;

            using (var dataContext = new BCATradeEntities1())
            {

                var tradeExtractor = new TradeExtractor(dataContext, sheet.Cells);

                // Start on the second row, first row is the header
                endRow = 10;
                for (var rowNumber = 2; rowNumber <= endRow; rowNumber++)
                {
                    Console.Write(".");
                    tradeExtractor.ApplyTradeDataFromRow(rowNumber);
                }

                File.WriteAllText("trade_eventlog.txt", tradeExtractor.ImportEventLog.ToString());
            }

            Console.WriteLine("Done, press any key to finish...");
            Console.ReadLine();
        }
    }
}

