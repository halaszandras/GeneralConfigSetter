using GeneralConfigSetter.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfFramework.Core;

namespace GeneralConfigSetter.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {       
        private string _lastMessage = "";
        private Brush _messageColor = Brushes.Transparent;
        private double _messageOpacity = 0.0;
        public string LastMessage
        {
            get { return _lastMessage; }
            set { SetField(ref _lastMessage, value, nameof(LastMessage)); }
        }
        public Brush MessageColor
        {
            get { return _messageColor; }
            set { SetField(ref _messageColor, value, nameof(MessageColor)); }
        }
        public double MessageOpacity
        {
            get { return _messageOpacity; }
            set { SetField(ref _messageOpacity, value, nameof(MessageOpacity)); }
        }

        public async void ShowMessage(NotificationModel notificationModel)
        {
            MessageColor = notificationModel.NotificationType.Color;
            LastMessage = notificationModel.NotificationText;
            MessageOpacity = 1.0;
            await Task.Run(() => VanishMessage()).ConfigureAwait(false);
        }

        private void VanishMessage()
        {
            Thread.Sleep(4000);
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                MessageOpacity -= 0.01;
            }
        }

        internal bool IsShowMessageEnabled(bool IsEnabled)
        {
            return IsEnabled;
        }
    }
}
