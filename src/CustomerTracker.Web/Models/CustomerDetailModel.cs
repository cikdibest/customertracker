using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models
{
    public class CustomerDetailModel
    {
        public int Id { get; set; }
        public string CustomerTitle { get; set; }
        public string CityName { get; set; }
        public string Explanation { get; set; }
        public List<CommunicationModel> Communications { get; set; }
        public List<RemoteMachineModel> RemoteMachines { get; set; }
        public List<ProductModel> Products { get; set; }
        
  
    }

    public class RemoteMachineModel
    { 
        public string Name { get; set; }
         
        public string Username { get; set; }
         
        public string Password { get; set; }
         
        public string Explanation { get; set; }
         
        public string RemoteAddress { get; set; }

        public List<ProductModel> ProductModels { get; set; }
  
        public string CustomerTitle { get; set; }

        public string RemoteConnectionType { get; set; }

        public string LogoName { get; set; }
        
    }

    public class CommunicationModel
    {
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string GenderName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
    }
}