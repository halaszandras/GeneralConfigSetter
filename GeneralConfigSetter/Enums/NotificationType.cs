using System.Windows.Media;

namespace GeneralConfigSetter.Enums
{
    public class NotificationType : NotificationEnumeration
    {
        public static NotificationType Information = new NotificationType("information", Brushes.Green);
        public static NotificationType Error = new NotificationType("error", Brushes.Yellow);
        public NotificationType(string name, Brush color) : base(name, color)
        {

        }
    }
}
