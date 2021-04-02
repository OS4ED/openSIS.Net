using opensis.data.Models;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentSchedule.Interfaces
{
    public interface IStudentScheduleService
    {
        public StudentCourseSectionScheduleAddViewModel AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel);
        public ScheduleStudentListViewModel SearchScheduledStudentForGroupDrop(PageResult pageResult);
        public ScheduledStudentDropModel GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel);
        public StudentScheduleReportViewModel StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel);
        public StudentCourseSectionScheduleAddViewModel DeleteStudentScheduleReport(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel);
    }
}
