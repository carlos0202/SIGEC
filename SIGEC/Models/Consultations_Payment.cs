using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Consultations_Payment
    {
        public int ID { get; set; }
        public int consultationID { get; set; }
        public decimal price { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<decimal> insurer { get; set; }
        public Nullable<decimal> netAmount { get; set; }
        public Nullable<decimal> total { get; set; }
        public string paymentForm { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
