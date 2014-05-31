

customerApp.factory('customerFactory', function ($http) {
    return {
        getCustomers: function (pageNumber, pageSize, sortBy, sortDir) {
            //return requestHandler({
            //    method: 'GET',
            //    url: customerApiUrl.getcustomers,
            //    params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } 
            //});
            return $http.get(customerApiUrl.getcustomers, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addCustomer: function (customer) {
            //return requestHandler({
            //    method: 'POST',
            //    url: customerApiUrl.postcustomer,
            //    data: customer
            //});
            return $http.post(customerApiUrl.postcustomer, customer);
        },
        deleteCustomer: function (customer) {
            //return requestHandler({
            //    method: 'DELETE',
            //    url: customerApiUrl.deletecustomer + customer.Id
            //});
            return $http.delete(customerApiUrl.deletecustomer + customer.Id);
        },
        updateCustomer: function (customer) {
            //return requestHandler({
            //    method: 'PUT',
            //    url: customerApiUrl.putcustomer + customer.Id,
            //    data: customer
            //});
            return $http.put(customerApiUrl.putcustomer + customer.Id, customer);
        },
      
        getSelectorCustomers: function () {
        //return requestHandler({
        //    method: 'GET',
        //    url: customerApiUrl.getselectorcustomers
        //});
            return $http.get(customerApiUrl.getselectorcustomers);
    }
      

    };
});