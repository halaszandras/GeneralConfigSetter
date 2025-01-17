﻿using System.Linq;
using GeneralConfigSetter.Models;
using GeneralConfigSetter.ViewModels;
using System.Windows.Controls;
using WpfFramework.Core;

namespace GeneralConfigSetter.Views.Pages
{
    /// <summary>
    /// Interaction logic for AttachmentConfigView.xaml
    /// </summary>
    public partial class AttachmentConfigView : Page
    {
        private readonly AttachmentConfigViewModel _attachmentConfigViewModel;

        public AttachmentConfigView(AttachmentConfigViewModel attachmentConfigViewModel)
        {
            InitializeComponent();
            _attachmentConfigGrid.DataContext = attachmentConfigViewModel;
            _attachmentConfigViewModel = attachmentConfigViewModel;
        }
        public RelayCommandGeneric<NotificationModel, bool> ShowMessageCommand { get { return _attachmentConfigViewModel.ShowMessageCommand; } internal set { _attachmentConfigViewModel.ShowMessageCommand = value; } }

        private void QueryTagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.First().AddedLength > 1)
            {
                QueryTagTextBox.CaretIndex = QueryTagTextBox.Text.Length;
            }
        }
    }
}
