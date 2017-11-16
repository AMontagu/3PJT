using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TrainCommander.API.Models;
using System.Globalization;

namespace TrainCommander.API.Providers

{
    class JourneyProvider

    {
        public static List<SuperTrip> GetMultiplesJourneys(string departure, string arrival, string date, bool representarrival)
        {

            List<SuperTrip> listOfJourneys = new List<SuperTrip>();

            string departureId = GetStationSNCFId(departure);
            string arrivalId = GetStationSNCFId(arrival);

            for (int i = 0; i < 4; i++)
            {
                SuperTrip superTrip = GetJourney(departureId, arrivalId, date, representarrival);
                listOfJourneys.Add(superTrip);
                if (!representarrival)
                {
                    date = listOfJourneys[i].departure_date.ToString("yyyyMMddThhmmss").Remove(listOfJourneys[i].departure_date.ToString("yyyyMMddThhmmss").Length - 1) + 1;
                }
                else
                {
                    date = listOfJourneys[i].arrival_date.ToString("yyyyMMddThhmmss").Remove(listOfJourneys[i].arrival_date.ToString("yyyyMMddThhmmss").Length - 1) + 1;
                }

            }

            return listOfJourneys;
        }

        public static SuperTrip GetJourney(string departureId, string arrivalId, string datetime, bool representarrival)
        {

            string sURL = "https://api.sncf.com/v1/coverage/sncf/journeys?from=" + departureId + "&to=" + arrivalId + "&datetime=" + datetime;
            if (representarrival) sURL = sURL + "&datetime_represents=arrival";


            WebRequest wrGETURL = WebRequest.Create(sURL);

            wrGETURL.Headers.Add("Authorization", "4a24c48a-ba0f-4b70-aead-5137ede74ffd");

            HttpWebRequest request = wrGETURL as HttpWebRequest;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;

            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                int i = 0;

                SNCFTrip trip = new SNCFTrip();
                List<SNCFTrip> tripList = new List<SNCFTrip>();

                Place stop = new Place();
                List<Place> stoplist = new List<Place>();

                string jsonText = reader.ReadToEnd();

                JObject journeyData = JObject.Parse(jsonText);

                List<JToken> journeys = journeyData["journeys"].ToList();

                foreach (JToken journey in journeys)
                {
                    if (i != 0) break;
                    journeyData = JObject.Parse(journey.ToString());

                    List<JToken> sections = journeyData["sections"].ToList();

                    foreach (JToken section in sections)
                    {

                        trip = JsonConvert.DeserializeObject<SNCFTrip>(section.ToString());

                        if ((JObject)section["from"] != null)
                        {

                            journeyData = JObject.Parse(section.ToString());

                            JObject deps = section["from"].Value<JObject>();

                            foreach (JProperty prop in deps.Properties())
                            {
                                if (prop.Name == "name") trip.departure = prop.First.ToString().Split(' ')[0];
                            }

                            JObject arivs = section["to"].Value<JObject>();

                            foreach (JProperty prop in arivs.Properties())
                            {
                                if (prop.Name == "name") trip.arrival = prop.First.ToString().Split(' ')[0];
                            }

                            trip.price = trip.duration / 300;

                            tripList.Add(trip);
                        }
                    }
                    i++;
                }


                tripList.RemoveAt(0);
                tripList.RemoveAt(tripList.Count - 1);
                for (int count = tripList.Count - 1; count >= 0; count--)
                {
                    if (tripList[count].arrival == tripList[count].departure) tripList.RemoveAt(count);
                }
                List<Trip> trips = ConvertToListTrip(tripList);
                SuperTrip superTrip = GetSuperTrip(trips);
                AddSuperTrip(superTrip);
                AddTrips(trips);
                return superTrip;
            }

        }



        public static string GetIDFromPlace(string name)
        {
            //Check if already inside database and abort API call
            //if not, add it at the end

            string sURL = "https://api.sncf.com/v1/coverage/sncf/places?q=" + name;

            WebRequest wrGETURL = WebRequest.Create(sURL);

            wrGETURL.Headers.Add("Authorization", "4a24c48a-ba0f-4b70-aead-5137ede74ffd");

            HttpWebRequest request = wrGETURL as HttpWebRequest;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;

            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string jsonText = reader.ReadToEnd();

                JObject placesData = JObject.Parse(jsonText);

                IList<JToken> results = placesData["places"].Children().ToList();

                IList<Place> searchResults = new List<Place>();

                foreach (JToken result in results)
                {
                    Place searchResult = JsonConvert.DeserializeObject<Place>(result.ToString());
                    searchResults.Add(searchResult);
                }

                return searchResults[0].id;
            }

        }


        public static string GetStationSNCFId(string name)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            string idSncf;
            var search = (from o in db.Station
                          where o.name.ToLower() == name.ToLower()
                          select o).FirstOrDefault();
            if (search.id_sncf == null)
            {
                idSncf = GetIDFromPlace(name);
                search.id_sncf = idSncf;
                db.SaveChanges();
            }
            return (search.id_sncf);
        }

        public static List<Trip> ConvertToListTrip(List<SNCFTrip> sncfTrips)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            List<Trip> trips = new List<Trip>();
            foreach (SNCFTrip sncfTrip in sncfTrips)
            {
                Station departure = GetStationBySNCFName(sncfTrip.departure);
                Station arrival = GetStationBySNCFName(sncfTrip.arrival);
                Trip trip = new Trip
                {
                    id_departure_station = departure.Id,
                    id_arrival_station = arrival.Id,
                    departure_date = DateTime.ParseExact(sncfTrip.departure_date_time, "yyyyMMddTHHmmss",CultureInfo.InvariantCulture),
                    arrival_date = DateTime.ParseExact(sncfTrip.arrival_date_time, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture),
                    price = sncfTrip.price
                };
                trips.Add(trip);
            }
            return trips;
        }

        public static SuperTrip GetSuperTrip(List<Trip> trips)
        {
            Trip firstTrip = trips.First();
            Trip lastTrip = trips.Last();
            return new SuperTrip
            {
                id_departure_station = firstTrip.id_departure_station,
                id_arrival_station = lastTrip.id_arrival_station,
                price = GetSuperPrice(trips),
                departure_date = firstTrip.departure_date,
                arrival_date = lastTrip.arrival_date
            };
        }

        public static decimal GetSuperPrice(List<Trip> trips)
        {
            decimal price = 0;
            foreach (Trip trip in trips)
            {
                price += trip.price;
            }
            return price;
        }

        private static void AddSuperTrip(SuperTrip superTrip)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            if (!db.SuperTrip.Any(o =>
                 o.id_departure_station == superTrip.id_departure_station &&
                 o.id_arrival_station == superTrip.id_arrival_station &&
                 o.departure_date == superTrip.departure_date))

                db.SuperTrip.Add(superTrip);
            db.SaveChanges();
        }

        private static void AddTrips(List<Trip> trips)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            DateTime departure_date = trips.First().departure_date;
            DateTime arrival_date = trips.Last().arrival_date;
            foreach (Trip trip in trips)
            {
                if (!db.Trip.Any(o =>
                    o.id_departure_station == trip.id_departure_station &&
                    o.id_arrival_station == trip.id_arrival_station &&
                    o.departure_date == trip.departure_date))
                {
                    SuperTrip supertrip = (from o in db.SuperTrip
                                           where o.departure_date == departure_date && o.arrival_date == arrival_date
                                           select o).FirstOrDefault();
                    trip.SuperTrip.Add(supertrip);
                    trip.Train = AddTrain();
                    db.Trip.Add(trip);
                    db.SaveChanges();
                }
            }
        }

        public static Station GetStationBySNCFName(string name)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            Station station = new Station();
            var query = (from o in db.Station
                         where o.name == name
                         select o).FirstOrDefault();
            if (query == null)
            {
                station.name = name;
                db.Station.Add(station);
                db.SaveChanges();
            }
            else station = query;
            return station;
        }

        public static Train AddTrain()
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            Train train = new Train { is_double = false, seat_number = 350, remaining_seat = 350 };
            db.Train.Add(train);
            return train;
        }
    }
}

