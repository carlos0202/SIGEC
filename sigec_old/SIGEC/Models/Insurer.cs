using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Insurer
    {
        public Insurer()
        {
            this.Phones = new List<Phone>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string RNC { get; set; }
        public Nullable<int> addressID { get; set; }
        public bool status { get; set; }
        public int createdBy { get; set; }
        public virtual Address Address { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
