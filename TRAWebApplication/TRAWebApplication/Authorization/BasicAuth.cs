using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TRAWebApplication.ConfigSetting;

namespace TRAWebApplication.Authorization
{
    public class BasicAuth:AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to particular API is not authorized");
            }
            else
            {
                string authorizeToken = actionContext.Request.Headers.Authorization.Parameter;
                string decryptString = Encryption.Decrypt(authorizeToken);
                string[] usernmPassword = decryptString.Split(':');
                string username = usernmPassword[0];
                string password = usernmPassword[1];
                password = password.Replace("\n", String.Empty);

                if (AuthCredential.Login(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                }
                else
                {

                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Access to particular API is not authorized");
                }
            }
        }
    }
}