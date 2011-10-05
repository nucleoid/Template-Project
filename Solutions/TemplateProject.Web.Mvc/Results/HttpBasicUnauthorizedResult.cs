using System;
using System.Web.Mvc;
using TemplateProject.Web.Mvc.Extensions;

namespace TemplateProject.Web.Mvc.Results
{
    /// <summary>
    /// Instead of returning a 401, we do a 403 and switch it back to 401 in Global.asax.cs Application_EndRequest method 
    /// to avoid the forms authentication login redirect for Restful requests.
    /// </summary>
    public class HttpBasicUnauthorizedResult : HttpStatusCodeResult
    {
        private const int UnauthorizedCode = 403;
 
        public HttpBasicUnauthorizedResult()
            : this(null) {
        }

        public HttpBasicUnauthorizedResult(string statusDescription)
            : base(UnauthorizedCode, statusDescription) { 
        } 

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) 
                throw new ArgumentNullException("context");

            if (context.HttpContext.Request.IsRestful())
                context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic");
            base.ExecuteResult(context);
        }
    }
}