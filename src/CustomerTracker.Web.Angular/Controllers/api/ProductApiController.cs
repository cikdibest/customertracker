using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Models.Attributes;
using CustomerTracker.Web.Angular.Utilities;

namespace CustomerTracker.Web.Angular.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class ProductApiController : ApiController
    {
        public IEnumerable<Product> GetProducts()
        {
            var products = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().SelectAll().ToList();

            return products;
        }

        public Product GetProduct(int id)
        {
            var product = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Find(id);

            if (product == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return product;
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != product.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Update(product);

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

        public HttpResponseMessage PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Create(product);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, product);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = product.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteProduct(int id)
        {
            var product = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Find(id);

            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>().Delete(product);

            try
            {
                var save = ConfigurationHelper.UnitOfWorkInstance.Save();
                if (save == -547)
                    return Request.CreateResponse(HttpStatusCode.MultipleChoices, new Exception("Silmek istediğiniz kaydın bağlantılı verileri var.Lütfen önce bu verileri siliniz"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        public List<KeyValuePair<int, string>> GetSelectorSubProducts()
        {
            var products = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Product>()
                .SelectAll()
                .Include("SubProducts")
                .AsEnumerable()
                .Where(q => q.SubProducts == null || !q.SubProducts.Any())
                .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
                .ToList();

            return products;

        }
    }
}
