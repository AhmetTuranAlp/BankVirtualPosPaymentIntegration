using System;
using System.Collections.Generic;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_DenizBank : IPayment
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
            string ShopCode = "3123";
            string PurchAmount = store.OrderInfos.OrderTotalPrice;
            string Currency = "949";
            string OrderId = store.OrderInfos.OrderNo;
            string OkUrl = "http://localhost:2428/Home/PaymentResult";
            string FailUrl = "http://localhost:2428/Home/PaymentResult";
            string Rnd = DateTime.Now.ToString();
            string InstallmentCount = "";
            string TxnType = "Auth";
            string MerchantPass = "gDg1N";
            string SecureType = "3DPay";
            string Lang = "tr";
            string Pan = store.CreditCardInfos.CardNo;
            string Cvv2 = store.CreditCardInfos.Cvc;
            string Expiry = store.CreditCardInfos.MonthExpire + store.CreditCardInfos.YearExpire;
            string BonusAmount = "";
            string CardType = "0";
            string str = ShopCode + OrderId + PurchAmount + OkUrl + FailUrl + TxnType + InstallmentCount + Rnd + MerchantPass;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(str);
            byte[] hashingbytes = sha.ComputeHash(bytes);

            string Hash = Convert.ToBase64String(hashingbytes);  // merchantpass açık şekilde gönderilmez. Hash değerine eklenerek sunucunun hash i kontrol etmesi sağlanır  

            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("Pan", Pan);
            postData.Add("Cvv2", Cvv2);
            postData.Add("Expiry", Expiry);
            postData.Add("BonusAmount", BonusAmount);
            postData.Add("CardType", CardType);
            postData.Add("ShopCode", ShopCode);
            postData.Add("PurchAmount", PurchAmount);
            postData.Add("Currency", Currency);
            postData.Add("OrderId", OrderId);
            postData.Add("OkUrl", OkUrl);
            postData.Add("FailUrl", FailUrl);
            postData.Add("Rnd", Rnd);
            postData.Add("Hash", Hash);
            postData.Add("TxnType", TxnType);
            postData.Add("InstallmentCount", InstallmentCount);
            postData.Add("SecureType", "3DPay");
            postData.Add("Lang", "tr");

            return "";
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }
    }
}
