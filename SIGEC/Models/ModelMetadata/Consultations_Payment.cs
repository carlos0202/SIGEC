using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(Consultation_PaymentMetadata))]
    public partial class Consultations_Payment
    { }

    class Consultation_PaymentMetadata
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> netAmount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> total { get; set; }
    }
}
