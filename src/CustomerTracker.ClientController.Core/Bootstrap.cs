using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CustomerTracker.ClientController.Core.Controllers;
using CustomerTracker.ClientController.Core.Models;
using NLog;
using RestSharp;

namespace CustomerTracker.ClientController.Core
{
    public class Bootstrap
    {
        private Logger _log;
    
        private string _machineCode;

        private List<TargetService> _serviceNameList = new List<TargetService>();

        private RamController _ramController;
        private DiskController _diskController;
        private CpuController _cpuController;
        private ServiceStateController _serviceController;
        private double _ramUsageAlarmLimit;
        private readonly double _diskUsageAlarmLimit;
        private readonly double _cpuUsageAlarmLimit;
        private readonly double _serviceThreadCountAlarmLimit;
        private readonly string _serverConditonPostApiAdress;
        private readonly string _serverConditonGetApiAdress;

        public Bootstrap(string machineCode, double ramUsageAlarmLimit, double diskUsageAlarmLimit, double cpuUsageAlarmLimit, double serviceThreadCountAlarmLimit, string serverConditonPostApiAdress, string serverConditonGetApiAdress)
        {
            this._log = LogManager.GetCurrentClassLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this._machineCode = machineCode;
            this._ramUsageAlarmLimit = ramUsageAlarmLimit;
            this._diskUsageAlarmLimit = diskUsageAlarmLimit;
            this._cpuUsageAlarmLimit = cpuUsageAlarmLimit;
            this._serviceThreadCountAlarmLimit = serviceThreadCountAlarmLimit;
            this._serverConditonPostApiAdress = serverConditonPostApiAdress;
            this._serverConditonGetApiAdress = serverConditonGetApiAdress;
            this._ramController = new RamController();
            this._diskController = new DiskController();
            this._cpuController = new CpuController();
            this._serviceController = new ServiceStateController();
        }

        public void Start()
        {
            _log.Debug("Verilen süre geldi.");
            if (!fillServiceNameList())
            {
                return;
            }
            var hardwareConditionList = new List<HardwareControlMessage>();
            var ramState = _ramController.GetRamState(_ramUsageAlarmLimit);
            _log.Debug("Ram State : " + ramState.Data + " Alarm mı :" + ramState.IsAlarm);
            hardwareConditionList.Add(ramState);
            var diskState = _diskController.GetDiskState(_diskUsageAlarmLimit);
            _log.Debug("Disk State : " + diskState.Data + " Alarm mı :" + diskState.IsAlarm);

            hardwareConditionList.Add(diskState);

            var cpuState = _cpuController.GetCpuState(_cpuUsageAlarmLimit);
            _log.Debug("CPU State : " + cpuState.Data + " Alarm mı :" + cpuState.IsAlarm);

            hardwareConditionList.Add(cpuState);


            var serviceConditionList = new List<ServiceControlMessage>();

            foreach (var service in _serviceNameList)
            {
                serviceConditionList.Add(_serviceController.GetServiceState(service, _serviceThreadCountAlarmLimit));
            }
            _log.Debug("İstenen Servislerin sayısı : " + serviceConditionList.Count);
            var serverCondition = new ServerCondition
            {
                HardwareControlMessages = hardwareConditionList,
                ServiceControlMessages = serviceConditionList,
                MachineCode = _machineCode
            };
            _log.Debug("Servis durumu gönderiliyor. Alarm var mı : " + (serverCondition.HardwareControlMessages.Count(c => c.IsAlarm) + serverCondition.ServiceControlMessages.Count(c => c.IsAlarm)));
            var restRequest = CreatePostRestRequest(_serverConditonPostApiAdress, serverCondition);
            var restClient = new RestClient();
            var response = restClient.Execute(restRequest);
        }

       

        private bool fillServiceNameList()
        {
            try
            {
                _log.Warn("Servis listesi alınıyor");
                RestRequest restRequest = CreateGetRestRequest(_serverConditonGetApiAdress);
                var restClient = new RestClient();
                var targetServices = restClient.Execute<List<TargetService>>(restRequest);
                _serviceNameList = new List<TargetService>(targetServices.Data);
                _log.Warn("Servis listesi alındı.");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("Servis bilgileri alınırken hata oluştu : " + ex);
                return false;
            }
        }

        public RestRequest CreateGetRestRequest(string path)
        {
            var restRequest = new RestRequest(Method.GET)
            {
                Resource = path,
                RequestFormat = DataFormat.Json,
            };

            restRequest.AddParameter("machineCode", _machineCode);

            return restRequest;
        }

        private static RestRequest CreatePostRestRequest(string path, object post)
        {

            var restRequest = new RestRequest(Method.POST)
            {
                Resource = path,
                RequestFormat = DataFormat.Json
            };

            restRequest.AddBody(post);

            return restRequest;
        }
    }
}