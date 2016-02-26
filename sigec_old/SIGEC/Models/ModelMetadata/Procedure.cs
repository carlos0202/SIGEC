using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(ProcedureMetadata))]
    public partial class Procedure
    {

    }

    class ProcedureMetadata
    {
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        [Required]
        public string name { get; set; }
        [Display(Name = "lblDescription", ResourceType = typeof(Language))]
        [Required]
        public string description { get; set; }
        public int createdBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "lblCreateDate", ResourceType = typeof(Language))]
        public Nullable<System.DateTime> createDate { get; set; }
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
    }
}
