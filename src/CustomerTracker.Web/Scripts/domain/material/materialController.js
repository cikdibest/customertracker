

customerApp.controller('materialController', function ($scope, materialFactory, customerFactory, communicationFactory, sharedFactory, departmentFactory, notificationFactory, baseControllerFactory) {

    $scope.searchResult = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {};

    $scope.selectedCustomer = null;

    $scope.selectedMaterialIndex = null;

    $scope.addCommunicationMode = false;
    
    $scope.genders = {};
    
    $scope.departments = {};
      

    $scope.setActiveSearchType = function (key, value) {
        $scope.activeSearchType = { Key: key, Value: value };
    };

    $scope.searchMaterials = function () {
        $scope.selectedCustomer = null;

        $scope.selectedMaterialIndex = null;

        materialFactory.searchMaterials({ searchCriteria: $scope.searchCriteria, searchTypeId: $scope.activeSearchType.Key })
                             .success(function (data) {
                                 $scope.searchResult = data;
                             })
                             .error(baseControllerFactory.errorCallback);

    };

    $scope.loadCustomer = function (customerId, index) {
        customerFactory.getCustomerAdvancedDetail(customerId)
                             .success(function (data) {
                                 $scope.selectedCustomer = data;
                                 $scope.selectedMaterialIndex = index;
                             })
                             .error(baseControllerFactory.errorCallback);

    };

    $scope.toogleAddCommunicationMode = function () {
        $scope.addCommunicationMode = !$scope.addCommunicationMode;
    };

    $scope.loadGenders = function() {
        sharedFactory.getSelectorGenders()
            .success(function (data) {
                $scope.genders = data;
            })
            .error(baseControllerFactory.errorCallback);
    };
    
    $scope.loadDepartments = function () {
        departmentFactory.getSelectorDepartments()
            .success(function (data) {
                $scope.departments = data;
            })
            .error(baseControllerFactory.errorCallback);
    };

    $scope.addCommunication = function () {
        var customerId = $scope.selectedCustomer.Id;
        $scope.communication.customerId = customerId;
        communicationFactory.addCommunication($scope.communication)
            .success(function (data, status, headers, config) {
                $scope.toogleAddCommunicationMode();
                $scope.communication = {};
                notificationFactory.success();
                return $scope.loadCustomer(customerId, 0);
            })
            .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

        $scope.loadGenders();

        $scope.loadDepartments();
    };

    $scope.init();
});



