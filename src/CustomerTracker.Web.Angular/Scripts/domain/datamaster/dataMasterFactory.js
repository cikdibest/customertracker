customerApp.factory('dataMasterFactory', function($http) {
    //return {        
    //  getDataMasters: function() {
    //    return   $http.get(dataMasterApiUrl.getdatamastersbycustomerid);
    //  }  
    //};
    return {
        getDataMastersByCustomerId: function (customerId) {
            return $http.get(dataMasterApiUrl.getdatamastersbycustomerid, {params:{customerId:customerId}});
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