using System.Runtime.Serialization;

namespace MvcRestApiLib.Tests
{
    [DataContract]
    public class TestSerial
    {
        [DataMember]
        public int Number { get; set; }
    }
}
