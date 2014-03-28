tradesApp.controller('dashController', function ($scope) {
    
    // Slow page
    // 1000, 200 - 1.4 mins - response 1 sec
    // 3000, 20 - 26 seconds - 10 sec resonse time
    // 2000, 20 - 15 seconds - 5 sec response time
    // 2000, 2 - 5 seconds - 5 sec response time
    // 3000, 2 - 10 seconds - 10 sec response time
    // Rough theory
    //  The number of items in the drop down impacts render time significantly
    //  The number of repeated combos impacts the response time significantly

    // Fast page
    // 5000, 200 - 20 seconds - page ok
    // 8000, 200 - 26 seconds - page unusable

    var boxCount = 3000,
        comboCount = 20;

    var data = [];
    for (var i = 0; i < boxCount; i++) {
        var dataItem = 
            {
                Name: 'Name ' + i,
                Value: i,
                Children: []
            };

        for (var j = 0; j < comboCount; j++) {
            dataItem.Children.push(
                {
                    Display: 'Element ' + i + ', ' + j,
                    Value: j
                });
        }

        data.push(dataItem);

    }
    
    $scope.showIt = true;

 
    $scope.comboChanged = function () {
        alert("Mr. Angular controller says... dashController: comboChanged");
    };
    
    $scope.getData = function() {
        return data;
    };
    
});
