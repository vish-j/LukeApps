using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LukeApps.Common.Helpers
{
    public static class OtherExtensions
    {
        public static string GetDisplay(this PropertyInfo property)
        {
            if (property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute dd)
                return dd.Name;
            else
                return property.Name;
        }
    }
}