using SIGEC.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGEC.Models
{
    [MetadataType(typeof(PhoneMetadata))]
    public partial class Phone
    {
        
    }

    class PhoneMetadata
    {
        [Display(Name = "lblPhone", ResourceType = typeof(Language))]
        public string number { get; set; }
        [Display(Name = "lblPhoneType", ResourceType = typeof(Language))]
        public int type { get; set; }
        [Display(Name = "lblPhoneNotes", ResourceType = typeof(Language))]
        public string notes { get; set; }
    }
}
