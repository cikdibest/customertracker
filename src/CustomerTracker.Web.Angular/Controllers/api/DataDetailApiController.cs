﻿using System;
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

            if (dataDetail == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return dataDetail;
        }

        [CustomAuthorize(Roles = "Admin")]
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

        [CustomAuthorize(Roles = "Admin")]
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
                var save = ConfigurationHelper.UnitOfWorkInstance.Save();
                if (save == -547)
                    return Request.CreateResponse(HttpStatusCode.MultipleChoices, new Exception("Silmek istediğiniz kaydın bağlantılı verileri var.Lütfen önce bu verileri siliniz"));
            }

            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, dataDetail);
        }


    }
}
