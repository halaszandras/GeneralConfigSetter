using Autofac.Features.AttributeFilters;
using GeneralConfigSetter.ViewModels;
using GeneralConfigSetter.Views.Pages;
using MahApps.Metro.Controls;

namespace GeneralConfigSetter.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        public MainWindowView(MainWindowViewModel mainWindowViewModel, [KeyFilter("ConfigUpdateView")]ConfigUpdateView configUpdateView)
        {
            InitializeComponent();

            _mainWindow.DataContext = mainWindowViewModel;
            mainWindowViewModel.NavigationService.SetFrame(_mainFrame);

            mainWindowViewModel.NavigationService.NavigateTo(configUpdateView);
        }
    }
}
