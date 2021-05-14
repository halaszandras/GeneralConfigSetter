using System.Windows.Controls;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for FieldMapperView.xaml
    /// </summary>
    public partial class FieldMapperView : Page
    {
        private readonly FiledMapperViewModel _filedMapperViewModel;

        public FieldMapperView(FiledMapperViewModel filedMapperViewModel)
        {
            InitializeComponent();
            _fieldMapperGrid.DataContext = filedMapperViewModel;
            _filedMapperViewModel = filedMapperViewModel;
        }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _filedMapperViewModel.ShowMessageCommand; } internal set { _filedMapperViewModel.ShowMessageCommand = value; } }
    }
}
