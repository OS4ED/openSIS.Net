using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class GradeAgeRange
    {
        public GradeAgeRange()
        {
            Gradelevels = new HashSet<Gradelevels>();
        }

        public int AgeRangeId { get; set; }
        public string AgeRange { get; set; }

        public virtual ICollection<Gradelevels> Gradelevels { get; set; }
    }
}
