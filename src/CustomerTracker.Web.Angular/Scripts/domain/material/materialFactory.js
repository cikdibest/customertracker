customerApp.factory('materialFactory', function ($http) {
    return {
        searchMaterials: function (data) {
            return $http.post(materialApiUrl.searchmaterials, data);
        },
        

    };
});