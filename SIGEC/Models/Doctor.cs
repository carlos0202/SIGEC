using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            this.Appointments = new List<Appointment>();
            this.Consultations = new List<Consultation>();
            this.DoctorRules = new List<DoctorRule>();
        }

        public int ID { get; set; }
        public string speciality { get; set; }
        public int userID { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<DoctorRule> DoctorRules { get; set; }
        public virtual User User { get; set; }
    }
}
