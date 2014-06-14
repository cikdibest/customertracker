using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using CustomerTracker.ClientController.Core.Models;

namespace CustomerTracker.ClientController.Core.Controllers
{
    public class RamController
    {
        public HardwareControlMessage GetRamState(double ramUsageAlarmLimit)
        {
            var cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            var value = ramCounter.NextValue();
            return new HardwareControlMessage()
            {
                Data = ramCounter.NextValue() + "MB",
                IsAlarm = value < ramUsageAlarmLimit
            };
        }
    }

    public class CpuController
    {
        public HardwareControlMessage GetCpuState(double cpuUsageAlarmLimit)
        {
           var cpuCounter = new PerformanceCounter
           {
               CategoryName = "Processor",
               CounterName = "% Processor Time",
               InstanceName = "_Total"
           };
            var valueFirst = cpuCounter.NextValue();
            var value = cpuCounter.NextValue();
            return new HardwareControlMessage()
            {
                Data = value + "%",
                IsAlarm = value > cpuUsageAlarmLimit
            };
        }
    }

    public class DiskController
    {
        public HardwareControlMessage GetDiskState(double diskUsageAlarmLimit)
        {
            var value = DriveInfo.GetDrives().First(p => p.Name == "C:\\").TotalFreeSpace/(1024*1024);

            return new HardwareControlMessage()
            {
                Data = value + "MB",
                IsAlarm = value < diskUsageAlarmLimit
            };
        }
    }

    public class ServiceStateController
    {
        //readonly Logger _log = LogManager.GetCurrentClassLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceControlMessage GetServiceState(TargetService targetService, double serviceThreadCountAlarmLimit)
        {
            //_log.Warn("Servisin durumu sorgulandı Servisin adı : "+targetService.Name);
            var sc = new ServiceController(targetService.Name);
            var threadCount = Process.GetProcessById(getProcessIDByServiceName(targetService.Name)).Threads.Count;
            try
            {
                var isAlarm = (sc.Status != ServiceControllerStatus.Running);
                return new ServiceControlMessage()
                {
                    NumberOfThreads = threadCount,
                    IsAlarm = threadCount > serviceThreadCountAlarmLimit || isAlarm,
                    TargetService = targetService,
                    State = sc.Status.ToString(),
                    DoesExist = true
                };
            }
            catch (Exception)
            {
                //_log.Error("Servis bulunamadı : Adı : "+targetService.Name);
                return new ServiceControlMessage()
                {
                    DoesExist = false,
                    IsAlarm = false,
                    TargetService = targetService,
                    NumberOfThreads = 0,
                    State = ""
                };
               
            }
        }

        private int getProcessIDByServiceName(string serviceName)
        {
            int processId = 0;
            string qry = "SELECT PROCESSID FROM WIN32_SERVICE WHERE NAME = '" + serviceName + "'";
            var searcher = new System.Management.ManagementObjectSearcher(qry);
            foreach (System.Management.ManagementObject mngntObj in searcher.Get())
            {
                processId = (int)mngntObj["PROCESSID"];
            }
            return processId;
        }
    }

    
}
