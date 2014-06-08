customerApp.factory('userFactory', function ($http) {
    return {
        getUsers: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(userApiUrl.getusers, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        //addUser: function (user) {
        //    return $http.post(userApiUrl.postuser , user);
        //},
        deleteUser: function (user) {
            return $http.delete(userApiUrl.deleteuser  + user.Id);
        },
        updateUser: function (user) {
            return $http.put(userApiUrl.putuser + user.Id, user);
        },
        getSelectorUsers: function () {
            return $http.get(userApiUrl.getselectorusers);
        }

    };
});
