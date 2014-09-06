using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(AppointmentMetadata))]
    public partial class Appointment
    {
        
    }

    class AppointmentMetadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> createDate { get; set; }
        public int createUser { get; set; }
        [Display(Name = "lblPatient", ResourceType = typeof(Language))]
        public int patientID { get; set; }
        [Display(Name = "lblAppointmentDate", ResourceType = typeof(Language))]
        public System.DateTime appointmentDate { get; set; }
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
        [Display(Name = "lblDoctor", ResourceType = typeof(Language))]
        public int doctorID { get; set; }
    }
}
