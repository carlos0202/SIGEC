using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class webpages_Roles
    {
        public webpages_Roles()
        {
            this.Actions = new List<Action>();
            this.Users = new List<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
