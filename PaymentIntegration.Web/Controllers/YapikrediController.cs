using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegration.Web.Controllers
{
    public class YapikrediController : Controller
    {
        // GET: Yapikredi
        public ActionResult Index()
        {
            return RedirectToAction("Payment");
        }

        public ActionResult Payment()
        {
            var xml = @"<?xml version='1.0' encoding='utf-8'?>" +
            "<posnetRequest>" +
            "<mid>6706598320</mid>" +
            "<tid>67555130</tid>" +
            "<oosRequestData>" +
            "<posnetid>29004</posnetid>" +
            "<XID>YKB_0000080603143050</XID>" +
            "<amount>5696</amount>" +
            "<currencyCode>TL</currencyCode>" +
            "<installment>00</installment>" +
            "<tranType>Sale</tranType>" +
            "<cardHolderName>ĞğÜüİıŞşÖöÇç</cardHolderName>" +
            "<ccno>4506344147404266</ccno>" +
            "<expDate>2203</expDate>" +
            "<cvc>000</cvc>" +
            "</oosRequestData>" +
            "</posnetRequest>";


            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create("https://setmpos.ykb.com/PosnetWebService/XML?xmldata=" + xml); //Mpi Enrollment Adresi
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

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}