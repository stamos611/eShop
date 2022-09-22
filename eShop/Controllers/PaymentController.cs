using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eShop.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult PaymentWithPaypal()
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string PayerId = Request.Params["PayerId"];
                if (string.IsNullOrEmpty(PayerId))
                {
                    string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority
                        + "PaymentWithPaypal/PaymentWithPaypal?";
                    var Guid = Convert.ToString((new Random()).Next(1000000));
                    var createPayment = this.CreatePayment(apiContext, baseUri + "guid" + Guid);
                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }

                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, PayerId, Session[guid] as string);


                    if (executedPayment.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                }
            }
            catch
            {
                return View("FailureView");
            }
            return View("SuccessView");
        }
        private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }

        private PayPal.Api.Payment payment;
        private Payment CreatePayment(APIContext apicontext,string redirectURL)
        {
            var ItemLIst = new ItemList() { items = new List<Item>() };

            if (Session["cart"] != null)
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)(Session["cart"]);
                foreach (var item in cart)
                {
                    ItemLIst.items.Add(new Item()
                    {
                        name = item.Product.ProductName.ToString(),
                        currency = "EURO",
                        price = item.Product.Price.ToString(),
                        quantity = item.Product.Quantity.ToString(),
                        sku = "sku"
                    });
                }

                var payer = new Payer() { payment_method = "paypal" };
                var redirURL = new RedirectUrls()
                {
                    cancel_url = redirectURL + "&Cancel=true",
                    return_url = redirectURL
                };
                var details = new Details()
                {
                    tax = "1",
                    shipping = "1",
                    subtotal = "1"
                };

                var amount = new Amount()
                {
                    currency = "ERUO",

                    total = Session["SesTotal"].ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Description",
                    invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemLIst

                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirURL
                };
            }
            return this.payment.Create(apicontext);

        }
    }
}