
customerApp.controller('userController', function ($scope, userFactory, sharedFactory, notificationFactory, baseControllerFactory, modalService) {

    $scope.users = [];

    $scope.roles = [];

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
        
        //kullanıcı editleniyorsa , rollerini her defasında set ediyoruz
        if (user.editMode == null || !user.editMode) {

            angular.forEach($scope.roles, function (role, index) {

                var userRoleIds = _.pluck(user.Roles, 'Id');

                role.selected = _.contains(userRoleIds, parseInt(role.Id));

            });

        }
        
        user.editMode = !user.editMode;
    };

    $scope.deleteUser = function (user) {

        var modalOptions = modalService.getDeleteConfirmationModal(user.FullName);
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
            userFactory.deleteUser(user).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
        });


    };

    $scope.updateUser = function (user) {
        if (user.SelectedRoles.length == 0) {
            notificationFactory.error('Yetki seçiniz!');
            return;
        }
        userFactory.updateUser(user).success(successCallbackWhenFormEdit).error(baseControllerFactory.errorCallback);
    };

    $scope.loadUsers = function () {
        return userFactory.getUsers($scope.pageNumber, $scope.pageSize, $scope.sortedBy, $scope.sortDir).success(function (data, status) {
            $scope.users = data.users;
            $scope.totalCount = data.totalCount;
        }).error(baseControllerFactory.errorCallback);
    };

    $scope.loadRoles = function () {
        return sharedFactory.getSelectorRoles().success(function (data, status) {

            $scope.roles = [
                { Id: "1", Name: "Admin"},
                { Id: "2", Name: "Personel" },
                { Id: "3", Name: "Customer" }
            ];

        }).error(baseControllerFactory.errorCallback);
    };

    $scope.sendPasswordToUser = function(userId) {
        var modalOptions = modalService.getConfirmationModal('Hayır','Evet,Şifreyi sıfırla ve mail gönder','Şifre değiştirme','Kullanıcı şifresi sıfırlanacak ve yeni şifre mail gönderilecektir.');
        modalService.showModal({}, modalOptions).then(function (result) {
            if (result != 'ok') return;
          
            userFactory.sendPasswordToUser({ userId: userId, password: '' }).success(function (data, status) {
                notificationFactory.success('Mail gönderildi!');
            }).error(baseControllerFactory.errorCallback);
        });

        
    };

    $scope.init = function () {
        $scope.loadRoles();

        $scope.loadUsers();
    };

    $scope.init();
});
