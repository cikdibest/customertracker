 
customerApp.controller('solutionController', function ($scope, solutionFactory, customerFactory, sharedFactory, productFactory, notificationFactory, baseControllerFactory, eventFactory, modalService) {
     
    $scope.solutions = [];

    $scope.customers = [];

    $scope.troubles = [];

    $scope.allProducts = [];

    $scope.addMode = false;

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadSolutions();
    };

    $scope.totalCount = 0;
    $scope.pageNumber = 1;
    $scope.pageSize = 10;
    $scope.sortedBy = 'id',
    $scope.sortDir = 'asc';
    $scope.$on('pageChangedEventHandler', function (instance, currentPageNumber) {
        $scope.pageNumber = currentPageNumber;
        $scope.loadSolutions();
    });
     
    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (solution) {
        solution.editMode = !solution.editMode;
    };

    $scope.addSolution = function () {
        solutionFactory.addSolution($scope.solution).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleAddMode();
                $scope.solution = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteSolution = function (solution) {

        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + solution.DecryptedName + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;

            solutionFactory.deleteSolution(solution).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });
         
    };

    $scope.updateSolution = function (solution) {
        solutionFactory.updateSolution(solution).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadSolutions = function () {
        return solutionFactory.getSolutions($scope.pageNumber, $scope.pageSize, $scope.sortedBy, $scope.sortDir)
                         .success(function (data, status) {
                             $scope.solutions = data.solutions;
                             $scope.totalCount = data.totalCount;
                             //eventFactory.firePagingModelInitiliaze({ totalCount: data.totalCount, pageSize: $scope.filterCriteria.pageSize });
                         })
                         .error(baseControllerFactory.errorCallback);
    };

    $scope.loadCustomers = function () {
        customerFactory.getSelectorCustomers()
                       .success(function (data) { $scope.customers = data; })
                       .error(baseControllerFactory.errorCallback);
    };

    $scope.loadTroubles = function () {
        sharedFactory.getSelectorTroubles()
                       .success(function (data) {
                           $scope.troubles = data;
                       })
                       .error(baseControllerFactory.errorCallback);
    };
      
    $scope.loadProducts = function () {
        productFactory.getProducts()
            .success(function (data) {
                $scope.allProducts = data;
            })
            .error(baseControllerFactory.errorCallback);
    };
    
    $scope.filterCanAddProduct = function (product) {

        return product.ParentProductId != null;
    };
     
    $scope.init = function () {
        $scope.loadSolutions();

        $scope.loadCustomers();
         
        $scope.loadProducts();

        $scope.loadTroubles();

    };

    $scope.init();

});