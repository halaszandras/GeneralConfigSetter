using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for PatConfigView.xaml
    /// </summary>
    public partial class PatConfigView : Page
    {
        public PatConfigView(PatConfigViewModel patConfigViewModel)
        {
            InitializeComponent();

            _patConfigGrid.DataContext = patConfigViewModel;
        }
    }
}
