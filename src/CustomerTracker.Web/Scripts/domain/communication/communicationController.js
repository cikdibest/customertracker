
customerApp.controller('communicationController', function ($scope, communicationFactory, notificationFactory, baseControllerFactory, modalService) {
     
    $scope.communications = [];

    $scope.addMode = false;

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (communication) {
        communication.editMode = !communication.editMode;
    };

    var getCommunicationsSuccessCallback = function (data, status) {
        $scope.communications = data;
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallbackWhenFormEdit(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.communication = {};
        });
    };

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadCommunications();
    };

    $scope.addCommunication = function () {
        communicationFactory.addCommunication($scope.communication).success(successPostCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteCommunication = function (communication) {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + communication.Name + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            communicationFactory.deleteCommunication(communication).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });

        
    };

    $scope.updateCommunication = function (communication) {
        communicationFactory.updateCommunication(communication).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadCommunications = function () {
       return  communicationFactory.getCommunications().success(getCommunicationsSuccessCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
        $scope.loadCommunications();
    };

    $scope.init();
});
