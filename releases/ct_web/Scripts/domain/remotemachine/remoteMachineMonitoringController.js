 
customerApp.controller('remoteMachineMonitoringController', function ($scope, remoteMachineFactory, notificationFactory, baseControllerFactory) {
      
    $scope.remoteMachineStates = [];
  
    $scope.loadRemoteMachineStates = function() {
        remoteMachineFactory.getRemoteMachineStates()
                    .success(function (data) { $scope.remoteMachineStates = data; })
                    .error(baseControllerFactory.errorCallback);
    };

    $scope.init = function () { 
        $scope.loadRemoteMachineStates(); 
    };

    $scope.init();

});