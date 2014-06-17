using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly RamController _ramController;
        private readonly DiskController _diskController;
        private readonly CpuController _cpuController;
        private readonly ServiceStateController _serviceController;
        private readonly double _ramUsageAlarmLimit;
        private readonly double _diskUsageAlarmLimit;
        private readonly double _cpuUsageAlarmLimit;
        private readonly double _serviceThreadCountAlarmLimit;
        private readonly string _apiAddressetGetApplicationServices;
        private readonly string _apiAddressPostServerCondition;

        public Bootstrap(string machineCode, double ramUsageAlarmLimit, double diskUsageAlarmLimit, double cpuUsageAlarmLimit, double serviceThreadCountAlarmLimit,
            string apiAddressetGetApplicationServices, string apiAddressPostApiAddressPostServerCondition)
        {
            this._log = LogManager.GetCurrentClassLogger();
            this._machineCode = machineCode;
            this._ramUsageAlarmLimit = ramUsageAlarmLimit;
            this._diskUsageAlarmLimit = diskUsageAlarmLimit;
            this._cpuUsageAlarmLimit = cpuUsageAlarmLimit;
            this._serviceThreadCountAlarmLimit = serviceThreadCountAlarmLimit;
            this._apiAddressetGetApplicationServices = apiAddressetGetApplicationServices;
            this._apiAddressPostServerCondition = apiAddressPostApiAddressPostServerCondition;
            this._ramController = new RamController();
            this._diskController = new DiskController();
            this._cpuController = new CpuController();
            this._serviceController = new ServiceStateController();
        }

        public void Start()
        {

            _log.Trace("Verilen süre geldi.");
            if (!FillServiceNameList()) return;
            var hardwareConditionList = new List<HardwareControlMessage>();

            var ramState = _ramController.GetRamState(_ramUsageAlarmLimit);
            _log.Trace("Ram State : " + ramState.Data + " Alarm mı :" + ramState.IsAlarm);
            hardwareConditionList.Add(ramState);


            var diskState = _diskController.GetDiskState(_diskUsageAlarmLimit);
            _log.Trace("Disk State : " + diskState.Data + " Alarm mı :" + diskState.IsAlarm);
            hardwareConditionList.Add(diskState);

            var cpuState = _cpuController.GetCpuState(_cpuUsageAlarmLimit);
            _log.Trace("CPU State : " + cpuState.Data + " Alarm mı :" + cpuState.IsAlarm);
            hardwareConditionList.Add(cpuState);


            var serviceConditionList = new List<ServiceControlMessage>();

            foreach (var service in _serviceNameList)
                serviceConditionList.Add(_serviceController.GetServiceState(service, _serviceThreadCountAlarmLimit));
            _log.Trace("İstenen Servislerin sayısı : " + serviceConditionList.Count);


            var serverCondition = new ServerCondition
            {
                HardwareControlMessages = hardwareConditionList,
                ServiceControlMessages = serviceConditionList,
                MachineCode = _machineCode,
                IsAlarm = hardwareConditionList.Any(q => q.IsAlarm) || serviceConditionList.Any(q => q.IsAlarm)
            };
            _log.Trace("Servis durumu gönderiliyor. Alarm var mı : " + (serverCondition.HardwareControlMessages.Count(c => c.IsAlarm) + serverCondition.ServiceControlMessages.Count(c => c.IsAlarm)));


            var restRequest = CreatePostRestRequest(_apiAddressetGetApplicationServices, serverCondition);
            var restClient = new RestClient();
            var response = restClient.Execute(restRequest);
            if (response.StatusCode==HttpStatusCode.OK)
                _log.Trace("Donanım bilgileri gönderildi");
            else
                _log.Trace("Hata!.Donanım bilgileri gönderilemedi");
        }

        #region private methods
        private bool FillServiceNameList()
        {
            try
            {
                _log.Trace("Servis listesi alınıyor");
                RestRequest restRequest = CreateGetRestRequest(_apiAddressPostServerCondition);
                var restClient = new RestClient();
                var targetServices = restClient.Execute<List<TargetService>>(restRequest);
                _serviceNameList = new List<TargetService>(targetServices.Data);
                _log.Trace("Servis listesi alındı.");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("Servis bilgileri alınırken hata oluştu : " + ex);
                return false;
            }
        }

        private RestRequest CreateGetRestRequest(string path)
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
        #endregion

        public void AddLog(Exception exc)
        {
            _log.Error(exc);
        }
    }
}