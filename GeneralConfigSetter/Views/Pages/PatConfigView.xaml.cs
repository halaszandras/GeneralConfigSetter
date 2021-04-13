using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for PatConfigView.xaml
    /// </summary>
    public partial class PatConfigView : Page
    {
        private readonly PatConfigViewModel patConfigViewModel;

        public PatConfigView(PatConfigViewModel patConfigViewModel)
        {
            InitializeComponent();

            _patConfigGrid.DataContext = patConfigViewModel;
            this.patConfigViewModel = patConfigViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return patConfigViewModel.ShowMessageCommand; } internal set{ patConfigViewModel.ShowMessageCommand = value; } }
    }
}
