
var departmentApiUrl = '/api/departmentapi/';
var customerApiUrl = '/api/customerapi/';
var searchMaterialUrl = '/searchmaterial/search/';
var getCitiesUrl = '/api/cityapi/';
 
 
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

customerApp.factory('searchMaterialFactory', function ($http) {
    return {
        getMaterials: function (data) {
            return $http.post(searchMaterialUrl, data);
        },

        getCustomer: function (url) {
            return $http.get(url);
        }

    };
});

customerApp.factory('departmentFactory', function ($http) {
    return {
        getDepartments: function () {
            return $http.get(departmentApiUrl);
        },
        addDepartment: function (department) {
            return $http.post(departmentApiUrl, department);
        },
        deleteDepartment: function (department) {
            return $http.delete(departmentApiUrl + department.Id);
        },
        updateDepartment: function (department) {
            return $http.put(departmentApiUrl + department.Id, department);
        }

    };
});
 
customerApp.factory('baseControllerFactory', function (notificationFactory) {
    return {
        errorCallback: function(data, status, haders, config) {
            notificationFactory.error(data.ExceptionMessage);
        }
    };
});

customerApp.factory('customerFactory', function ($http) {
    return {
        getCustomers: function () {
            return $http.get(customerApiUrl);
        },
        addCustomer: function (customer) {
            return $http.post(customerApiUrl, customer);
        },
        deleteCustomer: function (customer) {
            return $http.delete(customerApiUrl + customer.Id);
        },
        updateCustomer: function (customer) {
            return $http.put(customerApiUrl + customer.Id, customer);
        },
        getCities: function () {
            return $http.get(getCitiesUrl);
        }

    };
});
 


customerApp.controller('paginationController', function($scope) {
      
    $scope.setPage = function(pageNo) {
        $scope.currentPage = pageNo;
    };

    $scope.pageChanged = function() {
        console.log('Page changed to: ' + $scope.currentPage);
    };

    $scope.maxSize = 10;
    
    $scope.totalItems = 175;

    $scope.currentPage = 1;
});

customerApp.controller('searchMaterialController', function ($scope, searchMaterialFactory, notificationFactory, baseControllerFactory) {
     
    $scope.foundMaterials = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {};

    $scope.selectedCustomer = null;

    $scope.selectedMaterialIndex = null;

    $scope.setActiveSearchType = function (key, value) {
        $scope.activeSearchType = { Key: key, Value: value };
    };

    $scope.searchMaterials = function () {
        $scope.selectedCustomer = null;

        $scope.selectedMaterialIndex = null;

        searchMaterialFactory.getMaterials({ searchCriteria: $scope.searchCriteria, searchTypeId: $scope.activeSearchType.Key })
                             .success(function (data) { $scope.foundMaterials = data; })
                             .error(baseControllerFactory.errorCallback);

    };

    $scope.loadCustomer = function (item, index) {
         
        searchMaterialFactory.getCustomer(item.Url)
                             .success(function (data) { $scope.selectedCustomer = data; $scope.selectedMaterialIndex = index; })
                             .error(baseControllerFactory.errorCallback);
         
    };

    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

    };

    $scope.init();
});

customerApp.controller('departmentController', function ($scope, departmentFactory, notificationFactory, baseControllerFactory) {

    var getDepartmentsSuccessCallback = function (data, status) {
        $scope.departments = data;
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallback(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.department = {};
        });
    };

    var successCallback = function (data, status, headers, config) {
        notificationFactory.success();

        return departmentFactory.getDepartments().success(getDepartmentsSuccessCallback).error(baseControllerFactory.errorCallback);
    };
     
    $scope.departments = [];

    $scope.addMode = false;

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (department) {
        department.editMode = !department.editMode;
    };

    $scope.addDepartment = function () {
        departmentFactory.addDepartment($scope.department).success(successPostCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteDepartment = function (department) {
        departmentFactory.deleteDepartment(department).success(successCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.updateDepartment = function (department) {
        departmentFactory.updateDepartment(department).success(successCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {
        departmentFactory.getDepartments().success(getDepartmentsSuccessCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.init();
});

customerApp.controller('customerController', function ($scope, customerFactory, notificationFactory, baseControllerFactory) {


    var getCustomersSuccessCallback = function (data, status) {
        $scope.customers = data;
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallback(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.customer = {};
        });
    };

    var successCallback = function (data, status, headers, config) {
        notificationFactory.success();

        return customerFactory.getCustomers().success(getCustomersSuccessCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.customers = [];
    
    $scope.cities = [];

    $scope.addMode = false;

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (customer) {
        customer.editMode = !customer.editMode;
    };

    $scope.addCustomer = function () {
        customerFactory.addCustomer($scope.customer).success(successPostCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteCustomer = function (customer) {
        customerFactory.deleteCustomer(customer).success(successCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.updateCustomer = function (customer) {
        customerFactory.updateCustomer(customer).success(successCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {
        customerFactory.getCustomers().success(getCustomersSuccessCallback).error(baseControllerFactory.errorCallback);

        customerFactory.getCities().success(function(data) {
            $scope.cities = data;
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.init();
});

