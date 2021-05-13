using System.Linq;
using System.Runtime.InteropServices;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using Microsoft.Win32;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class TestItemsViewModel : ViewModelBase
    {
        private string _queryTag = "";
        private bool _isTestCaseProcessorEnabled = true;

        private bool _isTestVariablesProcessorEnabled = true;
        private bool _isTestConfigurationsProcessorEnabled = true;

        private string _testPlanName = "";
        private bool _isTestPlansAndSuitesProcessorEnabled = true;
        private IContext _context;

        public string QueryTag
        {
            get { return _queryTag; }
            set
            {
                value = InsertSplitters(value);
                SetField(ref _queryTag, value, nameof(QueryTag));
                OnPropertyChanged(nameof(QueryTagCounter));
            }
        }
        public int QueryTagCounter
        {
            get { return _queryTag.Count(x => x.Equals(';')); }
        }
        public bool IsTestCaseProcessorEnabled
        {
            get { return _isTestCaseProcessorEnabled; }
            set
            {
                SetField(ref _isTestCaseProcessorEnabled, value, nameof(IsTestCaseProcessorEnabled));
            }
        }

        public bool IsTestVariablesProcessorEnabled
        {
            get { return _isTestVariablesProcessorEnabled; }
            set
            {
                SetField(ref _isTestVariablesProcessorEnabled, value, nameof(IsTestVariablesProcessorEnabled));
            }
        }
        public bool IsTestConfigurationsProcessorEnabled
        {
            get { return _isTestConfigurationsProcessorEnabled; }
            set
            {
                SetField(ref _isTestConfigurationsProcessorEnabled, value, nameof(IsTestConfigurationsProcessorEnabled));
            }
        }

        public string TestPlanName
        {
            get { return _testPlanName; }
            set
            {
                value = InsertSplitters(value);
                SetField(ref _testPlanName, value, nameof(TestPlanName));
                OnPropertyChanged(nameof(TestPlanNameCounter));
            }
        }
        public int TestPlanNameCounter
        {
            get { return _testPlanName.Count(x => x.Equals(';')); }
        }
        public bool IsTestPlansAndSuitesProcessorEnabled
        {
            get { return _isTestPlansAndSuitesProcessorEnabled; }
            set
            {
                SetField(ref _isTestPlansAndSuitesProcessorEnabled, value, nameof(IsTestPlansAndSuitesProcessorEnabled));
            }
        }

        public IContext Context
        {
            get { return _context; }
            set
            {
                SetField(ref _context, value, nameof(Context));
            }
        }

        private void UpdateUiProperties()
        {
            QueryText = IsTestCaseProcessorEnabled ? Context.QueryText : "Disabled";
            TestVariablesText = IsTestVariablesProcessorEnabled ? "Enabled" : "Disabled";
            TestConfigurationsText = IsTestConfigurationsProcessorEnabled ? "Enabled" : "Disabled";
            TestPlanNamesText = IsTestPlansAndSuitesProcessorEnabled ? Context.TestPlanNamesText : "Disabled";
        }

        private string _queryText = "";
        private string _testVariablesText = "";
        private string _testConfigurationsText = "";
        private string _testPlanNamesText = "";

        public string QueryText
        {
            get { return _queryText; }
            set { SetField(ref _queryText, value, nameof(QueryText)); }
        }
        public string TestVariablesText
        {
            get { return _testVariablesText; }
            set { SetField(ref _testVariablesText, value, nameof(TestVariablesText)); }
        }
        public string TestConfigurationsText
        {
            get { return _testConfigurationsText; }
            set { SetField(ref _testConfigurationsText, value, nameof(TestConfigurationsText)); }
        }
        public string TestPlanNamesText
        {
            get { return _testPlanNamesText; }
            set { SetField(ref _testPlanNamesText, value, nameof(TestPlanNamesText)); }
        }


        public RelayCommand ExtractDataCommand { get; set; }
        public RelayCommand UpdateTestItemsConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public TestItemsViewModel(IContext context)
        {
            ExtractDataCommand = new RelayCommand(ExtractData, IsExtractDataEnabled);
            UpdateTestItemsConfigCommand = new RelayCommand(UpdateTestItemsConfig, IsUpdateTestItemsConfigEnabled);
            Context = context;
        }

        private string InsertSplitters(string tagStrings)
        {
            tagStrings = tagStrings.Replace(" ", "");
            tagStrings = CheckSplitters(tagStrings);

            if (tagStrings.Length == 1 && tagStrings[tagStrings.Length - 1] == ';')
            {
                tagStrings = "";
            }

            if (tagStrings.Length >= 1)
            {
                if (tagStrings[tagStrings.Length - 1] != ';')
                {
                    tagStrings += ';';
                }
            }

            return tagStrings;
        }

        private string CheckSplitters(string tagStrings)
        {
            while (tagStrings.Contains(";;"))
            {
                tagStrings = tagStrings.Replace(";;", ";");
            }
            return tagStrings;
        }

        private void ExtractData()
        {
            Context.QueryText = Services.ConfigUpdateService.CreateTestItemsQueryBit(QueryTag);
            Context.TestPlanNamesText = Services.ConfigUpdateService.CreateTestPlansQueryBit(TestPlanName);
            UpdateUiProperties();
        }

        private bool IsExtractDataEnabled()
        {
            if (QueryTag != "" || TestPlanName != "")
            {
                return true;
            }
            return false;
        }

        private void UpdateTestItemsConfig()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JSON files (*.json)|*.json"; // "XML config files (*.config)|*.config";
            var result = openFileDialog.ShowDialog();

            if (result != null && result == true)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;

                Services.ConfigUpdateService.UpdateTestItemsConfig(Context,
                                                                   IsTestCaseProcessorEnabled,
                                                                   IsTestVariablesProcessorEnabled,
                                                                   IsTestConfigurationsProcessorEnabled,
                                                                   IsTestPlansAndSuitesProcessorEnabled,
                                                                   filePath);
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            else
            {
                ShowMessageCommand.Execute(new NotificationModel("But why? :(", NotificationType.Error));
            }
        }

        private bool IsUpdateTestItemsConfigEnabled()
        {
            if (QueryText != "" || TestPlanNamesText != "")
            {
                return true;
            }
            return false;
        }
    }
}
