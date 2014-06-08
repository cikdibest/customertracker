
customerApp.controller('userController', function ($scope, userFactory, notificationFactory, baseControllerFactory, modalService) {
       
    $scope.users = [];
     
    $scope.totalCount = 0;
    $scope.pageNumber = 1;
    $scope.pageSize = 10;
    $scope.sortedBy = 'id',
    $scope.sortDir = 'asc';
    $scope.$on('pageChangedEventHandler', function (instance, currentPageNumber) {
        $scope.pageNumber = currentPageNumber;
        $scope.loadUsers();
    });
    
    var successCallbackWhenFormEdit = function (data, status, headers, config) {
        notificationFactory.success();

        return $scope.loadUsers();
    };
     
    $scope.toggleEditMode = function (user) {
        user.editMode = !user.editMode;
    };
     
    $scope.deleteUser = function (user) {
       
        var modalOptions = modalService.getStandartDeleteModal(user.FullName);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            userFactory.deleteUser(user).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });

        
    };

    $scope.updateUser = function (user) {
        userFactory.updateUser(user).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadUsers = function () {
        return userFactory.getUsers($scope.pageNumber, $scope.pageSize, $scope.sortedBy, $scope.sortDir).success(function (data, status) {
            $scope.users = data.users;
            $scope.totalCount = data.totalCount;
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.init = function() {
        $scope.loadUsers();
    };

    $scope.init();
});
