using PaymentIntegration.Banks.Garanti.BaseEntity;
using PaymentIntegration.Banks.Garanti.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_Garanti : IPayment
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
            Terminal terminal = new Terminal()
            {
                ID = "30691297", //Garanti bankası tarafından sağlanan kendi bilgileriniz ile değiştirmelisiniz. 
                MerchantID = "7000679", //Garanti bankası tarafından sağlanan kendi bilgileriniz ile değiştirmelisiniz. 
                ProvUserID = "PROVAUT", //Garanti bankası tarafından sağlanan kendi bilgileriniz ile değiştirmelisiniz. 
                UserID = "PROVAUT", //Garanti bankası tarafından sağlanan kendi bilgileriniz ile değiştirmelisiniz. 
                HashData = "", //Hesaplanacak
            };

            Settings settings = new Settings()
            {
                Version = "V0.1", // Sabit Kalmalı 
                Mode = "Test", // Kullandığınız ortama göre değiştirmelisiniz. 
                BaseUrl = "https://sanalposprovtest.garanti.com.tr/VPServlet", //Test Adresi  // Üretim otamına geçtiğinizde adresi değiştirmelisiniz. 
                Password = "123qweASD/" //Kendi şifreniz ile değiştirmelisiniz. 
            };

            Settings3D settings3D = new Settings3D()
            {
                mode = "Test",
                apiversion = "V0.1",
                BaseUrl = "https://sanalposprovtest.garanti.com.tr/servlet/gt3dengine",
                Password = "123qweASD/"
            };

            var request = new Sale3DSecureRequest();

            request.apiversion = settings3D.apiversion;
            request.mode = settings3D.mode;
            request.terminalid = terminal.ID;
            request.terminaluserid = terminal.UserID;
            request.terminalprovuserid = terminal.ProvUserID;
            request.terminalmerchantid = terminal.MerchantID;

            request.successurl = "http://localhost:2428/Home/Success"; //Başarılı Döndügünde Gidilecek Sayfa
            request.errorurl = "http://localhost:2428/Home/Error"; //Başarısız Döndügünde Gidilecek Sayfa
            request.customeremailaddress = store.BuyerInfos.Email; //Kullanıcı Mail Adresi
            request.customeripaddress = "127.0.0.1"; //Kullanıcı IP Adresi
            request.secure3dsecuritylevel = "3D"; //Ödeme Sekli(3DSecure)
            request.orderid = store.OrderInfos.OrderNo; //Her İşlemde Farklı Bir Değer Gönderilmeli    
            request.txnamount = store.OrderInfos.OrderTotalPrice;  //İşlem Tutarı 1.00 TL için 100 Gönderilmeli
            request.txntype = "sales"; //İşlem Tipi
            request.txninstallmentcount = store.OrderInfos.Installment; //Taksit Sayısı. Boş Gönderilirse Taksit Yapılmaz
            request.txncurrencycode = "949"; //Türk Lirası
            request.storekey = "12345678"; //Storekey Verisi Yazılması Gerekiyor.
            request.txntimestamp = DateTime.Now.Ticks.ToString();
            request.cardnumber = store.CreditCardInfos.CardNo;
            request.cardexpiredatemonth = store.CreditCardInfos.MonthExpire;
            request.cardexpiredateyear = store.CreditCardInfos.YearExpire;
            request.cardcvv2 = store.CreditCardInfos.Cvc;
            request.lang = "tr";
            request.refreshtime = "5";

            var form = Sale3DSecureRequest.Execute(request, settings3D);
            return form;
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }
    }
}
