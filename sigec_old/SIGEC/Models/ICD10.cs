using System;
using System.Collections.Generic;

namespace SIGEC.Models
{
    public partial class ICD10
    {
        public ICD10()
        {
            this.Diseases = new List<Disease>();
        }

        public string Code { get; set; }
        public string Description_es { get; set; }
        public string Description_en { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
    }
}
