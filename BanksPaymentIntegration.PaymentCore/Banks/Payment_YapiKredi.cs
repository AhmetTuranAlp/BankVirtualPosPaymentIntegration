using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_YapiKredi : IPayment
    {
        public string Cancel()
        {
            throw new NotImplementedException();
        }

        public string FinishPayment(object data)
        {
            throw new NotImplementedException();
        }

        public string OpenProvision(PaymentInfo store)
        {
            throw new NotImplementedException();
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }

        private string HASH(string originalString)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(originalString));
                return Convert.ToBase64String(bytes);
            }
        }
     
    }
}
