using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainCommander.API.Models
{
    class SNCFTrip
    {
        //OUI c'est pas aux normes ; mais pour parser s'plus simple vu que ça match à partir du nom des variables. Problem ?

        public string departure { get; set; }

        public string departure_date_time { get; set; }

        public string arrival { get; set; }

        public string arrival_date_time { get; set; }

        public int duration { get; set; }

        public int price { get; set; }
    }
}
