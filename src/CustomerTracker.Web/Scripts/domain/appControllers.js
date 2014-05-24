

var customerApp = angular.module('customerApp', []);

customerApp.controller('landingIndexController', function ($scope, $http) {

    $scope.searchResults = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = 'customer';

    $scope.setSearchType = function (searchType) {
        $scope.activeSearchType = searchType;
    };

    $scope.searchClicked = function (item, events) {
          
        var response = $http.post('/landingpage/search', { searchCriteria: $scope.searchCriteria, searchType: $scope.activeSearchType });

        response.success(function (data, status, headers, config) {
            $scope.searchResults = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });

    };

});