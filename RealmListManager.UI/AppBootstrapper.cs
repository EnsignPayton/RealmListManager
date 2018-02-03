using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using RealmListManager.UI.Shell;
using StructureMap;

namespace RealmListManager.UI
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IServiceLocator _serviceLocator;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var container = new Container(_ =>
            {
                _.For<IServiceLocator>().Use(__ => _serviceLocator);
                _.For<IWindowManager>().Singleton().Use<WindowManager>();
            });

            _serviceLocator = new StructureMapServiceLocator(container);

            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _serviceLocator.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _serviceLocator.GetAllInstances(service);
        }
    }
}
