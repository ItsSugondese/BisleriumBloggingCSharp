using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Web.Http.Controllers;

namespace Presentation.Blogging.OnException
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class UnAuthorizedException : AuthorizeAttribute
    {
        
        protected void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }
    }
}
