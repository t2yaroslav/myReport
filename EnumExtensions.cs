using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MyReport
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            if (enu == null)
                return null;

            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : Enum.GetName(enu.GetType(), enu);
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
