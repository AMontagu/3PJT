using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
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
    [RoutePrefix("api/trips")]
    public class TripsController : ApiController
    {
        private TrainCommanderEntities db = new TrainCommanderEntities();

        // GET: api/Trips
        public IQueryable<Trip> GetTrip()
        {
            return db.Trip;
        }

        [Route("GetSuperTripDetail/{supertripId}")]
        [ResponseType(typeof(List<TripViewModel>))]
        public IHttpActionResult GetSuperTripDetail(int supertripId)
        {
            List<TripViewModel> tripVMs = new List<TripViewModel>();
            List<Trip> trips = (from o in db.SuperTrip
                                where o.id == supertripId
                                select o.Trip).First().ToList<Trip>();
            foreach(Trip trip in trips)
            {
                TripViewModel tripVM = new TripViewModel();
                tripVM.DepartureStation = StationProvider.GetStationNameById(trip.id_departure_station);
                tripVM.DepartureDate = trip.departure_date;
                tripVM.ArrivalStation = StationProvider.GetStationNameById(trip.id_arrival_station);
                tripVM.ArrivalDate = trip.arrival_date;
                tripVMs.Add(tripVM);
            }
            return Ok(tripVMs);
        }



        // GET: api/Trips/5
        [Route("GetSuperTrips/{departureStation}/{arrivalStation}/{stringDate}/{isArrival}")]
        [ResponseType(typeof(List<SuperTrip>))]
        public IHttpActionResult GetSuperTrips(string departureStation, string arrivalStation, string stringDate,bool isArrival)
        {
            DateTime date = DateTime.ParseExact(stringDate,"yyyyMMddHHmmss",CultureInfo.InvariantCulture);
            DateTime verifDate = date.AddHours(5);
            List<SuperTrip> superTrips = new List<SuperTrip>();
            int departure_id = StationProvider.GetStationIdByName(departureStation);
            int arrival_id = StationProvider.GetStationIdByName(arrivalStation);
            var query = from o in db.SuperTrip
                        where o.id_departure_station == departure_id && o.id_arrival_station == arrival_id && o.departure_date >= date && o.departure_date <= verifDate
                        select o;
            var limitedquery = query.Take(4);
            if (query.Count() < 4)
            {
                superTrips = JourneyProvider.GetMultiplesJourneys(departureStation, arrivalStation, date.ToString("yyyyMMddTHHmmss"), isArrival);
            }
            else superTrips = query.ToList<SuperTrip>();
            return Ok(ConvertToSuperTripViewModel(superTrips));

        }


        // POST: api/Trips
        [ResponseType(typeof(Trip))]
        public IHttpActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trip.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.id }, trip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SuperTripViewModel> ConvertToSuperTripViewModel(List<SuperTrip> supertrips)
        {
            List<SuperTripViewModel> superTripVMs = new List<SuperTripViewModel>();
            foreach(SuperTrip supertrip in supertrips)
            {
                SuperTripViewModel superTripVM = new SuperTripViewModel();
                superTripVM.SupertripId = supertrip.id;
                superTripVM.DepartureStation = StationProvider.GetStationNameById(supertrip.id_departure_station);
                superTripVM.DepartureDate = supertrip.departure_date;
                superTripVM.ArrivalStation = StationProvider.GetStationNameById(supertrip.id_arrival_station);
                superTripVM.ArrivalDate = supertrip.arrival_date;
                superTripVM.IsDirect = IsDirect(supertrip);
                superTripVM.Price = Convert.ToDouble(supertrip.price);
                superTripVM.Quantity = 1;
                superTripVMs.Add(superTripVM);
            }
            return superTripVMs;

        }

        public bool IsDirect(SuperTrip supertrip)
        {
            if (supertrip.Trip.Count > 1)
                return false;
            else return true;
        }

    }
}