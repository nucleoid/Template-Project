using System.Text;
using System.Web.Mvc;

namespace MvpRestApiLib
{
    public class EnableJsonAttribute : EnableRestAttribute
    {
        private readonly static string[] JsonTypes = new [] { "application/json", "text/json" };

        public override string[] AcceptedTypes
        {
            get { return JsonTypes; }
        }

        protected override ActionResult Result(ActionExecutedContext filterContext, object model, Encoding contentEncoding)
        {
            return new JsonResult2
            {
                Data = model,
                ContentEncoding = contentEncoding,
                ContentType = filterContext.HttpContext.Request.ContentType
            }; 
        }
    }
}
