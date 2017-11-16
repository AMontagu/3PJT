using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainCommander.Models;
using Newtonsoft.Json;
using PayPal.Api;
using TrainCommander.Paypal;
using System.Text;
using System.Net;

namespace TrainCommander.Controllers
{
    public class TripController : Controller
    {
        // GET: Trip
        public ActionResult Index()
        {
            TripFormAndSuperTripViewModel model = new TripFormAndSuperTripViewModel();
            model.tripFormModel = new TripFormModel(); // maybe add session here for get last search
            return View(model);
        }

        [HttpPost]
        public ActionResult GetTrain(TripFormModel model)
        {
            List<SuperTripViewModel> result = new List<SuperTripViewModel>();
            //model.DateOneWay = DateTime.ParseExact(model.DateOneWay.ToString(), "dd/MM/yyyy", null);
            /*string[] keys = Request.Form.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(keys[i] + ": " + Request.Form[keys[i]] + "<br>");
            }*/
            if (ModelState.IsValid)
            {
                //System.Diagnostics.Debug.WriteLine("ici");
                //call API

                result = GetTripFromApi(model.DepartOrArrivalOneWay, model.DepartureStation, model.ArrivalStation, model.DateOneWay, model.HourOneWay);
                foreach (var trip in result)
                {
                    trip.isReturn = false;
                }

                if (model.TripWay == false)
                {
                    List<SuperTripViewModel> tmp = GetTripFromApi(model.DepartOrArrivalReturn, model.ArrivalStation, model.DepartureStation, (DateTime) model.DateReturn, model.HourReturn);
                    foreach(var trip in tmp)
                    {
                        trip.isReturn = true;
                        result.Add(trip);
                    }
                }
            }

            Session["listProposition"] = result;

            TripFormAndSuperTripViewModel modelReturn = new TripFormAndSuperTripViewModel();
            modelReturn.tripFormModel = model;
            modelReturn.superTripViewModel = result;


            return View("Index", modelReturn);
        }

        public List<SuperTripViewModel> GetTripFromApi(string departOrArrival, string departureStation, string arrivalStation, DateTime dateTrip, int hour)
        {
            string convert_de_porc;

            if (departOrArrival == "departTo")
            {
                convert_de_porc = "false";
            }
            else
            {
                convert_de_porc = "true";
            }
            TimeSpan ts = new TimeSpan(hour, 00, 0);
            DateTime dateWithHour = dateTrip.Date + ts;

            string date = dateWithHour.ToString("yyyyMMddHHmmss");
            date = date.Replace(@"/", string.Empty);
            date = date.Replace(@" ", string.Empty);
            date = date.Replace(":", string.Empty);

            string sURL = "http://localhost:51675/api/trips/GetSuperTrips/" + departureStation + "/" + arrivalStation + "/" + date + "/" + convert_de_porc;

            WebRequest wrGETURL = WebRequest.Create(sURL);

            HttpWebRequest request = wrGETURL as HttpWebRequest;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;

            string apiJson;

            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                apiJson = reader.ReadToEnd();

            }

            return JsonConvert.DeserializeObject<List<SuperTripViewModel>>(apiJson);
        }

        public ActionResult Promo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestPaypal()
        {
            var config = ConfigManager.Instance.GetProperties();

            // Use OAuthTokenCredential to request an access token from PayPal
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();

            var apiContext = new APIContext(accessToken);

            // A transaction defines the contract of a payment - what is the payment for and who is fulfilling it. 
            var transaction = new Transaction()
            {
                amount = new Amount()
                {
                    currency = "USD",
                    total = "7",
                    details = new Details()
                    {
                        shipping = "1",
                        subtotal = "5",
                        tax = "1"
                    }
                },
                description = "This is the payment transaction description.",
                item_list = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "Item Name",
                            currency = "USD",
                            price = "1",
                            quantity = "5",
                            sku = "sku"
                        }
                    }
                },
                invoice_number = new Random().Next(999999).ToString()
            };

            // A resource representing a Payer that funds a payment.
            var payer = new Payer()
            {
                payment_method = "paypal",
                payer_info = new PayerInfo
                {
                    email = "test@email.com"
                }
            };

            // A Payment resource; create one using the above types and intent as `sale` or `authorize`
            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = new List<Transaction>() { transaction }
            };

            // Create a payment using a valid APIContext
            var createdPayment = payment.Create(apiContext);

            return View("Index", "Home");
        }
    }
}