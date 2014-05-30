var materialSearchUrl = '/material/search/';
var sharedDataApiUrl = '/api/shareddataapi/';
var remoteMachineApiUrl = '/api/remotemachineapi/';
var departmentApiUrl = '/api/departmentapi/';
var customerApiUrl = '/api/customerapi/'; 



var customerApp = angular.module('customerApp', ['ui.bootstrap']);

customerApp.factory('notificationFactory', function () {

    return {
        success: function () {
            toastr.success("Success");
        },
        error: function (text) {
            toastr.error(text, "Error!");
        }
    };
});
 
customerApp.factory('baseControllerFactory', function (notificationFactory) {
    return {
        errorCallback: function (data, status, haders, config) {
            notificationFactory.error(data.ExceptionMessage);
        }
    };
});

customerApp.factory('eventFactory', function ($rootScope) {
    var eventFactory = {};

    eventFactory.pagingModel = { currentPageNumber: null, totalCount: null, pageSize: null };

    eventFactory.firePagingModelInitiliaze = function (pagingModel) {
        this.pagingModel = pagingModel;

        $rootScope.$broadcast('pagingModelInitiliazeEventHandler');
    };

    eventFactory.firePageChanged = function (pageChangedModel) {
        this.pagingModel.currentPageNumber = pageChangedModel.currentPageNumber;

        $rootScope.$broadcast('pageChangedEventHandler');
    };

    return eventFactory;
});

customerApp.controller('paginationController', function ($scope, eventFactory) {

    $scope.pageSize = null;

    $scope.totalCount = null;

    $scope.currentPageNumber = 1;

    $scope.setPageNumber = function (pageNo) {
        $scope.currentPageNumber = pageNo;
    };

    $scope.pageChanged = function () {
        eventFactory.firePageChanged({ currentPageNumber: $scope.currentPageNumber });
    };

    $scope.$on('pagingModelInitiliazeEventHandler', function () {
        $scope.totalCount = eventFactory.pagingModel.totalCount;

        $scope.pageSize = eventFactory.pagingModel.pageSize;
    });

});



