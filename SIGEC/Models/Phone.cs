using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Phone
    {
        public Phone()
        {
            this.Insurers = new List<Insurer>();
            this.Users = new List<User>();
        }

        public int ID { get; set; }
        public string number { get; set; }
        public int type { get; set; }
        public string notes { get; set; }
        public virtual ICollection<Insurer> Insurers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
