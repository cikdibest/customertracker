using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading;
using CustomerTracker.ClientController.Core.Models;
using NLog;

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
                Name = "Ram(Available)",
                Data = ramCounter.NextValue() + " Mb",
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

            HashSet<float> floats = new HashSet<float>();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                var nextValue = cpuCounter.NextValue();
                floats.Add(nextValue);
            }

            var value = floats.Average();

            return new HardwareControlMessage()
            {
                Name = "Cpu",
                Data = value + "%",
                IsAlarm = value > cpuUsageAlarmLimit
            };
        }
    }

    public class DiskController
    {
        public HardwareControlMessage GetDiskState(double diskUsageAlarmLimit)
        {
            var value = DriveInfo.GetDrives().First(p => p.Name == "C:\\").TotalFreeSpace / (1024 * 1024);

            return new HardwareControlMessage()
            {
                Name = "Disk(Free)",
                Data = value + "MB",
                IsAlarm = value < diskUsageAlarmLimit
            };
        }
    }

    public class ServiceStateController
    {
        readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ServiceControlMessage GetServiceState(TargetService targetService, double serviceThreadCountAlarmLimit)
        {
            _log.Trace("Servisin durumu sorgulandı Servisin adı : " + targetService.InstanceName);

            try
            {
                var serviceManagementBaseObject = GetServiceManagementBaseObject(targetService.InstanceName);
                if (serviceManagementBaseObject == null)
                {
                    return new ServiceControlMessage()
                    {
                        DoesExist = false,
                        IsAlarm = false,
                        TargetService = targetService,
                        NumberOfThreads = 0,
                        State = ""
                    };
                }

                var uint32Val = UInt32.Parse(serviceManagementBaseObject["PROCESSID"].ToString());

                var serviceProcessId = int.Parse(uint32Val.ToString());

                var threadCount = Process.GetProcessById(serviceProcessId).Threads.Count;

                var sc = new ServiceController(targetService.InstanceName);

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
            catch (Exception exc)
            {
                _log.Error(exc);

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

        private ManagementBaseObject GetServiceManagementBaseObject(string instanceName)
        {
            string qry = "SELECT PROCESSID FROM WIN32_SERVICE WHERE NAME = '" + instanceName + "'";

            var searcher = new System.Management.ManagementObjectSearcher(qry);

            return searcher.Get().Cast<ManagementBaseObject>().SingleOrDefault();
        }


    }


}
