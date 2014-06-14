using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestServerApiApp.Models
{
    public class ServerCondition
    {
        public string MachineCode { get; set; }

        public ComputerCondition ComputerCondition { get; set; }

        public List<ServiceCondition> ServiceConditions { get; set; }
    }

    public class ServiceCondition
    {
        public int TargetServiceId { get; set; }

        //status,description
    }

    public class ComputerCondition
    {
        //public int Ram { get; set; }
        //.
        //.
    }

    public class TargetService
    {
        public int ApplicationServiceId { get; set; }

        public string InstanceName { get; set; }

        public int ApplicationServiceTypeId { get; set; }
    }
}