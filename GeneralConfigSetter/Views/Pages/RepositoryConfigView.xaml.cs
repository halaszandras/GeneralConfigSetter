using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for RepositoryConfigView.xaml
    /// </summary>
    public partial class RepositoryConfigView : Page
    {
        private readonly RepositoryConfigViewModel repositoryConfigViewModel;

        public RepositoryConfigView(RepositoryConfigViewModel repositoryConfigViewModel)
        {
            InitializeComponent();
            _repositoryConfigGrid.DataContext = repositoryConfigViewModel;
            this.repositoryConfigViewModel = repositoryConfigViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return repositoryConfigViewModel.ShowMessageCommand; } internal set { repositoryConfigViewModel.ShowMessageCommand = value; } }
    }
}
