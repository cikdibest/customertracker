
customerApp.controller('dataMasterController', function ($scope, dataMasterFactory,customerFactory ,notificationFactory, baseControllerFactory, modalService) {

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadDataMasters();
    };

    $scope.dataMasters = [];

    $scope.activeDataMaster = null;

    $scope.addMode = false;
     
    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (dataMaster) {
        dataMaster.editMode = !dataMaster.editMode;
    };

    $scope.setActiveDataMaster = function (dataMaster) {
        $scope.activeDataMaster = dataMaster;
    };

    $scope.addDataMaster = function () {
        dataMasterFactory.addDataMaster($scope.dataMaster).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleAddMode();
                $scope.dataMaster = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteDataMaster = function (dataMaster) {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + dataMaster.Name + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            dataMasterFactory.deleteDataMaster(dataMaster).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });


    };

    $scope.updateDataMaster = function (dataMaster) {
        dataMasterFactory.updateDataMaster(dataMaster).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadDataMasters = function () {
        return dataMasterFactory.getDataMasters().success(function (data, status) {
            $scope.dataMasters = data;
        }).error(baseControllerFactory.errorCallback);
    };
    
    $scope.loadCustomers = function () {
        customerFactory.getSelectorCustomers()
                       .success(function (data) { $scope.customers = data; })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.addDataDetail = function () {
        $scope.dataDetail.DataMasterId = activeDataMaster.Id;
        dataMasterFactory.addDataDetail($scope.dataDetail).success().error(baseControllerFactory.errorCallback);
    };
      
    $scope.init = function () {

        $scope.loadCustomers();

        $scope.loadDataMasters();
        

    };

    $scope.init();
});
