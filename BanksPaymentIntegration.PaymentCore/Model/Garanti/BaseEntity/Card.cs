using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentIntegration.Banks.Garanti.BaseEntity
{
    [XmlRoot(ElementName = "Card")]
    public class Card
    {
        [XmlElement(ElementName = "Number")]
        public string Number { get; set; }
        [XmlElement(ElementName = "ExpireDate")]
        public string ExpireDate { get; set; }
        [XmlElement(ElementName = "CVV2")]
        public string CVV2 { get; set; }
    }
}
