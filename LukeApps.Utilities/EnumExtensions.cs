using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LukeApps.Utilities
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

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.GetDisplay() };
            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
}