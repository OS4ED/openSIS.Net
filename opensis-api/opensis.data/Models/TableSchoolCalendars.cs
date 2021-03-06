﻿using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class TableSchoolCalendars
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CalenderId { get; set; }
        public string Title { get; set; }
        public decimal? AcademicYear { get; set; }
        public string DefaultCalender { get; set; }
        public string Days { get; set; }
        public decimal? RolloverId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual TableSchoolMaster TableSchoolMaster { get; set; }
    }
}
