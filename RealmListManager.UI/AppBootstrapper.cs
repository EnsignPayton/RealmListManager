using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Caliburn.Micro;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using RealmListManager.UI.Core.Utilities;
using RealmListManager.UI.Shell;
using StructureMap;

namespace RealmListManager.UI
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IServiceLocator _serviceLocator;
        private IDbConnection _dbConnection;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var connectionProvider = new DbConnectionProvider();
            _dbConnection = connectionProvider.GetConnection();

            var container = new Container(_ =>
            {
                _.For<IServiceLocator>().Use(__ => _serviceLocator);
                _.For<IWindowManager>().Singleton().Use<WindowManager>();
                _.For<IDbConnection>().Use(_dbConnection);
                _.ForConcreteType<DbConnectionManager>().Configure.Singleton();
            });

            _serviceLocator = new StructureMapServiceLocator(container);

            var connectionManager = _serviceLocator.GetInstance<DbConnectionManager>();
            connectionManager.CreateTables();

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
