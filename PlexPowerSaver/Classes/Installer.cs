using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PlexPowerSaver
{
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        public WindowsServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();
            var asm = Assembly.GetExecutingAssembly();
            processInstaller.Account = ServiceAccount.User;

            serviceInstaller.DisplayName = asm.GetName().Name;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = asm.GetName().Name;

            Type type = typeof(AssemblyDescriptionAttribute);
            if (AssemblyDescriptionAttribute.IsDefined(asm, type))
            {
                AssemblyDescriptionAttribute a = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(asm, type);
                serviceInstaller.Description = a.Description;
            }

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
