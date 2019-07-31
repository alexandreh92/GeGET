using GeGET.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Login>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            // OR whatever you want like logging etc. MessageBox it's just example
            // for quick debugging etc.
            e.Handled = true;
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            var moduleInfrastructureType = typeof(InfrastructureModule);
            moduleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = moduleInfrastructureType.Name,
                ModuleType = moduleInfrastructureType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.OnDemand
            });
        }

    }
}
