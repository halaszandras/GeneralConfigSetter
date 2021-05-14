using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Services;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WpfFramework.Core;
using static GeneralConfigSetter.Services.ConfigUpdateService;

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
                value = ValidateInput(value);
                SetField(ref _patConfig, value, nameof(PatConfig));
                OnPropertyChanged(nameof(RowNumbers));
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
        public string RowNumbers => GetRowNumbersString();

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
            PatConfig = DataAccessService.GetConfigFileContent(_patConfigFilePath).Trim();
            _configState = PatConfig;
            _isUpdatePatConfigEnabled = false;

            UpdatePatConfigCommand = new RelayCommand(UpdatePatConfig, IsUpdatePatConfigEnabled);
        }

        private string GetRowNumbersString()
        {
            var stringBuilder = new StringBuilder("");
            var splittedRows = PatConfig.Split("\r\n");
            for (int i = 0; i < splittedRows.Length; i++)
            {
                var currentRow = splittedRows[i];
                var check = false;
                if (i % 3 == 0)
                {
                    try
                    {
                        check = !currentRow.Last().Equals(':');
                    }
                    catch (Exception)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i + 1} should contain a header with \":\" at the end.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    if (check)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i+1} should contain a header with \":\" at the end.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }
                else if (i % 3 == 1)
                {
                    try
                    {
                        check = currentRow.Length != 52;
                    }
                    catch (Exception)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i + 1} should contain a 52 char long PAT.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    if (check)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i+1} should contain a 52 char long PAT.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }
                else if (i % 3 == 2)
                {
                    try
                    {
                        check = !currentRow.Equals(string.Empty);
                    }
                    catch (Exception)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i + 1} should contain only an enter.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    if (check)
                    {
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {i + 1} should contain only an enter.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }

                stringBuilder.Append($"{i + 1}\r\n");
            }

            return stringBuilder.ToString();
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
