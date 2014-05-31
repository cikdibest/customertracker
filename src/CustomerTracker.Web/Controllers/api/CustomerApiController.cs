using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using CustomerTracker.Web.Models;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    public class CustomerApiController : ApiController
    {
        public dynamic GetCustomers(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var customerTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var customers = customerTrackerDataContext.Customers;

            var pageCustomers = customers.Include("City").OrderBy(q => q.Id)
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

        public Customer GetCustomerAdvancedDetail(int customerId)
        {
            var customer = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext()
                 .Set<Customer>()
                 .Include("City")
                 .Include("Communications.Department")
                 .Include("RemoteMachines")
                 .Include("DataMasters")
                  .Include("DataMasters.DataDetails")
                 .SingleOrDefault(q => q.Id == customerId);
             
            if (customer == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return customer;
        }

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
    }
}
