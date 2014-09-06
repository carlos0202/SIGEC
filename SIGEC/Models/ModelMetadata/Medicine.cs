using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGEC.Models
{
    [MetadataType(typeof(MedicineMetadata))]
    public partial class Medicine
    {
        
    }

    class MedicineMetadata
    {
        public int ID { get; set; }
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        [Required]
        public string name { get; set; }
        [Display(Name = "lblType", ResourceType = typeof(Language))]
        [Required]
        public string type { get; set; }
        [Display(Name = "lblUsage", ResourceType = typeof(Language))]
        [Required]
        public string usage { get; set; }
        [Display(Name = "lblDosage", ResourceType = typeof(Language))]
        [Required]
        public string dosage { get; set; }
        [Display(Name = "lblGenericName", ResourceType = typeof(Language))]
        public string genericName { get; set; }
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
    }
}
