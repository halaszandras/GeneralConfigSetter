using System.Reflection;
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

        public InfoViewModel()
        {
            InitializeAssemblyInfo();
        }

        private void InitializeAssemblyInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();

            //AssemblyTitleAttribute titleAttr =
            //    GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
            //if (titleAttr != null) Title = titleAttr.Title;

            //AssemblyVersionAttribute versionAttr =
            //    GetAssemblyAttribute<AssemblyVersionAttribute>(assembly);
            //if (versionAttr != null) Version = versionAttr.Version;

            Title = assembly.GetName().Name;
            Version = assembly.GetName().Version.ToString();
        }
    }
}
