// <copyright file="ReflectionHelpers.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    internal static class ReflectionHelpers
    {
        public static void SetInstanceProperty(object instance, string propertyName, object value)
        {
            Guard.AgainstNullArgument("instance", instance);
            var type = instance.GetType();
            var property = type.GetProperty(propertyName);
            SetInstanceProperty(instance, property, value);
        }

        public static void SetInstanceProperty(object instance, PropertyInfo property, object value)
        {
            Guard.AgainstNullArgument("instance", instance);
            Guard.AgainstNullArgument("property", property);
            property.SetValue(instance, value);
        }

        public static object GetInstanceProperty(object instance, string propertyName)
        {
            Guard.AgainstNullArgument("instance", instance);
            var type = instance.GetType();
            var property = type.GetProperty(propertyName);
            return GetInstanceProperty(instance, property);
        }

        public static object GetInstanceProperty(object instance, PropertyInfo property)
        {
            Guard.AgainstNullArgument("instance", instance);
            Guard.AgainstNullArgument("property", property);
            return property.GetValue(instance);
        }

        public static object ConvertToEnumOrDefault(Type enumType, string enumValueName)
        {
            Guard.AgainstNullArgument("enumType", enumType);
            if (!string.IsNullOrWhiteSpace(enumValueName))
            {
                var enumNames = Enum.GetNames(enumType);
                for (int i = 0; i < enumNames.Length; i++)
                {
                    if (enumValueName.Equals(enumNames[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return Enum.GetValues(enumType).GetValue(i);
                    }
                }
            }

            return Activator.CreateInstance(enumType);
        }
    }
}
