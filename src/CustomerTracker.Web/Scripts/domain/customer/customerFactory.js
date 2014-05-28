

customerApp.factory('customerFactory', function ($http) {
    return {
        getCustomers: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(customerApiUrl, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addCustomer: function (customer) {
            return $http.post(customerApiUrl, customer);
        },
        deleteCustomer: function (customer) {
            return $http.delete(customerApiUrl + customer.Id);
        },
        updateCustomer: function (customer) {
            return $http.put(customerApiUrl + customer.Id, customer);
        },
        getCities: function () {
            return $http.get(getCitiesUrl);
        }

    };
});