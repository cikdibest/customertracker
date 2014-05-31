

customerApp.factory('cityFactory', function ($http) {
    return { 
        getSelectorCities: function () {
            return $http.get(sharedDataApiUrl.getselectorcities);
        }

    };
});