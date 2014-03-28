'use strict';

tradesApp.factory('allocationsService', function ($resource, $q) {
    var resource = $resource('/odata/:entity?:odataParam', { entity: '@entity', odataParam: '@odataParam' });
        return {
        getAllocationSummaries: function (odataParam) {
            var deferred = $q.defer();
            resource.get({ entity: 'AllocationSummaries', odataParam: odataParam },
                function (data) {
                    deferred.resolve(data);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        },
        getPortfolioSummaries: function (odataParam) {
            var deferred = $q.defer();
            resource.get({ entity: 'PortfolioSummaries', odataParam: odataParam },
                function (data) {
                    deferred.resolve(data);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        },
        getAllocationHistories: function (allocationId, odataParam) {
            var deferred = $q.defer();
            resource.get({ entity: 'Allocations(' + allocationId + ')/History', odataParam: odataParam },
                function (data) {
                    deferred.resolve(data);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        },
        
        getPortfolioHistories: function (portfolioId, odataParam) {
        var deferred = $q.defer();
        resource.get({ entity: 'Portfolios(' + portfolioId + ')/History', odataParam: odataParam },
            function (data) {
                deferred.resolve(data);
            },
            function (response) {
                deferred.reject(response);
            });
        return deferred.promise;
    }
    };
});

