using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGEC.Models
{
    [MetadataType(typeof(AddressMetadata))]
    public partial class Address
    {

        
    }

    class AddressMetadata
    {
        public int ID { get; set; }
        [Display(Name = "lblCity", ResourceType = typeof(Language))]
        [Required]
        public string city { get; set; }
        [Display(Name = "lblMunicipality", ResourceType = typeof(Language))]
        [Required]
        public string municipality { get; set; }
        [Display(Name = "lblNumber", ResourceType = typeof(Language))]
        [Required]
        public string number { get; set; }
        [Display(Name = "lblSector", ResourceType = typeof(Language))]
        [Required]
        public string sector { get; set; }
        [Display(Name = "lblStreet", ResourceType = typeof(Language))]
        [Required]
        public string street { get; set; }
        [Display(Name = "lblCountry", ResourceType = typeof(Language))]
        [Required]
        public string country { get; set; }
        public virtual ICollection<Insurer> Insurers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
