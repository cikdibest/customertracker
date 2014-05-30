

customerApp.factory('customerFactory', function ($http) {
    return {
        getCustomers: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(customerApiUrl + 'getCustomers', { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addCustomer: function (customer) {
            return $http.post(customerApiUrl + 'postCustomer', customer);
        },
        deleteCustomer: function (customer) {
            return $http.delete(customerApiUrl + 'deleteCustomer/' + customer.Id);
        },
        updateCustomer: function (customer) {
            return $http.put(customerApiUrl + 'putCustomer/' + customer.Id, customer);
        },
        getCities: function () {
            return $http.get(sharedDataApiUrl + 'getCities');
        }

    };
});