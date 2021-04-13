using GeneralConfigSetter.Enums;

namespace GeneralConfigSetter.Models
{
    public class NotificationModel
    {
        public string NotificationText { get; private set; }
        public NotificationType NotificationType { get; private set; }

        public NotificationModel(string notificationText, NotificationType notificationType)
        {
            NotificationText = notificationText;
            NotificationType = notificationType;
        }
    }
}
