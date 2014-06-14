using System;
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
                Console.WriteLine("started");
                
                _bootstrap = new Bootstrap(Settings.Default.MachineCode, Settings.Default.RamUsageAlarmLimit, Settings.Default.DiskUsageAlarmLimit,
                                    Settings.Default.CpuUsageAlarmLimit, Settings.Default.ServiceThreadCountAlarmLimit, Settings.Default.ApiAddressPostServerCondition, Settings.Default.ApiAddressetGetApplicationServices);

                _timer = new Timer(Settings.Default.TimerInMinutes * 60000) { Enabled = true };
                
                _timer.Elapsed += _timer_Elapsed;
                
                Console.WriteLine("press key for stop");
                
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
                Console.WriteLine("scan started");
                _bootstrap.Start();
                Console.WriteLine("scan completed");
            }
            catch (Exception exc)
            {
                Console.WriteLine("scan fail!");
                _bootstrap.AddLog(exc);
            }
        }

    }
}
