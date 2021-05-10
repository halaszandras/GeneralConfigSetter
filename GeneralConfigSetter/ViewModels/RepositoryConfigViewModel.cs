using GeneralConfigSetter.Models;
using WpfFramework.Core;
using System.IO;
using GeneralConfigSetter.Enums;
using System;
using static GeneralConfigSetter.Services.DataAccessService;
using static GeneralConfigSetter.Services.ConfigUpdateService;

namespace GeneralConfigSetter.ViewModels
{
    public class RepositoryConfigViewModel : ViewModelBase
    {
        private string _repositoryConfigFilePath = "";
        private bool _isUpdateRepositoryConfigEnabled = false;
        private string _configState = "";
        private string _repositoryConfig;
        private IContext _context;

        public string RepositoryConfig
        {
            get { return _repositoryConfig; }
            set
            {
                value = ValidateInput(value);
                SetField(ref _repositoryConfig, value, nameof(RepositoryConfig));
                if (value == _configState)
                {
                    _isUpdateRepositoryConfigEnabled = false;
                }
                else
                {
                    _isUpdateRepositoryConfigEnabled = true;
                }
            }
        }
        public IContext Context
        {
            get { return _context; }
            set { SetField(ref _context, value, nameof(Context)); }
        }

        public RelayCommand UpdateRepositoryConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public RepositoryConfigViewModel(IContext context)
        {
            Context = context;
            _repositoryConfigFilePath = GetRepositoryConfigFilePath();
            RepositoryConfig = GetConfigFileContent(_repositoryConfigFilePath);
            _configState = RepositoryConfig;
            _isUpdateRepositoryConfigEnabled = false;

            UpdateRepositoryConfigCommand = new RelayCommand(UpdateRepositoryConfig, IsUpdateRespositoryConfigEnabled);
        }

        private void UpdateRepositoryConfig()
        {
            try
            {
                UpdateConfigFile(_repositoryConfigFilePath, RepositoryConfig);
                Context.InitializeRepositories();
                _configState = RepositoryConfig;
                _isUpdateRepositoryConfigEnabled = false;
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            catch (UnauthorizedAccessException unautharitedAccess)
            {
                ShowMessageCommand.Execute(new NotificationModel(unautharitedAccess.Message, NotificationType.Error));
            }
            catch (IOException IOException)
            {
                ShowMessageCommand.Execute(new NotificationModel(IOException.Message, NotificationType.Error));
            }
        }

        private bool IsUpdateRespositoryConfigEnabled()
        {
            return _isUpdateRepositoryConfigEnabled;
        }
    }
}
