using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
   public partial class GradeEquivalency
    {
        public GradeEquivalency()
        {
            Gradelevels = new HashSet<Gradelevels>();
        }

        public int EquivalencyId { get; set; }
        public string GradeLevelEquivalency { get; set; }

        public virtual ICollection<Gradelevels> Gradelevels { get; set; }
    }
}
