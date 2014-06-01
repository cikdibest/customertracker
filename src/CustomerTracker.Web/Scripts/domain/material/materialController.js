

customerApp.controller('materialController', function ($scope, materialFactory, customerFactory, communicationFactory,sharedFactory ,notificationFactory, baseControllerFactory) {

    $scope.searchResult = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {};

    $scope.selectedCustomer = null;

    $scope.selectedMaterialIndex = null;

    $scope.addCommunicationMode = false;
    
    $scope.genders = {};

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

    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

        $scope.loadGenders();
    };

    $scope.init();
});



