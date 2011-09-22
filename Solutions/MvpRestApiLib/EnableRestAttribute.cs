
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvpRestApiLib
{
    public abstract class EnableRestAttribute : ActionFilterAttribute
    {
        protected abstract string[] AcceptedTypes { get; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (typeof(RedirectToRouteResult).IsInstanceOfType(filterContext.Result))
                return;

            var acceptTypes = filterContext.HttpContext.Request.AcceptTypes ?? new[] { "text/html" };

            var contentEncoding = filterContext.HttpContext.Request.ContentEncoding ?? Encoding.UTF8;

            if (AcceptedTypes.Any(acceptTypes.Contains))
            {
                var model = filterContext.Controller.ViewData.Model as IRestModel;

                if (model == null)
                    throw new ArgumentException("REST Model must implement IRestModel");

                filterContext.Result = Result(filterContext, model.RestModel, contentEncoding);
            }
        }

        protected abstract ActionResult Result(ActionExecutedContext filterContext, object model, Encoding contentEncoding);
    }
}
