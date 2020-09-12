using LukeApps.BugsTracker;
using System.Web;
using System.Web.Mvc;

namespace LukePurchaseSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LukeExceptionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
