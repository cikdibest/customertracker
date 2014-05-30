

customerApp.factory('remoteMachineFactory', function ($http) {
    return {
        getRemoteMachines: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(remoteMachineApiUrl + 'getRemoteMachines', { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addRemoteMachine: function (remoteMachine) {
            return $http.post(remoteMachineApiUrl + 'postRemoteMachine', remoteMachine);
        },
        deleteRemoteMachine: function (remoteMachine) {
            return $http.delete(remoteMachineApiUrl + 'deleteRemoteMachine/' + remoteMachine.Id);
        },
        updateRemoteMachine: function (remoteMachine) {
            return $http.put(remoteMachineApiUrl + 'putRemoteMachine/' + remoteMachine.Id, remoteMachine);
        },
        getCustomers: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(customerApiUrl + 'getCustomers', { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        getRemoteConnectionTypes: function () {
            return $http.get(sharedDataApiUrl + 'getRemoteConnectionTypes');
        }

    };
});