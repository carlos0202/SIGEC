using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class HistAppointment
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public int createUser { get; set; }
        public int patientID { get; set; }
        public System.DateTime appointmentDate { get; set; }
        public string finalStatus { get; set; }
        public int doctorID { get; set; }
    }
}
