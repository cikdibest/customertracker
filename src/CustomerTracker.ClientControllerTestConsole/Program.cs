using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CustomerTracker.ClientController.Core;
using CustomerTracker.ClientControllerTestConsole.Properties;

namespace CustomerTracker.ClientControllerTestConsole
{
    class Program
    {
        private static Timer _timer;
        private static Bootstrap _bootstrap;

        static void Main(string[] args)
        {
            try
            {
                _bootstrap = new Bootstrap(Settings.Default.MachineCode, Settings.Default.RamUsageAlarmLimit, Settings.Default.DiskUsageAlarmLimit,
         Settings.Default.CpuUsageAlarmLimit, Settings.Default.ServiceThreadCountAlarmLimit, Settings.Default.StateReceiverApiAddress, Settings.Default.ServiceNamesApiAddress);

                _timer = new Timer(Settings.Default.TimerInMinutes * 60000) { Enabled = true };
                _timer.Elapsed += _timer_Elapsed;
                Console.ReadKey();
            }
            catch (Exception exc)
            {
                if (_bootstrap != null)
                    _bootstrap.AddLog(exc);
            }
        }


        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
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

    }
}
