using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class GradeEducationalStage
    {
        public GradeEducationalStage()
        {
            Gradelevels = new HashSet<Gradelevels>();
        }

        public int IscedCode { get; set; }
        public string EducationalStage { get; set; }

        public virtual ICollection<Gradelevels> Gradelevels { get; set; }
    }
}
