

customerApp.controller('customerController', function ($scope, customerFactory, notificationFactory, baseControllerFactory, eventFactory, modalService, requestHandler) {
     
    $scope.customers = [];

    $scope.filterCriteria = {
        pageNumber: 1,
        pageSize: 10,
        sortedBy: 'id',
        sortDir: 'asc',
    };

    $scope.cities = [];

    $scope.addMode = false;
    
    var getCustomersSuccessCallback = function (data, status) {
        
        $scope.customers = data.customers;

        eventFactory.firePagingModelInitiliaze({ totalCount: data.totalCount, pageSize: $scope.filterCriteria.pageSize });
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallbackWhenFormEdit(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.customer = {};
        });
    };

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadCustomers();
    };

    $scope.$on('pageChangedEventHandler', function () {
        $scope.filterCriteria.pageNumber = eventFactory.pagingModel.currentPageNumber;

        $scope.loadCustomers();
    });
     
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
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + customer.Title + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            customerFactory.deleteCustomer(customer).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });
     
    };

    $scope.updateCustomer = function (customer) {
        customerFactory.updateCustomer(customer).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadCustomers = function () {
      //return  requestHandler({
      //      method: 'GET',
      //      url: customerApiUrl.getcustomers,
      //      params: { pageNumber: $scope.filterCriteria.pageNumber, pageSize: $scope.filterCriteria.pageSize, sortBy: $scope.filterCriteria.sortedBy, sortDir: $scope.filterCriteria.sortDir }
      //      // stopNotification: true // will stop the notification from being displayed.
      //}).success(getCustomersSuccessCallback)
      // .error(baseControllerFactory.errorCallback);

        return customerFactory.getCustomers($scope.filterCriteria.pageNumber, $scope.filterCriteria.pageSize, $scope.filterCriteria.sortedBy, $scope.filterCriteria.sortDir)
                         .success(getCustomersSuccessCallback)
                         .error(baseControllerFactory.errorCallback);
    };

    $scope.loadCities = function () {
        customerFactory.getCities()
                       .success(function (data) { $scope.cities = data; })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
        $scope.loadCustomers();

        $scope.loadCities();
    };

    $scope.init();

});