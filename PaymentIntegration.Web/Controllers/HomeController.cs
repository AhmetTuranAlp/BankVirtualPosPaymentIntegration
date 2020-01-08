
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PaymentIntegration.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var xml = @"<?xml version='1.0' encoding='utf-8'?>" +
                "<posnetRequest>" +
                "<mid>6706598320</mid>" +
                "<tid>67005551</tid>" +
                "<oosRequestData>" +
                "<posnetid>9644</posnetid>" +
                "<XID>YKB_0000080603143050</XID>" +
                "<amount>5696</amount>" +
                "<currencyCode>TL</currencyCode>" +
                "<installment>00</installment>" +
                "<tranType>Sale</tranType>" +
                "<cardHolderName>ĞğÜüİıŞşÖöÇç</cardHolderName>" +
                "<ccno>4506347022052795</ccno>" +
                "<expDate>0819</expDate>" +
                "<cvc>000</cvc>" +
                "</oosRequestData>" +
                "</posnetRequest>";


            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + xml);
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



            return View();
        }

        public ActionResult Sale3D()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
    }
}