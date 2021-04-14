using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using Microsoft.Win32;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class AttachmentConfigViewModel : ViewModelBase
    {
        private string _queryTag = "";
        private string _firstLinkInput = "";
        private string _secondLinkInput = "";
        private IContext _context;

        public string QueryTag
        {
            get { return _queryTag; }
            set { SetField(ref _queryTag, value, nameof(QueryTag)); }
        }
        public string FirstLinkInput
        {
            get { return _firstLinkInput; }
            set { SetField(ref _firstLinkInput, value, nameof(FirstLinkInput)); }
        }
        public string SecondLinkInput
        {
            get { return _secondLinkInput; }
            set { SetField(ref _secondLinkInput, value, nameof(SecondLinkInput)); }
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

            SourceServerName = Context.SourceServerName.Equals("defaultKey") ? "< Invalid link!!! >" : Context.SourceServerName;
            SourceCollectionUrl = Context.SourceCollectionUrl;
            SourceProjectName = Context.SourceProjectName;

            TargetServerName = Context.TargetServerName.Equals("defaultKey") ? "< Invalid link!!! >" : Context.TargetServerName;
            TargetCollectionUrl = Context.TargetCollectionUrl;
            TargetProjectName = Context.TargetProjectName;
        }

        private string _queryText = "";
        private string _sourceServerName = "";
        private string _sourceCollectionUrl = "";
        private string _sourceProjectName = "";
        private string _targetServerName = "";
        private string _targetCollectionUrl = "";
        private string _targetProjectName = "";

        public string QueryText
        {
            get { return _queryText; }
            set { SetField(ref _queryText, value, nameof(QueryText)); }
        }
        public string SourceServerName
        {
            get { return _sourceServerName; }
            set { SetField(ref _sourceServerName, value, nameof(SourceServerName)); }
        }
        public string SourceCollectionUrl
        {
            get { return _sourceCollectionUrl; }
            set { SetField(ref _sourceCollectionUrl, value, nameof(SourceCollectionUrl)); }
        }
        public string SourceProjectName
        {
            get { return _sourceProjectName; }
            set { SetField(ref _sourceProjectName, value, nameof(SourceProjectName)); }
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
        public RelayCommand UpdateAttachmentConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public AttachmentConfigViewModel(IContext context)
        {
            ExtractLinkDataCommand = new RelayCommand(ExtractLinkData, IsExtractLinkDataEnabled);
            UpdateAttachmentConfigCommand = new RelayCommand(UpdateAttachmentConfig, IsUpdateAttachmentConfigEnabled);
            Context = context;
        }

        private void ExtractLinkData()
        {
            Context.QueryText = $"AND [System.Tags] contains '{QueryTag}'";
            Services.LinkService.GetSourceAndTargetData(Context, FirstLinkInput, SecondLinkInput);
            UpdateUiProperties();
        }

        private bool IsExtractLinkDataEnabled()
        {
            if (QueryTag != "" && FirstLinkInput != "" && SecondLinkInput != "")
            {
                return true;
            }
            return false;
        }

        private void UpdateAttachmentConfig()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "XML config files (*.config)|*.config";
            var result = openFileDialog.ShowDialog();

            if (result != null && result == true)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;

                Services.ConfigUpdateService.UpdateGeneralConfig(Context, filePath);
                ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
            }
            else
            {
                ShowMessageCommand.Execute(new NotificationModel("But why? :(", NotificationType.Error));
            }
        }

        private bool IsUpdateAttachmentConfigEnabled()
        {
            if (TargetCollectionUrl != "" && TargetProjectName != "" && TargetServerName != "")
            {
                return true;
            }
            return false;
        }
    }
}
