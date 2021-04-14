﻿using Autofac.Features.AttributeFilters;
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
        private readonly DeleterConfigView _deleterConfigView;
        private readonly AttachmentConfigView _attachmentConfigView;
        private readonly RepositoryConfigView _repositoryConfigView;
        private readonly ConfigUpdateView _configUpdateView;

        public NavigationService NavigationService { get; }
        public RelayCommand LoadConfigUpdateViewCommand { get; set; }
        public RelayCommand LoadPatConfigViewCommand { get; set; }
        public RelayCommand LoadDeleterConfigViewCommand { get; set; }
        public RelayCommand LoadAttachmentConfigViewCommand { get; set; }
        public RelayCommand LoadRepositoryConfigViewCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; set; }
        public IContext Context { get; set; }
        public NotificationViewModel NotificationViewModel { get; set; }

        public MainWindowViewModel(NavigationService navigationService,
                                   [KeyFilter("ConfigUpdateView")] ConfigUpdateView configUpdateView,
                                   PatConfigView patConfigView,
                                   DeleterConfigView deleterConfigView,
                                   AttachmentConfigView attachmentConfigView,
                                   RepositoryConfigView repositoryConfigView,
                                   IContext context,
                                   NotificationViewModel notificationViewModel)
        {
            NavigationService = navigationService;

            _patConfigView = patConfigView;
            _deleterConfigView = deleterConfigView;
            _attachmentConfigView = attachmentConfigView;
            _repositoryConfigView = repositoryConfigView;
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
            ShowMessageCommand = new RelayCommandGeneric<NotificationModel, bool>(NotificationViewModel.ShowMessage, NotificationViewModel.IsShowMessageEnabled);

            _patConfigView.ShowMessageCommand = ShowMessageCommand;
            _configUpdateView.ShowMessageCommand = ShowMessageCommand;
            _deleterConfigView.ShowMessageCommand = ShowMessageCommand;
            _attachmentConfigView.ShowMessageCommand = ShowMessageCommand;
            _repositoryConfigView.ShowMessageCommand = ShowMessageCommand;


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
    }
}
