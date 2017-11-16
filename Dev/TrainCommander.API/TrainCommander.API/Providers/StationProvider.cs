using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;

namespace TrainCommander.API.Providers
{
    public class StationProvider
    {
        public static int GetStationIdByName(string name)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            int returnVar = 0;
            try
            {
                returnVar = (from o in db.Station
                              where o.name == name
                              select o.Id).First();
            }
            catch(EntityCommandCompilationException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return returnVar;
            }
            return returnVar;
        }

        public static string GetStationNameById(int id)
        {
            TrainCommanderEntities db = new TrainCommanderEntities();
            return (from o in db.Station
                    where o.Id == id
                    select o.name).First();
        }
    }
}