tradesApp.controller('allocationsController', function ($scope) {
    
    $scope.allocationsGridConfig = { nige: "hello" },

    $scope.demoConfig1 = {
        colNames: [
            'ID',
            'FAVOURITE',
            'SERVICE',
            'TRADE TYPE',
            'TRADE STRUCTURE',
            'DESCRIPTION / LABEL',
            'ASSET CLASS',
            'DIRECTION',
            
            'INSTRUMENT / OBJECT',
            'INSTRUCTION TYPE',
            'INSTRUCTION',
            'ENTRY',
            'EXIT',
            'START DATE',
            'CLOSE DATE',
            'ABSOLUTE PERFORMANCE',
            'RELATIVE PERFORMANCE',
            'PERFORMANCE BENCHMARK',
            'LAST UPDATED'],
        colModel: [
            { name: "trade_id", key: true, hidden: true },
            { name: "is_favourite", formatter: 'checkbox' },
            { name: "service_code" },
            { name: "length_type_label" },
            { name: "structure_type_label" },
            { name: "description" },
            { name: "asset_class" },
            { name: "direction" },
            
            { name: "instrument" },
            { name: "instruction_type_label" },
            { name: "instruction_label" },
            { name: "instruction_entry" },
            { name: "instruction_exit" },
            
            { name: "instruction_entry_date" },
            { name: "instruction_exit_date" },
            { name: "absolute_performance" },
            { name: "relative_performance" },
            { name: "benchmark_label" },
            { name: "last_updated" }
        ],
        
        //colNames: [
        //    'ID',
        //    'SERVICE',
        //    'TRADE TYPE',
        //    'TRADE STRUCTURE',
        //    'DESCRIPTION / LABEL',
        //    'ASSET CLASS',
        //    'DIRECTION',
        //    'INSTRUMENT / OBJECT',
        //    'INSTRUCTION TYPE',
        //    'INSTRUCTION',
        //    'ENTRY',
        //    'EXIT',
        //    'START DATE',
        //    'CLOSE DATE',
        //    'ABSOLUTE PERFORMANCE',
        //    'RELATIVE PERFORMANCE',
        //    'PERFORMANCE BENCHMARK',
        //    'LAST UPDATED'],
        //colModel: [
        //    { name: "trade_id", key: true, hidden: false },
        //    { name: "service_code" },
        //    { name: "length_type_label" },
        //    { name: "trade_editorial_label" },
        //    { name: "instruction_type_label" },
        //    { name: "trade_id" },
        //    { name: "trade_id" },
        //    { name: "trade_id" },
        //    { name: "instruction_type_label" },
        //    { name: "instruction_label" },
        //    { name: "instruction_entry" },
        //    { name: "instruction_exit" },
        //    { name: "instruction_entry_date" },
        //    { name: "instruction_exit_date" },
        //    { name: "absolute_performance" },
        //    { name: "relative_performance" },
        //    { name: "benchmark_label" },
        //    { name: "last_updated" }
        //],
        //colNames: [
        //          'ID',
        //          'SERVICE',
        //          'TRADE TYPE',
        //        'TRADE TYPE2'
        //         ],
        //colModel: [
        //    { name: "trade_id", key: true, hidden: true },
        //    { name: "service_code" },
        //    { name: "length_type_label" },
        //    { name: "service_code" }
        //],

    sortname: "trade_id",
        caption: "Trades Data Demo 1",

    };

    $scope.moreInfoContentCalled = function (data) {
        alert("allocationsController: moreInfoContentCalled: " + data);
    };

    $scope.gridLoadComplete = function (data) {
        //console.log(data);
    };
    
    $scope.demoConfig2 = {

        colModel: [
            { name: "trade_id" },
            { name: "trade_uri" },
            { name: "relativity_id" },
            { name: "length_type_id" }
        ],

        sortname: "trade_id",
        caption: "Trades Data Demo 2",

    };

});