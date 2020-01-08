using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentIntegration.Banks.Iyzico
{
    public class PaymentResultVM
    {
        public string PaymentId { get; set; }
        public string ReturnStatus { get; set; }
    }
}
