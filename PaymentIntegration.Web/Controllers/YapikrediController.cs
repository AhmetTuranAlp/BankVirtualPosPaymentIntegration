using PaymentIntegration.Banks.YapıKredi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace PaymentIntegration.Web.Controllers
{
    public class YapikrediController : Controller
    {
        // GET: Yapikredi
        public ActionResult Index()
        {
            var xml = "<?xml version='1.0' encoding='utf-8'?>" +
              "<posnetRequest>" +
              "<mid>6706598320</mid>" +
              "<tid>67555130</tid>" +
              "<oosRequestData>" +
              "<posnetid>29004</posnetid>" +
              "<XID>YKB_0000080603143023</XID>" +
              "<amount>1111</amount>" +
              "<currencyCode>TL</currencyCode>" +
              "<installment>00</installment>" +
              "<tranType>Sale</tranType>" +
              "<cardHolderName>ĞğÜüİıŞşÖöÇç</cardHolderName>" +
              "<ccno>4462127002890642</ccno>" +
              "<expDate>2203</expDate>" +
              "<cvc>000</cvc>" +
              "</oosRequestData>" +
              "</posnetRequest>";

            var asd = Uri.EscapeUriString(xml);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + asd);
            webRequest.Method = "POST";
            webRequest.ContentType = "=application/xwww-form-urlencoded; charset=utf-8";
            webRequest.KeepAlive = false;
            string responseFromServer = "";


            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseFromServer);
            
            var data1 = xmlDocument.SelectSingleNode("posnetResponse/oosRequestDataResponse/data1").InnerText;
            var data2 = xmlDocument.SelectSingleNode("posnetResponse/oosRequestDataResponse/data2").InnerText;
            var sign = xmlDocument.SelectSingleNode("posnetResponse/oosRequestDataResponse/sign").InnerText;

            Response response = new Response()
            {
                data1 = data1,
                data2 = data2,
                sign = sign
            };


            return View(response);
        }

        
        public ActionResult Payment()
        {
            return View();
        }

        public string KullaniciDoğrulama(NameValueCollection Form)
        {
            string MerchantPackage = Request.Form["MerchantPacket"];
            string BankPackage = Request.Form["BankPacket"];
            string Sign = Request.Form["Sign"];

            string encKey = "10,10,10,10,10,10,10,10";
            string terminalID = "67555130";
            string xid = Request.Form["Xid"];
            string amount = Request.Form["Amount"];
            string currency = "TL";
            string merchantNo = "6706598320";
            string firstHash = HASH(encKey + ';' + terminalID);

            string MAC = HASH(xid + ";" + amount + ";" + currency + ";" + merchantNo + ";" + firstHash);
            
            var xml = "<?xml version='1.0' encoding='ISO-8859-9'?>" +
                     "<posnetRequest>" +
                     "<mid>" + merchantNo + "</mid>" +
                     "<tid>" + terminalID + "</tid>" +
                     "<oosResolveMerchantData>" +
                     "<bankData>" + BankPackage + "</bankData>" +
                     "<merchantData>" + MerchantPackage + "</merchantData>" +
                     "<sign>" + Sign + "</sign>" +
                     "<mac>" + MAC + "</mac>" +
                     "</oosResolveMerchantData>" +
                     "</posnetRequest>";

            var asd = Server.UrlEncode(xml);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + asd);
            webRequest.Method = "POST";
            webRequest.ContentType = "=application/xwww-form-urlencoded; charset=utf-8";
            webRequest.KeepAlive = false;
            string responseFromServer = "";


            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }
            return responseFromServer;
        }
        
        public string Finansallastirma(NameValueCollection Form)
        {
            string MerchantPackage = Request.Form["MerchantPacket"];
            string BankPackage = Request.Form["BankPacket"];
            string Sign = Request.Form["Sign"];
            string xid = Request.Form["Xid"]; ;
            string amount = Request.Form["Amount"];
            string currency = "TL";
            string merchantNo = "6706598320";
            string encKey = "10,10,10,10,10,10,10,10";
            string terminalID = "67555130";
            string MAC = HASH(xid + ';' + amount + ';' + currency + ';' + merchantNo + ';' + HASH(encKey + ';' + terminalID));

            var xml = "<?xml version='1.0' encoding='ISO-8859-9'?>" +
              "<posnetRequest >" +
              "<mid>" + merchantNo + "</mid>" +
              "<tid>" + terminalID + "</tid>" +
              "<oosTranData>" +
              "<bankData>" + BankPackage + "</bankData>" +
              "<wpAmount>" + amount + "</wpAmount>" +
              "<mac>" + MAC + "</mac>" +
              "</oosTranData>" +
              "</posnetRequest>";

            var asd = Server.UrlEncode(xml);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + asd);
            webRequest.Method = "POST";
            webRequest.ContentType = "=application/xwww-form-urlencoded; charset=utf-8";
            webRequest.KeepAlive = false;
            string responseFromServer = "";


            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }
            return responseFromServer;
        }

        public ActionResult ResultPage()
        {
            string kd = KullaniciDoğrulama(Request.Form);

            string fn = Finansallastirma(Request.Form);


            return View();
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