'use strict';

tradesApp.factory('tradesKendoGridBuilder', function () {
    return {
        
        buildCommonConfig: function () {
            return {
                dataSource: {
                    type: "odata",
                    transport: {
                        read: {
                            url: "http://localhost:53403/odata/ActiveTradeSummaries",
                            dataType: "json"
                        },
                    },
                    schema: {
                        data: function(data) {
                            return data["value"];
                        },
                        total: function(data) {

                            return data["odata.count"];
                        },
                        model: {
                            fields: {
                                trade_id: { type: "number" },
                                service_code: { type: "string" },
                                length_type_label: { type: "string" },
                                trade_editorial_label: { type: "string" },
                                structure_type_label: { type: "string" },
                                last_updated: { type: "date" },
                                instruction_entry: { type: "number" },
                                instruction_entry_date: { type: "date" },
                                instruction_exit: { type: "number" },
                                instruction_exit_date: { type: "date" },
                                instruction_label: { type: "string" },
                                instruction_type_label: { type: "string" },
                                absolute_performance: { type: "string" },
                                absolute_measure_type: { type: "string" },
                                absolute_currency_code: { type: "string" },
                                relative_performance: { type: "string" },
                                relative_measure_type: { type: "string" },
                                benchmark_label: { type: "string" },
                                relative_currency_code: { type: "string" },
                                isOpen: { type: "number" }
                            }
                        }
                    },
                    pageSize: 30,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true
                },
                height: 430,
                sortable: true,
                filterable: true,
                columnMenu: true,
                pageable: true,
                reorderable: true,
                resizable: true,

                columns: [{
                    field: "trade_id",
                    title: "Trade Id",
                    width: 100
                }, {
                    field: "service_code",
                    title: "Service Code",
                    width: 100
                }, {
                    field: "length_type_label",
                    title: "length type label",
                    width: 100
                }, {
                    field: "trade_editorial_label",
                    title: "trade editorial label",
                    width: 100
                }, {
                    field: "structure_type_label",
                    title: "structure type label",
                    width: 100
                }, {
                    field: "last_updated",
                    title: "last updated",
                    width: 100
                }, {
                    field: "instruction_entry",
                    title: "instruction entry",
                    width: 100
                }, {
                    field: "instruction_entry_date",
                    title: "instruction entry date",
                    width: 100
                }, {
                    field: "instruction_exit",
                    title: "instruction exit",
                    width: 100
                }, {
                    field: "instruction_exit_date",
                    title: "instruction exit date",
                    width: 100
                }, {
                    field: "instruction_label",
                    title: "instruction label",
                    width: 100
                }, {
                    field: "instruction_type_label",
                    title: "instruction type label",
                    width: 100
                }, {
                    field: "absolute_performance",
                    title: "absolute performance",
                    width: 100
                }, {
                    field: "absolute_measure_type",
                    title: "absolute measure type",
                    width: 100
                }, {
                    field: "absolute_currency_code",
                    title: "absolute currency code",
                    width: 100
                }, {
                    field: "relative_performance",
                    title: "relative performance",
                    width: 100
                }, {
                    field: "relative_measure_type",
                    title: "relative measure type",
                    width: 100
                }, {
                    field: "benchmark_label",
                    title: "benchmark label",
                    width: 100
                }, {
                    field: "relative_currency_code",
                    title: "relative currency code",
                    width: 100
                }, {
                    field: "isOpen",
                    title: "Is Open",
                    width: 100
                }
                ]
            };
        },
        
        initialiseGrid: function (element, gridName, config) {
            var div;

            element.children().empty();
            div = angular.element('<div id="' + gridName + '"></div>');
            element.append(div);
            $(div).kendoGrid(config);
        }
    };
});

