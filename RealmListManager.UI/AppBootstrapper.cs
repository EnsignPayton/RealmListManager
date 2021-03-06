﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using RealmListManager.UI.Core;
using RealmListManager.UI.Core.Utilities;
using RealmListManager.UI.Screens;
using StructureMap;

namespace RealmListManager.UI
{
    public class AppBootstrapper : BootstrapperBase
    {
        private IServiceLocator _serviceLocator;
        private IDbConnection _dbConnection;
        private IConfigurationManager _configurationManager;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _dbConnection = DbConnectionProvider.GetConnection();

            var container = new Container(_ =>
            {
                _.ForConcreteType<DbConnectionManager>().Configure.Singleton();
                _.ForConcreteType<ShellViewModel>().Configure.Singleton();

                _.For<IServiceLocator>().Use(__ => _serviceLocator);
                _.For<IWindowManager>().Singleton().Use<WindowManager>();
                _.For<IWindowConductor>().Singleton().Use<ShellViewModel>();
                _.For<IEventAggregator>().Singleton().Use<EventAggregator>();
                _.For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
                _.For<IDbConnection>().Use(_dbConnection);
            });

            _serviceLocator = new StructureMapServiceLocator(container);

            var connectionManager = _serviceLocator.GetInstance<DbConnectionManager>();
            connectionManager.CreateTables();

            _configurationManager = _serviceLocator.GetInstance<IConfigurationManager>();
            _configurationManager.Load();

            DisplayRootViewFor<ShellViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _configurationManager.Save();

            base.OnExit(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _serviceLocator.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _serviceLocator.GetAllInstances(service);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var windowConductor = _serviceLocator.GetInstance<IWindowConductor>();
            windowConductor.ShowMessageBox($"An error has occured: {e.Exception.Message}{Environment.NewLine}Application will now terminate.",
                "Unexpected Error", MessageBoxButton.OK);
            base.OnUnhandledException(sender, e);
        }
    }
}
