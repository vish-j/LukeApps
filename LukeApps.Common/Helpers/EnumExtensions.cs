using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LukeApps.Common.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplay<T>(this T value)
            where T : struct
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] displayAttributes)   
                return (displayAttributes.Length > 0) ? displayAttributes[0].Name : value.ToString();
            else
                return string.Empty;
        }


    }
}