using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using WpfFramework.Core;
using static GeneralConfigSetter.Services.ConfigUpdateService;

namespace GeneralConfigSetter.ViewModels
{
    public class PatConfigViewModel : ViewModelBase
    {
        private string _patConfigFilePath = "";
        private bool _isPatConfigUpdateEnabled = false;
        private string _rowNumbers;
        private string _patConfig;
        private IContext _context;

        public string PatConfig
        {
            get { return _patConfig; }
            set
            {
                value = ValidateInput(value);
                SetField(ref _patConfig, value, nameof(PatConfig));
                OnPropertyChanged(nameof(IsErrorCheckEnabled));
                UpdateRowNumbers();
            }
        }
        public string ConfigState
        {
            get { return DataAccessService.GetConfigFileContent(_patConfigFilePath).Trim(); }
        }
        public string RowNumbers
        {
            get { return _rowNumbers; }
            set
            {
                SetField(ref _rowNumbers, value, nameof(RowNumbers));
            }
        }
        public bool IsPatConfigUpdateEnabled
        {
            get { return _isPatConfigUpdateEnabled; }
            set { SetField(ref _isPatConfigUpdateEnabled, value, nameof(IsPatConfigUpdateEnabled)); }
        }
        public bool IsErrorCheckEnabled => !PatConfig.Equals(ConfigState);       

        public IContext Context
        {
            get { return _context; }
            set { SetField(ref _context, value, nameof(Context)); }
        }

        public RelayCommand ErrorCheckCommand { get; set; }
        public RelayCommand UpdatePatConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public PatConfigViewModel(IContext context)
        {
            Context = context;
            _patConfigFilePath = DataAccessService.GetPatConfigFilePath();
            PatConfig = ConfigState;
            IsPatConfigUpdateEnabled = false;

            ErrorCheckCommand = new RelayCommand(ErrorCheck, IsErrorCheckCommandEnabled);
            UpdatePatConfigCommand = new RelayCommand(UpdatePatConfig, IsUpdatePatConfigEnabled);
        }

        private void UpdateRowNumbers()
        {
            var splittedRows = PatConfig.Split("\r\n");
            var stringBuilder = new StringBuilder("");
            for (int index = 0; index < splittedRows.Length; index++)
            {
                stringBuilder.Append($"{index + 1}\r\n");
            }
            RowNumbers = stringBuilder.ToString();
        }

        private void ErrorCheck()
        {
            var splittedRows = PatConfig.Split("\r\n");
            RowNumbers = GetRowNumbersWithErrorCheck(splittedRows);
        }

        private bool IsErrorCheckCommandEnabled()
        {
            return IsErrorCheckEnabled;
        }

        private string GetRowNumbersWithErrorCheck(string[] splittedRows)
        {
            var stringBuilder = new StringBuilder("");
            for (int index = 0; index < splittedRows.Length; index++)
            {
                var currentRow = splittedRows[index];
                var check = false;
                if (index % 3 == 0)
                {
                    try
                    {
                        check = !currentRow.Last().Equals(':');
                    }
                    catch (Exception e) { Debug.WriteLine(e); }

                    if (check)
                    {
                        IsPatConfigUpdateEnabled = false;
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {index + 1} should contain a header with \":\" at the end.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    else if (index == splittedRows.Length - 1)
                    {
                        IsPatConfigUpdateEnabled = false;
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Pat config can only end with a key or a key and one empty row.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    IsPatConfigUpdateEnabled = true;
                }
                else if (index % 3 == 1)
                {
                    Regex characterWhiteList = new(@"^[a-zA-Z0-9]*$");
                    var regexInput = currentRow.Trim('\n').Trim('\r');
                    try
                    {
                        check = currentRow.Length != 52;
                    }
                    catch (Exception e) { Debug.WriteLine(e); }
                    if (check)
                    {
                        IsPatConfigUpdateEnabled = false;
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {index + 1} should contain a 52 char long PAT.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    else if (!characterWhiteList.IsMatch(regexInput))
                    {
                        IsPatConfigUpdateEnabled = false;
                        stringBuilder.Append("ERR");

                        Regex mismatchfinder = new(@"\W|_");
                        var mismatching = "    ";
                        foreach (Match item in mismatchfinder.Matches(regexInput))
                        {
                            mismatching += item.ToString() + " ";
                        }
                        MessageBox.Show($"The PAT at Row {index + 1} should contain only alphanumerical values.{Environment.NewLine}Missmatching characters are:{Environment.NewLine}{mismatching}", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    IsPatConfigUpdateEnabled = true;
                }
                else if (index % 3 == 2)
                {
                    try
                    {
                        check = !currentRow.Equals(string.Empty);
                    }
                    catch (Exception e) { Debug.WriteLine(e); }
                    if (check)
                    {
                        IsPatConfigUpdateEnabled = false;
                        stringBuilder.Append("ERR");
                        MessageBox.Show($"Row {index + 1} should contain only an enter.", "Pat Text", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                    IsPatConfigUpdateEnabled = true;
                }

                stringBuilder.Append($"{index + 1}\r\n");
            }

            return stringBuilder.ToString();
        }

        private void UpdatePatConfig()
        {
            try
            {
                DataAccessService.UpdateConfigFile(_patConfigFilePath, PatConfig);
                Context.InitializePats();
                IsPatConfigUpdateEnabled = false;
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
            return IsPatConfigUpdateEnabled;
        }
    }
}
