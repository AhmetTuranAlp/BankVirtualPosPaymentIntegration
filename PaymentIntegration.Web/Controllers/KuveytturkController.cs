using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace PaymentIntegration.Web.Controllers
{
    public class KuveytturkController : Controller
    {
        // GET: Kuveytturk
        public ActionResult Index()
        {
            return RedirectToAction("Payment");
        }

        public ActionResult Payment()
        {
            string Name = "ahmet";
            string CardNumber = "4033602562020327";
            string CardExpireDateMonth = "01";
            string CardExpireDateYear = "30";
            string CardCVV2 = "861";
            string Type = "Sale";
            string CurrencyCode = "0949";
            string MerchantOrderId = "siparis123123";
            string Amount = "8650";
            string CustomerId = "400235";
            string MerchantId = "496";
            string OkUrl = "http://localhost:2428/Kuveytturk/Success";
            string FailUrl = "http://localhost:2428/Kuveytturk/Error";
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

            Response.Write(responseString);
            return View();
        }

        public string ComputeHash(string hashstr)
        {
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            String hash = Convert.ToBase64String(inputbytes);
            return hash;
        }

        public ActionResult Error()
        {
            string requestContent = string.Empty;
            requestContent = Request.Form["AuthenticationResponse"];
            requestContent = System.Web.HttpUtility.UrlDecode(requestContent);
            return View();
        }

        public ActionResult Success()
        {
            string requestContent = string.Empty;
            requestContent = Request.Form["AuthenticationResponse"];
            requestContent = System.Web.HttpUtility.UrlDecode(requestContent);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(requestContent);
            string MD = xmlDocument.SelectSingleNode("VPosTransactionResponseContract/MD").InnerText;

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

            return View();
        }
    }
}