using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_Kuveytturk : IPayment
    {
        public string Cancel()
        {
            throw new NotImplementedException();
        }

        public string FinishPayment(object data)
        {
            string MD = "";

            string MerchantOrderId = "siparis123123";
            string Amount = "8650";
            string Type = "Sale";
            string CurrencyCode = "0949";
            string CustomerId = "400235";
            string MerchantId = "496";
            string UserName = "apitest";
            string Password = "api123";
            string HashedPassword = ComputeHash(Password);
            string Data = MerchantId + MerchantOrderId + Amount + UserName + HashedPassword;
            string HashData = ComputeHash(Data);


            var xml = "<KuveytTurkVPosMessage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                  "<APIVersion>1.0.0</APIVersion>" +
                  "<HashData>" + HashData + "</HashData>" +
                  "<MerchantId>" + MerchantId + "</MerchantId>" +
                  "<CustomerId>" + CustomerId + "</CustomerId>" +
                  "<UserName>" + UserName + "</UserName>" +
                  "<TransactionType>" + Type + "</TransactionType>" +
                  "<InstallmentCount>0</InstallmentCount>" +
                  "<Amount>" + Amount + "</Amount>" +
                  "<MerchantOrderId>" + MerchantOrderId + "</MerchantOrderId>" +
                  "<TransactionSecurity>3</TransactionSecurity>" +
                  "<KuveytTurkVPosAdditionalData>" +
                  "<AdditionalData>" +
                  "<Key>MD</Key>" +
                  "<Data>" + MD + "</Data>" +
                  "</AdditionalData>" +
                  "</KuveytTurkVPosAdditionalData>" +
                  "</KuveytTurkVPosMessage>";


            byte[] buffer = Encoding.UTF8.GetBytes(xml);
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("https://boatest.kuveytturk.com.tr/boa.virtualpos.services/Home/ThreeDModelProvisionGate");
            WebReq.Timeout = 5 * 60 * 1000;
            WebReq.Method = "POST";
            WebReq.ContentType = "application/xml";
            WebReq.ContentLength = buffer.Length;
            WebReq.CookieContainer = new CookieContainer();
            Stream ReqStream = WebReq.GetRequestStream();
            ReqStream.Write(buffer, 0, buffer.Length);
            ReqStream.Close();
            WebResponse WebRes = WebReq.GetResponse();
            Stream ResStream = WebRes.GetResponseStream();
            StreamReader ResReader = new StreamReader(ResStream);
            string responseString = ResReader.ReadToEnd();

            return responseString;
        }

        public string OpenProvision(PaymentInfo store)
        {
            string Name = store.CreditCardInfos.CardOwner;
            string CardNumber = store.CreditCardInfos.CardNo;
            string CardExpireDateMonth = store.CreditCardInfos.MonthExpire;
            string CardExpireDateYear = store.CreditCardInfos.YearExpire;
            string CardCVV2 = store.CreditCardInfos.Cvc;
            string Type = "Sale";
            string CurrencyCode = "0949";
            string MerchantOrderId = store.OrderInfos.OrderNo;
            string Amount =  store.OrderInfos.OrderTotalPrice;
            string CustomerId = "400235";
            string MerchantId = "496";
            string OkUrl = "http://localhost:2428/Home/PaymentResult";
            string FailUrl = "http://localhost:2428/Home/PaymentResult";
            string UserName = "apitest";
            string Password = "api123";
            string HashedPassword = ComputeHash(Password);
            string Data = MerchantId + MerchantOrderId + Amount + OkUrl + FailUrl + UserName + HashedPassword;
            string HashData = ComputeHash(Data);
            string TransactionSecurity = "3";

            var xml = "<KuveytTurkVPosMessage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
                "<APIVersion>1.0.0</APIVersion>" +
                "<OkUrl>" + OkUrl + "</OkUrl>" +
                "<FailUrl>" + FailUrl + "</FailUrl>" +
                "<HashData>" + HashData + "</HashData>" +
                "<MerchantId>" + MerchantId + "</MerchantId>" +
                "<CustomerId>" + CustomerId + "</CustomerId>" +
                "<UserName>" + UserName + "</UserName>" +
                "<CardNumber>" + CardNumber + "</CardNumber>" +
                "<CardExpireDateYear>" + CardExpireDateYear + "</CardExpireDateYear>" +
                "<CardExpireDateMonth>" + CardExpireDateMonth + "</CardExpireDateMonth>" +
                "<CardCVV2>" + CardCVV2 + "</CardCVV2>" +
                "<CardHolderName>" + Name + "</CardHolderName>" +
                "<CardType></CardType>" +
                "<BatchID>0</BatchID>" +
                "<TransactionType>" + Type + "</TransactionType>" +
                "<InstallmentCount>0</InstallmentCount>" +
                "<Amount>" + Amount + "</Amount>" +
                "<DisplayAmount>" + Amount + "</DisplayAmount>" +
                "<CurrencyCode>" + CurrencyCode + "</CurrencyCode>" +
                "<MerchantOrderId>" + MerchantOrderId + "</MerchantOrderId>" +
                "<TransactionSecurity>3</TransactionSecurity>" +
                "<TransactionSide>Sale</TransactionSide>" +
                "</KuveytTurkVPosMessage>";


            byte[] buffer = Encoding.UTF8.GetBytes(xml);
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("https://boatest.kuveytturk.com.tr/boa.virtualpos.services/Home/ThreeDModelPayGate");
            WebReq.Timeout = 5 * 60 * 1000;
            WebReq.Method = "POST";
            WebReq.ContentType = "application/xml";
            WebReq.ContentLength = buffer.Length;
            WebReq.CookieContainer = new CookieContainer();
            Stream ReqStream = WebReq.GetRequestStream();
            ReqStream.Write(buffer, 0, buffer.Length);
            ReqStream.Close();
            WebResponse WebRes = WebReq.GetResponse();
            Stream ResStream = WebRes.GetResponseStream();
            StreamReader ResReader = new StreamReader(ResStream);
            string responseString = ResReader.ReadToEnd();

            return responseString;
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }

        public string ComputeHash(string hashstr)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            String hash = Convert.ToBase64String(inputbytes);
            return hash;
        }
    }
}
