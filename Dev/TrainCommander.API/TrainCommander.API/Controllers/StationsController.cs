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
using TrainCommander.API.Providers;

namespace TrainCommander.API.Controllers
{
    [RoutePrefix("api/stations")]
    public class StationsController : ApiController
    {
        private TrainCommanderEntities db = new TrainCommanderEntities();

        [Route("searchstations/{station}")]
        [ResponseType(typeof(String))]
        [HttpGet]
        public IHttpActionResult SearchStation(string station)
        {
            var search = from o in db.Station
                         where o.name.ToLower().Contains(station.ToLower())
                         select o.name;
            return Ok(search);
        }

        // GET: api/Stations/5
        [Route("GetStationIdSncf/{name}")]
        [ResponseType(typeof(String))]
        [HttpGet]
        public IHttpActionResult GetStationSNCFId(string name)
        {
            string idSncf;
            var search = (from o in db.Station
                         where o.name.ToLower() == name.ToLower()
                         select o).FirstOrDefault();
            if (search.id_sncf == null)
            {
                idSncf = JourneyProvider.GetIDFromPlace(name);
                search.id_sncf = idSncf;
                db.SaveChanges();
            }
            return Ok(search.id_sncf);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}