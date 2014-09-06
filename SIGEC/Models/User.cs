using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class User
    {
        public User()
        {
            this.Analyses = new List<Analysis>();
            this.Appointments = new List<Appointment>();
            this.Doctors = new List<Doctor>();
            this.Insurers = new List<Insurer>();
            this.Medicines = new List<Medicine>();
            this.Patients = new List<Patient>();
            this.Patients1 = new List<Patient>();
            this.Procedures = new List<Procedure>();
            this.Studies = new List<Study>();
            this.Phones = new List<Phone>();
            this.webpages_Roles = new List<webpages_Roles>();
        }

        public int ID { get; set; }
        public int addressID { get; set; }
        public System.DateTime bornDate { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string dni { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string maritalStatus { get; set; }
        public string username { get; set; }
        public bool status { get; set; }
        public string occupation { get; set; }
        public Nullable<System.DateTime> lastVisit { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public Nullable<bool> superUser { get; set; }
        public string religion { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Analysis> Analyses { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Insurer> Insurers { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Patient> Patients1 { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
        public virtual ICollection<Study> Studies { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}
