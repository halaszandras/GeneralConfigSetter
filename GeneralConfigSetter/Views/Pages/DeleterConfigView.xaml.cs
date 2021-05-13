using System.Linq;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for DeleterConfigView.xaml
    /// </summary>
    public partial class DeleterConfigView : Page
    {
        private readonly DeleterConfigViewModel _deleterConfigViewModel;

        public DeleterConfigView(DeleterConfigViewModel deleterConfigViewModel)
        {
            InitializeComponent();
            _deleterConfigUpdateGrid.DataContext = deleterConfigViewModel;
            _deleterConfigViewModel = deleterConfigViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _deleterConfigViewModel.ShowMessageCommand; } internal set { _deleterConfigViewModel.ShowMessageCommand = value; } }

        private void QueryTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.First().AddedLength > 1)
            {
                QueryTagTextBox.CaretIndex = QueryTagTextBox.Text.Length;
            }
        }
    }
}
