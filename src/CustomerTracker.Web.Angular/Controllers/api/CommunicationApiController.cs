﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Models.Attributes;
using CustomerTracker.Web.Angular.Utilities;

namespace CustomerTracker.Web.Angular.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class CommunicationApiController : ApiController
    {
        public IEnumerable<Communication> GetCommunications()
        {
            var communications = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().SelectAll().ToList();

            return communications;
        }

        public Communication GetCommunication(int id)
        {
            var communication = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Find(id);

            if (communication == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return communication;
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutCommunication(int id, Communication communication)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != communication.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Update(communication);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, communication);
        }

        public HttpResponseMessage PostCommunication(Communication communication)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Create(communication);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, communication);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = communication.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteCommunication(int id)
        {
            var communication = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Find(id);

            if (communication == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().Delete(communication);

            var save = ConfigurationHelper.UnitOfWorkInstance.Save();
            if (save == -547)
                return Request.CreateResponse(HttpStatusCode.MultipleChoices, new Exception("Silmek istediğiniz kaydın bağlantılı verileri var.Lütfen önce bu verileri siliniz"));

            return Request.CreateResponse(HttpStatusCode.OK, communication);
        }
    }
}
