

customerApp.controller('materialController', function ($scope, $filter, materialFactory, customerFactory, remoteMachineFactory, communicationFactory, modalService, productFactory, remoteMachineConnectionTypeFactory, sharedFactory, departmentFactory, notificationFactory, baseControllerFactory) {

    $scope.searchResult = {};

    $scope.searchCriteria = '';

    $scope.activeSearchType = {};

    $scope.selectedCustomer = null;

    $scope.selectedMaterialIndex = null;

    $scope.communicationAddMode = false;

    $scope.remoteMachineAddMode = false;

    $scope.customerEditMode = false;

    $scope.genders = {};

    $scope.departments = {};

    $scope.allProducts = {};

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
                                 $scope.selectedCustomer = {};//bu satırın sebebi; müşteri değiştrilidği halde eski müşterinin logusu kalıyordu.
                                 $scope.selectedCustomer = data;
                                 $scope.loadProducts();
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

    $scope.toggleCustomerEditMode = function () {
        $scope.customerEditMode = !$scope.customerEditMode;
    };



    $scope.loadGenders = function () {
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
   
        var modalOptions = modalService.getStandartDeleteModal(communication.FullName);
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
   
        var modalOptions = modalService.getStandartDeleteModal(remoteMachine.DecryptedName);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            remoteMachineFactory.deleteRemoteMachine(remoteMachine).success(function (data, status, headers, config) {
                $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
                notificationFactory.success();
            }).error(baseControllerFactory.errorCallback);
        });


    };
     
    $scope.loadProducts = function () {
        productFactory.getProducts()
            .success(function (data) {
                $scope.allProducts = data;
            })
            .error(baseControllerFactory.errorCallback);
    };

    $scope.filterCanAddProductToCustomer = function (product) {

        if ($scope.selectedCustomer==null) {
            return false;
        }
        return product.ParentProductId != null && _.where($scope.selectedCustomer.Products, { Id: product.Id }).length==0;
    };

    $scope.addProductToCustomer = function () {
        var customerId = $scope.selectedCustomer.Id;
        var productId = $scope.newProduct.Id;
        if (angular.isUndefined(productId)) {
            notificationFactory.error('Ürün seçiniz');
            return;
        }
        customerFactory.addProductToCustomer({ customerId: customerId, productId: productId })
          .success(function (data, status, headers, config) {
              $scope.newProduct = {};
              notificationFactory.success();
              return $scope.loadCustomer(customerId, $scope.selectedMaterialIndex);
          })
          .error(baseControllerFactory.errorCallback);
    };

    $scope.removeProductFromCustomer = function (product) {
        var customerId = $scope.selectedCustomer.Id;
        var productId = product.Id;
        customerFactory.removeProductFromCustomer({ customerId: customerId, productId: productId }).success(function (data, status, headers, config) {
            $scope.loadCustomer($scope.selectedCustomer.Id, $scope.selectedMaterialIndex);
            notificationFactory.success();
        }).error(baseControllerFactory.errorCallback);

    };
     
    $scope.init = function () {
        $scope.setActiveSearchType(1, "Customer");

        $scope.loadGenders();

        $scope.loadDepartments();

        $scope.loadRemoteMachineConnectionTypes();
         

    };

    $scope.init();
});



