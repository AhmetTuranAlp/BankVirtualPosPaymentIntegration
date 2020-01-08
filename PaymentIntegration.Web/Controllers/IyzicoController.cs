using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using PaymentIntegration.Banks.Iyzico;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegration.Web.Controllers
{
    public class IyzicoController : Controller
    {
        // GET: Iyzico
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentTypeSave()
        {
            Options options = Options();

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.SUBSCRIPTION.ToString();
            request.CallbackUrl = "http://localhost:2428/Iyzico/PaymentResult";


            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "John Doe";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;


            Buyer buyer = new Buyer();
            buyer.Id = Guid.NewGuid().ToString();
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = Request.UserHostAddress;
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;


            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;


            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);

            if (threedsInitialize.Status == "success")
            {
                return Content(threedsInitialize.HtmlContent);
            }
            else
            {
                Payment payment = new Payment();
                payment.Status = "failure";
                return RedirectToAction("PaymentResult", payment);
            }
        }

        [HttpPost]
        public ActionResult PaymentResult(CheckoutForm checkoutForm1)
        {
            if (checkoutForm1.Status == "success")
            {
                PaymentResultVM returnResult = new PaymentResultVM();
                Options options = Options();
                RetrievePaymentRequest request = new RetrievePaymentRequest();
                request.Locale = Locale.TR.ToString();
                request.ConversationId = checkoutForm1.ConversationId;// "123456789";
                request.PaymentId = checkoutForm1.PaymentId;
                request.PaymentConversationId = checkoutForm1.ConversationId; //"123456789";

                Payment payment = Payment.Retrieve(request, options);
                returnResult.PaymentId = payment.PaymentId;

                if (payment.Status == "success")
                {
                    returnResult.ReturnStatus = "success";
                }
                return View(returnResult);
            }
            else
            {
                PaymentResultVM PaymentResultVM = new PaymentResultVM();
                PaymentResultVM.ReturnStatus = "failure";
                PaymentResultVM.PaymentId = checkoutForm1.PaymentId;
                return View(PaymentResultVM);
            }
        }



        public Options Options()
        {
            Options options = new Options();
            options.ApiKey = "sandbox-WfjBsjY2kNDXHnehDBtq45MBwQ7Kkyv9";
            options.SecretKey = "sandbox-30uaEn94Ma92HpfvVuaXy52aokBweJG2";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            return options;
        }
    }
}