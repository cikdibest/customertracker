customerApp.factory('departmentFactory', function ($http) {
    return {
        getDepartments: function () {
            return $http.get(departmentApiUrl);
        },
        addDepartment: function (department) {
            return $http.post(departmentApiUrl, department);
        },
        deleteDepartment: function (department) {
            return $http.delete(departmentApiUrl + department.Id);
        },
        updateDepartment: function (department) {
            return $http.put(departmentApiUrl + department.Id, department);
        }

    };
});
