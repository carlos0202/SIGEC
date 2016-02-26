using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Disease
    {
        public int ID { get; set; }
        public string diagnoseCode { get; set; }
        public string observations { get; set; }
        public Nullable<System.DateTime> startTime { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }
        public int patientID { get; set; }
        public Nullable<int> consultationID { get; set; }
        public virtual Consultation Consultation { get; set; }
        public virtual ICD10 ICD10 { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
