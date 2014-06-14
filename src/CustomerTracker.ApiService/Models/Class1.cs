using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestServerApiApp.Models
{
    public class ServerCondition
    {
        public string MachineCode { get; set; }

        public MachineCondition MachineCondition { get; set; }

        public List<ServiceCondition> ServiceConditions { get; set; }
    }

    public class ServiceCondition
    {
        public int TargetServiceId { get; set; }

        //status,description
    }

    public class MachineCondition
    {
        //public int Ram { get; set; }
        //.
        //.
    }

    public class TargetService
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}