 
customerApp.controller('remoteMachineController', function ($scope, remoteMachineFactory, customerFactory,remoteMachineConnectionTypeFactory, notificationFactory, baseControllerFactory, eventFactory, modalService) {
     
    $scope.remoteMachines = [];

    $scope.customers = [];

    $scope.remoteConnectionTypes = [];

    $scope.addMode = false;

    $scope.totalCount = 0;
    $scope.pageNumber = 1;
    $scope.pageSize = 10;
    $scope.sortedBy = 'id',
    $scope.sortDir = 'asc'; 
    $scope.$on('pageChangedEventHandler', function (instance, currentPageNumber) {
        $scope.pageNumber = currentPageNumber;
        $scope.loadRemoteMachines();
    });
      
    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadRemoteMachines();
    };
     
    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (remoteMachine) {
        remoteMachine.editMode = !remoteMachine.editMode;
    };

    $scope.addRemoteMachine = function () {
        remoteMachineFactory.addRemoteMachine($scope.remoteMachine).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleAddMode();
                $scope.remoteMachine = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteRemoteMachine = function (remoteMachine) {
         
        var modalOptions = modalService.getStandartDeleteModal(remoteMachine.DecryptedName);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            remoteMachineFactory.deleteRemoteMachine(remoteMachine).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });
         
    };

    $scope.updateRemoteMachine = function (remoteMachine) {
        remoteMachineFactory.updateRemoteMachine(remoteMachine).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadRemoteMachines = function () {
        return remoteMachineFactory.getRemoteMachines($scope.pageNumber, $scope.pageSize, $scope.sortedBy, $scope.sortDir)
                         .success(function (data, status) {
                             $scope.remoteMachines = data.remoteMachines;
                             $scope.totalCount = data.totalCount;
                         })
                         .error(baseControllerFactory.errorCallback);
    };

    $scope.loadCustomers = function () {
        customerFactory.getSelectorCustomers()
                       .success(function (data) { $scope.customers = data; })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.loadRemoteMachineConnectionTypes = function () {
        remoteMachineConnectionTypeFactory.getSelectorRemoteMachineConnectionTypes()
                       .success(function (data) {
                           $scope.remoteMachineConnectionTypes = data;
                       })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () { 
        $scope.loadCustomers();

        $scope.loadRemoteMachineConnectionTypes();
         
        $scope.loadRemoteMachines();
    };

    $scope.init();

});