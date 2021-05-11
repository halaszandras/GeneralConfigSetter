using System.Linq;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using Microsoft.Win32;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class BugFamilyViewModel : ViewModelBase
    {
        private string _queryTag = "";
        private int _queryTagCounter;
        private string _linkInput = "";
        private IContext _context;

        public string QueryTag
        {
            get { return _queryTag; }
            set
            {
                value = InsertSplitters(value);
                SetField(ref _queryTag, value, nameof(QueryTag));
                QueryTagCounter = _queryTag.Count(x => x.Equals(';'));
            }
        }
        public int QueryTagCounter
        {
            get { return _queryTagCounter; }
            set
            {
                SetField(ref _queryTagCounter, value, nameof(QueryTagCounter));
            }
        }
        public string LinkInput
        {
            get { return _linkInput; }
            set { SetField(ref _linkInput, value, nameof(LinkInput)); }
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
            QueryText = Context.QueryText;

            TargetServerName = Context.TargetServerName.Equals("defaultKey") ? "111LinkIsNull111" : Context.TargetServerName;
            TargetCollectionUrl = Context.TargetCollectionUrl;
            TargetProjectName = Context.TargetProjectName;
        }

        private string _queryText = "";
        private string _targetServerName = "";
        private string _targetCollectionUrl = "";
        private string _targetProjectName = "";

        public string QueryText
        {
            get { return _queryText; }
            set { SetField(ref _queryText, value, nameof(QueryText)); }
        }
        public string TargetServerName
        {
            get { return _targetServerName; }
            set { SetField(ref _targetServerName, value, nameof(TargetServerName)); }
        }
        public string TargetCollectionUrl
        {
            get { return _targetCollectionUrl; }
            set { SetField(ref _targetCollectionUrl, value, nameof(TargetCollectionUrl)); }
        }
        public string TargetProjectName
        {
            get { return _targetProjectName; }
            set { SetField(ref _targetProjectName, value, nameof(TargetProjectName)); }
        }


        public RelayCommand ExtractLinkDataCommand { get; set; }
        public RelayCommand UpdateBugFamilyConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public BugFamilyViewModel(IContext context)
        {
            ExtractLinkDataCommand = new RelayCommand(ExtractLinkData, IsExtractLinkDataEnabled);
            UpdateBugFamilyConfigCommand = new RelayCommand(UpdateBugFamilyConfig, IsUpdateBugFamilyConfigEnabled);
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

        private void ExtractLinkData()
        {
            Context.QueryText = QueryTag.Trim(';');
            Services.LinkService.GetTargetData(Context, LinkInput);
            UpdateUiProperties();
        }

        private bool IsExtractLinkDataEnabled()
        {
            if (QueryTag != "" && LinkInput != "" && LinkInput != "")
            {
                return true;
            }
            return false;
        }

        private void UpdateBugFamilyConfig()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "XML config files (*.config)|*.config";
            var result = openFileDialog.ShowDialog();

            if (result != null && result == true)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;

                Services.ConfigUpdateService.UpdateBugFamilyConfig(Context, filePath);
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            else
            {
                ShowMessageCommand.Execute(new NotificationModel("But why? :(", NotificationType.Error));
            }
        }

        private bool IsUpdateBugFamilyConfigEnabled()
        {
            if (TargetCollectionUrl != "" && TargetProjectName != "" && TargetServerName != "")
            {
                return true;
            }
            return false;
        }
    }
}
