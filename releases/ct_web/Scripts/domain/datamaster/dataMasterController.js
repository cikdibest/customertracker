
customerApp.controller('dataMasterController', function ($scope, dataMasterFactory, customerFactory, dataDetailFactory, notificationFactory, baseControllerFactory, modalService) {

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        $scope.activeDataMaster = null;

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
        var modalOptions = modalService.getDeleteConfirmationModal(dataMaster.Name);
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
        $scope.dataDetail.DataMasterId = $scope.activeDataMaster.Id;
        dataDetailFactory.addDataDetail($scope.dataDetail).success(function (data) {
            $scope.activeDataMaster.DataDetails.push(data);
            $scope.dataDetail = {};
            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.updateDataDetail = function (dataDetail) {
        dataDetailFactory.updateDataDetail(dataDetail).success(function (data) {
            //burada load datadetail çaışması gerkebilir
            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteDataDetail = function (dataDetail) {

        dataDetailFactory.deleteDataDetail(dataDetail).success(function (data) {
            //burada load datadetail çaışması gerkebilir 
            var index = $scope.activeDataMaster.DataDetails.indexOf(dataDetail);
            $scope.activeDataMaster.DataDetails.splice(index, 1);

            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {

        $scope.loadCustomers();

        $scope.loadDataMasters();


    };

    $scope.init();
});
