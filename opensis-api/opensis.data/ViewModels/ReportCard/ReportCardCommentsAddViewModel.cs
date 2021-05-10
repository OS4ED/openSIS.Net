using System;
using System.Collections.Generic;
using System.Text;
using opensis.data.Models;

namespace opensis.data.ViewModels.ReportCard
{
    public class ReportCardCommentsAddViewModel : CommonFields
    {
        public List<ReportCardComments> reportCardComments { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        //public int CourseId { get; set; }
        public int CourseCommentId { get; set; }


    }
}

