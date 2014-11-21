customerApp.factory('dataMasterFactory', function($http) {
    //return {        
    //  getDataMasters: function() {
    //    return   $http.get(dataMasterApiUrl.getdatamasters);
    //  }  
    //};
    return {
        getDataMasters: function () {
            return $http.get(dataMasterApiUrl.getdatamasters);
        },
        addDataMaster: function (dataMaster) {
            return $http.post(dataMasterApiUrl.postdatamaster, dataMaster);
        },
        deleteDataMaster: function (dataMaster) {
            return $http.delete(dataMasterApiUrl.deletedatamaster + dataMaster.Id);
        },
        updateDataMaster: function (dataMaster) {
            return $http.put(dataMasterApiUrl.putdatamaster + dataMaster.Id, dataMaster);
        }, 
       
    };

})