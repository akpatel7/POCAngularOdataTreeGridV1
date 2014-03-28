using System.Web;
using System.Web.Mvc;

namespace EuromoneyArchitecturePoc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Makes all controllers require authorization
            //filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
    }
}