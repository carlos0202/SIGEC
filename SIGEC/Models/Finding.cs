using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Finding
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int consultationID { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
