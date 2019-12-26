using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentIntegration.Banks.Garanti.BaseEntity
{
    [XmlRoot(ElementName = "Terminal")]
    public class Terminal
    {
        [XmlElement(ElementName = "ProvUserID")]
        public string ProvUserID { get; set; }
        [XmlElement(ElementName = "HashData")]
        public string HashData { get; set; }
        [XmlElement(ElementName = "UserID")]
        public string UserID { get; set; }
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
        [XmlElement(ElementName = "MerchantID")]
        public string MerchantID { get; set; }
    }
}
