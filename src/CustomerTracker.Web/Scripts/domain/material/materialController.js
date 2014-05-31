

customerApp.controller('materialController', function ($scope, materialFactory, notificationFactory, baseControllerFactory) {

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

        materialFactory.searchMaterials({ searchCriteria: $scope.searchCriteria, searchTypeId: $scope.activeSearchType.Key })
                             .success(function (data) { $scope.foundMaterials = data; })
                             .error(baseControllerFactory.errorCallback);

    };

    //bu methot buradan taşınmalı..get customer cutomerctonollerin sorumlluğudur
    $scope.loadCustomer = function (item, index) {

        materialFactory.getMaterialDetail(item.Url)
                             .success(function (data) {
                                 $scope.selectedCustomer = data;
                                 $scope.selectedMaterialIndex = index;
                             })
                             .error(baseControllerFactory.errorCallback);

    };

    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

    };

    $scope.init();
});



