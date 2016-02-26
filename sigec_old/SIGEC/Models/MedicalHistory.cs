using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class MedicalHistory
    {
        public MedicalHistory()
        {
            this.InheritedDiseases = new List<InheritedDiseas>();
        }

        public int ID { get; set; }
        public bool alcohol { get; set; }
        public bool CCF { get; set; }
        public string childhoodIllnesses { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string currentCondition { get; set; }
        public bool diabetes { get; set; }
        public string drugs { get; set; }
        public string gastrointestinal { get; set; }
        public string GDM { get; set; }
        public bool HTN { get; set; }
        public string menarche { get; set; }
        public string menstruation { get; set; }
        public string naturalMedicines { get; set; }
        public string others { get; set; }
        public string preclampsia { get; set; }
        public string respiratory { get; set; }
        public string skinAppendages { get; set; }
        public string surgeries { get; set; }
        public bool tobacco { get; set; }
        public string transfusions { get; set; }
        public string urinaryReproductive { get; set; }
        public int patientID { get; set; }
        public bool completed { get; set; }
        public string allergies { get; set; }
        public string psychiatric { get; set; }
        public virtual ICollection<InheritedDiseas> InheritedDiseases { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
