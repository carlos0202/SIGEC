using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class AppointmentRule
    {
        public int ID { get; set; }
        public int doctorID { get; set; }
        public int maxPatients { get; set; }
        public System.TimeSpan consultationStart { get; set; }
        public System.TimeSpan consultationEnd { get; set; }
        public string availableDays { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
