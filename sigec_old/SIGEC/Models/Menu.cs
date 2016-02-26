using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class Menu
    {
        public Menu()
        {
            this.Actions = new List<Action>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
    }
}
