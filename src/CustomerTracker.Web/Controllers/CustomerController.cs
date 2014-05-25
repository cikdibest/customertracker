using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;
using Ninject;

namespace CustomerTracker.Web.Controllers
{
    public class CustomerController : Controller
    {
        
         
        public ActionResult Detail(int customerId)
        { 
            var customer = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext()
                 .Set<Customer>()
                 .Include("City")
                 .Include("Communications.Department") 
                 .SingleOrDefault(q => q.Id == customerId);

            var customerDetailModel = Mapper.Map<CustomerDetailModel>(customer);

            return Json(customerDetailModel, JsonRequestBehavior.AllowGet);
        }
    }

   
}