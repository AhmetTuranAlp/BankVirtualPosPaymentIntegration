using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore
{
    public class PaymentInfo
    {
        public PaymentInfo()
        {
            CreditCardInfos = new CreditCardInfos();
            OrderInfos = new OrderInfos();
            BuyerInfos = new BuyerInfos();
            AddressInfos = new List<AddressInfos>();
            BasketInfos = new List<BasketInfos>();
        }

        public CreditCardInfos CreditCardInfos { get; set; }
        public OrderInfos OrderInfos { get; set; }
        public BuyerInfos BuyerInfos { get; set; }
        public List<AddressInfos> AddressInfos { get; set; }
        public List<BasketInfos> BasketInfos { get; set; }
    }

    public class CreditCardInfos
    {
        public CreditCardInfos()
        {
            CardNo = "";
            CardOwner = "";
            MonthExpire = "";
            YearExpire = "";
            Cvc = "";
            CardType = "";
        }
        public string CardOwner { get; set; }
        public string CardNo { get; set; }
        public string MonthExpire { get; set; }
        public string YearExpire { get; set; }
        public string Cvc { get; set; }
        public string CardType { get; set; }
    }

    public class OrderInfos
    {
        public OrderInfos()
        {
            OrderNo = "";
            Currency = "";
            Installment = "";
            OrderTotalPrice = "";
        }
        public string OrderNo { get; set; }
        public string Currency { get; set; }
        public string Installment { get; set; }
        public string OrderTotalPrice { get; set; }
    }

    public class BuyerInfos
    {
        public BuyerInfos()
        {
            Name = "";
            Surname = "";
            GsmNumber = "";
            Email = "";
            IdentityNumber = "";
            RegistrationAddress = "";
            IP = "";
            Country = "";
            City = "";
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GsmNumber { get; set; }
        public string Email { get; set; }
        public string IdentityNumber { get; set; } // Kimlik Numarası
        public string RegistrationAddress { get; set; }//Kayıt adresi
        public string IP { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class AddressInfos
    {
        public AddressInfos()
        {
            ContactName = "";
            City = "";
            Country = "";
        }
        public PaymentAddressType PaymentAddressType { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class BasketInfos
    {
        public BasketInfos()
        {
            Id = "";
            Name = "";
            Category = "";
            Price = "";
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
    }



    public enum PaymentType
    {
        [Description("Iyzico")]
        Iyzico = 0,
        [Description("Akbank")]
        Akbank = 1,
        [Description("Halkbank")]
        Halkbank = 2,
        [Description("Garanti Bankası")]
        Garanti = 3,
        [Description("Türkiye İş Bankası")]
        IsBankasi = 4,
        [Description("Ziraat Bankası")]
        Ziraat = 5,
        [Description("QnbFinans Bankası")]
        QnbFinans = 6,
        [Description("Türkiye Finans")]
        TurkiyeFinans = 7,
        [Description("DenizBank")]
        DenizBank = 8,
        [Description("VakıfBank")]
        VakifBank = 9,
        [Description("AnadoluBank")]
        AnadoluBank = 10,
        [Description("ŞekerBank")]
        SekerBank = 11,
        [Description("YapıKredi")]
        YapiKredi = 12,
        [Description("Kuveyttürk")]
        Kuveytturk = 13,
        [Description("OdeaBank")]
        OdeaBank = 14,
        [Description("Hsbc Bankası")]
        Hsbc = 15,
        [Description("İng Bankası")]
        IngBank = 16,
        [Description("TEB")]
        TEB = 17
    }

    public enum PaymentAddressType
    {
        [Description("Fatura Adresi")]
        Billing = 0,
        [Description("Teslimat Adresi")]
        Shipping = 1,
    }
}
