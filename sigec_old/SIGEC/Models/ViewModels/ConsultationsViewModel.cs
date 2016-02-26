using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    public class ConsultationsViewModel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "lblPatient", ResourceType = typeof(Language))]
        public int patientID { get; set; }
        [Required]
        [Display(Name = "lblDoctor", ResourceType = typeof(Language))]
        public int doctorID { get; set; }
        public bool ended { get; set; }
        [Display(Name = "lblConsultReason", ResourceType = typeof(Language))]
        public string reason { get; set; }
        [Display(Name = "lblTreatment", ResourceType = typeof(Language))]
        public string treatment { get; set; }
        [Display(Name = "lblObservations", ResourceType = typeof(Language))]
        public string comments { get; set; }
        [Display(Name = "lblDiagnostic", ResourceType = typeof(Language))]
        public ICollection<int> diagnosticID { get; set; }
        [Display(Name = "lblAnalysisIndications", ResourceType = typeof(Language))]
        public ICollection<int> analysisID { get; set; }
        [Display(Name = "lblStudiesIndications", ResourceType = typeof(Language))]
        public ICollection<int> studyID { get; set; }
        [Display(Name = "lblProceduresIndications", ResourceType = typeof(Language))]
        public ICollection<int> procedureID { get; set; }
        public virtual Patient Patient { get; set; }
        [Display(Name = "lblReferredTo", ResourceType = typeof(Language))]
        public string referredTo { get; set; }
        [Display(Name = "lblNextAppointment", ResourceType = typeof(Language))]
        [System.Web.Mvc.Remote("CheckNextAppointment", "Consultations",  AdditionalFields="ID,patientID,doctorID")]
        public Nullable<DateTime> nextAppointment { get; set; }
    }
}