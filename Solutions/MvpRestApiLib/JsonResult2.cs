using System;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Runtime.Serialization.Json;

namespace MvpRestApiLib
{
    internal class JsonResult2 : ActionResult
    {
        public JsonResult2() { }
        public JsonResult2(object data) { Data = data; }

        public string ContentType { get; set; }
        public Encoding ContentEncoding { get; set; }
        public object Data { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(ContentType))
                response.ContentType = ContentType;
            else
                response.ContentType = "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            var serializer = new DataContractJsonSerializer(Data.GetType());
            serializer.WriteObject(response.OutputStream, Data);
        }
    }
}
