using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentIntegration.Banks.Garanti.BaseEntity
{
    [XmlRoot(ElementName = "Order")]
    public class Order
    {
        [XmlElement(ElementName = "OrderID")]
        public string OrderID { get; set; }
        [XmlElement(ElementName = "GroupID")]
        public string GroupID { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
    }
}
