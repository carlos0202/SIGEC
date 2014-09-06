using DataAnnotationsExtensions;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    public class DoctorRulesViewModel
    {
        public int ID { get; set; }
        public int doctorID { get; set; }
        [Required]
        [Display(Name="lblConsultationPrice", ResourceType=typeof(Resources.Language))]
        public decimal consultationPrice { get; set; }
        [Digits]
        [Required]
        [Display(Name = "lblMaxPatients", ResourceType = typeof(Resources.Language))]
        public int maxPatients { get; set; }
        [Required]
        [Display(Name = "lblConsultationStart", ResourceType = typeof(Resources.Language))]
        public System.TimeSpan consultationStart { get; set; }
        [Required]
        [Display(Name = "lblConsultationEnd", ResourceType = typeof(Resources.Language))]
        public System.TimeSpan consultationEnd { get; set; }
        [Required]
        [Display(Name = "lblAvailableDays", ResourceType = typeof(Resources.Language))]
        public List<int> availableDays { get; set; }
        public Doctor doctor { get; set; }
    }
}