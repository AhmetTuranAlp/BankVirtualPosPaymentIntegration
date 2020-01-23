using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentIntegration.Banks.Garanti.BaseEntity
{
    [XmlRoot(ElementName = "Customer")]
    public class Customer
    {
        [XmlElement(ElementName = "IPAddress")]
        public string IPAddress { get; set; }
        [XmlElement(ElementName = "EmailAddress")]
        public string EmailAddress { get; set; }
    }
}
