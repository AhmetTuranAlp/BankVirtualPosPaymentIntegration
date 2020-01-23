using PaymentIntegration.Banks.NestPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_TurkiyeFinans :IPayment
    {
        public string Cancel()
        {
            throw new NotImplementedException();
        }

        public string FinishPayment(object data)
        {
            throw new NotImplementedException();
        }

        public string OpenProvision(PaymentInfo store)
        {
            string processType = "Auth";//İşlem tipi
            string clientId = "280000200";//Mağaza numarası
            string storeKey = "TRPS2828";//Mağaza anahtarı
            string storeType = "3D_PAY";//SMS onaylı ödeme modeli 3DPay olarak adlandırılıyor.
            string successUrl = "http://localhost:2428/Home/PaymentResult";//Başarılı Url
            string unsuccessUrl = "http://localhost:2428/Home/PaymentResult";//Hata Url
            string randomKey = ThreeDHelper.CreateRandomValue(10, false, false, true, false);
            string installment = store.OrderInfos.Installment;//Taksit
            string orderNumber = store.OrderInfos.OrderNo;//Sipariş numarası
            string currencyCode = "949";//TL ISO code | EURO "978" | Dolar "840"
            string languageCode = "tr";// veya "en"
            string cardType = store.CreditCardInfos.CardType; //Kart Ailesi Visa 1 | MasterCard 2 | Amex 3
            string orderAmount = store.OrderInfos.OrderTotalPrice;//Decimal seperator nokta olmalı!

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
            paymentCollection.Add("pan", store.CreditCardInfos.CardNo);
            paymentCollection.Add("cardHolderName", store.CreditCardInfos.CardOwner);
            paymentCollection.Add("cv2", store.CreditCardInfos.Cvc);
            paymentCollection.Add("Ecom_Payment_Card_ExpDate_Year", store.CreditCardInfos.YearExpire);
            paymentCollection.Add("Ecom_Payment_Card_ExpDate_Month", store.CreditCardInfos.MonthExpire);
            paymentCollection.Add("taksit", installment);
            paymentCollection.Add("cartType", cardType);

            //Normalde ThreeDHelper.PrepareForm'dan string dönüyor fakat return view diyip string değişken verirsek bunu view adı olarak algılar ve hata verir.
            //Hem model hemde view adı vermek yerine object olarak model göndermek daha kolay.
            string paymentForm = ThreeDHelper.PrepareForm("https://entegrasyon.asseco-see.com.tr/fim/est3Dgate", paymentCollection);

            return paymentForm;
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }
    }
}
