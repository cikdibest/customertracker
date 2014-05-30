
customerApp.controller('departmentController', function ($scope, departmentFactory, notificationFactory, baseControllerFactory, modalService) {

    var getDepartmentsSuccessCallback = function (data, status) {
        $scope.departments = data;
    };

    var successPostCallback = function (data, status, headers, config) {
        successCallbackWhenFormEdit(data, status, headers, config).success(function () {
            $scope.toggleAddMode();
            $scope.department = {};
        });
    };

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
        departmentFactory.addDepartment($scope.department).success(successPostCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.deleteDepartment = function (department) {
        var modalOptions = {
            closeButtonText: 'Cancel',
            actionButtonText: 'Delete Row',
            headerText: 'Delete ' + department.Name + '?',
            bodyText: 'Are you sure you want to delete this row?'
        };

        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            departmentFactory.deleteDepartment(department).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });

        
    };

    $scope.updateDepartment = function (department) {
        departmentFactory.updateDepartment(department).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadDepartments = function () {
       return  departmentFactory.getDepartments().success(getDepartmentsSuccessCallback).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
        $scope.loadDepartments();
    };

    $scope.init();
});
