
var departmentApiUrl = '/api/DepartmentApi/';

var customerApp = angular.module('customerApp', []);

customerApp.controller('searchController', function ($scope, $http) {

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

customerApp.factory('departmentFactory', function($http) {
    return {
        getDepartments: function() {
            return $http.get(departmentApiUrl);
        },
        addDepartment: function (department) {
            return $http.post(departmentApiUrl, department);
        },
        deleteDepartment: function (department) {
            return $http.delete(departmentApiUrl + department.Id);
        },
        updateDepartment: function(department) {
            return $http.put(departmentApiUrl + department.Id, department);
        }

};
});

customerApp.controller('departmentController', function ($scope, departmentFactory, notificationFactory) {

    $scope.departments = [];
    
    $scope.addMode = false;

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };
    
    $scope.toggleEditMode = function (department) {
        department.editMode = !department.editMode;
    };
     
    var getDepartmentsSuccessCallback = function (data, status) {
        $scope.departments = data;
    };
     
    var successCallback = function(data, status, headers, config) {
        notificationFactory.success();

        return departmentFactory.getDepartments().success(getDepartmentsSuccessCallback).error(errorCallback);
    };

    var successPostCallback = function(data, status, headers, config) {
        successCallback(data, status, headers, config).success(function() {
            $scope.toggleAddMode();
            $scope.department = {};
        });
    };

    var errorCallback = function(data,status,haders,config) {
        notificationFactory.error(data.ExceptionMessage);
    };

    departmentFactory.getDepartments().success(getDepartmentsSuccessCallback).error(errorCallback);

    $scope.addDepartment = function() {
        departmentFactory.addDepartment($scope.department).success(successPostCallback).error(errorCallback);
    };
    
    $scope.deleteDepartment = function (department) {
        departmentFactory.deleteDepartment(department).success(successCallback).error(errorCallback);
    };
    
    $scope.updateDepartment = function (department) {
        departmentFactory.updateDepartment(department).success(successCallback).error(errorCallback);
    };
});