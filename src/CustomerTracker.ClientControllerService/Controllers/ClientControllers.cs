using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using CustomerTracker.ClientControllerService.Models;
using CustomerTracker.ClientControllerService.Properties;

namespace CustomerTracker.ClientControllerService.Controllers
{
    public class RamController
    {
        public HardwareControlMessage GetRamState()
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
                IsAlarm = value < Settings.Default.RamUsageAlarmLimit
            };
        }
    }

    public class CpuController
    {
        public HardwareControlMessage GetCpuState()
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
                IsAlarm = value > Settings.Default.CpuUsageAlarmLimit
            };
        }
    }

    public class DiskController
    {
        public HardwareControlMessage GetDiskState()
        {
            var value = DriveInfo.GetDrives().First(p => p.Name == "C:\\").TotalFreeSpace/(1024*1024);

            return new HardwareControlMessage()
            {
                Data = value + "MB",
                IsAlarm = value < Settings.Default.DiskUsageAlarmLimit
            };
        }
    }

    public class ServiceStateController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceControlMessage GetServiceState(TargetService targetService)
        {
            log.Warn("Servisin durumu sorgulandı Servisin adı : "+targetService.Name);
            var sc = new ServiceController(targetService.Name);
            var threadCount = Process.GetProcessById(getProcessIDByServiceName(targetService.Name)).Threads.Count;
            try
            {
                var isAlarm = (sc.Status != ServiceControllerStatus.Running);
                return new ServiceControlMessage()
                {
                    NumberOfThreads = threadCount,
                    IsAlarm = threadCount > Settings.Default.ServiceThreadCountAlarmLimit || isAlarm,
                    TargetService = targetService,
                    State = sc.Status.ToString(),
                    DoesExist = true
                };
            }
            catch (Exception)
            {
                log.Error("Servis bulunamadı : Adı : "+targetService.Name);
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
