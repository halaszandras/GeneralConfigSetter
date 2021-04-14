using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Services;
using System;
using System.IO;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class PatConfigViewModel : ViewModelBase
    {
        private string _patConfigFilePath = "";
        private bool _isUpdatePatConfigEnabled = false;
        private string _configState = "";
        private string _patConfig;
        private IContext _context;

        public string PatConfig
        {
            get { return _patConfig; }
            set
            {
                SetField(ref _patConfig, value, nameof(PatConfig));
                if (value == _configState)
                {
                    _isUpdatePatConfigEnabled = false;
                }
                else
                {
                    _isUpdatePatConfigEnabled = true;
                }
            }
        }
        public IContext Context
        {
            get { return _context; }
            set { SetField(ref _context, value, nameof(Context)); }
        }

        public RelayCommand UpdatePatConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public PatConfigViewModel(IContext context)
        {
            Context = context;
            _patConfigFilePath = DataAccessService.GetPatConfigFilePath();
            PatConfig = DataAccessService.GetConfigFileContent(_patConfigFilePath);
            _configState = PatConfig;
            _isUpdatePatConfigEnabled = false;

            UpdatePatConfigCommand = new RelayCommand(UpdatePatConfig, IsUpdatePatConfigEnabled);
        }

        private void UpdatePatConfig()
        {
            try
            {
                DataAccessService.UpdateConfigFile(_patConfigFilePath, PatConfig);
                Context.InitializePats();
                _configState = PatConfig;
                _isUpdatePatConfigEnabled = false;
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

        private bool IsUpdatePatConfigEnabled()
        {
            return _isUpdatePatConfigEnabled;
        }
    }
}
