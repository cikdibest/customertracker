customerApp.factory('materialFactory', function ($http) {
    return {
        getMaterials: function (data) {
            return $http.post(materialSearchUrl + 'getMaterials', data);
        },

        getCustomer: function (url) {
            return $http.get(url);
        }

    };
});