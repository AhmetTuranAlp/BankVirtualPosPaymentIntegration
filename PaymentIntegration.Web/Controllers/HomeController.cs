
using BanksPaymentIntegration.PaymentCore;
using BanksPaymentIntegration.PaymentCore.Banks;
using Iyzipay.Model;
using PaymentIntegration.Banks.Garanti.Response;
using System;
using System.Collections;
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
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PaymentIntegration.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string HtmlContent = "";

            #region Ödeme Bilgileri
            PaymentInfo paymentInfo = new PaymentInfo();
            AddressInfos addressInfosBilling = new AddressInfos()
            {
                City = "İstanbul",
                ContactName = "Ahmet Turan ALP",
                Country = "Turkey",
                PaymentAddressType = PaymentAddressType.Billing
            };
            paymentInfo.AddressInfos.Add(addressInfosBilling);
            AddressInfos addressInfosShipping = new AddressInfos()
            {
                City = "İstanbul",
                ContactName = "Ahmet Turan ALP",
                Country = "Turkey",
                PaymentAddressType = PaymentAddressType.Shipping
            };
            paymentInfo.AddressInfos.Add(addressInfosShipping);
            paymentInfo.CreditCardInfos = new CreditCardInfos()
            {
                CardNo = "4022774022774026",
                CardOwner = "Ahmet Turan ALP",
                CardType = "1",
                Cvc = "000",
                MonthExpire = "12",
                YearExpire = "26"
            };
            paymentInfo.OrderInfos = new OrderInfos()
            {
                Currency = Currency.TRY.ToString(),
                Installment = "1",
                OrderNo = "123456",
                OrderTotalPrice = "1.01"

            };
            paymentInfo.BuyerInfos = new BuyerInfos()
            {
                City = "İstanbul",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Country = "Turkey",
                Email = "email@email.com",
                GsmNumber = "+905350000000",
                IdentityNumber = "74300864791",
                IP = "",
                Name = "Ahmet Turan",
                Surname = "Alp"

            };
            paymentInfo.BasketInfos.Add(new BasketInfos
            {
                Category = "Collectibles",
                Id = "BI102",
                Name = "Game code",
                Price = "1"

            });
            #endregion

            PaymentType paymentType = PaymentType.Halkbank;
            switch (paymentType)
            {
                case PaymentType.Iyzico:
                    Payment_Iyzico payment_Iyzico = new Payment_Iyzico();
                    HtmlContent = payment_Iyzico.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.Akbank: //NestPay
                    Payment_Akbank payment_Akbank = new Payment_Akbank();
                    HtmlContent = payment_Akbank.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.Halkbank: //NestPay
                    Payment_Halkbank payment_Halkbank = new Payment_Halkbank();
                    HtmlContent = payment_Halkbank.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.Garanti:
                    Payment_Garanti payment_Garanti = new Payment_Garanti();
                    HtmlContent = payment_Garanti.OpenProvision(paymentInfo);
                    return View(HtmlContent);
                case PaymentType.IsBankasi: //NestPay
                    Payment_IsBankasi payment_IsBankasi = new Payment_IsBankasi();
                    HtmlContent = payment_IsBankasi.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.Ziraat: //NestPay
                    Payment_Ziraat payment_Ziraat = new Payment_Ziraat();
                    HtmlContent = payment_Ziraat.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.QnbFinans: //NestPay
                    Payment_QnbFinans payment_QnbFinans = new Payment_QnbFinans();
                    HtmlContent = payment_QnbFinans.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.TurkiyeFinans: //NestPay
                    Payment_TurkiyeFinans payment_TurkiyeFinans = new Payment_TurkiyeFinans();
                    HtmlContent = payment_TurkiyeFinans.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.DenizBank:
                    Payment_DenizBank payment_DenizBank = new Payment_DenizBank();
                    HtmlContent = payment_DenizBank.OpenProvision(paymentInfo);
                    break;
                case PaymentType.VakifBank:
                    Payment_Vakifbank payment_Vakifbank = new Payment_Vakifbank();
                    HtmlContent = payment_Vakifbank.OpenProvision(paymentInfo);
                    Response.ContentType = "text/html";
                    Response.Write(HtmlContent);
                    break;
                case PaymentType.AnadoluBank: //NestPay
                    Payment_AnadoluBank payment_AnadoluBank = new Payment_AnadoluBank();
                    HtmlContent = payment_AnadoluBank.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.SekerBank: //NestPay
                    Payment_SekerBank payment_SekerBank = new Payment_SekerBank();
                    HtmlContent = payment_SekerBank.OpenProvision(paymentInfo);
                    return Content(HtmlContent);
                case PaymentType.YapiKredi:
                    break;
                case PaymentType.Kuveytturk:
                    Payment_Kuveytturk payment_Kuveytturk = new Payment_Kuveytturk();
                    HtmlContent = payment_Kuveytturk.OpenProvision(paymentInfo);
                    Response.Write(HtmlContent);
                    break;
                case PaymentType.OdeaBank:
                    break;
                case PaymentType.Hsbc:
                    break;
                case PaymentType.IngBank:
                    break;
                case PaymentType.TEB: //NestPay
                    Payment_Teb payment_Teb = new Payment_Teb();
                    HtmlContent = payment_Teb.OpenProvision(paymentInfo);
                    return Content(HtmlContent);

                default:
                    break;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PaymentResult()
        {
            return View();
        }


    }
}