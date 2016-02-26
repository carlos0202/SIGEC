using SIGEC.Resources;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        [NotMapped]
        public string CompleteName
        {
            get
            {
                return string.Format("{0} {1}", firstName, lastName);
            }
        }

        [NotMapped]
        public bool isDoctor
        {
            get
            {
                return (Doctors.Count > 0) ? true : false;
            }
        }

        [NotMapped]
        public Patient PatientAccount
        {
            get { return Patients1.FirstOrDefault(p => p.userID == ID); }
        }
    }

    class UserMetadata
    {
        public int ID { get; set; }
        public int addressID { get; set; }
        [Display(Name = "lblBornDate", ResourceType = typeof(Language))]
        public System.DateTime bornDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "lblCreateDate", ResourceType = typeof(Language))]
        public Nullable<System.DateTime> createDate { get; set; }
        [Display(Name = "lblDni", ResourceType = typeof(Language))]
        public string dni { get; set; }
        [Display(Name = "lblEmail", ResourceType = typeof(Language))]
        public string email { get; set; }
        [Display(Name = "lblFirstName", ResourceType = typeof(Language))]
        public string firstName { get; set; }
        [Display(Name = "lblLastName", ResourceType = typeof(Language))]
        public string lastName { get; set; }
        [Display(Name = "lblGender", ResourceType = typeof(Language))]
        public string gender { get; set; }
        [Display(Name = "lblMaritalStatus", ResourceType = typeof(Language))]
        public string maritalStatus { get; set; }
        [Display(Name = "lblUsername", ResourceType = typeof(Language))]
        public string username { get; set; }
        [Display(Name = "lblStatus", ResourceType = typeof(Language))]
        public bool status { get; set; }
        [Display(Name = "lblOccupation", ResourceType = typeof(Language))]
        public string occupation { get; set; }

        public Nullable<System.DateTime> lastVisit { get; set; }

        public string password { get; set; }
        public string salt { get; set; }
        public Nullable<bool> superUser { get; set; }
        [Display(Name = "lblReligion", ResourceType = typeof(Language))]
        public string religion { get; set; }
    }
}
