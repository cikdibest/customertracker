
customerApp.controller('departmentController', function ($scope, departmentFactory, notificationFactory, baseControllerFactory, modalService) {
      
    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadDepartments();
    };

    $scope.departments = [];

    $scope.addMode = false;

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
    };

    $scope.toggleEditMode = function (department) {
        department.editMode = !department.editMode;
    };

    $scope.addDepartment = function () {
        departmentFactory.addDepartment($scope.department).success(function (data, status, headers, config) {
            successCallbackWhenFormEdit(data, status, headers, config).success(function () {
                $scope.toggleAddMode();
                $scope.department = {};
            });
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteDepartment = function (department) {
        var modalOptions = modalService.getDeleteConfirmationModal(department.Name);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            departmentFactory.deleteDepartment(department).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });

        
    };

    $scope.updateDepartment = function (department) {
        departmentFactory.updateDepartment(department).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadDepartments = function () {
        return departmentFactory.getDepartments().success(function (data, status) {
            $scope.departments = data;
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
        $scope.loadDepartments();
    };

    $scope.init();
});
