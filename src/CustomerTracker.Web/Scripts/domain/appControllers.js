

var customerApp = angular.module('customerApp', []);

customerApp.controller('landingIndexController', function ($scope, $http, $sce) {

    $scope.searchResults = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = 'customer';
 
    $scope.selectedCustomer = null;

    $scope.selectedIndex = null;
     
    $scope.setSearchType = function (searchType) {
        $scope.activeSearchType = searchType;
    };

    $scope.searchClicked = function (item, events) {
        
        $scope.selectedCustomer = null;
        
        $scope.selectedIndex = null;

        var response = $http.post('/search/search', { searchCriteria: $scope.searchCriteria, searchType: $scope.activeSearchType });

        response.success(function (data, status, headers, config) {
            $scope.searchResults = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });

    };

    $scope.loadDetail = function (item, index) {

        $scope.selectedIndex = index;

        var response = $http.get(item.Url);

        response.success(function (data, status, headers, config) {
            $scope.selectedCustomer = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });
    };

  
});