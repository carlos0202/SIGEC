using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Helpers
{
    public abstract class ViewBase<TModel> : System.Web.Mvc.WebViewPage<TModel> where TModel : class
    {
        public Func<object, MvcHtmlString> del
        {
            get { return FormaterCustom; }
        }
        public MvcHtmlString FormaterCustom(object e)
        {
            var errors = e as ModelErrorCollection;
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"alert alert-dismissable alert-danger\">");
            builder.Append("<button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button>");
            builder.Append("<ul>");
            foreach (var error in errors)
            {
                builder.Append("<li>");
                builder.Append(this.Html.Encode(error.ErrorMessage));
                builder.AppendLine("</li>");
            }
            builder.Append("</ul>");
            builder.Append("</div>");

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}