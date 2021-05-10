using GeneralConfigSetter.Models;
using GeneralConfigSetter.Services;
using GeneralConfigSetter.Views.Pages;
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

            var generalConfigSetter = Assembly.GetExecutingAssembly();
            var wpfFramework = typeof(NavigationService).Assembly;

            Container.RegisterTypeWithInterface<IContext, ContextModel>();
            Container.RegisterKeyedType<ConfigUpdateView>(typeof(ConfigUpdateView), "ConfigUpdateView");
            Container.RegisterKeyedType<PatConfigView>(typeof(PatConfigView), "PatConfigView");
            Container.RegisterSingleInstancesFromAssembly(generalConfigSetter);
            Container.RegisterSingleInstancesFromAssembly(wpfFramework);

            Container.Build();
            var mainWindow = Container.Resolve<MainWindowView>();
            mainWindow.Show();
        }
    }
}
