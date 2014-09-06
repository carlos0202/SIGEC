using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Prescription
    {
        public Prescription()
        {
            this.Prescriptions_Medicines = new List<Prescriptions_Medicines>();
        }

        public int ID { get; set; }
        public string notes { get; set; }
        public int patientID { get; set; }
        public int consultationID { get; set; }
        public virtual Consultation Consultation { get; set; }
        public virtual ICollection<Prescriptions_Medicines> Prescriptions_Medicines { get; set; }
    }
}
