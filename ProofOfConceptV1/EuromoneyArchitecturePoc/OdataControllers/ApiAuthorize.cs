using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EuromoneyArchitecturePoc.OdataControllers
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}