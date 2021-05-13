using System.Reflection;
using System.Windows;
using GeneralConfigSetter.Enums;
using GeneralConfigSetter.Models;
using WpfFramework.Core;
using static GeneralConfigSetter.Services.InfoService;

namespace GeneralConfigSetter.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        private string _title = "";
        public string Title
        {
            get { return _title; }
            set
            {
                SetField(ref _title, value, nameof(Title));
            }
        }
        private string _version = "";
        public string Version
        {
            get { return _version; }
            set
            {
                SetField(ref _version, value, nameof(Version));
            }
        }
        private string _copyRight = "";
        public string CopyRight
        {
            get { return _copyRight; }
            set
            {
                SetField(ref _copyRight, value, nameof(CopyRight));
            }
        }
        private string _companyName = "";
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                SetField(ref _companyName, value, nameof(CompanyName));
            }
        }
        private string _location = "";
        public string Location
        {
            get { return _location; }
            set
            {
                SetField(ref _location, value, nameof(Location));
            }
        }

        private string _authors = "Kereszturi András, Halász András Péter";
        public string Authors
        {
            get { return _authors; }
            set
            {
                SetField(ref _authors, value, nameof(Authors));
            }
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get; internal set; }
        public RelayCommand CopyLocationToClipBoardCommand { get; set; }

        public InfoViewModel()
        {
            InitializeAssemblyInfo();
            CopyLocationToClipBoardCommand = new RelayCommand(CopyLocationToClipBoard, IsCopyLocationToClipBoardEnabled);
        }

        private void InitializeAssemblyInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();

            Title = assembly.GetName().Name;
            Version = assembly.GetName().Version.ToString();

            AssemblyCopyrightAttribute copyRightAttribute =
                GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);
            if (copyRightAttribute != null) CopyRight = copyRightAttribute.Copyright;

            AssemblyCompanyAttribute companyNameAttribute =
                GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
            if (companyNameAttribute != null) CompanyName = companyNameAttribute.Company;

            Location = assembly.Location;
        }

        private void CopyLocationToClipBoard()
        {
            
            Clipboard.SetText(Location);
            ShowMessageCommand.Execute(new NotificationModel("SUCCESS!!!!", NotificationType.Information));
        }

        private bool IsCopyLocationToClipBoardEnabled()
        {
            return !Clipboard.GetText().Equals(Location);
        }
    }
}
