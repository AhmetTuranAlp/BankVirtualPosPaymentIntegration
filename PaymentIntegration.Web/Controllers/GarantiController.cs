using PaymentIntegration.Banks.Garanti;
using PaymentIntegration.Banks.Garanti.BaseEntity;
using PaymentIntegration.Banks.Garanti.Request;
using PaymentIntegration.Banks.Garanti.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegration.Web.Controllers
{
    public class GarantiController : GarantiBaseController
    {
        public ActionResult Sale3DSecure()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sale3DSecure(string creditCardNo, string expireMonth, string expireYear, string cvv, string transactionAmount, string orderID)
        {
            var request = new Sale3DSecureRequest();

            request.apiversion = settings3D.apiversion;
            request.mode = settings3D.mode;
            request.terminalid = terminal.ID;
            request.terminaluserid = terminal.UserID;
            request.terminalprovuserid = terminal.ProvUserID;
            request.terminalmerchantid = terminal.MerchantID;

            request.successurl = "http://localhost:2428/Garanti/Success"; //Başarılı Döndügünde Gidilecek Sayfa
            request.errorurl = "http://localhost:2428/Garanti/Error"; //Başarısız Döndügünde Gidilecek Sayfa
            request.customeremailaddress = "ahmetturanalp@gmail.com"; //Kullanıcı Mail Adresi
            request.customeripaddress = "127.0.0.1"; //Kullanıcı IP Adresi
            request.secure3dsecuritylevel = "3D"; //Ödeme Sekli(3DSecure)
            request.orderid = orderID; //Her İşlemde Farklı Bir Değer Gönderilmeli    
            request.txnamount = transactionAmount;  //İşlem Tutarı 1.00 TL için 100 Gönderilmeli
            request.txntype = "sales"; //İşlem Tipi
            request.txninstallmentcount = ""; //Taksit Sayısı. Boş Gönderilirse Taksit Yapılmaz
            request.txncurrencycode = "949"; //Türk Lirası
            request.storekey = "12345678"; //Storekey Verisi Yazılması Gerekiyor.
            request.txntimestamp = DateTime.Now.Ticks.ToString();
            request.cardnumber = creditCardNo;
            request.cardexpiredatemonth = expireMonth;
            request.cardexpiredateyear = expireYear;
            request.cardcvv2 = cvv;
            request.lang = "tr";
            request.refreshtime = "5";

            var form = Sale3DSecureRequest.Execute(request, settings3D);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(form);
            System.Web.HttpContext.Current.Response.End();

            return View();
        }

        public ActionResult Error()
        {
            string[] keys = Request.Form.AllKeys;
            StringBuilder strForm = new StringBuilder();
            for (int i = 0; i < keys.Length; i++)
            {
                strForm.Append(keys[i] + ": " + Request.Form[keys[i]]);
                strForm.AppendLine();
            }

            ServicesXmlResponse responseMessage = new ServicesXmlResponse();
            responseMessage.XmlResponse = strForm.ToString();
            return View(responseMessage);
        }

        [HttpPost]
        public ActionResult Success()
        {
            bool valid = false;

            #region Validation kontrolü yapılır.
            string hash = Request.Form.Get("hash");
            string hashParamsVal = "";
            string storeKey = "12345678";  // kendi storekey değeri girilmelidir.
            string hashParams = Request.Form.Get("hashparams");

            if (hashParams != null && hashParams != "")
            {
                var result = hashParams.Split(':');

                foreach (string param in result)
                {
                    hashParamsVal += Request.Form.Get(param);
                }
                hashParamsVal += storeKey;
                valid = Helper.Validate3DReturn(hashParamsVal, hash);
            }
            #endregion

            if (valid)
            {
                // işlem başarılı ise değerler alınır.
                //mdStatus = 1 alan işlem tam doğrulama olarak adlandırılır. Bu işlemde müşteri tarafından  kart şifresi başarılı olarak girilmiştir.
                //mdStatus = 2,3,4 alan işlemler yarım doğrulama olarak da degerlendirilir.
                if ((Request.Form.Get("mdstatus").ToString() == "1") || (Request.Form.Get("mdstatus").ToString() == "2")
                || (Request.Form.Get("mdstatus").ToString() == "3") || (Request.Form.Get("mdstatus").ToString() == "4"))
                {

                    var secure3DResponse = new Secure3DResponse()
                    {
                        orderID = Request.Form.Get("orderid"),
                        authenticationCode = Server.UrlEncode(Request.Form.Get("cavv")),
                        securityLevel = Server.UrlEncode(Request.Form.Get("eci")),
                        txnID = Server.UrlEncode(Request.Form.Get("xid")),
                        MD = Server.UrlEncode(Request.Form.Get("md")),

                        mode = Request.Form.Get("mode"),
                        apiversion = Request.Form.Get("apiversion"),

                        terminalProvUserID = Request.Form.Get("terminalprovuserid"),
                        installmentCount = Request.Form.Get("txninstallmentcount"),
                        terminalUserID = Request.Form.Get("terminaluserid"),
                        terminalID = Request.Form.Get("clientid"),

                        amount = Request.Form.Get("txnamount"),
                        currencyCode = Request.Form.Get("txncurrencycode"),
                        customerIpAddres = Request.Form.Get("customeripaddress"),
                        customerEmailAddress = Request.Form.Get("customeremailaddress"),
                        terminalMerchantID = Request.Form.Get("terminalmerchantid"),
                        txnType = Request.Form.Get("txntype"),
                        procreturnCode = Request.Form.Get("procreturncode"),
                        authcode = Request.Form.Get("authcode"),
                        response = Request.Form.Get("response"),
                        mdstatus = Request.Form.Get("mdstatus"),
                        rnd = Request.Form.Get("rnd"),
                        xmlResponse = ""

                    };

                    var request = new Secure3DCompleteRequest();
                    request.Version = secure3DResponse.apiversion;
                    request.Mode = secure3DResponse.mode;

                    #region Terminal Bilgileri Alınır.
                    request.Terminal = new Terminal()
                    {
                        ID = secure3DResponse.terminalID,
                        MerchantID = secure3DResponse.terminalMerchantID,
                        ProvUserID = secure3DResponse.terminalProvUserID,
                        UserID = secure3DResponse.terminalUserID
                    };
                    #endregion

                    #region Kart Bilgileri Alınır.
                    request.Card = new Card()
                    {
                        CVV2 = "",
                        ExpireDate = "",
                        Number = ""

                    };
                    #endregion

                    #region Kullanıcı Mail ve IP Adresi Alınır.
                    request.Customer = new Customer()
                    {
                        EmailAddress = secure3DResponse.customerEmailAddress,
                        IPAddress = secure3DResponse.customerIpAddres
                    };
                    #endregion

                    var secure3D = new Secure3D();
                    secure3D.AuthenticationCode = secure3DResponse.authenticationCode;
                    secure3D.Md = secure3DResponse.MD;
                    secure3D.SecurityLevel = secure3DResponse.securityLevel;
                    secure3D.TxnID = secure3DResponse.txnID;

                    request.Order = new Order()
                    {
                        OrderID = secure3DResponse.orderID,
                        Description = string.Empty
                    };

                    request.Transaction = new Secure3DCompleteTransactionRequest()
                    {
                        Amount = secure3DResponse.amount,
                        CurrencyCode = secure3DResponse.currencyCode,
                        InstallmentCnt = secure3DResponse.installmentCount,
                        Type = secure3DResponse.txnType,
                        MotoInd = "N",
                        CardholderPresentCode = 13,
                        Secure3D = secure3D
                    };

                    var response = Secure3DCompleteRequest.Execute(request, settings);
                    secure3DResponse.xmlResponse = response;
                    return View(secure3DResponse);
                }
                return View();
            }
            else
                Response.Write("Hash Doğrulaması Yapılamadı.");
            return View();
        }

    }
}