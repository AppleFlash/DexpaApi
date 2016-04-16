using System.Web.Mvc;
using Dexpa.WebAPI.Filters;

namespace Dexpa.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
