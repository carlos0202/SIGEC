using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEC.Models
{
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {
        [NotMapped]
        public User CreateUser
        {
            get { return User; }
            set { User = value; }
        }

        [NotMapped]
        public User UserAccount
        {
            get { return User1; }
            set { User1 = value; }
        }
    }

    class PatientMetadata
    {
        public int ID { get; set; }
        public int createBy { get; set; }
        public int userID { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
