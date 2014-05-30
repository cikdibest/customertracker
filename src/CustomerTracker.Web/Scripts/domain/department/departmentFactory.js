customerApp.factory('departmentFactory', function ($http) {
    return {
        getDepartments: function () {
            return $http.get(departmentApiUrl + 'getDepartments');
        },
        addDepartment: function (department) {
            return $http.post(departmentApiUrl + 'postDepartment', department);
        },
        deleteDepartment: function (department) {
            return $http.delete(departmentApiUrl + 'deleteDepartment/' + department.Id);
        },
        updateDepartment: function (department) {
            return $http.put(departmentApiUrl + 'putDepartment/' + department.Id, department);
        }

    };
});
