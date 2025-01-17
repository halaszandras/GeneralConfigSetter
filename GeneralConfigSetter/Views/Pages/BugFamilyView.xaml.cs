﻿using System.Linq;
using System.Windows.Controls;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for BugFamilyView.xaml
    /// </summary>
    public partial class BugFamilyView : Page
    {
        private readonly BugFamilyViewModel _bugFamilyViewModel;
        public BugFamilyView(BugFamilyViewModel bugFamilyViewModel)
        {
            InitializeComponent();
            _bugFamilyGrid.DataContext = bugFamilyViewModel;
            _bugFamilyViewModel = bugFamilyViewModel;
        }

        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _bugFamilyViewModel.ShowMessageCommand; } internal set { _bugFamilyViewModel.ShowMessageCommand = value; } }

        private void QueryTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.First().AddedLength > 1)
            {
                QueryTagTextBox.CaretIndex = QueryTagTextBox.Text.Length;
            }
        }
    }
}
