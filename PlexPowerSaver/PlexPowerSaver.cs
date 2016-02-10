using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;

namespace PlexPowerSaver
{

    static class Program
    {
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new IdleWaiter());
        }
    }

    public class IdleWaiter : ApplicationContext
    {
        private const int Minutes = 60000;
        private const int PollInterval = (int)(1 * Minutes);

        private static int _idleTimeout = (int)(30 * Minutes);
        private static string _clientId;
        private static System.Timers.Timer _timer;
        private static PlexAccess _plex;
        private static List<String> _blacklistedProcesses;

        private NotifyIcon trayIcon;

        public IdleWaiter()
        {
            initialize();
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

            var aName = Assembly.GetExecutingAssembly().GetName();

            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.timer,
                Text = aName.Name + " " + aName.Version,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            // Load settings
            _clientId = Properties.Settings.Default.ClientId;
            _blacklistedProcesses = Properties.Settings.Default.BlacklistedProcesses.Cast<string>().ToList<string>();
            _idleTimeout = int.Parse(Properties.Settings.Default.IdleTimeout) * Minutes;

            // Create the plex api client
            _plex = new PlexAccess(_clientId);
            Trace.WriteLine("Initialized plex connections...");

            // Configure and start the timer
            Trace.WriteLine("Starting timer, waiting for idle...");
            _timer = new System.Timers.Timer(PollInterval);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            _timer.Enabled = false;
            Application.Exit();
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