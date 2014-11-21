customerApp.factory('communicationFactory', function ($http) {
    return {
        getCommunications: function () {
            return $http.get(communicationApiUrl.getcommunications);
        },
        addCommunication: function (communication) {
            return $http.post(communicationApiUrl.postcommunication , communication);
        },
        deleteCommunication: function (communication) {
            return $http.delete(communicationApiUrl.deletecommunication  + communication.Id);
        },
        updateCommunication: function (communication) {
            return $http.put(communicationApiUrl.putcommunication + communication.Id, communication);
        }

    };
});
