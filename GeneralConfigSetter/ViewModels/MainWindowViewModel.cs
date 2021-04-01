using GeneralConfigSetter.Models;
using GeneralConfigSetter.Views.Pages;
using GeneralConfigSetter.Views.Utility;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PatConfigView _patConfigView;
        private readonly ViewContainer _viewContainer;

        public NavigationService NavigationService { get; }
        public RelayCommand LoadConfigUpdateViewCommand { get; set; }
        public RelayCommand LoadPatConfigViewCommand { get; set; }
        public IContext Context { get; set; }

        public MainWindowViewModel(NavigationService navigationService, PatConfigView patConfigView, ViewContainer viewContainer, IContext context)
        {
            NavigationService = navigationService;

            _patConfigView = patConfigView;
            _viewContainer = viewContainer;
            Context = context;
            Context.Initialize();

            LoadConfigUpdateViewCommand = new RelayCommand(LoadConfigUpdateView, IsConfigUpdateViewEnabled);
            LoadPatConfigViewCommand = new RelayCommand(LoadPatConfigView, IsPatConfigViewEnabled);
        }

        private void LoadConfigUpdateView()
        {
            NavigationService.NavigateTo(_viewContainer.ConfigUpdateView);            
        }

        private bool IsConfigUpdateViewEnabled()
        {
            return !NavigationService.IsActiveContent(_viewContainer.ConfigUpdateView);
        }

        private void LoadPatConfigView()
        {
            NavigationService.NavigateTo(_patConfigView);
        }

        private bool IsPatConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_patConfigView);
        }

        public void LoadMainPage()
        {
            LoadConfigUpdateView();
            IsConfigUpdateViewEnabled();
        }
    }
}
