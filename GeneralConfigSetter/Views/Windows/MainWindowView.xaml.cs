using GeneralConfigSetter.ViewModels;
using GeneralConfigSetter.Views.Pages;
using GeneralConfigSetter.Views.Utility;
using System.Windows;

namespace GeneralConfigSetter.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly ViewContainer viewContainer;

        public MainWindowView(MainWindowViewModel mainWindowViewModel, ViewContainer viewContainer)
        {
            InitializeComponent();

            _mainGrid.DataContext = mainWindowViewModel;
            mainWindowViewModel.NavigationService.SetFrame(_mainFrame);

            //mainWindowViewModel.NavigationService.NavigateTo(viewContainer.ConfigUpdateView);
            //mainWindowViewModel.NavigationService.ActivePageChanged += NavigationService_ActivePageChanged;
            mainWindowViewModel.LoadMainPage();

            this.mainWindowViewModel = mainWindowViewModel;
            this.viewContainer = viewContainer;
        }

        //private void NavigationService_ActivePageChanged()
        //{
        //    mainWindowViewModel.LoadConfigUpdateViewCommand.RaiseCanExecuteChanged();
        //}
    }
}
