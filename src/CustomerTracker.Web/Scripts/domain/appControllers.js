
var customerApp = angular.module('customerApp', []);

customerApp.controller('landingIndexController', function ($scope, $http) {

    $scope.searchResults = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {  };
 
    $scope.selectedCustomer = null;

    $scope.selectedResultIndex = null;
     
    $scope.setSearchType = function (key,value) {
        $scope.activeSearchType = { Key: key, Value: value };
    };

    $scope.searchClicked = function (item, events) {
        
        $scope.selectedCustomer = null;
        
        $scope.selectedResultIndex = null;

        var response = $http.post(searchCustomerOrCommunicationOnLandingPage, { searchCriteria: $scope.searchCriteria, searchTypeId: $scope.activeSearchType.Key });

        response.success(function (data, status, headers, config) {
            $scope.searchResults = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });

    };

    $scope.loadCustomerDetail = function (item, index) {

        $scope.selectedResultIndex = index;

        var response = $http.get(item.Url);

        response.success(function (data, status, headers, config) {
            $scope.selectedCustomer = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });
    };

    $scope.init = function() {
        $scope.setSearchType(1, "Customer");

    };

    $scope.init();
});