using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentIntegration.Banks.NestPay
{
    public class CreditCardModel
    {
        public string ClientId { get; set; }
        public string Storekey { get; set; }
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string CV2 { get; set; }
    }
}
