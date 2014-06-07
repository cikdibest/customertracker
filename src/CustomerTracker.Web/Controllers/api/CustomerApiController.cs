using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Web.Models.Attributes;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class CustomerApiController : ApiController
    {
        public dynamic GetCustomers(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var customerTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var customers = customerTrackerDataContext.Customers;

            var pageCustomers = customers
                .Include("City")
                .OrderBy(q => q.Id)
                .Skip(skippedRow)
                .Take(pageSize)
                .ToList();

            return new { customers = pageCustomers, totalCount = customers.Count() };
        }

        public Customer GetCustomer(int id)
        {
            var customerTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var customer = customerTrackerDataContext.Customers.Include("City").SingleOrDefault(q => q.Id == id);

            if (customer == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return customer;
        }

        public Customer GetCustomerAdvancedDetail(int id)
        {
            var customer = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext()
                 .Set<Customer>()
                 .Include("City")
                 .Include("Communications.Department")
                 .Include("RemoteMachines")
                 .Include("RemoteMachines.RemoteMachineConnectionType")
                 .Include("DataMasters")
                 .Include("DataMasters.DataDetails")
                 .Include("Products")
                 .SingleOrDefault(q => q.Id == id);

            if (customer == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return customer;
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != customer.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Update(customer);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Create(customer);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customer);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = customer.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteCustomer(int id)
        {
            var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Find(id);

            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Delete(customer);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }

        public List<KeyValuePair<int, string>> GetSelectorCustomers()
        {
            var customers = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>()
                .SelectAll()
                .AsEnumerable()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
                .ToList();

            return customers;
        }

        public HttpResponseMessage AddProductToCustomer(ProductCustomerModel productCustomerModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>()
                .SelectAll()
                .Include("Products")
                .SingleOrDefault(q => q.Id == productCustomerModel.customerId);

            if (customer == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var product = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>()
                .SelectAll()
                .SingleOrDefault(q => q.Id == productCustomerModel.productId);
            
            if (product == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            if (customer.Products == null)
                customer.Products = new List<Product>();

            if (customer.Products.Any(q => q.Id == product.Id)) 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new DuplicateNameException("Ürün iki kere eklenemez!"));


            if (!customer.Products.Any(q => q.Id == product.Id))
                customer.Products.Add(product);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);


        }

        public HttpResponseMessage RemoveProductFromCustomer(ProductCustomerModel productCustomerModel)
        {
            var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().SelectAll().Include("Products").SingleOrDefault(q => q.Id == productCustomerModel.customerId);

            if (customer == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var product = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Find(productCustomerModel.productId);

            if (product == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            customer.Products.Remove(product);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, customer);


        }
    }

    public class ProductCustomerModel
    {
        public int customerId { get; set; }
        public int productId { get; set; }
    }
}
