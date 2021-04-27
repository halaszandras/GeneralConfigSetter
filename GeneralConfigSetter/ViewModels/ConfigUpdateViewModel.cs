using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using Microsoft.Win32;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class ConfigUpdateViewModel : ViewModelBase
    {
        private string _projectName = "";
        private string _firstLinkInput = "";
        private string _secondLinkInput = "";
        private IContext _context;

        public string ProjectName
        {
            get { return _projectName; }
            set { SetField(ref _projectName, value, nameof(ProjectName)); }
        }
        public string FirstLinkInput
        {
            get { return _firstLinkInput; }
            set { SetField(ref _firstLinkInput, value, nameof(FirstLinkInput));}
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
            WorkItemProjectName = Context.WorkItemProjectName;

            SourceServerName = Context.SourceServerName.Equals("defaultKey") ? "111LinkIsNull111" : Context.SourceServerName;
            SourceCollectionUrl = Context.SourceCollectionUrl;
            SourceProjectName = Context.SourceProjectName;

            TargetServerName = Context.TargetServerName.Equals("defaultKey") ? "111LinkIsNull111" : Context.TargetServerName;
            TargetCollectionUrl = Context.TargetCollectionUrl;
            TargetProjectName = Context.TargetProjectName;
        }

        private string _workItemProjectName = "";
        private string _sourceServerName = "";
        private string _sourceCollectionUrl = "";
        private string _sourceProjectName = "";
        private string _targetServerName = "";
        private string _targetCollectionUrl = "";
        private string _targetProjectName = "";

        public string WorkItemProjectName
        {
            get { return _workItemProjectName; }
            set { SetField(ref _workItemProjectName, value, nameof(WorkItemProjectName)); }
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
        public RelayCommand UpdateGeneralConfigCommand { get; set; }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }

        public ConfigUpdateViewModel(IContext context)
        {
            ExtractLinkDataCommand = new RelayCommand(ExtractLinkData, IsExtractLinkDataEnabled);
            UpdateGeneralConfigCommand = new RelayCommand(UpdateGeneralConfig, IsUpdateGeneralConfigEnabled);
            Context = context;
        }

        private void ExtractLinkData()
        {
            Context.WorkItemProjectName = ProjectName;
            Services.LinkService.GetSourceAndTargetData(Context, FirstLinkInput, SecondLinkInput);
            UpdateUiProperties();
        }

        private bool IsExtractLinkDataEnabled()
        {
            if (ProjectName != "" && FirstLinkInput != "" && SecondLinkInput != "")
            {
                return true;
            }
            return false;
        }

        private void UpdateGeneralConfig()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JSON files (*.json)|*.json";
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

        private bool IsUpdateGeneralConfigEnabled()
        {
            if (TargetCollectionUrl != "" && TargetProjectName != "" && TargetServerName != "")
            {
                return true;
            }
            return false;
        }
    }
}
