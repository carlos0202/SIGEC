using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Address
    {
        public Address()
        {
            this.Insurers = new List<Insurer>();
            this.Users = new List<User>();
        }

        public int ID { get; set; }
        public string city { get; set; }
        public string municipality { get; set; }
        public string number { get; set; }
        public string sector { get; set; }
        public string street { get; set; }
        public string country { get; set; }
        public virtual ICollection<Insurer> Insurers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
