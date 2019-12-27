using Fluentx.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegration.Web.Controllers
{
    public class DenizBanksController : Controller
    {
        public ActionResult Sale3D()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale3D(string creditCardNo, string expireMonth, string expireYear, string cvv, string transactionAmount, string orderID)
        {
            string ShopCode = "3123";
            string PurchAmount = transactionAmount;
            string Currency = "949";
            string OrderId = orderID;
            string OkUrl = "http://localhost:2428/DenizBanks/Success";
            string FailUrl = "http://localhost:2428/DenizBanks/Error";
            string Rnd = DateTime.Now.ToString();
            string InstallmentCount = "";
            string TxnType = "Auth";
            string MerchantPass = "gDg1N";
            string SecureType = "3DPay";
            string Lang = "tr";
            string Pan = creditCardNo;
            string Cvv2 = cvv;
            string Expiry = expireMonth + expireYear;
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

            return this.RedirectAndPost("https://sanaltest.denizbank.com/MPI/Default.aspx", postData);            
        }

        //Provizyon Sonucunu Test Ortamında Göndermediği için Succes ve Error Sayfaları Yapılmadı. Gercek Ortamda Deneme Yapılırken Yazılacak.
        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}