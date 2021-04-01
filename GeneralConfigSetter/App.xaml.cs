﻿using Autofac;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Views.Windows;
using System.Reflection;
using System.Windows;
using WpfFramework.Core;

namespace GeneralConfigSetter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();

            var generalConfigSetter = Assembly.GetExecutingAssembly();
            var wpfFramework = typeof(NavigationService).Assembly;

            builder.RegisterAssemblyTypes(generalConfigSetter);
            builder.RegisterAssemblyTypes(wpfFramework).SingleInstance();
            builder.RegisterType<ContextModel>().As<IContext>().SingleInstance();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var mainWindow = scope.Resolve<MainWindowView>();
                mainWindow.Show();
            }
        }
    }
}