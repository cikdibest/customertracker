
var materialSearchUrl = '/material/search/getMaterials';

var sharedDataApiUrl = {
    getcities: '/api/shareddataapi/getcities/',
    getremoteconnectiontypes: '/api/shareddataapi/getremoteconnectiontypes/',
};

var remoteMachineApiUrl = {
    getremotemachines: '/api/remotemachineapi/getremotemachines/',
    getremotemachine: '/api/remotemachineapi/getremotemachine/',
    putremotemachine: '/api/remotemachineapi/putremotemachine/',
    postremotemachine: '/api/remotemachineapi/postremotemachine/',
    deleteremotemachine: '/api/remotemachineapi/deleteremotemachine/',
};

var departmentApiUrl = {
    getdepartments: '/api/departmentapi/getdepartments/',
    getdepartment: '/api/departmentapi/getdepartment/',
    putdepartment: '/api/departmentapi/putdepartment/',
    postdepartment: '/api/departmentapi/postdepartment/',
    deletedepartment: '/api/departmentapi/deletedepartment/',
};

var customerApiUrl = {
    getcustomers: '/api/customerapi/getcustomers/',
    getcustomer: '/api/customerapi/getcustomer/',
    putcustomer: '/api/customerapi/putcustomer/',
    postcustomer: '/api/customerapi/postcustomer/',
    deletecustomer: '/api/customerapi/deletecustomer/',
};



var customerApp = angular.module('customerApp', ['ui.bootstrap']);

var modalService = function ($modal) {

    var modalDefaults = {
        backdrop: true,
        keyboard: true,
        modalFade: true,
        templateUrl: '/modal.html'
    };

    var modalOptions = {
        closeButtonText: 'Close',
        actionButtonText: 'OK',
        headerText: 'Proceed?',
        bodyText: 'Perform this action?'
    };

    this.showModal = function (customModalDefaults, customModalOptions) {
        if (!customModalDefaults) customModalDefaults = {};
        customModalDefaults.backdrop = 'static';
        return this.show(customModalDefaults, customModalOptions);
    };

    this.show = function (customModalDefaults, customModalOptions) {
        //Create temp objects to work with since we're in a singleton service
        var tempModalDefaults = {};
        var tempModalOptions = {};

        //Map angular-ui modal custom defaults to modal defaults defined in this service
        angular.extend(tempModalDefaults, modalDefaults, customModalDefaults);

        //Map modal.html $scope custom properties to defaults defined in this service
        angular.extend(tempModalOptions, modalOptions, customModalOptions);

        if (!tempModalDefaults.controller) {
            tempModalDefaults.controller = function ($scope, $modalInstance) {
                $scope.modalOptions = tempModalOptions;
                $scope.modalOptions.ok = function (result) {
                    $modalInstance.close('ok');
                };
                $scope.modalOptions.close = function (result) {
                    $modalInstance.close('cancel');
                };
            }
        }

        return $modal.open(tempModalDefaults).result;
    };

};

customerApp.service('modalService', ['$modal', modalService]);

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

