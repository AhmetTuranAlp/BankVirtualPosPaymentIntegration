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

        #region 3D Secure Satış
        public ActionResult Sale3DSecure()
        {
            return View();
        }

        /// <summary>
        /// 3D Secure Satış Post
        /// </summary>
        /// <param name="creditCardNo"></param>
        /// <param name="expireMonth"></param>
        /// <param name="expireYear"></param>
        /// <param name="cvv"></param>
        /// <param name="transactionAmount"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
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

            request.successurl = "http://localhost:2428/Garanti/Success";
            request.errorurl = "http://localhost:2428/Garanti/Error";
            request.customeremailaddress = "ahmetturanalp@gmail.com";
            request.customeripaddress = "127.0.0.1"; 
            request.secure3dsecuritylevel = "3D"; // Ödeme Sekli Seçilmektedir.(3DSecure)
            request.orderid = orderID;
            request.txnamount = transactionAmount;
            request.txntype = "sales";
            request.txninstallmentcount = "";
            request.txncurrencycode = "949";
            request.storekey = "12345678";
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
        #endregion

        #region 3D Error/Success Page
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
            //Validation kontrolü yapılır.
            string hash = Request.Form.Get("hash");
            string hashParamsVal = "";
            string storeKey = "12345678";  // kendi storekey değeri girilmelidir.
            string hashParams = Request.Form.Get("hashparams");
            bool valid = false;

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

            if (valid)
            {
                // işlem başarılı ise değerler alınır.
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

                    request.Terminal = new Terminal()
                    {
                        ID = secure3DResponse.terminalID,
                        MerchantID = secure3DResponse.terminalMerchantID,
                        ProvUserID = secure3DResponse.terminalProvUserID,
                        UserID = secure3DResponse.terminalUserID
                    };
                    request.Card = new Card()
                    {
                        CVV2 = "",
                        ExpireDate = "",
                        Number = ""

                    };
                    request.Customer = new Customer()
                    {
                        EmailAddress = secure3DResponse.customerEmailAddress,
                        IPAddress = secure3DResponse.customerIpAddres
                    };

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
        #endregion














    }
}