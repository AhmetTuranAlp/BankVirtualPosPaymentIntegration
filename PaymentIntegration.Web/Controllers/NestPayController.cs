using PaymentIntegration.Banks.NestPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegration.Web.Controllers
{
    public class NestPayController : Controller
    {
        // NestPay(assecco) => Ziraat Bankası, Finansbank, Halkbank, Teb, TürkiyeFinans, Akbank, İş Bankası, Şekerbank, Anadolubank nestpay altyapısını kullanmaktadır.

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CreditCardModel model)
        {
            TempData["CreditCard"] = model;

            return RedirectToAction("ThreeDGate");
        }

        public ActionResult ThreeDGate()
        {
            //Indexden tempdata ile object olarak gelen değeri geri CreditCardModel'e cast ediyoruz.
            var cardModel = TempData["CreditCard"] as CreditCardModel;
            if (cardModel == null)
                return RedirectToAction("Home");

            string processType = "Auth";//İşlem tipi
            string clientId = cardModel.ClientId;//Mağaza numarası
            string storeKey = cardModel.Storekey;//Mağaza anahtarı
            string storeType = "3D_PAY";//SMS onaylı ödeme modeli 3DPay olarak adlandırılıyor.
            string successUrl = "http://localhost:2428/NestPay/Success";//Başarılı Url
            string unsuccessUrl = "http://localhost:2428/NestPay/UnSuccess";//Hata Url
            string randomKey = ThreeDHelper.CreateRandomValue(10, false, false, true, false);
            string installment = "1";//Taksit
            string orderNumber = ThreeDHelper.CreateRandomValue(8, false, false, true, false);//Sipariş numarası
            string currencyCode = "949"; //TL ISO code | EURO "978" | Dolar "840"
            string languageCode = "tr";// veya "en"
            string cardType = "1"; //Kart Ailesi Visa 1 | MasterCard 2 | Amex 3
            string orderAmount = "24.90";//Decimal seperator nokta olmalı!

            //Güvenlik amaçlı olarak birleştirip şifreliyoruz. Banka decode edip bilgilerin doğruluğunu kontrol ediyor. Alanların sırasına dikkat etmeliyiz.
            string hashFormat = clientId + orderNumber + orderAmount + successUrl + unsuccessUrl + processType + installment + randomKey + storeKey;

            var paymentCollection = new NameValueCollection();

            //Mağaza bilgileri
            paymentCollection.Add("hash", ThreeDHelper.ConvertSHA1(hashFormat));
            paymentCollection.Add("clientid", clientId);
            paymentCollection.Add("storetype", storeType);
            paymentCollection.Add("rnd", randomKey);
            paymentCollection.Add("okUrl", successUrl);
            paymentCollection.Add("failUrl", unsuccessUrl);
            paymentCollection.Add("islemtipi", processType);
            //Ödeme bilgileri
            paymentCollection.Add("currency", currencyCode);
            paymentCollection.Add("lang", languageCode);
            paymentCollection.Add("amount", orderAmount);
            paymentCollection.Add("oid", orderNumber);
            //Kredi kart bilgileri
            paymentCollection.Add("pan", cardModel.CardNumber);
            paymentCollection.Add("cardHolderName", cardModel.HolderName);
            paymentCollection.Add("cv2", cardModel.CV2);
            paymentCollection.Add("Ecom_Payment_Card_ExpDate_Year", cardModel.ExpireYear);
            paymentCollection.Add("Ecom_Payment_Card_ExpDate_Month", cardModel.ExpireMonth);
            paymentCollection.Add("taksit", installment);
            paymentCollection.Add("cartType", cardType);


            //Normalde ThreeDHelper.PrepareForm'dan string dönüyor fakat return view diyip string değişken verirsek bunu view adı olarak algılar ve hata verir.
            //Hem model hemde view adı vermek yerine object olarak model göndermek daha kolay.
            object paymentForm = ThreeDHelper.PrepareForm("https://entegrasyon.asseco-see.com.tr/fim/est3Dgate", paymentCollection);

            return View(paymentForm);
        }

        public ActionResult Success()
        {
            //Üye iş yeri, bankadan HASHPARAMS & HASHPARAMSVAL & Odeme sonucunda dönen HASH'i kendi tarafında kontrol etmelidir. 
            return View();
        }

        public ActionResult UnSuccess()
        {
            return View();
        }
    }
}