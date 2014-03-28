
'use strict';
tradesApp.directive('emFastDashWithCustomAngularBinding', function ($compile) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            comboChanged: '=combochanged', // 'cos angular has downcased this
            showIt: '=showit', // 'cos angular has downcased this
            loaded: "=",
            config: '=',
            data: '=',
        },
        link: function (scope, element, attrs) {

            scope.$watch('config', function (newValue) {

                element.children().empty();
              
                var $html = $('<div/>');

                jQuery.each(scope.data, function (dataIndex, data) {

                    var htmlForSelector = '<div class=\'dash-box\'><div>' + data.Name + '</div><div>' + data.Value + '</div>';

                    htmlForSelector += '<select>';

                    jQuery.each(data.Children, function (dataItemIndex, dataItem) {
                        htmlForSelector += '<option value=\'' + dataItem.Value + '\'>' + dataItem.Display + '</option>';
                    });
                    
                    htmlForSelector += '</select>';

                    htmlForSelector += '</div>';

                    var $htmlForSelector = $(htmlForSelector);

                    $htmlForSelector.find('select').change(scope.comboChanged);

                    $html.append($htmlForSelector);

                    scope.$watch('showIt', function(newValue) {
                        if (newValue) {
                            $htmlForSelector.show();
                        } else {
                            $htmlForSelector.hide();
                        }
                    });

                });

                element.append($html);

            });

        }
    };
});
