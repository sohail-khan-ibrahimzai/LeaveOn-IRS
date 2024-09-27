using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveON.Helpers
{
  public class GreeceTimeZoneHelper
  {
    public static TimeZoneInfo GetDefaultGreeceTimeZone()
    {
      string timeZoneId = ConfigurationManager.AppSettings["DefaultTimeZoneGTB"];
      return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
  }
}
