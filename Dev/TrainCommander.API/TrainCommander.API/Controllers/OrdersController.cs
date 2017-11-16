using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TrainCommander.API;
using TrainCommander.API.Models;
using TrainCommander.API.Providers;

namespace TrainCommander.API.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private TrainCommanderEntities db = new TrainCommanderEntities();

        // GET: api/Orders/5
        
        [ResponseType(typeof(Orders))]
        public IHttpActionResult GetOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [Route("GetSavedCart")]
        [Authorize]
        [ResponseType(typeof(SuperTripViewModel))]
        public IHttpActionResult GetSavedCart(int id)
        {
            SuperTripViewModel superTripVM = new SuperTripViewModel();
            string user_id = User.Identity.GetUserId();
            AspNetUsers user = (from o in db.AspNetUsers
                                where o.Id == user_id
                                select o).FirstOrDefault();
            Orders order = (from o in db.Orders
                            where o.AspNetUsers == user && o.status == 0
                            select o).FirstOrDefault();
            superTripVM = ConvertToSuperTripViewModel(order);
            return Ok(superTripVM);
        }

        // POST: api/Orders
        [Route("SaveCart")]
        [Authorize]
        [ResponseType(typeof(SuperTripViewModel))]
        public IHttpActionResult SaveCart(SuperTripViewModel superTripVM)
        {
            string user_id = User.Identity.GetUserId();
            AspNetUsers user = (from o in db.AspNetUsers
                                where o.Id == user_id
                                select o).FirstOrDefault();
            Orders ordertodelete = (from o in db.Orders
                            where o.AspNetUsers == user && o.status == 0
                            select o).FirstOrDefault();
            if (ordertodelete != null)
                db.Orders.Remove(ordertodelete);
            Orders order = new Orders { id_supertrip = superTripVM.SupertripId, id_user = user_id, quantity = superTripVM.Quantity };
            db.Orders.Add(order);
            db.SaveChanges();
            return Ok(superTripVM);
        }

        [Route("GetOrders")]
        [Authorize]
        [ResponseType(typeof(List<SuperTripViewModel>))]
        public IHttpActionResult GetOrders()
        {
            string user_id = User.Identity.GetUserId();
            List<SuperTripViewModel> superTripVMs = new List<SuperTripViewModel>();
            List<Orders> orders = new List<Orders>();
            AspNetUsers user = (from o in db.AspNetUsers
                                where o.Id == user_id
                                select o).FirstOrDefault();
            var query = from o in db.Orders
                            where o.AspNetUsers == user && o.status == 1
                            select o;
            if (query.Count() != 0 || query != null)
                orders = query.ToList<Orders>();
            foreach(Orders order in orders)
            {
                superTripVMs.Add(ConvertToSuperTripViewModel(order));
            }
            return Ok(superTripVMs);
        }

        [Route("ValidateOrder")]
        [Authorize]
        [ResponseType(typeof(SuperTripViewModel))]
        public IHttpActionResult ValidateOrder(SuperTripViewModel superTripVM)
        {
            string user_id = User.Identity.GetUserId();
            AspNetUsers user = (from o in db.AspNetUsers
                                where o.Id == user_id
                                select o).FirstOrDefault();
            Orders ordertovalidate = (from o in db.Orders
                                    where o.AspNetUsers == user && o.status == 0
                                    select o).FirstOrDefault();
            if (ordertovalidate != null)
                db.Orders.Remove(ordertovalidate);
            ordertovalidate.status = 1;
            db.Orders.Add(ordertovalidate);
            db.SaveChanges();
            return Ok(superTripVM);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public SuperTripViewModel ConvertToSuperTripViewModel(Orders order)
        {
            SuperTripViewModel superTripVM = new SuperTripViewModel();
            SuperTrip supertrip = order.SuperTrip;
            superTripVM.SupertripId = supertrip.id;
            superTripVM.DepartureStation = StationProvider.GetStationNameById(supertrip.id_departure_station);
            superTripVM.DepartureDate = supertrip.departure_date;
            superTripVM.ArrivalStation = StationProvider.GetStationNameById(supertrip.id_arrival_station);
            superTripVM.ArrivalDate = supertrip.arrival_date;
            superTripVM.IsDirect = IsDirect(supertrip);
            superTripVM.Price = Convert.ToDouble(supertrip.price);
            superTripVM.Quantity = order.quantity;

            return superTripVM;
        }

        public bool IsDirect(SuperTrip supertrip)
        {
            if (supertrip.Trip.Count > 1)
                return false;
            else return true;
        }
    }
}