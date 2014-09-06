using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Patient
    {
        public Patient()
        {
            this.Appointments = new List<Appointment>();
            this.Consultations = new List<Consultation>();
            this.Diseases = new List<Disease>();
            this.MedicalHistories = new List<MedicalHistory>();
        }

        public int ID { get; set; }
        public int createBy { get; set; }
        public int userID { get; set; }
        public Nullable<System.DateTime> lastConsultation { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
