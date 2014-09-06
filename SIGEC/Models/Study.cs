using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Study
    {
        public Study()
        {
            this.Consultations = new List<Consultation>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int createdBy { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public bool status { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}
