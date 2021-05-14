using System.Windows;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for PatConfigView.xaml
    /// </summary>
    public partial class PatConfigView : Page
    {
        private readonly PatConfigViewModel _patConfigViewModel;

        public PatConfigView(PatConfigViewModel patConfigViewModel)
        {
            InitializeComponent();

            _patConfigGrid.DataContext = patConfigViewModel;
            this._patConfigViewModel = patConfigViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _patConfigViewModel.ShowMessageCommand; } internal set{ _patConfigViewModel.ShowMessageCommand = value; } }
    }
}
