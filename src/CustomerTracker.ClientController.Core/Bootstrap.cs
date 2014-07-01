using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
        private readonly IMessageNotifier _messageNotifier;
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

        public Bootstrap(IMessageNotifier messageNotifier, string machineCode, double ramUsageAlarmLimit, double diskUsageAlarmLimit, double cpuUsageAlarmLimit, double serviceThreadCountAlarmLimit, string apiAddressetGetApplicationServices, string apiAddressPostApiAddressPostServerCondition)
        {
            this._log = LogManager.GetCurrentClassLogger();
            _messageNotifier = messageNotifier;
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
            if (!FillServiceNameList()) return;
            var hardwareConditionList = new List<HardwareControlMessage>();

            var ramState = _ramController.GetRamState(_ramUsageAlarmLimit);
            hardwareConditionList.Add(ramState);


            var diskState = _diskController.GetDiskState(_diskUsageAlarmLimit);
              hardwareConditionList.Add(diskState);

            var cpuState = _cpuController.GetCpuState(_cpuUsageAlarmLimit);
            hardwareConditionList.Add(cpuState);


            var serviceConditionList = new List<ServiceControlMessage>();

            foreach (var service in _serviceNameList)
                serviceConditionList.Add(_serviceController.GetServiceState(service, _serviceThreadCountAlarmLimit));
             
            var serverCondition = new ServerCondition
            {
                HardwareControlMessages = hardwareConditionList,
                ServiceControlMessages = serviceConditionList,
                MachineCode = _machineCode,
                IsAlarm = hardwareConditionList.Any(q => q.IsAlarm) || serviceConditionList.Any(q => q.IsAlarm)
            };
          
            var restRequest = CreatePostRestRequest(_apiAddressetGetApplicationServices, serverCondition);
          
            var restClient = new RestClient();

            _messageNotifier.MessageFired("Try post machine state");

            _log.Trace(serverCondition);

            var response = restClient.Execute(restRequest);
            
            if (response.StatusCode == HttpStatusCode.OK)
            { 
                _messageNotifier.MessageFired("Succcesfull! Posted machine state");
            }

            else
            { 
                _log.Error(response.ErrorException);
                 
                _messageNotifier.MessageFired("Fail! Not post it machine state");
            }
                
        }

        #region private methods
        private bool FillServiceNameList()
        {
            try
            { 
                _messageNotifier.MessageFired("Request service list");
                
                RestRequest restRequest = CreateGetRestRequest(_apiAddressPostServerCondition);
                
                var restClient = new RestClient();
                
                var targetServices = restClient.Execute<List<TargetService>>(restRequest);

                _messageNotifier.MessageFired("Service list count = " + targetServices.Data.Count);

                _serviceNameList = new List<TargetService>(targetServices.Data);
              
                return true;
            }
            catch (Exception ex)
            {
                _messageNotifier.MessageFired("Error occured during request service list");

                AddLog( ex);
             
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