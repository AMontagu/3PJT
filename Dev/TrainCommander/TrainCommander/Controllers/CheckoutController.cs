using Newtonsoft.Json;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TrainCommander.Models;
using TrainCommander.Paypal;

namespace TrainCommander.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }
            List<SuperTripViewModel> cart;
            if (Session["Cart"] != null)
            {
                cart = (List<SuperTripViewModel>)Session["Cart"];
            }
            else
            {
                cart = new List<SuperTripViewModel>();
            }

            ListCarViewModel model = new ListCarViewModel();
            model.superTripViewModel = cart;
            return View("Cart", model);
        }

        public ActionResult Confirmation()
        {
            ConfirmationViewModel model = new ConfirmationViewModel();
            List<SuperTripViewModel> cart;
            CoordonneesViewModel coordonneesViewModel = new CoordonneesViewModel();

            if (Session["Cart"] != null)
            {
                cart = (List<SuperTripViewModel>)Session["Cart"];
                foreach (var trip in cart)
                {
                    model.TotalPrice += trip.Price * trip.Quantity;
                }
            }
            else
            {
                ListCarViewModel modelEmpty = new ListCarViewModel();
                cart = new List<SuperTripViewModel>();
                modelEmpty.superTripViewModel = cart;
                return View("Cart", modelEmpty);
            }

            if (Request.IsAuthenticated)
            {
                coordonneesViewModel.Email = User.Identity.Name;
            }

            model.superTripViewModel = cart;
            model.coordonneesViewModel = coordonneesViewModel;
            Session["TotalPrice"] = model.TotalPrice;
            return View("Confirmation", model);
        }

        public ActionResult Buy(ConfirmationViewModel model)
        {
            return View("Command");
        }

        public ActionResult CreatePayment(ConfirmationViewModel model)
        {
            //need model state valid + redirect to action if error
            Session["Command"] = model;
            var payment = PayPalPaymentService.CreatePayment(GetBaseUrl(), "sale", (double)Session["TotalPrice"]);
            return Redirect(payment.GetApprovalUrl());

        }

        public ActionResult PaymentCancelled()
        {
            Session["Command"] = null;
            TempData["Error"] = @langage.errorPayment;
            return RedirectToAction("Cart");
        }

        public ActionResult PaymentSuccessful(string paymentId, string token, string PayerID)
        {
            // Execute Payment
            var payment = PayPalPaymentService.ExecutePayment(paymentId, PayerID);
            Session["Cart"] = null;

            ConfirmationViewModel confirmModel = (ConfirmationViewModel)Session["Command"];

            var toAddress = confirmModel.coordonneesViewModel.Email;
            var fromAddress = "ticket@train-commander.fr";
            var subject = "Your Ticket";
            var message = new StringBuilder();
            int j = 0;
            foreach (var trip in confirmModel.superTripViewModel) //make disctionnary trip, people
            {
                for (var i = 0; i < trip.Quantity; i++)
                {
                    message.Append("Ticket for trip from : " + trip.DepartureStation + " to " + trip.ArrivalStation + "\n");
                    message.Append("Departure date : " + trip.DepartureDate + " Arrival date " + trip.ArrivalDate + "\n");
                    message.Append("Ticket For : " + confirmModel.personneViewModel[j + i].FirstName + confirmModel.personneViewModel[j + i].LastName + "\n");

                }
                j++;
            }

            //start email Thread
            var tEmail = new Thread(() => SendEmail(toAddress, fromAddress, subject, message.ToString()));
            tEmail.Start();

            //TODO mail success + save database
            return RedirectToAction("Buy");
        }

        public string GetBaseUrl()
        {
            return Request.Url.Scheme + "://" + Request.Url.Host + ":44387";
        }

        public ActionResult AddToCart()
        {
            List<SuperTripViewModel> cart;
            if (Session["Cart"] != null)
            {
                cart = (List<SuperTripViewModel>)Session["Cart"];
            }
            else
            {
                cart = new List<SuperTripViewModel>();
            }
            int index = int.Parse(Request.QueryString["index"]);
            List<SuperTripViewModel> listPropose = (List<SuperTripViewModel>)Session["listProposition"];
            if (listPropose != null)
            {
                SuperTripViewModel choosenTrip = listPropose[index];
                Session["listProposition"] = null;
                cart.Add(choosenTrip);
            }
            Session["Cart"] = cart;
            ListCarViewModel model = new ListCarViewModel();
            model.superTripViewModel = cart;
            return View("Cart", model);
        }

        public ActionResult AddToCartjson(int index)
        {
            List<SuperTripViewModel> cart;
            if (Session["Cart"] != null)
            {
                cart = (List<SuperTripViewModel>)Session["Cart"];
            }
            else
            {
                cart = new List<SuperTripViewModel>();
            }
            List<SuperTripViewModel> listPropose = (List<SuperTripViewModel>)Session["listProposition"];
            if (listPropose != null)
            {
                SuperTripViewModel choosenTrip = listPropose[index];
                cart.Add(choosenTrip);
            }
            Session["Cart"] = cart;
            return Json(new { result = "ok" });
        }

        public ActionResult DeleteAll()
        {
            Session["Cart"] = null;
            return RedirectToAction("Cart", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteOne(int index)
        {
            List<SuperTripViewModel> cart = (List<SuperTripViewModel>)Session["Cart"];
            cart.RemoveAt(index);
            Session["Cart"] = cart;
            return Json(new { result = "ok" });
        }

        public ActionResult SaveCart()
        {
            if (Request.IsAuthenticated)
            {
                List<SuperTripViewModel> cart = (List<SuperTripViewModel>)Session["Cart"];
                string jsonString = JsonConvert.SerializeObject(cart);
                System.Diagnostics.Debug.WriteLine(jsonString);
                //call api here
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult AddQuantity(int index)
        {
            List<SuperTripViewModel> cart = (List<SuperTripViewModel>)Session["Cart"];
            cart[index].Quantity += 1;
            Session["Cart"] = cart;
            return Json(new { result = "ok" });
        }

        [HttpPost]
        public ActionResult RemoveQuantity(int index)
        {
            List<SuperTripViewModel> cart = (List<SuperTripViewModel>)Session["Cart"];
            cart[index].Quantity -= 1;
            Session["Cart"] = cart;
            return Json(new { result = "ok" });
        }

        [HttpPost]
        public ActionResult ChangeQuantity(int index, int quantity)
        {
            List<SuperTripViewModel> cart = (List<SuperTripViewModel>)Session["Cart"];
            if (quantity > 0)
            {
                cart[index].Quantity = quantity;
            }
            else
            {
                if (cart.Count != 0)
                {
                    cart.RemoveAt(index);
                }
                else
                {
                    cart = null;
                }
            }
            Session["Cart"] = cart;
            return Json(new { result = "ok" });
        }

        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "username@train-commander.fr";
                    const string password = "passwordMail2016!";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient(
                                                         "smtp.mail.train-commander.fr", 465))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message to" + t.FailedRecipient.ToString());
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
    }
}