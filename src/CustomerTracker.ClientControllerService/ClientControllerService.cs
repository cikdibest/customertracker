using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using CustomerTracker.ClientControllerService.Controllers;
using CustomerTracker.ClientControllerService.Models;
using CustomerTracker.ClientControllerService.Properties;
using NLog;
using RestSharp;

namespace CustomerTracker.ClientControllerService
{
    public partial class ClientControllerService : ServiceBase
    {
        readonly Logger _log = LogManager.GetCurrentClassLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Timer _timer;
        private static string _machineCode;

        private static List<TargetService> _serviceNameList = new List<TargetService>();

        private static RamController _ramController;
        private static DiskController _diskController;
        private static CpuController _cpuController;
        private static ServiceStateController _serviceController;
        public ClientControllerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _log.Debug("Start çağırıldı.");
            _timer = new Timer(Settings.Default.TimerInMinutes * 60000) {Enabled = true};
            _timer.Elapsed += _timer_Elapsed;
            _machineCode = Settings.Default.MachineCode;
            _ramController = new RamController();
            _diskController = new DiskController();
            _cpuController = new CpuController();
            _serviceController = new ServiceStateController();
        }

        protected override void OnStop()
        {
            _log.Error("Stop çağırıldı.");
            _timer.Dispose();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _log.Debug("Verilen süre geldi.");
            if (!fillServiceNameList())
            {
                return;
            }
            var hardwareConditionList = new List<HardwareControlMessage>();
            var ramState = _ramController.GetRamState();
            _log.Debug("Ram State : " + ramState.Data + " Alarm mı :" + ramState.IsAlarm);
            hardwareConditionList.Add(ramState);
            var diskState = _diskController.GetDiskState();
            _log.Debug("Disk State : " + diskState.Data + " Alarm mı :" + diskState.IsAlarm);

            hardwareConditionList.Add(diskState);

            var cpuState = _cpuController.GetCpuState();
            _log.Debug("CPU State : " + cpuState.Data + " Alarm mı :" + cpuState.IsAlarm);

            hardwareConditionList.Add(cpuState);
            

            var serviceConditionList = new List<ServiceControlMessage>();

            foreach (var service in _serviceNameList)
            {
                serviceConditionList.Add(_serviceController.GetServiceState(service));
            }
            _log.Debug("İstenen Servislerin sayısı : " + serviceConditionList.Count);
            var serverCondition = new ServerCondition
            {
                HardwareControlMessages = hardwareConditionList,
                ServiceControlMessages = serviceConditionList,
                MachineCode = Settings.Default.MachineCode
            };
            _log.Debug("Servis durumu gönderiliyor. Alarm var mı : "+(serverCondition.HardwareControlMessages.Count(c => c.IsAlarm)+serverCondition.ServiceControlMessages.Count(c => c.IsAlarm)));
            var restRequest = CreatePostRestRequest(Settings.Default.StateReceiverApiAddress, serverCondition);
            var restClient = new RestClient();
            var response = restClient.Execute(restRequest);

        }

        private bool fillServiceNameList()
        {
            try
            {
                _log.Warn("Servis listesi alınıyor");
                RestRequest restRequest = CreateGetRestRequest(Settings.Default.ServiceNamesApiAddress);
                var restClient = new RestClient();
                var targetServices = restClient.Execute<List<TargetService>>(restRequest);
                _serviceNameList = new List<TargetService>(targetServices.Data);
                _log.Warn("Servis listesi alındı.");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("Servis bilgileri alınırken hata oluştu : "+ex);
                return false;
            }
        }

        public RestRequest CreateGetRestRequest(string path)
        {
            var restRequest = new RestRequest(Method.GET)
            {
                Resource = path,
                RequestFormat = DataFormat.Json
            };

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
