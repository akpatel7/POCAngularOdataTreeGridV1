
'use strict';
tradesApp.directive('ngTradesKendoGrid', function (tradesKendoGridBuilder) {
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
                showColumnChooser = attrs.showcolumnchooser != undefined,
                gridLoaded = scope.loaded;
            
            scope.$watch('config', function (newValue) {


                // Get the common configuration for the grid
                var config = tradesKendoGridBuilder.buildCommonConfig();

                //// Merge in the config from the controller
                //$.extend(config, newValue);

                //if (gridLoaded != undefined) {
                //    config.loadComplete = gridLoaded;
                //}

                tradesKendoGridBuilder.initialiseGrid(element, gridName, config);

            });

        }
    };
});
