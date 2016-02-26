using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class InheritedDiseas
    {
        public int ID { get; set; }
        public string disease { get; set; }
        public string familyMember { get; set; }
        public int medicalHistoryID { get; set; }
        public virtual MedicalHistory MedicalHistory { get; set; }
    }
}
