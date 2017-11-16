using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrainCommander.API.Models
{
    [DataContract]
    public class SuperTripViewModel
    {
        [DataMember]
        public int SupertripId { get; set; }

        [DataMember]
        public string DepartureStation { get; set; }

        [DataMember]
        public string ArrivalStation { get; set; }

        [DataMember]
        public bool IsDirect { get; set; }

        [DataMember]
        public DateTime DepartureDate { get; set; }

        [DataMember]
        public DateTime ArrivalDate { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}