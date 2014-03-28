
'use strict';
tradesApp.directive('ngTradesJqGrid', function (tradesJqGridBuilder) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            loaded: "=",
            config: '=',
            data: '='
        },
        link: function (scope, element, attrs) {

            var gridName = attrs.name,
                gridTableName = gridName + "_table",
                gridPagerName = gridName + "_pager",
                showColumnChooser = attrs.showcolumnchooser != undefined,
                gridLoaded = scope.loaded;
           
            scope.$watch('config', function (newValue) {


                // Get the common configuration for the grid
                var config = tradesJqGridBuilder.buildCommonConfig(gridTableName, gridPagerName);

                // Merge in the config from the controller
                $.extend(config, newValue);
        
                if (gridLoaded != undefined) {
                    config.loadComplete = gridLoaded;
                }

                tradesJqGridBuilder.initialiseGrid(element, gridTableName, gridPagerName, config, showColumnChooser);

            });

        }
    };
});
