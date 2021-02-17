using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class CourseSectionAddViewModel : CommonFields
    {
        public CourseSection courseSection { get; set; }
        public CourseFixedSchedule courseFixedSchedule { get; set; }
        public List<CourseVariableSchedule> courseVariableScheduleList { get; set; }
        public List<CourseCalendarSchedule> courseCalendarScheduleList { get; set; }
        public List<CourseBlockSchedule> courseBlockScheduleList { get; set; }
        public string MarkingPeriodId { get; set; }
    }
}
