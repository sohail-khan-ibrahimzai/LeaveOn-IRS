using LeaveON.UtilityClasses;
using System.Web;
using System.Web.Mvc;

namespace LeaveON
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {

      filters.Add(new HandleErrorAttribute());
      //filters.Add(new LoginFilter());
    }
  }
}
