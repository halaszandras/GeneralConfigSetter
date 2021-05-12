using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for TestItemsView.xaml
    /// </summary>
    public partial class TestItemsView : Page
    {
        private readonly TestItemsViewModel _testItemsViewModel;

        public TestItemsView(TestItemsViewModel testItemsViewModel)
        {
            InitializeComponent();
            _testItemsGrid.DataContext = testItemsViewModel;
            _testItemsViewModel = testItemsViewModel;
        }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _testItemsViewModel.ShowMessageCommand; } internal set { _testItemsViewModel.ShowMessageCommand = value; } }

        private void QueryTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.First().AddedLength > 1)
            {
                QueryTagTextBox.CaretIndex = QueryTagTextBox.Text.Length;
            }
        }
    }
}
