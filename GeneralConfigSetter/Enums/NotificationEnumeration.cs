using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GeneralConfigSetter.Enums
{
    public abstract class NotificationEnumeration : IEquatable<NotificationEnumeration>
    {
        public string Name { get; private set; }
        public Brush Color { get; private set; }

        protected NotificationEnumeration(string name, Brush color)
        {
            Name = name;
            Color = color;
        }

        public bool Equals(NotificationEnumeration notificationEnumeration)
        {
            return notificationEnumeration is NotificationEnumeration enumeration &&
                   Name == enumeration.Name &&
                   EqualityComparer<Brush>.Default.Equals(Color, enumeration.Color);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Color);
        }
    }
}
