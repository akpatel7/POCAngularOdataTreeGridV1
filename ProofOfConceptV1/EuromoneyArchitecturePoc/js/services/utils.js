'use strict';

tradesApp.factory('utils', function() {
    return {
        cfgToOdataParam: function(cfg) {
            function addOdataFilterIfPresent(fieldName, oDataFilters, filterCfg) {

                // this method is not IE8 friendly because it won't handle treating the xml as a dom element
                // Just return for now
                
                var filterValue = filterCfg.attr(fieldName);
                if (filterValue == undefined) return;

                var filterOperation = filterCfg.attr(fieldName + 'Filter');

                if (filterOperation == undefined) {
                    alert("cfgToOdataParam.getFilter: Missing attributte  " + fieldName + 'Filter');
                    return;
                }
                
                var odataFilterOperation = '';
                //debugger;
                switch(filterOperation)
                {
                    case '3':
                        odataFilterOperation = fieldName + '+lt+' + filterValue;
                        break;
                        
                    case '5':
                        odataFilterOperation = fieldName + '+gt+' + filterValue;
                        break;
                        
                    case '7':
                        odataFilterOperation = 'startswith(' + fieldName + ',\'' + filterValue + '\')';
                        break;
                        
                    case '11':
                        odataFilterOperation = 'substringof(\'' + filterValue + '\', ' + fieldName + ')';
                        break;
                        
                    default:
                }
                if (odataFilterOperation.length != 0) {
                    oDataFilters.push(odataFilterOperation);
                }

                return;

            }

            var oDataParams = [];
            // Needs to be made IE8 friendly
            
            //oDataParams.push('$top=30');
            //debugger;
            //var sortCfg = $(cfg).find('Cfg').attr('Sort');
            //if (sortCfg.length != 0) {
            //    var oDataOrderByArr = [];
            //    $.each(sortCfg.split(','), function(i, sortColumn) {
            //        if (sortColumn.indexOf('-') == 0) {
            //            oDataOrderByArr.push(sortColumn.substring(1) + '+desc');
            //        } else {
            //            oDataOrderByArr.push(sortColumn);
            //        }
            //    });
            //    oDataParams.push('$orderby=' + oDataOrderByArr.join(','));
            //}

            //var filterCfg = $(cfg).find('Filters i');

            ////debugger;
            
            //var oDataFilters = [];
            //addOdataFilterIfPresent('Instrument', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('Comments', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('CurrentAllocation', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('PreviousAllocation', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('CurrentBenchmark', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('CurrentBenchmarkMin', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('CurrentBenchmarkMax', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('PreviousBenchmark', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('PreviousBenchmarkMax', oDataFilters, filterCfg);
            //addOdataFilterIfPresent('PreviousBenchmarkMin', oDataFilters, filterCfg);
     
            //if (oDataFilters.length != 0) {
            //    var tt = oDataFilters.join('+and+');
            //    oDataParams.push('$filter=' + tt);
            //}


            //"<Grid>" +
            //    "<Cfg Sort="" SortCols="" Group="" GroupCols="" SearchAction="" TimeZone="0" ScrollLeft="0" LeftScrollLeft="0" ScrollTop="0" Focused="Filter1" FocusedCol="Comments"/>" +
            //    "<Filters><I id="Filter1" Comments="Bond" CommentsFilter="7"/></Filters><IO/></Grid>"

            return oDataParams.join('&');

        },
        xmlToJson: function(xml) {
            // Create the return object

            var obj = {};

            if (xml.nodeType == 1) { // element
                // do attributes
                if (xml.attributes.length > 0) {
                    obj["@attributes"] = {};
                    for (var j = 0; j < xml.attributes.length; j++) {
                        var attribute = xml.attributes.item(j);
                        obj["@attributes"][attribute.nodeName] = attribute.nodeValue;
                    }
                }
            } else if (xml.nodeType == 3) { // text
                obj = xml.nodeValue;
            }

            // do children
            if (xml.hasChildNodes()) {
                for (var i = 0; i < xml.childNodes.length; i++) {
                    var item = xml.childNodes.item(i);
                    var nodeName = item.nodeName;
                    if (typeof(obj[nodeName]) == "undefined") {
                        obj[nodeName] = xmlToJson(item);
                    } else {
                        if (typeof(obj[nodeName].push) == "undefined") {
                            var old = obj[nodeName];
                            obj[nodeName] = [];
                            obj[nodeName].push(old);
                        }
                        obj[nodeName].push(xmlToJson(item));
                    }
                }
            }
            return obj;
        }
    };
});
