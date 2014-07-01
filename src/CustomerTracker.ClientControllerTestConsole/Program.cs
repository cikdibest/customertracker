using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Timers;
using CustomerTracker.ClientController.Core;
using CustomerTracker.ClientControllerTestConsole.Properties;
using Timer = System.Timers.Timer;

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
                WriteApplicationInfo();
                
                IMessageNotifier messageNotifier=new MessageNotifier();

                messageNotifier.OnMessageFired += (sender, eventArgs) =>
                {
                    Console.WriteLine(sender as string);
                };

                _bootstrap = new Bootstrap(messageNotifier,Settings.Default.MachineCode, Settings.Default.RamUsageAlarmLimit, Settings.Default.DiskUsageAlarmLimit,
                                    Settings.Default.CpuUsageAlarmLimit, Settings.Default.ServiceThreadCountAlarmLimit, Settings.Default.ApiAddressPostServerCondition, Settings.Default.ApiAddressetGetApplicationServices);

                _timer = new Timer(Settings.Default.TimerInMinutes * 60000) { Enabled = true };
                
                _timer.Elapsed += _timer_Elapsed;

                _timer_Elapsed(null, null);

                Console.ReadKey();
            }
            catch (Exception exc)
            {
                if (_bootstrap != null)
                    _bootstrap.AddLog(exc);
            }
        }

        private static void WriteApplicationInfo()
        {
            Assembly assem = Assembly.GetExecutingAssembly();

            AssemblyName assemName = assem.GetName();

            Version ver = assemName.Version;

            Console.WriteLine("{0},Version {1}", assemName.Name, ver);

            Console.WriteLine("Timer period(min)     : {0}", Settings.Default.TimerInMinutes);

            Console.WriteLine("Service get url       : {0}", Settings.Default.ApiAddressetGetApplicationServices);

            Console.WriteLine("Machine state post url: {0}", Settings.Default.ApiAddressPostServerCondition);

            Console.WriteLine("--------------------Applicaton started-------------------");

            Console.WriteLine("press key for stop");
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("-------------------------Scanned-------------------------");
                _bootstrap.Start();
                Console.WriteLine("------------------------Completed------------------------");
            }
            catch (Exception exc)
            {
                Console.WriteLine("scan fail!");
                _bootstrap.AddLog(exc);
            }
        }

    }
}
