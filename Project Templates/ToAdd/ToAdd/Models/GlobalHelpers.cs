using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGII_PFD.Models
{
    public static class GlobalHelpers
    {
        public static string GetCurrentUserName<T>(this T caller)
        {
            return HttpContext.Current.User.Identity.Name;
        }



    }
}