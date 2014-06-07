
customerApp.controller('productController', function ($scope, productFactory, notificationFactory, baseControllerFactory, modalService) {

    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadProducts();
    };

    $scope.subProducts = [];
     
    $scope.filterIsParentProduct = function (product) {
        return product.ParentProductId == null;
    };
    $scope.filterHasSubProducts = function (product) {
        return product.SubProducts != null;
    };

    $scope.toggleProductEditMode = function (product) {
        product.isEditMode = !product.isEditMode;
    };

    $scope.toggleSubProductCreateMode = function (product) {
        $scope.isSubProductCreateMode = !$scope.isSubProductCreateMode;
        product.isSubProductCreateMode = !product.isSubProductCreateMode;
    };

    $scope.addParentProduct = function () { 
        productFactory.addProduct($scope.parentProduct).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () { 
                $scope.parentProduct = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };
    
    $scope.addSubProduct = function (product) {

        if (angular.isDefined(product)) {
            $scope.subProduct.ParentProductId = product.Id;
        }

        productFactory.addProduct($scope.subProduct).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleSubProductCreateMode(product);
                $scope.subProduct = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteProduct = function (product) {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + product.Name + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            productFactory.deleteProduct(product).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });


    };

    $scope.updateProduct = function (product) {
        productFactory.updateProduct(product).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadProducts = function () {
        return productFactory.getProducts().success(function (data, status) {
            $scope.subProducts = data;
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () {
        $scope.loadProducts();
    };

    $scope.init();
});
