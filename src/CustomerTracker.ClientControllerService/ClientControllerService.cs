using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using CustomerTracker.ClientController.Core;
using CustomerTracker.ClientController.Core.Controllers;
using CustomerTracker.ClientController.Core.Models;
using CustomerTracker.ClientControllerService.Properties;
using NLog;
using RestSharp;

namespace CustomerTracker.ClientControllerService
{
    public partial class ClientControllerService : ServiceBase
    {
        private Timer _timer;
        private Bootstrap _bootstrap;
       
        public ClientControllerService()
        {
            InitializeComponent();

          
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            _bootstrap.Start(); 
        }

        protected override void OnStart(string[] args)
        {
              _bootstrap = new Bootstrap(Settings.Default.MachineCode, Settings.Default.RamUsageAlarmLimit, Settings.Default.DiskUsageAlarmLimit,
            Settings.Default.CpuUsageAlarmLimit, Settings.Default.ServiceThreadCountAlarmLimit, Settings.Default.StateReceiverApiAddress, Settings.Default.ServiceNamesApiAddress);

            _timer = new Timer(Settings.Default.TimerInMinutes * 60000) { Enabled = true };
            _timer.Elapsed += _timer_Elapsed;
        }

        protected override void OnStop()
        {
            //_log.Error("Stop çağırıldı.");
            //_timer.Dispose();
        }

   
    }
}
