using System.Text;
using System.Web.Mvc;

namespace MvpRestApiLib
{
    public class EnableXmlAttribute : EnableRestAttribute
    {
        private readonly static string[] XmlTypes = new [] { "application/xml", "text/xml" };

        protected override string[] AcceptedTypes { get { return XmlTypes; } }

        protected override ActionResult Result(ActionExecutedContext filterContext, object model, Encoding contentEncoding)
        {
            return new XmlResult
            {
                Data = model,
                ContentEncoding = contentEncoding,
                ContentType = filterContext.HttpContext.Request.ContentType
            };
        }
    }
}
