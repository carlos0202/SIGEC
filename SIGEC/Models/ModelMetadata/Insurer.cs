using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(InsurerMetadata))]
    public partial class Insurer
    {
        
    }

    class InsurerMetadata
    {
        public int ID { get; set; }
        [Display(Name = "lblName", ResourceType = typeof(Language))]
        [Required]
        public string name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<System.DateTime> createDate { get; set; }
        [Display(Name = "lblRNC", ResourceType = typeof(Language))]
        [Required]
        public string RNC { get; set; }
        public Nullable<int> addressID { get; set; }
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
        public int createdBy { get; set; }
        public virtual Address Address { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
