using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGEC.CustomCode
{
    public class IsViewAttribute : Attribute 
    {
        public String ViewName;

        public IsViewAttribute()
        {
            this.ViewName = "";
        }

        public IsViewAttribute(String ViewName)
        {
            this.ViewName = ViewName;
        }
    }
}