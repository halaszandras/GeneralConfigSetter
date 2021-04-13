using Autofac.Features.AttributeFilters;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Views.Pages;
using static GeneralConfigSetter.Services.PatService;
using WpfFramework.Core;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Services;

namespace GeneralConfigSetter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PatConfigView _patConfigView;
        private readonly ConfigUpdateView _configUpdateView;

        public NavigationService NavigationService { get; }
        public RelayCommand LoadConfigUpdateViewCommand { get; set; }
        public RelayCommand LoadPatConfigViewCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; set; }
        public IContext Context { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }

        public MainWindowViewModel(NavigationService navigationService, PatConfigView patConfigView, [KeyFilter("ConfigUpdateView")]ConfigUpdateView configUpdateView, IContext context, NotificationViewModel notificationViewModel)
        {
            NavigationService = navigationService;

            _patConfigView = patConfigView;
            _configUpdateView = configUpdateView;
            Context = context;
            Context.Initialize();
            NotificationViewModel = notificationViewModel;

            LoadConfigUpdateViewCommand = new RelayCommand(LoadConfigUpdateView, IsConfigUpdateViewEnabled);
            LoadPatConfigViewCommand = new RelayCommand(LoadPatConfigView, IsPatConfigViewEnabled);
            ShowMessageCommand = new RelayCommandGeneric<NotificationModel, bool>(NotificationViewModel.ShowMessage, NotificationViewModel.IsShowMessageEnabled);

            _patConfigView.ShowMessageCommand = ShowMessageCommand;
            _configUpdateView.ShowMessageCommand = ShowMessageCommand;

            CheckPatFreshness(DataAccessService.GetPatConfigFilePath());
        }

        private void CheckPatFreshness(string filePath)
        {
            int difference = PatComparedToToday(filePath);
            if (difference < 0)
            {
                ShowMessageCommand.Execute(new NotificationModel("Pat Config is up to date!", NotificationType.Information));
            }
            else if (difference > 0)
            {
                ShowMessageCommand.Execute(new NotificationModel("PAT CONFIG EXPIRED!!!!", NotificationType.Error));
            }
            else if (difference == 0)
            {
                ShowMessageCommand.Execute(new NotificationModel("Pat Config will expire tomorrow!", NotificationType.Error));
            }
        }

        private void LoadConfigUpdateView()
        {
            NavigationService.NavigateTo(_configUpdateView);            
        }

        private bool IsConfigUpdateViewEnabled()
        {
            return !NavigationService.IsActiveContent(_configUpdateView);
        }

        private void LoadPatConfigView()
        {
            NavigationService.NavigateTo(_patConfigView);
        }

        private bool IsPatConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_patConfigView);
        }
    }
}
