using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DGII_PFD.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class MaxFileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int MaxFileSize;
        public MaxFileSizeAttribute(int MaxFileSize)
            : base(() => "Tamaño archivo no puede exceder {0} bytes.")
        {
            this.MaxFileSize = MaxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
            return file.ContentLength <= MaxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(MaxFileSize.ToString());
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(MaxFileSize.ToString()),
                ValidationType = "maxfilesize"
            };
            rule.ValidationParameters.Add("maxsize", MaxFileSize);

            yield return rule;
        }
    }
}