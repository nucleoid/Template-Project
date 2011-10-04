using System;
using System.Linq;
using System.Web.Mvc;
using MvpRestApiLib;
using TemplateProject.Web.Mvc.Extensions;

namespace TemplateProject.Web.Mvc.Results
{
    public class HttpBasicUnauthorizedResult : HttpUnauthorizedResult
    {
        public HttpBasicUnauthorizedResult()
        {
        }

        public HttpBasicUnauthorizedResult(string statusDescription) : base(statusDescription)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) 
                throw new ArgumentNullException("context");

            if (context.HttpContext.Request.IsRestful())
                context.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic");
            else
                base.ExecuteResult(context);
        }
    }
}