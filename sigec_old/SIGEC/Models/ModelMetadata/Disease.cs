using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(DiseaseMetadata))]
    public partial class Disease
    { }

    class DiseaseMetadata
    {
        [Display(Name = "lblDiagnoseCode", ResourceType = typeof(Language))]
        public string diagnoseCode { get; set; }
        [Display(Name = "lblObservations", ResourceType = typeof(Language))]
        public string observations { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> startTime { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }
    }
}
