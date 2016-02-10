using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace PlexPowerSaver
{
    public class ConsoleService : ServiceBase
    {
        private const int Minutes = 60000;
        private const int PollInterval = (int)(1 * Minutes);

        private static int _idleTimeout = (int)(30 * Minutes);
        private static string _clientId;
        private static Timer _timer;
        private static PlexAccess _plex;
        private static List<String> _blacklistedProcesses;

        public ConsoleService()
        {
            this.ServiceName = Assembly.GetExecutingAssembly().GetName().Name;
            initialize();
        }

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ConsoleService service = new ConsoleService();
            if (Environment.UserInteractive)
            {
                try
                {
                    string option = args.Length > 0 ? args[0].ToUpperInvariant() : String.Empty;
                    switch (option)
                    {
                        case "-I":
                        case "/I":
                            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetCallingAssembly().Location });
                            break;

                        case "-U":
                        case "/U":
                            ManagedInstallerClass.InstallHelper(new string[] { "/U", Assembly.GetCallingAssembly().Location });
                            break;

                        default:
                            service.OnStart(args);
                            Trace.WriteLine("Running... Press any key to stop");
                            Trace.WriteLine("");
                            Console.ReadKey();
                            service.OnStop();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
            else
            {
                ServiceBase.Run(service);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
                Trace.WriteLine(((Exception)e.ExceptionObject).Message);
        }

        /// <summary>
        /// Determine if computer is idle
        /// </summary>
        /// <returns>true if the computer has been idle longer than set timeout</returns>
        private static bool ComputerIdle()
        {
            var currentIdle = SystemIdle.GetIdleTime();
            Trace.WriteLine("System has been idle for " + currentIdle / 60000 + " minutes.");
            return currentIdle >= _idleTimeout;
        }

        private void initialize()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            // Load settings
            _clientId = Properties.Settings.Default.ClientId;
            _blacklistedProcesses = Properties.Settings.Default.BlacklistedProcesses.Cast<string>().ToList<string>();
            _idleTimeout = int.Parse(Properties.Settings.Default.IdleTimeout) * Minutes;

            // Create the plex api client
            _plex = new PlexAccess(_clientId);
            Trace.WriteLine("Initialized plex connections...");

            // Configure and start the timer
            Trace.WriteLine("Starting timer, waiting for idle...");
            _timer = new Timer(PollInterval);
            _timer.Elapsed += Timer_Elapsed;
        }

        protected override void OnStart(string[] args)
        {
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
        }

        /// <summary>
        /// Check if any blacklisted process names are currently running
        /// </summary>
        /// <returns>true if no listed process is found to be running</returns>
        private static bool NoBlackListProgramOpen()
        {
            foreach (string processName in _blacklistedProcesses)
            {
                if (Process.GetProcessesByName(processName).Length > 0)
                {
                    Trace.WriteLine("Blacklisted process was found to be running: " + processName);
                    return false;
                }
            }
            Trace.WriteLine("No blacklisted process was found to be running.");
            return true;
        }

        private static void Shutdown()
        {
            Trace.WriteLine("Shutdown Activated");
            Process.Start("Shutdown", "-s -t 60 -c \"The system has been detected as idle, and will shutdown in one minute.\"");
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            Trace.WriteLine("Testing system idle status...");

            if (ComputerIdle() && NoBlackListProgramOpen() && _plex.NoActiveStreams())
            {
                Shutdown();
            }
            else
            {
                _timer.Enabled = true;
            }
        }
    }
}