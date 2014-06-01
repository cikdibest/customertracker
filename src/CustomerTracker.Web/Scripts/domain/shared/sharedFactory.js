

customerApp.factory('sharedFactory', function ($http) {
    return { 
        getSelectorCities: function () {
            return $http.get(sharedDataApiUrl.getselectorcities);
        },
        getSelectorGenders: function () {
            return $http.get(sharedDataApiUrl.getselectorgenders);
        }

    };
});