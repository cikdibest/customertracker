customerApp.factory('productFactory', function ($http) {
    return {
        getProducts: function () {
            return $http.get(productApiUrl.getproducts);
        },
        addProduct: function (product) {
            return $http.post(productApiUrl.postproduct , product);
        },
        deleteProduct: function (product) {
            return $http.delete(productApiUrl.deleteproduct  + product.Id);
        },
        updateProduct: function (product) {
            return $http.put(productApiUrl.putproduct + product.Id, product);
        },
        getSelectorSubProducts: function () {
            return $http.get(productApiUrl.getselectorsubproducts);
        }
        

    };
});
