using DataAnnotationsExtensions;
using Foolproof;
using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    public class ConsultationPaymentViewModel
    {
        [Required]
        [Display(Name = "lblPatient", ResourceType = typeof(Language))]
        public int patientID { get; set; }
        [Required]
        [Display(Name = "lblDoctor", ResourceType = typeof(Language))]
        public int doctorID { get; set; }
        [Display(Name = "lblInsurer", ResourceType = typeof(Language))]
        public int insurerID { get; set; }
        [Numeric]
        [Display(Name = "lblPrice", ResourceType = typeof(Language))]
        public decimal price { get; set; }
        [Numeric]
        [LessThanOrEqualTo("price", ErrorMessageResourceName = "lblLessThanErr", ErrorMessageResourceType = typeof(Resources.Language))]
        [Display(Name = "lblDiscount", ResourceType = typeof(Language))]
        public decimal discount { get; set; }
        [Display(Name = "lblInsurerCoberture", ResourceType = typeof(Language))]
        [LessThanOrEqualTo("price", ErrorMessageResourceName = "lblLessThanErr", ErrorMessageResourceType = typeof(Resources.Language))]
        [Numeric]
        public decimal insurer { get; set; }
        [Display(Name = "lblPaymentForm", ResourceType = typeof(Language))]
        [Required]
        public string paymentForm { get; set; }
    }
}