

customerApp.controller('remoteMachineController', function ($scope, remoteMachineFactory, customerFactory,remoteMachineConnectionTypeFactory, notificationFactory, baseControllerFactory, eventFactory, modalService) {

    var getRemoteMachinesSuccessCallback = function (data, status) {
        $scope.remoteMachines = data.remoteMachines;

        eventFactory.firePagingModelInitiliaze({ totalCount: data.totalCount, pageSize: $scope.filterCriteria.pageSize });
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallbackWhenFormEdit(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.remoteMachine = {};
        });
    };

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadRemoteMachines();
    };

    $scope.remoteMachines = [];

    $scope.customers = [];

    $scope.remoteConnectionTypes = [];

    $scope.addMode = false;

    $scope.filterCriteria = {
        pageNumber: 1,
        pageSize: 10,
        sortedBy: 'id',
        sortDir: 'asc',
    };


    $scope.$on('pageChangedEventHandler', function () {
        $scope.filterCriteria.pageNumber = eventFactory.pagingModel.currentPageNumber;

        $scope.loadRemoteMachines();
    });

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (remoteMachine) {
        remoteMachine.editMode = !remoteMachine.editMode;
    };

    $scope.addRemoteMachine = function () {
        remoteMachineFactory.addRemoteMachine($scope.remoteMachine).success(successPostCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteRemoteMachine = function (remoteMachine) {

        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + remoteMachine.DecryptedName + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            remoteMachineFactory.deleteRemoteMachine(remoteMachine).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });
         
    };

    $scope.updateRemoteMachine = function (remoteMachine) {
        remoteMachineFactory.updateRemoteMachine(remoteMachine).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadRemoteMachines = function () {
        return remoteMachineFactory.getRemoteMachines($scope.filterCriteria.pageNumber, $scope.filterCriteria.pageSize, $scope.filterCriteria.sortedBy, $scope.filterCriteria.sortDir)
                         .success(getRemoteMachinesSuccessCallback)
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
                           $scope.remoteConnectionTypes = data;
                       })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {
        $scope.loadRemoteMachines();

        $scope.loadCustomers();

        $scope.loadRemoteMachineConnectionTypes();
    };

    $scope.init();

});