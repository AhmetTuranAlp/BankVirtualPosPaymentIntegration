using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanksPaymentIntegration.PaymentCore.Banks
{
    public class Payment_Iyzico : IPayment
    {
        public Options Options()
        {
            Options options = new Options
            {
                ApiKey = "sandbox-WfjBsjY2kNDXHnehDBtq45MBwQ7Kkyv9",
                SecretKey = "sandbox-30uaEn94Ma92HpfvVuaXy52aokBweJG2",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };
            return options;
        }

        public string Cancel()
        {
            throw new NotImplementedException();
        }

        public string FinishPayment(object data)
        {
            CheckoutForm checkoutForm1 = data != null ? data as CheckoutForm : new CheckoutForm();
            if (checkoutForm1.Status == "success")
            {
                Options options = Options();
                CreateThreedsPaymentRequest istek = new CreateThreedsPaymentRequest();
                istek.Locale = Locale.TR.ToString();
                istek.ConversationId = checkoutForm1.ConversationId;
                istek.PaymentId = checkoutForm1.PaymentId;
                // istek.ConversationData = "conversation data";

                ThreedsPayment threedsPayment = ThreedsPayment.Create(istek, options);

                return threedsPayment.Status == "success" ? "success" : "failure";
            }
            else
            {
                return "failure";
            }
            throw new NotImplementedException();
        }

        public string OpenProvision(PaymentInfo store)
        {
            Options options = Options();

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString(); //Zorunlu
            request.ConversationId = store.OrderInfos.OrderNo;
            request.Price = store.OrderInfos.OrderTotalPrice;//Zorunlu
            request.PaidPrice = store.OrderInfos.OrderTotalPrice;//Zorunlu
            request.Currency = Currency.TRY.ToString();//Zorunlu
            request.Installment = Convert.ToInt32(store.OrderInfos.Installment);//Zorunlu
            request.BasketId = "";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.SUBSCRIPTION.ToString();
            request.CallbackUrl = "http://localhost:2428/Home/PaymentResult";//Zorunlu

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = store.CreditCardInfos.CardOwner;//Zorunlu
            paymentCard.CardNumber = store.CreditCardInfos.CardNo;//Zorunlu
            paymentCard.ExpireMonth = store.CreditCardInfos.MonthExpire;//Zorunlu
            paymentCard.ExpireYear = store.CreditCardInfos.YearExpire;//Zorunlu
            paymentCard.Cvc = store.CreditCardInfos.Cvc;//Zorunlu
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = Guid.NewGuid().ToString();//Zorunlu
            buyer.Name = store.BuyerInfos.Name;//Zorunlu
            buyer.Surname = store.BuyerInfos.Surname;//Zorunlu
            buyer.GsmNumber = store.BuyerInfos.GsmNumber;
            buyer.Email = store.BuyerInfos.Email;//Zorunlu
            buyer.IdentityNumber = store.BuyerInfos.IdentityNumber;//Zorunlu
            buyer.LastLoginDate = "2013-04-21 15:12:09";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = store.BuyerInfos.RegistrationAddress;//Zorunlu
            buyer.Ip = store.BuyerInfos.IP; //Zorunlu
            buyer.City = store.BuyerInfos.City;//Zorunlu
            buyer.Country = store.BuyerInfos.Country;//Zorunlu
            buyer.ZipCode = "";
            request.Buyer = buyer;

            AddressInfos addressInfosShipping = store.AddressInfos.FirstOrDefault(x => x.PaymentAddressType == PaymentAddressType.Shipping);
            Address shippingAddress = new Address();
            shippingAddress.ContactName = addressInfosShipping.ContactName;//Zorunlu
            shippingAddress.City = addressInfosShipping.City;//Zorunlu
            shippingAddress.Country = addressInfosShipping.Country;//Zorunlu
            shippingAddress.Description = "asd";
            shippingAddress.ZipCode = "34732";
            request.ShippingAddress = shippingAddress;//Zorunlu

            AddressInfos addressInfosBilling = store.AddressInfos.FirstOrDefault(x => x.PaymentAddressType == PaymentAddressType.Billing);
            Address billingAddress = new Address();
            billingAddress.ContactName = addressInfosBilling.ContactName;//Zorunlu
            billingAddress.City = addressInfosBilling.City;//Zorunlu
            billingAddress.Country = addressInfosBilling.Country;//Zorunlu
            billingAddress.Description = "asd";
            billingAddress.ZipCode = "34732";
            request.BillingAddress = billingAddress;//Zorunlu

            List<BasketItem> basketItems = new List<BasketItem>();
            store.BasketInfos.ForEach(x =>
            {
                BasketItem BasketItem = new BasketItem();
                BasketItem.Id = x.Id;//Zorunlu
                BasketItem.Name = x.Name;//Zorunlu
                BasketItem.Category1 = x.Category;//Zorunlu
                BasketItem.Category2 = "";
                BasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                BasketItem.Price = x.Price;//Zorunlu
                basketItems.Add(BasketItem);
            });
            request.BasketItems = basketItems;
            ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);
            if (threedsInitialize.Status == "success")
            {
                return threedsInitialize.HtmlContent;
            }
            else
            {
                return threedsInitialize.Status = "failure";

            }
        }

        public string Refund()
        {
            throw new NotImplementedException();
        }
    }
}
