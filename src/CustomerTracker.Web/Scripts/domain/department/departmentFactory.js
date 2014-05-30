customerApp.factory('departmentFactory', function ($http) {
    return {
        getDepartments: function () {
            return $http.get(departmentApiUrl.getdepartments);
        },
        addDepartment: function (department) {
            return $http.post(departmentApiUrl.postdepartment , department);
        },
        deleteDepartment: function (department) {
            return $http.delete(departmentApiUrl.deletedepartment  + department.Id);
        },
        updateDepartment: function (department) {
            return $http.put(departmentApiUrl.putdepartment + department.Id, department);
        }

    };
});
