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
using CustomerTracker.Web.Models.Attributes;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class SolutionApiController : ApiController
    {
        public dynamic GetSolutions(int pageNumber, int pageSize, string sortBy, string sortDir, int customerId, int productId, int troubleId)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var customerTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var solutions = customerTrackerDataContext.Solutions.AsQueryable();

            if (customerId>0)
                solutions = solutions.Where(q => q.CustomerId == customerId);

            if (productId>0)
                solutions = solutions.Where(q => q.ProductId == productId);

            if (troubleId > 0)
                solutions = solutions.Where(q => q.TroubleId == troubleId);

            var pagingSolutions = solutions
                .Include(q => q.Customer)
                .Include(q => q.Product)
                .Include(q => q.Trouble)
                .Include(q => q.SolutionUser)
                .OrderBy(q => q.Id)
                .Skip(skippedRow)
                .Take(pageSize)
                .ToList();

            return new { solutions = pagingSolutions, totalCount = solutions.Count() };
        }

        public Solution GetSolution(int id)
        {
            var solution = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Solution>().Find(id);

            if (solution == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return solution;
        }
         
        public HttpResponseMessage PutSolution(int id, Solution solution)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != solution.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Solution>().Update(solution);

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

        public HttpResponseMessage PostSolution(Solution solution)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<Solution>().Create(solution);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, solution);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = solution.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
         
        public HttpResponseMessage DeleteSolution(int id)
        {
            var solution = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Solution>().Find(id);

            if (solution == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<Solution>().Delete(solution);

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

            return Request.CreateResponse(HttpStatusCode.OK, solution);
        }


    }
}
