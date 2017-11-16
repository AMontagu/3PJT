
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainCommander.Models
{
    public class TripModel
    {
        public int id { get; set; }
        public int id_departure_station { get; set; }
        public int id_arrival_station { get; set; }
        public int id_train { get; set; }
        public DateTime departure_date { get; set; }
        public DateTime arrival_date { get; set; }
        public double price { get; set; }
    }

    public class TripFormModel
    {
        [Required]
        public bool TripWay { get; set; }
        [Required]
        [Display(Name = "departure", ResourceType = typeof(Resources.langage))]
        public string DepartureStation { get; set; }
        [Required]
        [Display(Name = "arrival", ResourceType = typeof(Resources.langage))]
        public string ArrivalStation { get; set; }

        [Required]
        [Display(Name = "depart", ResourceType = typeof(Resources.langage))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOneWay { get; set; }
        [Required]
        public string DepartOrArrivalOneWay { get; set; }
        [Required]
        [Display(Name = "firstName", ResourceType = typeof(Resources.langage))]
        public int HourOneWay { get; set; }

        
        [Display(Name = "returnId", ResourceType = typeof(Resources.langage))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateReturn { get; set; }
        public string DepartOrArrivalReturn { get; set; }
        public int HourReturn { get; set; }

        [Required]
        public string KindOfTrip { get; set; }
        [Display(Name = "city", ResourceType = typeof(Resources.langage))]
        public string CityThrough { get; set; }

        public string idForm { get; set; }
    }

    public class TripFormAndSuperTripViewModel
    {
        public TripFormModel tripFormModel { get; set; }
        public List<SuperTripViewModel> superTripViewModel { get; set; }
    }

    public class SuperTripViewModel
    {
        public int SupertripId { get; set; }

        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }

        public bool IsDirect { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool isReturn { get; set; }
    }

    
}