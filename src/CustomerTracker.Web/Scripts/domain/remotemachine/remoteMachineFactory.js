

customerApp.factory('remoteMachineFactory', function ($http) {
    return {
        getRemoteMachines: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(remoteMachineApiUrl.getremotemachines, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addRemoteMachine: function (remoteMachine) {
            return $http.post(remoteMachineApiUrl.postremotemachine, remoteMachine);
        },
        deleteRemoteMachine: function (remoteMachine) {
            return $http.delete(remoteMachineApiUrl.deleteremotemachine + remoteMachine.Id);
        },
        updateRemoteMachine: function (remoteMachine) {
            return $http.put(remoteMachineApiUrl.putremotemachine + remoteMachine.Id, remoteMachine);
        }
        ,
        getRemoteMachineStates: function () {
            return $http.get(remoteMachineApiUrl.getremotemachinestates);
        },
        //getRemoteConnectionTypes: function () {
        //    return $http.get(sharedDataApiUrl.getremoteconnectiontypes);
        //}

    };
});