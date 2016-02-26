using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            this.Consultations = new List<Consultation>();
        }

        public int ID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public int createUser { get; set; }
        public int patientID { get; set; }
        public System.DateTime appointmentDate { get; set; }
        public bool status { get; set; }
        public int doctorID { get; set; }
        public string finalStatus { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}
