using System;
using System.ServiceProcess;
using System.Timers;
using CustomerTracker.ClientController.Core;
using CustomerTracker.ClientControllerService.Properties;

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
            try
            {
                _bootstrap.Start();
            }
            catch (Exception exc)
            {

                _bootstrap.AddLog(exc);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _bootstrap = new Bootstrap(Settings.Default.MachineCode, Settings.Default.RamUsageAlarmLimit, Settings.Default.DiskUsageAlarmLimit,
         Settings.Default.CpuUsageAlarmLimit, Settings.Default.ServiceThreadCountAlarmLimit, Settings.Default.StateReceiverApiAddress, Settings.Default.ServiceNamesApiAddress);

                _timer = new Timer(Settings.Default.TimerInMinutes * 60000) { Enabled = true };
                _timer.Elapsed += _timer_Elapsed;
            }
            catch (Exception exc)
            {
                _bootstrap.AddLog(exc);
            }
        }

        protected override void OnStop()
        {
            //_log.Error("Stop çağırıldı.");
            if (_timer != null)
                _timer.Dispose();
        }


    }
}
