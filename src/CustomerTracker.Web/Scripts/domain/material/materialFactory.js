customerApp.factory('materialFactory', function ($http) {
    return {
        getMaterials: function (data) {
            return $http.post(materialSearchUrl, data);
        },

        getCustomer: function (url) {
            return $http.get(url);
        }

    };
});