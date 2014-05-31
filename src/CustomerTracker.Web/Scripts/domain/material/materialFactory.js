customerApp.factory('materialFactory', function ($http) {
    return {
        searchMaterials: function (data) {
            return $http.post(materialApiUrl.searchmaterials, data);
        },
        //dynamic url , by searchengine implementation
        getMaterialDetail: function (url) {
            return $http.get(url);
        }

    };
});