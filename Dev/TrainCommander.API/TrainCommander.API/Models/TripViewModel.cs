using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrainCommander.API.Models
{
    [DataContract]
    public class TripViewModel
    {

        [DataMember]
        public string DepartureStation { get; set; }

        [DataMember]
        public string ArrivalStation { get; set; }

        [DataMember]
        public DateTime DepartureDate { get; set; }

        [DataMember]
        public DateTime ArrivalDate { get; set; }

    }
}