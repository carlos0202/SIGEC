using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Action
    {
        public Action()
        {
            this.webpages_Roles = new List<webpages_Roles>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public int controllerID { get; set; }
        public string displayName { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}
