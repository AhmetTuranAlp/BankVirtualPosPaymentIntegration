using System;
using System.Collections.Generic;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore
{
    public interface IPayment
    {
        string OpenProvision(PaymentInfo store);

        string FinishPayment(object data);

        string Cancel();

        string Refund();
    }
}
