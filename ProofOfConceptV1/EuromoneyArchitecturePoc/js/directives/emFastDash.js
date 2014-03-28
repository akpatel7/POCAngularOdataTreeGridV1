
'use strict';
tradesApp.directive('emFastDash', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            loaded: "=",
            config: '=',
            data: '=',
        },
        link: function (scope, element, attrs) {

            scope.$watch('config', function (newValue) {

                element.children().empty();

                var html = '';

                jQuery.each(scope.data, function (dataIndex, data) {
                    html += '<div class=\'dash-box\'><div>' + data.Name + '</div><div>' + data.Value + '</div>';

                    //html += '<select onchange="alert(this.options[this.selectedIndex].value);">';
                    html += '<select>';

                    jQuery.each(data.Children, function (dataItemIndex, dataItem) {
                        html += '<option value=\'' + dataItem.Value + '\'>' + dataItem.Display + '</option>';
                    });
                    
                    html += '</select>';

                    html += '</div>';
                });

                element.append(html);

            });

        }
    };
});
