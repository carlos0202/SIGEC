using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGEC.Models.ViewModels
{
    public class MedicalHistoryViewModel
    {
        public int medicalhistoryID { get; set; }
        [Display(Name = "lblAlcohol", ResourceType = typeof(Resources.Language))]
        public bool alcohol { get; set; }
        public bool CCF { get; set; }
        [Display(Name = "lblChildhoodIllnesses", ResourceType = typeof(Resources.Language))]
        public string childhoodIllnesses { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        [Display(Name = "lblCurrentCondition", ResourceType = typeof(Resources.Language))]
        public string currentCondition { get; set; }
        public bool diabetes { get; set; }
        [Display(Name = "lblDrugs", ResourceType = typeof(Resources.Language))]
        public string drugs { get; set; }
        [Display(Name = "lblGastrointestinal", ResourceType = typeof(Resources.Language))]
        public string gastrointestinal { get; set; }
        [Display(Name = "lblGDM", ResourceType = typeof(Resources.Language))]
        public string GDM { get; set; }
        public bool HTN { get; set; }
        [Display(Name = "lblMenarche", ResourceType = typeof(Resources.Language))]
        public string menarche { get; set; }
        [Display(Name = "lblMenstruation", ResourceType = typeof(Resources.Language))]
        public string menstruation { get; set; }
        [Display(Name = "lblNaturalMedicines", ResourceType = typeof(Resources.Language))]
        public string naturalMedicines { get; set; }
        [Display(Name = "lblOthers", ResourceType = typeof(Resources.Language))]
        public string others { get; set; }
        [Display(Name = "lblPreclampsia", ResourceType = typeof(Resources.Language))]
        public string preclampsia { get; set; }
        [Display(Name = "lblRespiratory", ResourceType = typeof(Resources.Language))]
        public string respiratory { get; set; }
        [Display(Name = "lblSkinAppendages", ResourceType = typeof(Resources.Language))]
        public string skinAppendages { get; set; }
        [Display(Name = "lblSurgeries", ResourceType = typeof(Resources.Language))]
        public string surgeries { get; set; }
        [Display(Name = "lblTobacco", ResourceType = typeof(Resources.Language))]
        public bool tobacco { get; set; }
        [Display(Name = "lblTransfusions", ResourceType = typeof(Resources.Language))]
        public string transfusions { get; set; }
        [Display(Name = "lblUrinaryReproductive", ResourceType = typeof(Resources.Language))]
        public string urinaryReproductive { get; set; }
        public int patientID { get; set; }
        public virtual Patient Patient { get; set; }
        //public string[] inheritedDiseases { get; set; }
        [Display(Name="lblFamilyMember", ResourceType=typeof(Resources.Language))]
        public string familyMember { get; set; }
        [Display(Name = "lblDisease", ResourceType = typeof(Resources.Language))]
        public string disease { get; set; }
        [Display(Name = "lblAllergies", ResourceType = typeof(Resources.Language))]
        public string allergies { get; set; }
        [Display(Name = "lblPsychiatric", ResourceType = typeof(Resources.Language))]
        public string psychiatric { get; set; }
    }
}