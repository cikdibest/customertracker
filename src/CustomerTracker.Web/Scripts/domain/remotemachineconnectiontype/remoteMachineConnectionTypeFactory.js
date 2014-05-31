

customerApp.factory('remoteMachineConnectionTypeFactory', function ($http) {
    return { 
        getSelectorRemoteMachineConnectionTypes: function () {
            return $http.get(sharedDataApiUrl.getselectorremotemachineconnectiontypes);
        }

    };
});