'use strict';

tradesApp.factory('allocationsSummariesService', function ($resource, $q) {
    var resource = $resource('/odata/AllocationSummaries?:odataParam', { odataParam: '@odataParam' });
    return {
        getAllocationSummaries: function (odataParam) {
            var deferred = $q.defer();
            resource.get({ odataParam: odataParam },
                function (allocationSummaries) {
                    deferred.resolve(allocationSummaries);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        }
    };
});

