using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for ConfigUpdateView.xaml
    /// </summary>
    public partial class ConfigUpdateView : Page
    {
        public ConfigUpdateView(ConfigUpdateViewModel configUpdateViewModel)
        {
            InitializeComponent();

            _configUpdateGrid.DataContext = configUpdateViewModel;
        }
    }
}
