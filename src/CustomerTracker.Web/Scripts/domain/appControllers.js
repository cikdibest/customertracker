
//var materialSearchUrl = '/material/search/getMaterials';

var materialApiUrl = {
    searchmaterials: '/ct/api/materialapi/searchmaterials/',
 

};

var dataMasterApiUrl = {
    getdatamasters: '/ct/api/datamasterapi/getdatamasters/',
    getdatamaster: '/ct/api/datamasterapi/getdatamaster/',
    putdatamaster: '/ct/api/datamasterapi/putdatamaster/',
    postdatamaster: '/ct/api/datamasterapi/postdatamaster/',
    deletedatamaster: '/ct/api/datamasterapi/deletedatamaster/',
};

var dataDetailApiUrl = {
    getdatadetails: '/ct/api/datadetailapi/getdatadetails/',
    getdatadetail: '/ct/api/datadetailapi/getdatadetail/',
    putdatadetail: '/ct/api/datadetailapi/putdatadetail/',
    postdatadetail: '/ct/api/datadetailapi/postdatadetail/',
    deletedatadetail: '/ct/api/datadetailapi/deletedatadetail/',
};

var sharedDataApiUrl = {
    getselectorcities: '/ct/api/shareddataapi/getselectorcities/',
    getselectorgenders: '/ct/api/shareddataapi/getselectorgenders/',
};

var remoteMachineConnectionTypeApiUrl = {
    getselectorremotemachineconnectiontypes: '/ct/api/remotemachineconnectiontypeapi/getselectorremotemachineconnectiontypes/',
};

var remoteMachineApiUrl = {
    getremotemachines: '/ct/api/remotemachineapi/getremotemachines/',
    getremotemachine: '/ct/api/remotemachineapi/getremotemachine/',
    putremotemachine: '/ct/api/remotemachineapi/putremotemachine/',
    postremotemachine: '/ct/api/remotemachineapi/postremotemachine/',
    deleteremotemachine: '/ct/api/remotemachineapi/deleteremotemachine/',
};

var departmentApiUrl = {
    getdepartments: '/ct/api/departmentapi/getdepartments/',
    getdepartment: '/ct/api/departmentapi/getdepartment/',
    putdepartment: '/ct/api/departmentapi/putdepartment/',
    postdepartment: '/ct/api/departmentapi/postdepartment/',
    deletedepartment: '/ct/api/departmentapi/deletedepartment/',
    getselectordepartments: '/ct/api/departmentapi/getselectordepartments/',
    
};

var productApiUrl = {
    getproducts: '/ct/api/productapi/getproducts/',
    getproduct: '/ct/api/productapi/getproduct/',
    putproduct: '/ct/api/productapi/putproduct/',
    postproduct: '/ct/api/productapi/postproduct/',
    deleteproduct: '/ct/api/productapi/deleteproduct/', 
    getselectorsubproducts: '/ct/api/productapi/getselectorsubproducts/',
};


var customerApiUrl = {
    getcustomers: '/ct/api/customerapi/getcustomers/',
    getcustomer: '/ct/api/customerapi/getcustomer/',
    putcustomer: '/ct/api/customerapi/putcustomer/',
    postcustomer: '/ct/api/customerapi/postcustomer/',
    deletecustomer: '/ct/api/customerapi/deletecustomer/',
    getselectorcustomers: '/ct/api/customerapi/getselectorcustomers/',
    getcustomeradvanceddetail: '/ct/api/customerapi/getcustomeradvanceddetail/',
    addproducttocustomer: '/ct/api/customerapi/addproducttocustomer/',
    removeproductfromcustomer: '/ct/api/customerapi/removeproductfromcustomer/',
    
     
};

var communicationApiUrl = {
    getcommunications: '/ct/api/communicationapi/getcommunications/',
    getcommunication: '/ct/api/communicationapi/getcommunication/',
    putcommunication: '/ct/api/communicationapi/putcommunication/',
    postcommunication: '/ct/api/communicationapi/postcommunication/',
    deletecommunication: '/ct/api/communicationapi/deletecommunication/',
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
        templateUrl: '/ct/modal.html'
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
            tempModalDefaults.controller = function($scope, $modalInstance) {
                $scope.modalOptions = tempModalOptions;
                $scope.modalOptions.ok = function(result) {
                    $modalInstance.close('ok');
                };
                $scope.modalOptions.close = function(result) {
                    $modalInstance.close('cancel');
                };
            };
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

