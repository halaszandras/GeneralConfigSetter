using System.Windows.Controls;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : Page
    {
        private readonly InfoViewModel _infoViewModel;
        public InfoView(InfoViewModel infoViewModel)
        {
            InitializeComponent();
            _infoGrid.DataContext = infoViewModel;
            _infoViewModel = infoViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _infoViewModel.ShowMessageCommand; } internal set { _infoViewModel.ShowMessageCommand = value; } }
    }
}
