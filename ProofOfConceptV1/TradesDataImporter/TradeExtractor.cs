using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using EuromoneyBca.Domain.Data.Models;
using OfficeOpenXml;

namespace TradesDataImporter
{
    internal class TradeExtractor
    {
        private void Log(string text)
        {
            ImportEventLog.AppendLine(text);
            Console.WriteLine(text);
        }

        public StringBuilder ImportEventLog { get; set; }

        private readonly BCATradeEntities1 _dataContext;
        private readonly ExcelRange _cells;
        private int _currentTradeId;
        private int _currentRowNumber;
        private string _currentTradeUri;
        private Trade _currentTradeToUpdate;
        private DateTime? _currentTradeDate;

        public TradeExtractor(BCATradeEntities1 dataContext, ExcelRange cells)
        {
            ImportEventLog = new StringBuilder();
            _dataContext = dataContext;
            _cells = cells;
        }

        public void ApplyTradeDataFromRow(int rowNumber)
        {
            _currentRowNumber = rowNumber;

            // If there is no data in the row then return
            if (IsEmpty(_cells, _currentRowNumber, "a")) return;

            SetTheCurrentTradeIdentifiers();

            if (rowNumber % 10 == 0)
            {
                Console.WriteLine(string.Format("\nId:{0}, row:{1}", _currentTradeId, _currentRowNumber));
            }

            SetTheCurrentTrade();

            SetTheServiceForTheCurrentTrade("d");

            SetTheTradeTypeForTheCurrentTrade("e");

            SetTheBenchmarkRelativityForTheCurrentTrade("f");

            SetTheCreatedUpdateDateForTheCurrentTrade("h");

            SetTheEditorialLabelForTheCurrentTrade("i");

            SetTheTradeStructureForTheCurrentTrade("j");

            SetTradeGroupForTheCurrentTrade(new List<string>() { "k", "l", "m", "n", "o", "p", "q", "r" });

            SetTradeGroupForTheCurrentTrade(new List<string>() { "t", "u", "v", "w", "x", "y" });

            SetTheInstructionForTheCurrentTrade();

            SetThePerformanceFromSpotAndCarry("aj", "ak");
            
            SetTheCommentForTheCurrentTrade("aq");

            UpdateTheCurrentTradeInTheDatabase();

        }

        private void UpdateTheCurrentTradeInTheDatabase()
        {
            if (_currentTradeToUpdate.trade_id == 0)
            {
                _dataContext.Trades.Add(_currentTradeToUpdate);
                //Log(string.Format("Row: {0}, Adding trade: {1}", _currentRowNumber, _currentTradeToUpdate.trade_uri));
            }
            else
            {
                //  Log(string.Format(("Row: {0}, Updating trade: {1}", _currentRowNumber, _currentTradeToUpdate.trade_uri);
            }
            _dataContext.SaveChanges();
        }

        private void SetTheCurrentTradeIdentifiers()
        {
            // Get the tradeId and the uri Todo: this may need to be minted, we need a way of identifying an existing trade
            _currentTradeId = GetInteger(_cells, _currentRowNumber, "a");
            _currentTradeUri = "imported_" + _currentTradeId;
            _currentTradeDate = GetDate(_cells, _currentRowNumber, "h");
        }

        private void SetTheServiceForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            var data = _dataContext.Services.FirstOrDefault(d => d.service_code == id);
            if (data == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised service: {1}", _currentTradeId, id));
                return;
            }

            _currentTradeToUpdate.Service = data;
        }

        private void SetTheTradeTypeForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            var data = _dataContext.Length_Type.FirstOrDefault(d => d.length_type_label == id);
            if (data == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised trade type: {1}", _currentTradeId, id));
                return;
            }

            _currentTradeToUpdate.Length_Type = data;
        }

        private void SetTheBenchmarkRelativityForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            var data = _dataContext.Relativities.FirstOrDefault(d => d.relativity_label == id);
            if (data == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised benchmark/relativity code: {1}", _currentTradeId, id));
                return;
            }

            _currentTradeToUpdate.Relativity = data;
        }

        private void SetTheEditorialLabelForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            _currentTradeToUpdate.trade_editorial_label = id;
        }

        private void SetTheTradeStructureForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            var data = _dataContext.Structure_Type.FirstOrDefault(d => d.structure_type_label == id);
            if (data == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised structure code: {1}", _currentTradeId, id));
                return;
            }

            _currentTradeToUpdate.Structure_Type = data;
        }

        private void SetTheCreatedUpdateDateForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            var date = GetDate(_cells, _currentRowNumber, columnAddress);

            _currentTradeToUpdate.last_updated = date;
        }

        private void SetThePerformanceFromSpotAndCarry(string fxSpotColumnAddress, string fxCarryColumnAddress)
        {
            var fxSpotValue = GetDecimal(_cells, _currentRowNumber, fxSpotColumnAddress);
            if (fxSpotValue == null || fxSpotValue == -9999) return;

            var fxCarryValue = GetDecimal(_cells, _currentRowNumber, fxCarryColumnAddress);
            if (fxCarryValue == null || fxCarryValue == -9999) return;

            Log(string.Format("Trade Id: {0} creating new trade performance for spot:{1} and carry: {2}", _currentTradeId, fxSpotValue, fxCarryValue));

            var tradePerformance = new Trade_Performance()
            {
                Measure_Type = _dataContext.Measure_Type.First(d => d.measure_type_label == "Percent"),
                return_value = (fxSpotValue + fxCarryValue).ToString(),
                return_date = _currentTradeDate,
                created_on = _currentTradeDate,
                last_updated = _currentTradeDate
            };

            _currentTradeToUpdate.Trade_Performance.Add(tradePerformance);
            
        }

        private Trade_Line GetTradeLineAndCreateInstumentIfNeeded(string tradeLinePositionColumnAddress, string tradeLineFinancialIntrumentColumnAddress)
        {

            var tradeLinePositionLabel = GetString(_cells, _currentRowNumber, tradeLinePositionColumnAddress);
            if (string.IsNullOrEmpty(tradeLinePositionLabel)) return null;

            var tradeLineFinancialIntrumentLabel = GetString(_cells, _currentRowNumber, tradeLineFinancialIntrumentColumnAddress);
            if (string.IsNullOrEmpty(tradeLineFinancialIntrumentLabel)) return null;

            // Does the financial instrument exist
            var financialInstrument = _dataContext.Tradable_Thing.FirstOrDefault(d => d.tradable_thing_label == tradeLineFinancialIntrumentLabel);
            if (financialInstrument == null)
            {
                Log(string.Format("Trade Id: {0} has unknown financial intrument: {1}, creating new financial instrument to cover it", _currentTradeId, tradeLineFinancialIntrumentLabel));
                financialInstrument = new Tradable_Thing()
                {
                    tradable_thing_uri = "generated_" + _currentTradeId,
                    tradable_thing_label = tradeLineFinancialIntrumentLabel
                };
                _dataContext.Tradable_Thing.Add(financialInstrument);
                _dataContext.SaveChanges();
            }

            // Get the position
            var position = _dataContext.Positions.FirstOrDefault(d => d.position_label == tradeLinePositionLabel);
            if (position == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised Trade position code: {1}", _currentTradeId, tradeLinePositionLabel));
                return null;
            }

            var tradeLine =
                _currentTradeToUpdate.Trade_Line.FirstOrDefault(
                    d => d.Tradable_Thing == financialInstrument && d.Position == position);

            if (tradeLine != null) return tradeLine;

            Log(string.Format("Trade Id: {0} creating new trade line {1}", _currentTradeId, tradeLineFinancialIntrumentLabel));
            // Create the tradeline
            tradeLine = new Trade_Line()
                {
                    Position = position,
                    Tradable_Thing = financialInstrument
                };

            _currentTradeToUpdate.Trade_Line.Add(tradeLine);
            _dataContext.SaveChanges();

            return tradeLine;

        }

        private Trade_Line_Group_Type GetTradeLineGroupType(string columnAddress)
        {
            var groupStructureCode = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(groupStructureCode)) return null;

            var groupStructure = _dataContext.Trade_Line_Group_Type.FirstOrDefault(d => d.trade_line_group_type_label == groupStructureCode);
            if (groupStructure == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised group structure code: {1}", _currentTradeId, groupStructureCode));
                return null;
            }
            return groupStructure;
        }

        private void SetTheInstructionForTheCurrentTrade()
        {
            var entryLevel = GetDecimal(_cells, _currentRowNumber, "z");
            if (entryLevel == null) return;

            var startDate = GetDate(_cells, _currentRowNumber, "aa");
            if (startDate == null) return;

            var instructionText = GetString(_cells, _currentRowNumber, "ae");
            if (instructionText == null) return;

            var exitLevel = GetDecimal(_cells, _currentRowNumber, "ab");

            var closeDate = GetDate(_cells, _currentRowNumber, "ac");

            var instructionType = GetIntructionType("ad");

            var hedgeType = GetHedgeType("af");

            if (_currentTradeToUpdate.Trade_Instruction.Any(d => 
                d.instruction_entry == entryLevel &&
                d.instruction_entry_date == startDate &&
                d.instruction_label == instructionText
                )) return;

            Log(string.Format("Creating new instruction for Trade Id: {0}", _currentTradeId));

            var instructionDate = _currentTradeDate;

            _currentTradeToUpdate.Trade_Instruction.Add(new Trade_Instruction()
            {
                instruction_entry = entryLevel,
                instruction_entry_date = startDate,
                instruction_exit = exitLevel,
                instruction_exit_date = closeDate,
                Instruction_Type = instructionType,
                instruction_label = instructionText,
                Hedge_Type = hedgeType,
                created_on = instructionDate,
                last_updated = instructionDate
            });

        }

        private Hedge_Type GetHedgeType(string columnAddress)
        {
            var code = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(code)) return null;

            var hedgeType = _dataContext.Hedge_Type.FirstOrDefault(d => d.hedge_label == code);
            if (hedgeType == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised hedge type code: {1}", _currentTradeId, code));
                return null;
            }
            return hedgeType;
        }

        private Instruction_Type GetIntructionType(string columnAddress)
        {
            var code = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(code)) return null;

            var instructionType = _dataContext.Instruction_Type.FirstOrDefault(d => d.instruction_type_label == code);
            if (instructionType == null)
            {
                Log(string.Format("Trade Id: {0} has unrecognised instruction type code: {1}", _currentTradeId, code));
                return null;
            }
            return instructionType;
        }
        

        private void SetTradeGroupForTheCurrentTrade(IReadOnlyList<string> columns)
        {

            var tradeLineGroupType = GetTradeLineGroupType(columns[0]);
            if (tradeLineGroupType == null) return;

            var groupEditorialLabel = GetString(_cells, _currentRowNumber, columns[1]);
            if (string.IsNullOrEmpty(groupEditorialLabel)) return;

            var tradeLine1 = GetTradeLineAndCreateInstumentIfNeeded(columns[2], columns[3]);
            var tradeLine2 = GetTradeLineAndCreateInstumentIfNeeded(columns[4], columns[5]);
            Trade_Line tradeLine3 = null;
            if (columns.Count > 6)
            {
                tradeLine3 = GetTradeLineAndCreateInstumentIfNeeded(columns[6], columns[7]);
            }

            Trade_Line_Group tradeGroup = null;
            if (tradeLine1 != null && tradeLine1.Trade_Line_Group != null)
            {
                tradeGroup = tradeLine1.Trade_Line_Group;
            }
            if (tradeGroup == null && tradeLine2 != null && tradeLine2.Trade_Line_Group != null)
            {
                tradeGroup = tradeLine2.Trade_Line_Group;
            }
            if (tradeGroup == null && tradeLine3 != null && tradeLine3.Trade_Line_Group != null)
            {
                tradeGroup = tradeLine3.Trade_Line_Group;
            }

            if (tradeGroup == null)
            {
                Log(string.Format("Trade Id: {0} creating new trade line group: {1}", _currentTradeId, groupEditorialLabel));
                tradeGroup = new Trade_Line_Group()
                    {
                        Trade_Line_Group_Type = tradeLineGroupType,
                        trade_line_group_editorial_label = groupEditorialLabel
                    };

                _dataContext.Trade_Line_Group.Add(tradeGroup);
                _dataContext.SaveChanges();
            }

            if (tradeLine1 != null)
            {
                if (!tradeGroup.Trade_Line.Contains(tradeLine1))
                {
                    tradeGroup.Trade_Line.Add(tradeLine1);
                    _dataContext.SaveChanges();
                }
            }
            if (tradeLine2 != null)
            {
                if (!tradeGroup.Trade_Line.Contains(tradeLine2))
                {
                    tradeGroup.Trade_Line.Add(tradeLine2);
                    _dataContext.SaveChanges();
                }
            }
            if (tradeLine3 != null)
            {
                if (!tradeGroup.Trade_Line.Contains(tradeLine3))
                {
                    tradeGroup.Trade_Line.Add(tradeLine3);
                    _dataContext.SaveChanges();
                }
            }
        }

        private void SetTheCommentForTheCurrentTrade(string columnAddress)
        {
            var id = GetString(_cells, _currentRowNumber, columnAddress);
            if (string.IsNullOrEmpty(id)) return;

            // Todo: This may not be desireable, the column in only 255 wide
            if (id.Length > 255)
            {
                id = id.Substring(0, 255);
            }

            if (_currentTradeToUpdate.Trade_Comment.Any(c => c.comment_label == id)) return;

            Log(string.Format("Creating new comment for Trade Id: {0}", _currentTradeId));

            var commentDate = _currentTradeDate;

            _currentTradeToUpdate.Trade_Comment.Add(new Trade_Comment()
                {
                    comment_label = id,
                    created_on = commentDate,
                    last_updated = commentDate
                });
        }

        private void SetTheCurrentTrade()
        {
            // Does this trade exist already
            _currentTradeToUpdate = _dataContext.Trades.FirstOrDefault(t => t.trade_uri == _currentTradeUri);

            if (_currentTradeToUpdate == null)
            {
                Log(string.Format("Trade Id: {0}, (tradeUri: {1}) does not exists so creating a new trade", _currentTradeId, _currentTradeUri));
                _currentTradeToUpdate = new Trade()
                    {
                        trade_uri = _currentTradeUri
                    };
            }
            else
            {
                //Log(string.Format(("Trade Id: {0}, (DB Id: {1}) exists already and has {2} comments",
                //    _currentTradeId, _currentTradeToUpdate.trade_id, _currentTradeToUpdate.Trade_Comment.Count());
            }
        }

        private decimal? GetDecimal(ExcelRange cells, int row, string column)
        {
            var decimalString = GetString(cells, row, column);
            if (string.IsNullOrEmpty(decimalString))
            {
                return null;
            }
            decimal decimalVal = 0;
            if (decimal.TryParse(decimalString, out decimalVal))
            {
                return decimalVal;
            }
            Log(string.Format("ERROR - Trade Id: {0}, (Row: {1}) could not parse decimal: {2}", _currentTradeId,
                              _currentRowNumber, decimalString));
            return -9999;

        }

        private static int GetInteger(ExcelRange cells, int row, string column)
        {
            return int.Parse(cells[column + row].Value.ToString());
        }

        private static DateTime ? GetDate(ExcelRange cells, int row, string column)
        {
            var daysSinceStartOf20CenturyString = GetString(cells, row, column);
            if (string.IsNullOrEmpty(daysSinceStartOf20CenturyString))
            {
                return null;
            }

            var daysSinceStartOf20Century = int.Parse(daysSinceStartOf20CenturyString);
            return new DateTime(1900, 1, 1).AddDays(daysSinceStartOf20Century - 2);
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
