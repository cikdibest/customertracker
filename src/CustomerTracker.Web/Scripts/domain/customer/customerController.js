

customerApp.controller('customerController', function ($scope, customerFactory, sharedFactory,notificationFactory, baseControllerFactory, eventFactory, modalService) {
     
    $scope.customers = [];

    $scope.totalCount = 0;
    $scope.pageNumber = 1;
    $scope.pageSize = 10;
    $scope.sortedBy = 'id',
    $scope.sortDir = 'asc'; 
    $scope.$on('pageChangedEventHandler', function (instance, currentPageNumber) {
        $scope.pageNumber = currentPageNumber;
        $scope.loadCustomers();
    });

    $scope.cities = [];

    $scope.addMode = false;
      
    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadCustomers();
    };
     
    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (customer) {
        customer.editMode = !customer.editMode;
    };

    $scope.addCustomer = function () {
        customerFactory.addCustomer($scope.customer).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleAddMode();
                $scope.customer = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteCustomer = function (customer) {
        var modalOptions = modalService.getDeleteConfirmationModal(customer.Name);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            customerFactory.deleteCustomer(customer).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });
     
    };

    $scope.updateCustomer = function (customer) {
        customerFactory.updateCustomer(customer).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadCustomers = function () {
        return customerFactory.getCustomers($scope.pageNumber, $scope.pageSize, $scope.sortedBy, $scope.sortDir)
                         .success(function (data) {
                             $scope.customers = data.customers;
                             $scope.totalCount = data.totalCount;
                         })
                         .error(baseControllerFactory.errorCallback);
    };

    $scope.loadCities = function () {
        sharedFactory.getSelectorCities()
                       .success(function (data) { $scope.cities = data; })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
       
        $scope.loadCities();
        
        $scope.loadCustomers();

    };

    $scope.init();

});