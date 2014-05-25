

var customerApp = angular.module('customerApp', []);

customerApp.controller('landingIndexController', function ($scope, $http, $sce) {

    $scope.searchResults = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = 'customer';

    $scope.detailView = '<strong>seyhan</strong>';

    $scope.setSearchType = function (searchType) {
        $scope.activeSearchType = searchType;
    };

    $scope.searchClicked = function (item, events) {
          
        var response = $http.post('/search/search', { searchCriteria: $scope.searchCriteria, searchType: $scope.activeSearchType });

        response.success(function (data, status, headers, config) {
            $scope.searchResults = data;
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });

    };

    $scope.loadDetail = function(item,events) {
        var response = $http.get(item.Url);

        response.success(function (data, status, headers, config) {
            $scope.detailView = $sce.trustAsHtml(data);
        });
        response.error(function (data, status, headers, config) {
            alert("AJAX failed!");
        });
    };

    $scope.body = '<div style="width:200px; height:200px; border:1px solid blue;"></div>';
     
    $scope.renderHtml = function (html_code) {
        return $sce.trustAsHtml(html_code);
    };
});