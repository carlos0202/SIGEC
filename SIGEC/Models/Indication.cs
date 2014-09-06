using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Indication
    {
        public Indication()
        {
            this.Analyses = new List<Analysis>();
            this.Procedures = new List<Procedure>();
            this.Studies = new List<Study>();
        }

        public int ID { get; set; }
        public int consultationID { get; set; }
        public int patientID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public virtual Consultation Consultation { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Analysis> Analyses { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<Study> Studies { get; set; }
    }
}
