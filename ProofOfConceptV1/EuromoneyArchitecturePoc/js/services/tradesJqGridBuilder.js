'use strict';

tradesApp.factory('tradesJqGridBuilder', function (tradeLineSummariesService) {
    return {
        buildCommonConfig: function (gridTableName, pagerInstanceName) {
            return {
                // Allow column reordering, depends on grid.jqueryui.js
                // sortable: true,

                sortable: {
                    update: function (permutation) {
                        //debugger;

                        var grid = $('#' + gridTableName);
                        var rowIds = grid.getDataIDs();
                        $.each(rowIds, function (index, rowId) {
                            grid.collapseSubGridRow(rowId);
                        });



                    }
                },

                //autowidth: true,
                width: 1500,

                height: 'auto',

                cmTemplate: { editable: true, editrules: { required: true } },
                rowList: [10, 20, 30],
                rowNum: 10,

                rowattr: function (rd) {

                    var classes = [];

                    if (rd.length_type_label == 'Tactical') {
                        classes.push("bkRed");
                    }

                    if (rd.structure_type_label == 'Single') {
                        classes.push("txtRed");
                    }

                    return { "class": classes.join(' ') };


                },
                url: 'http://localhost:53403/odata/ActiveTradeSummaries',
                prmNames: { search: null, nd: null },
                ajaxGridOptions: {
                    contentType: 'application/json',
                    headers: {}
                },
                datatype: 'json',
                serializeGridData: function (postData) {
                    if (postData.sidx) {
                        return {
                            $skip: (parseInt(postData.page, 10) - 1) * postData.rows,
                            $orderby: postData.sidx + ' ' + postData.sord,
                            $inlinecount: 'allpages'
                        };
                    } else {
                        return {
                            $top: postData.rows,
                            $skip: (parseInt(postData.page, 10) - 1) * postData.rows,
                            $inlinecount: 'allpages'
                        };
                    }
                },
                beforeProcessing: function (data, textStatus, jqXHR) {
                    var rows = parseInt($(this).jqGrid('getGridParam', 'rowNum'), 10);
                    data.count = data['odata.count'];
                    data.total = Math.ceil(data.count / rows);
                },
                jsonReader: {
                    repeatitems: false,
                    root: 'value',
                    records: 'count'
                },

                loadError: function (jqXHR, textStatus, errorThrown) {
                    alert('HTTP status code: ' + jqXHR.status + '\n' +
                        'textStatus: ' + textStatus + '\n' +
                        'errorThrown: ' + errorThrown);
                    alert('HTTP message body (jqXHR.responseText): ' + '\n' + jqXHR.responseText);
                },
                pager: '#' + pagerInstanceName,
                viewrecords: true,
                gridview: true,

                subGrid: true,


                subGridRowColapsed: function (subgridId, rowId) {
                    var grid = $(this);
                    grid.find('.' + subgridId).remove();
                },

                subGridRowExpanded: function (subgridId, rowId) {
                    var subGrid = $("#" + subgridId),
                        trToRemove = subGrid.closest("tr");

                    // Get a handle on the previous row
                    var previousRow = trToRemove.prev();

                    // Remove the row jQgrid added
                    trToRemove.remove();

                    // get the name of the table
                    var tableId = subGrid.attr("id").replace('_' + rowId, '');

                    // Display a message while we fetch the data
                    var fetchingDataMessage = $("<tr role='row' class='ui-subgrid'><td colspan='20'>Fetching data....</td></tr>");
                    previousRow.after(fetchingDataMessage);

                    // Call our angular resource to get the trade line summaries
                    tradeLineSummariesService.getTradeLineSummaries(rowId).then(function (tradeLineSummaries) {

                        var tradeLineSummariesHtml = '';
                        
                        // For each trade line summary return...
                        jQuery.each(tradeLineSummaries.value, function (dataIndex, dataValue) {

                            tradeLineSummariesHtml += '<tr role="row" class="ui-subgrid ' + subgridId + '">';

                            // For each of the columns in the parent...
                            jQuery.each(previousRow.children(), function (rowIndex, rowValue) {

                                var dataColumnName = $(rowValue).attr('aria-describedby').replace(tableId + '_', '');

                                if (dataColumnName == 'subgrid' || dataColumnName == 'is_favourite') {
                                    tradeLineSummariesHtml += '<td></td>';
                                } else {
                                    var dataValueToRender = eval('dataValue' + '.' + dataColumnName);
                                    if (dataValueToRender == undefined) dataValueToRender = '';
                                    tradeLineSummariesHtml += '<td aria-describedby=\'' + dataColumnName + '\' style=\'' + rowValue.style.cssText + '\'>' + dataValueToRender + '</td>';
                                }

                            });
                        });

                        fetchingDataMessage.remove();

                        previousRow.after(tradeLineSummariesHtml);

                    });

                }

            };
        },
        initialiseGrid: function (element, gridTableName, gridPagerName, config, showColumnChooser) {
            var table, pager;
            element.children().empty();
            table = angular.element('<table id="' + gridTableName + '"></table>');
            pager = angular.element('<table id="' + gridPagerName + '"></table>');
            element.append(table);
            element.append(pager);
            
            $(table).jqGrid(config);
            if (showColumnChooser) {
                $(table).jqGrid('navGrid', '#' + gridPagerName, { add: false, edit: false, del: false, search: false, refresh: false });
                $(table).jqGrid('navButtonAdd', '#' + gridPagerName, {
                    caption: "Columns",
                    title: "Reorder Columns",
                    onClickButton: function () {
                        jQuery('#' + gridTableName).jqGrid('columnChooser',
                            {
                                "done": function (perm) {
                                    //debugger;
                                    if (perm) {
                                        this.jqGrid("remapColumns", perm, true);
                                        var grid = this;
                                        var rowIds = grid.getDataIDs();
                                        $.each(rowIds, function (index, rowId) {
                                            grid.collapseSubGridRow(rowId);
                                        });
                                        
                                    }
                                    // here you can do some additional actions
                                }
                            });

                    }
                });
            }
        }
    };
});
