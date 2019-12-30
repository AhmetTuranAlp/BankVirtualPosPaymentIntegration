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
    public class VakifBanksController : Controller
    {
        // GET: VakifBanks
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale3D(string creditCardNo, string expireMonth, string expireYear, string cvv, string transactionAmount, string orderID)
        {
            string MerchantId = "000100000013506"; //Üye İşyeri Numarası
            string MerchantPassword = "123456"; //Üye İşyeri Şifresi
            string TransactionId = "Sale"; //Peşin ve Taksitli Satış işlemleri için “Sale” olarak gönderilmelidir.
            string VerifyEnrollmentRequestId = Guid.NewGuid().ToString("N"); //ÜİY tarafından üretilen işlem numarasıdır. Benzersiz olmalıdır. ÜİY, işlemlerini bu numara üzerinden takip edebilir. 
            string Pan = creditCardNo; //Kredi kartı numarası
            string ExpiryDate = expireYear + expireMonth; //Kredi kartı son kullanma tarihi. YYAA formatında 4 karakter olarak gönderilmelidir. 
            string PurchaseAmount = transactionAmount; //Satış tutarı
            string Currency = "949"; //İşlemin yapıldığı sayısal para birimi kodu 
            string BrandName = "100"; //Kredi kartı Kart Kuruluşu Bilgisi. 100:VISA 200:MASTERCARD 300:TROY 
            string SessionInfo = ""; //Oturum Bilgisi. ÜİY tarafında gönderilmesi Opsiyonel, sadece bilgi amaçlı tutulan bir alandır.
            string SuccessUrl = "http://localhost:2428/VakifBanks/Success"; //ÜİY’nin işlem sonucun başarılı olması durumunda, dönüş yapılmasını istediği sayfa URL si. 
            string FailureUrl = "http://localhost:2428/VakifBanks/Error"; //ÜİY’nin işlem sonucunun başarısız olması durumunda, dönüş yapılmasını istediği sayfa URL si.
            string InstallmentCount = ""; //İşlem taksit sayısı. Eğer ÜİY tarafından gönderilirse, 1'den büyük bir değer olmalıdır. 0 ya da 1 gönderildiğinde MPI işlemi reddeder. 
            
            string data = "Pan=" + creditCardNo +
                "&ExpiryDate=" + expireYear + expireMonth +
                "&PurchaseAmount=" + transactionAmount +
                "&Currency=" + Currency +
                "&BrandName=" + BrandName +
                "&VerifyEnrollmentRequestId=" + VerifyEnrollmentRequestId +
                "&MerchantID=" + MerchantId +
                "&MerchantPassword=" + MerchantPassword +
                "&TransactionId=" + TransactionId +
                "&SuccessUrl=" + SuccessUrl +
                "&FailureUrl=" + FailureUrl +
                "&SessionInfo="; 

            byte[] dataStream = Encoding.UTF8.GetBytes(data);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://3dsecuretest.vakifbank.com.tr:4443/MPIAPI/MPI_Enrollment.aspx"); //Mpi Enrollment Adresi
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;
            webRequest.KeepAlive = false;
            string responseFromServer = "";

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }

            if (string.IsNullOrEmpty(responseFromServer))
            {
                int a = 0;
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseFromServer);


            var statusNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/Status");
            var pareqNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/PaReq");
            var acsUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/ACSUrl");
            var termUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/TermUrl");
            var mdNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/MD");
            var messageErrorCodeNode = xmlDocument.SelectSingleNode("IPaySecure/MessageErrorCode");

            string statusText = "";

            if (statusNode != null)
            {
                statusText = statusNode.InnerText;
            }

            //3d secure programına dahil
            if (statusText == "Y")
            {
                string postBackForm =
                   @"<html>
                          <head>
                            <meta name='viewport' content='width=device-width' />
                            <title>MpiForm</title>
                            <script>
                              function postPage() {
                              document.forms['frmMpiForm'].submit();
                              }
                            </script>
                          </head>
                          <body onload='javascript:postPage();'>
                            <form action='@ACSUrl' method='post' id='frmMpiForm' name='frmMpiForm'>
                              <input type='hidden' name='PaReq' value='@PAReq' />
                              <input type='hidden' name='TermUrl' value='@TermUrl' />
                              <input type='hidden' name='MD' value='@MD' />
                              <noscript>
                                <input type='submit' id='btnSubmit' value='Gönder' />
                              </noscript>
                            </form>
                          </body>
                        </html>";

                postBackForm = postBackForm.Replace("@ACSUrl", acsUrlNode.InnerText);
                postBackForm = postBackForm.Replace("@PAReq", pareqNode.InnerText);
                postBackForm = postBackForm.Replace("@TermUrl", termUrlNode.InnerText);
                postBackForm = postBackForm.Replace("@MD", mdNode.InnerText);

                Response.ContentType = "text/html";
                Response.Write(postBackForm);
            }
            else if (statusText == "E")
            {
                string errorCode = messageErrorCodeNode.InnerText;
            }
            return View();
        }

        public ActionResult Success()
        {
            string Status = Request.Form["Status"];
            string MerchantId = Request.Form["MerchantId"];
            string VerifyEnrollmentRequestId = Request.Form["VerifyEnrollmentRequestId"];
            string Xid = Request.Form["Xid"];
            string PurchAmount = Request.Form["PurchAmount"];
            string SessionInfo = Request.Form["SessionInfo"];
            string PurchCurrency = Request.Form["PurchCurrency"];
            string Pan = Request.Form["Pan"];
            string ExpiryDate = Request.Form["Expiry"];
            string Eci = Request.Form["Eci"];
            string Cavv = Request.Form["Cavv"];
            string InstallmentCount = Request.Form["InstallmentCount"];

            return View();
        }
        public ActionResult Error()
        {
            string Status = Request.Form["Status"];
            return View();
        }
    }
}