using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Consultation
    {
        public Consultation()
        {
            this.Consultations_Payment = new List<Consultations_Payment>();
            this.Diseases = new List<Disease>();
            this.Prescriptions = new List<Prescription>();
            this.Analyses = new List<Analysis>();
            this.Procedures = new List<Procedure>();
            this.Studies = new List<Study>();
        }

        public int ID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public int patientID { get; set; }
        public int doctorID { get; set; }
        public Nullable<int> appointmentID { get; set; }
        public bool ended { get; set; }
        public string reason { get; set; }
        public string treatment { get; set; }
        public string observations { get; set; }
        public string referredTo { get; set; }
        public virtual Appointment Appointment { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Consultations_Payment> Consultations_Payment { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<Analysis> Analyses { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<Study> Studies { get; set; }
    }
}
