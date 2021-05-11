using System;
using System.Linq;
using System.Reflection;

namespace GeneralConfigSetter.Services
{
    public static class InfoService
    {

        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            if ((attributes == null) || (attributes.Length == 0))
                return null;

            return (T)attributes[0];
        }
    }
}
