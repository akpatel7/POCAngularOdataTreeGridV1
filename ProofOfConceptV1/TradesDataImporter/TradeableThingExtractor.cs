using System;
using System.Linq;
using System.Text;
using EuromoneyBca.Domain.Data.Models;
using OfficeOpenXml;

namespace TradesDataImporter
{
    internal class TradeableThingExtractor
    {
        private void Log(string text)
        {
            ImportEventLog.AppendLine(text);
            Console.WriteLine(text);
        }

        public StringBuilder ImportEventLog { get; set; }

        private readonly BCATradeEntities1 _dataContext;
        private readonly ExcelRange _cells;
        private int _currentRowNumber;

        public TradeableThingExtractor(BCATradeEntities1 dataContext, ExcelRange cells)
        {
            ImportEventLog = new StringBuilder();
            _dataContext = dataContext;
            _cells = cells;
        }

        public void ApplyTradeableThingDataFromRow(int rowNumber)
        {
            _currentRowNumber = rowNumber;

            // If there is no data in the row then return
            if (IsEmpty(_cells, _currentRowNumber, "a")) return;

            if (rowNumber%10 == 0)
            {
                Console.WriteLine(string.Format("\nrow:{0}", _currentRowNumber));
            }

            var tradeableThingClassUri = GetString(_cells, _currentRowNumber, "B");
             
            // Ignore AnnotationConcepts
            if (tradeableThingClassUri.EndsWith("AnnotationConcept")) return;

            var tradeableThingClass =_dataContext.Tradable_Thing_Class.FirstOrDefault(
                    d => d.tradable_thing_class_uri == tradeableThingClassUri);            
            if (tradeableThingClass == null)
            {
                throw new ArgumentException("Error: The tradable thing class cannot be found for uri: " + tradeableThingClassUri);
            }

            // If the tradeable thing class is CommodityMarket or CurrencyMarket then the location is World
            Location tradeableThingLocation = null;
            var tradeableThingLocationUri = GetString(_cells, _currentRowNumber, "B");

            if (tradeableThingClass.tradable_thing_class_label == "CommodityMarket" ||
                tradeableThingClass.tradable_thing_class_label == "CurrencyMarket")
            {
                tradeableThingLocation = _dataContext.Locations.First(d => d.location_code == "WLD");
            }
            else
            {
                tradeableThingLocation = _dataContext.Locations.FirstOrDefault(
                    d => d.location_uri == tradeableThingLocationUri);
                if (tradeableThingLocation == null)
                {
                    throw new ArgumentException("Error: The tradable thing location cannot be found for uri: " +
                                                tradeableThingLocationUri);
                }
            }

            var setLabelAsCode = (tradeableThingClass.tradable_thing_class_editorial_label == "Currency");
            var tradableThingLabel = GetString(_cells, _currentRowNumber, "C");

            var tradableThing = new Tradable_Thing()
                {
                    tradable_thing_uri = GetString(_cells, _currentRowNumber, "A"),
                    Tradable_Thing_Class = tradeableThingClass,
                    Location = tradeableThingLocation,
                    tradable_thing_label = tradableThingLabel,
                    tradable_thing_code = setLabelAsCode ? tradableThingLabel : null
                };

            _dataContext.Tradable_Thing.Add(tradableThing);

            _dataContext.SaveChanges();
        }

        private static string GetString(ExcelRange cells, int row, string column)
        {
            var value = cells[column + row].Value;
            return value == null ? string.Empty : value.ToString().Trim();
        }

        private static bool IsEmpty(ExcelRange cells, int row, string column)
        {
            var cellValue = cells[column + row].Value;

            return (cellValue == null || (cellValue.ToString()).IsNullOrWhiteSpace());
        }

    }
}
