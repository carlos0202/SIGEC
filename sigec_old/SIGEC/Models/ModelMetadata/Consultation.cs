using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(ConsultationMetadata))]
    public partial class Consultation{ }

    class ConsultationMetadata
    {
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "lblCreateDate", ResourceType = typeof(Resources.Language))]
        public Nullable<System.DateTime> createDate { get; set; }
        [Display(Name="lblPatient", ResourceType=typeof(Resources.Language))]
        public int patientID { get; set; }
        [Display(Name = "lblDoctor", ResourceType = typeof(Resources.Language))]
        public int doctorID { get; set; }
        public Nullable<int> appointmentID { get; set; }
    }
}
