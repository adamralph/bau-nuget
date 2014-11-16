// <copyright file="ReflectionHelpers.cs" company="Bau contributors">
//  Copyright (c) Bau contributors. (baubuildch@gmail.com)
// </copyright>

namespace BauNuGet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
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

        public static void ComposeExportedValue(this CompositionContainer compositionContainer, object value, Type forceType = null)
        {
            var valueType = forceType ?? value.GetType();
            var method = typeof(System.ComponentModel.Composition.AttributedModelServices)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == "ComposeExportedValue" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new[] { compositionContainer, value });
        }

        public static object GetExportedValue(this ExportProvider compositionContainer, Type type)
        {
            var method = typeof(System.ComponentModel.Composition.Hosting.ExportProvider)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name == "GetExportedValue" && m.GetParameters().Length == 0);
            var genericMethod = method.MakeGenericMethod(new[] { type });
            return genericMethod.Invoke(compositionContainer, new object[0]);
        }

        public static IEnumerable<object> GetExportedValues(this ExportProvider compositionContainer, Type type)
        {
            var method = typeof(System.ComponentModel.Composition.Hosting.ExportProvider)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name == "GetExportedValues" && m.GetParameters().Length == 0);
            var genericMethod = method.MakeGenericMethod(new[] { type });
            return ((System.Collections.IEnumerable)genericMethod.Invoke(compositionContainer, new object[0])).Cast<object>();
        }
    }
}
