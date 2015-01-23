using DGII_PFD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CommonTasksLib.Data;
using Oracle.ManagedDataAccess.Client;

namespace DGII_PFD.Helpers
{
    public static class CustomHelpers
    {
        public static MvcHtmlString CustomCheck<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            // get the metdata
            ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            //var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            // get value
            var value = Convert.ToBoolean(fieldmetadata.Model);
            var label = new TagBuilder("label");
            label.Attributes.Add("style", "padding-left: 0px;");
            var hiddenCheck = new TagBuilder("input");
            hiddenCheck.Attributes.Add("type", "hidden");
            hiddenCheck.Attributes.Add("name", fieldName);
            hiddenCheck.Attributes.Add("value", fieldmetadata.Model.ToString());
            //<input name="Parametros[0].Required" type="hidden" value="false">
            if (value)
            {
                TagBuilder tagSpan = new TagBuilder("span");
                tagSpan.AddCssClass("label label-warning");
                tagSpan.Attributes.Add("style", "font-size: 15px");
                tagSpan.SetInnerText("Campo obligatorio");
                label.InnerHtml = tagSpan.ToString(TagRenderMode.Normal);
                label.InnerHtml += hiddenCheck.ToString(TagRenderMode.Normal);
                return MvcHtmlString.Create(label.ToString(TagRenderMode.Normal));
            }
            return MvcHtmlString.Create(hiddenCheck.ToString(TagRenderMode.Normal));

        }

        public static MvcHtmlString CustomEditorFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int tipo, bool requerido, IDictionary<string, object> htmlAttributes = null)
        {
            // get the metdata
            ModelMetadata fieldmetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            // get the field name
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            // get full field (template may have prefix)
            //var fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            // get value
            var value = fieldmetadata.Model;


            //Textbox Control
            var input = new TagBuilder("input");
            input.AddCssClass("form-control");
            input.Attributes.Add("name", fieldName);
            //textbox.Attributes.Add("value", value.ToString());
            input.GenerateId(fieldName);
            if (requerido)
            {
                //textbox.Attributes.Add("required",requerido.ToString());
                input.AddCssClass("requiredValidation");
            }

            //Retorna un input dependiendo del tipo de dato

            DataTypes itemType = (DataTypes)tipo;
            switch (itemType)
            {
                case DataTypes.Caracter:
                    input.Attributes.Add("type", "text");
                    break;
                case DataTypes.Texto:
                    input.Attributes.Add("type", "text");
                    break;
                case DataTypes.Entero:
                    input.Attributes.Add("type", "number");
                    input.AddCssClass("numberValidation");
                    break;
                case DataTypes.Entero_Largo:
                    input.Attributes.Add("type", "number");
                    input.AddCssClass("numberValidation");
                    break;
                case DataTypes.Entero_Corto:
                    input.Attributes.Add("type", "number");
                    input.AddCssClass("numberValidation");
                    break;
                case DataTypes.Decimal:
                    input.Attributes.Add("type", "number");
                    input.AddCssClass("numberValidation");
                    break;
                case DataTypes.Flotante:
                    input.Attributes.Add("type", "number");
                    input.AddCssClass("numberValidation");
                    break;
                case DataTypes.Fecha:
                    input.Attributes.Add("type", "text");
                    input.AddCssClass("datepicker dateValidation");
                    break;
                case DataTypes.Hora:
                    input.Attributes.Add("type", "text");
                    break;
                case DataTypes.Byte:
                    input.Attributes.Add("type", "text");
                    break;
                case DataTypes.Objeto:
                    input.Attributes.Add("type", "file");
                    var span = new TagBuilder("span");
                    span.AddCssClass("input-group-btn");
                    span.InnerHtml = "<span class='btn btn-primary btn-file'>Elejir Archivo " + input.ToString(TagRenderMode.Normal) + " </span>";
                    var fileInfoTextbox = new TagBuilder("input");
                    fileInfoTextbox.Attributes.Add("type", "text");
                    fileInfoTextbox.Attributes.Add("readonly", "true");
                    fileInfoTextbox.AddCssClass("form-control");

                    var divContainer = new TagBuilder("div");
                    divContainer.AddCssClass("input-group");
                    divContainer.InnerHtml = span.ToString(TagRenderMode.Normal) + fileInfoTextbox.ToString(TagRenderMode.Normal) + "<span class='input-group-btn btnClearInput'> <button class='btn btn-default clearFileInput' type='button'>Resetear</button></span>";
                    return MvcHtmlString.Create(divContainer.ToString(TagRenderMode.Normal));
                //break;
                default:
                    input.Attributes.Add("type", "text");
                    break;
            }

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CustomLabel<TModel>(this HtmlHelper<TModel> html, string forName, string labelText, IDictionary<string, object> htmlAttributes = null)
        {
            var tag = new TagBuilder("label");
            tag.AddCssClass("control-label");
            tag.Attributes.Add("for", forName);
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(htmlAttributes);
            }
            tag.SetInnerText(labelText);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString CustomBoolLabel<TModel>(this HtmlHelper<TModel> html, short requerido)
        {
            string response = requerido == 1 ? "SI" : "NO";

            return MvcHtmlString.Create(response);
        }

        public static MvcHtmlString CustomTypeLabel<TModel>(this HtmlHelper<TModel> html, decimal tipo)
        {
            string response = "";
            int enumType = Convert.ToInt32(tipo);
            DataTypes itemType = (DataTypes)enumType;
            response = itemType.ToString().Replace("_", " ");
            return MvcHtmlString.Create(response);
        }

        public static MvcHtmlString SummaryFormatter(object e)
        {
            var errors = e as IEnumerable<ModelError>;
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"alert alert-dismissable alert-danger\">");
            builder.Append("<button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button>");
            builder.Append("<ul>");
            foreach (var error in errors)
            {
                builder.Append("<li>");
                builder.Append(error.ErrorMessage.Encode());
                builder.AppendLine("</li>");
            }
            builder.Append("</ul>");
            builder.Append("</div>");

            return MvcHtmlString.Create(builder.ToString());
        }

        public static void WriteInformation(string Info)
        {
            var root = HttpContext.Current.Server.MapPath("~");
            var f = string.Format(@"{0}\debug.txt", root);
            var file = new System.IO.StreamWriter(f, true);
            file.WriteLine(Info);
            file.Close();
        }
    }

    public class DAO : CommonTasksLib.Data.ADOExtensions.GenericDAO<OracleCommand, OracleConnection, OracleDataAdapter>
    {
        public DAO(string ConnString, CommonTasksLib.Data.ADOExtensions.InstanceType instance)
            : base(ConnString, instance) { }

        public DAO(string ConnString)
            : base(ConnString) { }
    }
}