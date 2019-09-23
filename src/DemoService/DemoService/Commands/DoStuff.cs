using System;
using System.Xml.Serialization;

namespace DemoService.Commands
{
    [XmlRoot(Namespace = "http://tempuri.net/DemoService.Commands")]
    public class DoStuff
    {
        public string Message { get; set; }
    }
}
