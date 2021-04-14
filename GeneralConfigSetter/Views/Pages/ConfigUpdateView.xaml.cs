using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for ConfigUpdateView.xaml
    /// </summary>
    public partial class ConfigUpdateView : Page
    {
        private readonly ConfigUpdateViewModel _configUpdateViewModel;

        public ConfigUpdateView(ConfigUpdateViewModel configUpdateViewModel)
        {
            InitializeComponent();

            _configUpdateGrid.DataContext = configUpdateViewModel;
            _configUpdateViewModel = configUpdateViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _configUpdateViewModel.ShowMessageCommand; } internal set { _configUpdateViewModel.ShowMessageCommand = value; } }
    }
}
