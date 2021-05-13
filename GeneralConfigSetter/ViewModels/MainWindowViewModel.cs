using System.Collections.Generic;
using Autofac.Features.AttributeFilters;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Views.Pages;
using static GeneralConfigSetter.Services.PatService;
using WpfFramework.Core;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Services;
using ACrypto;
using Microsoft.Win32;

namespace GeneralConfigSetter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly PatConfigView _patConfigView;
        private readonly DeleterConfigView _deleterConfigView;
        private readonly AttachmentConfigView _attachmentConfigView;
        private readonly RepositoryConfigView _repositoryConfigView;
        private readonly ConfigUpdateView _configUpdateView;
        private readonly BugFamilyView _bugFamilyView;
        private readonly InfoView _infoView;
        private readonly TestItemsView _testItemsView;

        public NavigationService NavigationService { get; }
        public RelayCommand LoadConfigUpdateViewCommand { get; set; }
        public RelayCommand LoadPatConfigViewCommand { get; set; }
        public RelayCommand LoadDeleterConfigViewCommand { get; set; }
        public RelayCommand LoadAttachmentConfigViewCommand { get; set; }
        public RelayCommand LoadRepositoryConfigViewCommand { get; set; }
        public RelayCommand LoadBugFamilyViewCommand { get; set; }
        public RelayCommand LoadInfoViewCommand { get; set; }
        public RelayCommand LoadTestItemsViewCommand { get; set; }
        public RelayCommand LockWorkItemsCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; set; }
        public IContext Context { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }

        public MainWindowViewModel(NavigationService navigationService,
                                   [KeyFilter("ConfigUpdateView")] ConfigUpdateView configUpdateView,
                                   PatConfigView patConfigView,
                                   DeleterConfigView deleterConfigView,
                                   AttachmentConfigView attachmentConfigView,
                                   RepositoryConfigView repositoryConfigView,
                                   BugFamilyView bugFamilyView,
                                   InfoView infoView,
                                   TestItemsView testItemsView,
                                   IContext context,
                                   NotificationViewModel notificationViewModel)
        {
            NavigationService = navigationService;

            _patConfigView = patConfigView;
            _deleterConfigView = deleterConfigView;
            _attachmentConfigView = attachmentConfigView;
            _repositoryConfigView = repositoryConfigView;
            _bugFamilyView = bugFamilyView;
            _infoView = infoView;
            _testItemsView = testItemsView;
            _configUpdateView = configUpdateView;
            Context = context;
            Context.InitializePats();
            Context.InitializeRepositories();
            NotificationViewModel = notificationViewModel;

            LoadConfigUpdateViewCommand = new RelayCommand(LoadConfigUpdateView, IsConfigUpdateViewEnabled);
            LoadPatConfigViewCommand = new RelayCommand(LoadPatConfigView, IsPatConfigViewEnabled);
            LoadDeleterConfigViewCommand = new RelayCommand(LoadDeleterConfigView, IsDeleterConfigViewEnabled);
            LoadAttachmentConfigViewCommand = new RelayCommand(LoadAttachmentConfigView, IsAttachmentConfigViewEnabled);
            LoadRepositoryConfigViewCommand = new RelayCommand(LoadRepositoryConfigView, IsRepositoryConfigViewEnabled);
            LoadBugFamilyViewCommand = new RelayCommand(LoadBugFamilyView, IsBugFamilyViewEnabled);
            LoadInfoViewCommand = new RelayCommand(LoadInfoView, IsInfoViewEnabled);
            LoadTestItemsViewCommand = new RelayCommand(LoadTestItemsView, IsTestItemsViewEnabled);
            ShowMessageCommand = new RelayCommandGeneric<NotificationModel, bool>(NotificationViewModel.ShowMessage, NotificationViewModel.IsShowMessageEnabled);
            LockWorkItemsCommand = new RelayCommand(LockWorkItems, IsLockWorkItemsEnabled);

            _patConfigView.ShowMessageCommand = ShowMessageCommand;
            _configUpdateView.ShowMessageCommand = ShowMessageCommand;
            _deleterConfigView.ShowMessageCommand = ShowMessageCommand;
            _attachmentConfigView.ShowMessageCommand = ShowMessageCommand;
            _repositoryConfigView.ShowMessageCommand = ShowMessageCommand;
            _bugFamilyView.ShowMessageCommand = ShowMessageCommand;
            _testItemsView.ShowMessageCommand = ShowMessageCommand;
            _infoView.ShowMessageCommand = ShowMessageCommand;

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

        private void LoadBugFamilyView()
        {
            NavigationService.NavigateTo(_bugFamilyView);
        }

        private bool IsBugFamilyViewEnabled()
        {
            return !NavigationService.IsActiveContent(_bugFamilyView);
        }
        private void LoadInfoView()
        {
            NavigationService.NavigateTo(_infoView);
        }

        private bool IsInfoViewEnabled()
        {
            return !NavigationService.IsActiveContent(_infoView);
        }
        
        private void LoadTestItemsView()
        {
            NavigationService.NavigateTo(_testItemsView);
        }
        private bool IsTestItemsViewEnabled()
        {
            return !NavigationService.IsActiveContent(_testItemsView);
        }

        private bool IsConfigUpdateViewEnabled()
        {
            return !NavigationService.IsActiveContent(_configUpdateView);
        }
        private void LoadConfigUpdateView()
        {
            NavigationService.NavigateTo(_configUpdateView);
        }
        private void LoadPatConfigView()
        {
            NavigationService.NavigateTo(_patConfigView);
        }

        private bool IsPatConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_patConfigView);
        }

        private void LoadDeleterConfigView()
        {
            NavigationService.NavigateTo(_deleterConfigView);
        }

        private bool IsDeleterConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_deleterConfigView);
        }

        private void LoadAttachmentConfigView()
        {
            NavigationService.NavigateTo(_attachmentConfigView);
        }

        private bool IsAttachmentConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_attachmentConfigView);
        }

        private void LoadRepositoryConfigView()
        {
            NavigationService.NavigateTo(_repositoryConfigView);
        }

        private bool IsRepositoryConfigViewEnabled()
        {
            return !NavigationService.IsActiveContent(_repositoryConfigView);
        }

        private void LockWorkItems()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select Configs which should lock workitems.";
            var result = openFileDialog.ShowDialog();

            if (result != null && result == true)
            {
                //Get the path of specified file
                string[] filePaths = openFileDialog.FileNames;

                WitLockingService.Lock(filePaths);
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            else
            {
                ShowMessageCommand.Execute(new NotificationModel("But why? :(", NotificationType.Error));
            }
        }

        private bool IsLockWorkItemsEnabled()
        {
            return true;
        }
    }
}
