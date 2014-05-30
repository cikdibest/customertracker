

customerApp.factory('customerFactory', function ($http) {
    return {
        getCustomers: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(customerApiUrl.getcustomers, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addCustomer: function (customer) {
            return $http.post(customerApiUrl.postcustomer, customer);
        },
        deleteCustomer: function (customer) {
            return $http.delete(customerApiUrl.deletecustomer + customer.Id);
        },
        updateCustomer: function (customer) {
            return $http.put(customerApiUrl.putcustomer + customer.Id, customer);
        },
        getCities: function () {
            return $http.get(sharedDataApiUrl.getcities);
        }

    };
});