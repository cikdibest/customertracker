
//var materialSearchUrl = '/material/search/getMaterials';

var materialApiUrl = {
    searchmaterials: '/api/materialapi/searchmaterials/',
 

};

var dataMasterApiUrl = {
    getdatamasters: '/api/datamasterapi/getdatamasters/',
    getdatamaster: '/api/datamasterapi/getdatamaster/',
    putdatamaster: '/api/datamasterapi/putdatamaster/',
    postdatamaster: '/api/datamasterapi/postdatamaster/',
    deletedatamaster: '/api/datamasterapi/deletedatamaster/',
};

var dataDetailApiUrl = {
    getdatadetails: '/api/datadetailapi/getdatadetails/',
    getdatadetail: '/api/datadetailapi/getdatadetail/',
    putdatadetail: '/api/datadetailapi/putdatadetail/',
    postdatadetail: '/api/datadetailapi/postdatadetail/',
    deletedatadetail: '/api/datadetailapi/deletedatadetail/',
};

var sharedDataApiUrl = {
    getselectorcities: '/api/shareddataapi/getselectorcities/',
    getselectorgenders: '/api/shareddataapi/getselectorgenders/',
};

var remoteMachineConnectionTypeApiUrl = {
    getselectorremotemachineconnectiontypes: '/api/remotemachineconnectiontypeapi/getselectorremotemachineconnectiontypes/',
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
    getselectordepartments: '/api/departmentapi/getselectordepartments/',
    
};

var customerApiUrl = {
    getcustomers: '/api/customerapi/getcustomers/',
    getcustomer: '/api/customerapi/getcustomer/',
    putcustomer: '/api/customerapi/putcustomer/',
    postcustomer: '/api/customerapi/postcustomer/',
    deletecustomer: '/api/customerapi/deletecustomer/',
    getselectorcustomers: '/api/customerapi/getselectorcustomers/',
    getcustomeradvanceddetail: '/api/customerapi/getcustomeradvanceddetail/',
};

var communicationApiUrl = {
    getcommunications: '/api/communicationapi/getcommunications/',
    getcommunication: '/api/communicationapi/getcommunication/',
    putcommunication: '/api/communicationapi/putcommunication/',
    postcommunication: '/api/communicationapi/postcommunication/',
    deletecommunication: '/api/communicationapi/deletecommunication/',
};

angular.module('SharedServices', [])
    .config(function ($httpProvider) {
        $httpProvider.responseInterceptors.push('myHttpInterceptor');
        var spinnerFunction = function (data, headersGetter) {
            // todo start the spinner here
            //alert('start spinner');
            $('#mydiv').show();
            return data;
        };
        $httpProvider.defaults.transformRequest.push(spinnerFunction);
    })
// register the interceptor as a service, intercepts ALL angular ajax http calls
    .factory('myHttpInterceptor', function ($q, $window) {
        return function (promise) {
            return promise.then(function (response) {
                // do something on success
                // todo hide the spinner
                //alert('stop spinner');
                $('#mydiv').hide();
                return response;

            }, function (response) {
                // do something on error
                // todo hide the spinner
                //alert('stop spinner');
                $('#mydiv').hide();
                return $q.reject(response);
            });
        };
    });

var customerApp = angular.module('customerApp', ['ui.bootstrap', 'SharedServices']);

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

customerApp.filter('getById', function () {
    return function(input, id) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i].Id == +id) {
                return input[i];
            }
        }
        return null;
    };
});

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

            if (status==403) {
                notificationFactory.error(config.method + ' işlemi için yetkiniz yok.');
                return;
            }

            notificationFactory.error(data.Message);
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

