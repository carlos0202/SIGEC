using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            this.Prescriptions_Medicines = new List<Prescriptions_Medicines>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string usage { get; set; }
        public string dosage { get; set; }
        public string genericName { get; set; }
        public bool status { get; set; }
        public int createdBy { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Prescriptions_Medicines> Prescriptions_Medicines { get; set; }
    }
}
