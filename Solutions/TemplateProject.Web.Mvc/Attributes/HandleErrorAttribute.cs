
using System.Web.Mvc;
using NLog;

namespace TemplateProject.Web.Mvc.Attributes
{
    public class HandleErrorsAttribute : HandleErrorAttribute
    {
        public HandleErrorsAttribute()
        {
            View = "~/Views/Error/Excepted.cshtml";
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            logger.LogException(LogLevel.Error, "Error occured in a controller", filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}