using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MvpRestApiLib
{
    // Source: http://www.hackersbasement.com/csharp/post/2009/06/07/XmlResult-for-ASPNet-MVC.aspx
    internal class XmlResult : ActionResult
    {
        public XmlResult() { }
        public XmlResult(object data) { Data = data; }

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
                response.ContentType = "text/xml";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                if (Data is XmlNode)
                    response.Write(((XmlNode)Data).OuterXml);
                else if (Data is XNode)
                    response.Write((Data).ToString());
                else
                {
                    var dataType = Data.GetType();
                    // OMAR: For generic types, use DataContractSerializer because 
                    // XMLSerializer cannot serialize generic interface lists or types.
                    if (dataType.IsGenericType || 
                        dataType.GetCustomAttributes(typeof(DataContractAttribute), true).FirstOrDefault() != null)
                    {
                        var dSer = new DataContractSerializer(dataType);
                        dSer.WriteObject(response.OutputStream, Data);
                    }
                    else
                    {
                        var xSer = new XmlSerializer(dataType);
                        xSer.Serialize(response.OutputStream, Data);
                    }
                }
            }
        }
    }
}
