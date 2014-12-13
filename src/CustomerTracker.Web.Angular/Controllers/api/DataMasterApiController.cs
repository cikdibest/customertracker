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
    public class DataMasterApiController : ApiController
    {
        public IEnumerable<DataMaster> GetDataMastersByCustomerId(int customerId)
        {
            var dataMasters = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>()
                .Find(customerId)
                .DataMasters
                .ToList();
                //.Include(q => q.Customer)
                //.Include(q => q.DataDetails).ToList();

            return dataMasters;
        }

        public IEnumerable<DataMaster> GetDataMastersByUserId(int userId)
        {
            var dataMasters = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>()
                .Find(userId)
                .DataMasters
                .ToList();
            //.Include(q => q.Customer)
            //.Include(q => q.DataDetails).ToList();

            return dataMasters;
        }


        public DataMaster GetDataMaster(int id)
        {
            var dataMaster = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>().Find(id);

            if (dataMaster == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dataMaster;
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutDataMaster(int id, DataMaster dataMaster)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dataMaster.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>().Update(dataMaster);

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

        public HttpResponseMessage PostDataMaster(DataMaster dataMaster)
        {
            if (ModelState.IsValid)
            {
                
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>().Create(dataMaster);

                var customer = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().Find(dataMaster.TempCustomerId);

                customer.DataMasters.Add(dataMaster);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dataMaster);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = dataMaster.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteDataMaster(int id)
        {
            var dataMaster = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>().Find(id);

            if (dataMaster == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>().Delete(dataMaster);
           
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

            return Request.CreateResponse(HttpStatusCode.OK, dataMaster);
        }

        public List<KeyValuePair<int, string>> GetSelectorDataMasters()
        {
            var dataMasters = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataMaster>()
                .SelectAll()
                .AsEnumerable()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
                .ToList();

            return dataMasters;
        }
    }
}
