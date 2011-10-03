using System.IO;
using System.Text;
using System.Web;

namespace MvcRestApiLib.Tests
{
    public  class FakeResponse : HttpResponseBase
    {
        private readonly Stream _stream = new MemoryStream();

        public override string ContentType { get; set; }
        public override Encoding ContentEncoding { get; set; }
        public override Stream OutputStream
        {
            get
            {
                return _stream;
            }
        }

        public string TestString { get; set; }

        public override void Write(string s)
        {
            TestString = s;
        }
    }
}
