using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Prescriptions_Medicines
    {
        public int prescriptionID { get; set; }
        public int medicineID { get; set; }
        public int patientID { get; set; }
        public int consultationID { get; set; }
        public string administration { get; set; }
        public int ID { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
