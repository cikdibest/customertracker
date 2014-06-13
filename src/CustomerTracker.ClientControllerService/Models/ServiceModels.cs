﻿using System.Collections.Generic;

namespace CustomerTracker.ClientControllerService.Models
{
    public class ServiceControlMessage
    {
        public TargetService TargetService { get; set; }

        public bool IsAlarm { get; set; }

        public int NumberOfThreads { get; set; }

        public string State { get; set; }

        public bool DoesExist { get; set; }

    }

    public class HardwareControlMessage
    {
        public string Data { get; set; }

        public bool IsAlarm { get; set; }
    }

    public class TargetService
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }

    public class ServerCondition
    {
        public List<HardwareControlMessage> HardwareControlMessages { get; set; }
        public List<ServiceControlMessage> ServiceControlMessages { get; set; }
        public string MachineCode { get; set; }
    }
}
