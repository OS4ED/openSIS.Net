using opensis.data.Models;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentAttendances
{
    public class CourseSectionForAttendanceViewModel : CommonFields
    {
        public CourseSectionForAttendanceViewModel()
        {
            courseSectionsList = new List<CourseSection>();
            courseSectionViewLists = new List<CourseSectionViewList>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public decimal? AcademicYear { get; set; }
        public List<CourseSection> courseSectionsList { get; set; }
        public List<AllCourseSectionView> allCourseSectionViews { get; set; }
        public List<CourseSectionViewList> courseSectionViewLists { get; set; }
        
    }
}
