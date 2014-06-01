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
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class DataDetailApiController : ApiController
    {
        public IEnumerable<DataDetail> GetDataDetails()
        {
            var dataDetails = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().SelectAll().ToList();

            return dataDetails;
        }

        public DataDetail GetDataDetail(int id)
        {
            var dataDetail = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().Find(id);
            
            if (dataDetail==null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dataDetail;
        }

        public HttpResponseMessage PutDataDetail(int id, DataDetail dataDetail)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != dataDetail.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().Update(dataDetail);

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

        public HttpResponseMessage PostDataDetail(DataDetail dataDetail)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().Create(dataDetail);
                
                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, dataDetail);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = dataDetail.Id }));
                
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            } 
        }

        public HttpResponseMessage DeleteDataDetail(int id)
        {
            var dataDetail = ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().Find(id);
            
            if (dataDetail == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<DataDetail>().Delete(dataDetail);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dataDetail); 
        }
         
        
    }
}
