using System.Web;
using System.Web.Mvc;
using TrainCommander.Classes;

namespace TrainCommander
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LocalizationAttribute("fr"), 0);
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
