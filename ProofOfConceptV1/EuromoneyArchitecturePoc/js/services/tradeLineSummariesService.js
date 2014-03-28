'use strict';

tradesApp.factory('tradeLineSummariesService', function ($resource, $q) {
    var resource = $resource('/odata/TradeLineSummaries(:id)', { id: '@id' });
    return {
        getTradeLineSummaries: function (tradeLineId) {
            var deferred = $q.defer();
            resource.get({ id: tradeLineId },
                function (tradeLineSummaries) {
                    deferred.resolve(tradeLineSummaries);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }
    };
});

