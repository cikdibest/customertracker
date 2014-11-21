customerApp.factory('dataDetailFactory', function($http) {
     
    return {
        getDataDetails: function () {
            return $http.get(dataDetailApiUrl.getdatadetails);
        },
        addDataDetail: function (dataDetail) {
            return $http.post(dataDetailApiUrl.postdatadetail, dataDetail);
        },
        deleteDataDetail: function (dataDetail) {
            return $http.delete(dataDetailApiUrl.deletedatadetail + dataDetail.Id);
        },
        updateDataDetail: function (dataDetail) {
            return $http.put(dataDetailApiUrl.putdatadetail + dataDetail.Id, dataDetail);
        }, 
      

    };

})