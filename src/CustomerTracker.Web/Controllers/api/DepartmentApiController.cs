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
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class DepartmentApiController : ApiController
    {
        public IEnumerable<Department> GetDepartments()
        {
            var departments = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().SelectAll().ToList();

            return departments;
        }

        public Department GetDepartment(int id)
        {
            var department = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Find(id);
            
            if (department==null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return department;
        }

        public HttpResponseMessage PutDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != department.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Update(department);

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

        public HttpResponseMessage PostDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Create(department);
                
                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, department);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = department.Id }));
                
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            } 
        }

        public HttpResponseMessage DeleteDepartment(int id)
        {
            var department = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Find(id);
            
            if (department == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Department>().Delete(department);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, department); 
        }
    }
}
