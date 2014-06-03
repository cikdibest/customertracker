

customerApp.controller('materialController', function ($scope, $filter, materialFactory, customerFactory, remoteMachineFactory, communicationFactory,modalService ,remoteMachineConnectionTypeFactory, sharedFactory, departmentFactory, notificationFactory, baseControllerFactory) {

    $scope.searchResult = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {};

    $scope.selectedCustomer = null;

    $scope.selectedMaterialIndex = null;

    $scope.communicationAddMode = false;
    
    $scope.remoteMachineAddMode = false;
    
    $scope.genders = {};
    
    $scope.departments = {};

    $scope.remoteMachineConnectionTypes = {};
       
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

    $scope.toogleCommunicationAddMode = function () {
        $scope.communicationAddMode = !$scope.communicationAddMode;
    };

    $scope.toggleCommunicationEditMode = function (communication) {
        communication.editMode = !communication.editMode;
    };
            
    $scope.toggleRemoteMachineAddMode = function () {
        $scope.remoteMachineAddMode = !$scope.remoteMachineAddMode;
    };
           
    $scope.toggleRemoteMachineEditMode = function (remoteMachine) {
        remoteMachine.editMode = !remoteMachine.editMode;
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
    
    $scope.loadRemoteMachineConnectionTypes = function () {
        remoteMachineConnectionTypeFactory.getSelectorRemoteMachineConnectionTypes()
            .success(function (data) {
                $scope.remoteMachineConnectionTypes = data;
            })
            .error(baseControllerFactory.errorCallback);
    };

    $scope.addCommunication = function () {
        var customerId = $scope.selectedCustomer.Id;
        $scope.communication.CustomerId = customerId;
        communicationFactory.addCommunication($scope.communication)
            .success(function (data, status, headers, config) {
                $scope.toogleCommunicationAddMode();
                $scope.communication = {};
                notificationFactory.success();
                return $scope.loadCustomer(customerId, 0);
            })
            .error(baseControllerFactory.errorCallback);
    };
     
    $scope.updateCommunication = function (communication) {
        communicationFactory.updateCommunication(communication).success(function (data, status, headers, config) {
            $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);
    };
    
    $scope.deleteCommunication = function (communication) {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + communication.FullName + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            communicationFactory.deleteCommunication(communication).success(function (data, status, headers, config) {
                $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
                notificationFactory.success();
            }).error(baseControllerFactory.errorCallback);
        });
           
     
    };
    
    $scope.addRemoteMachine = function () {
        var customerId = $scope.selectedCustomer.Id;
        $scope.remoteMachine.CustomerId = customerId;
        remoteMachineFactory.addRemoteMachine($scope.remoteMachine)
            .success(function (data, status, headers, config) {
                $scope.toggleRemoteMachineAddMode();
                $scope.remoteMachine = {};
                notificationFactory.success();
                return $scope.loadCustomer(customerId, $scope.selectedMaterialIndex);
            })
            .error(baseControllerFactory.errorCallback);
    };

    $scope.updateRemoteMachine = function (remoteMachine) {
        remoteMachineFactory.updateRemoteMachine(remoteMachine).success(function (data, status, headers, config) {
            $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);
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

            remoteMachineFactory.deleteRemoteMachine(remoteMachine).success(function (data, status, headers, config) {
                $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
                notificationFactory.success();
            }).error(baseControllerFactory.errorCallback);
        });


    };

     
    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

        $scope.loadGenders();

        $scope.loadDepartments();

        $scope.loadRemoteMachineConnectionTypes();
    };

    $scope.init();
});



