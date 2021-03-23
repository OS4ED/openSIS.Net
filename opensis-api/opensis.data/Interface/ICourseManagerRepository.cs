using opensis.data.ViewModels.CourseManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface ICourseManagerRepository
    {
        //public ProgramAddViewModel AddProgram(ProgramAddViewModel programAddViewModel);
        public ProgramListViewModel GetAllProgram(ProgramListViewModel programListViewModel);
        public ProgramListViewModel AddEditProgram(ProgramListViewModel programListViewModel);
        public ProgramAddViewModel DeleteProgram(ProgramAddViewModel programAddViewModel);
        //public SubjectAddViewModel AddSubject(SubjectAddViewModel subjectAddViewModel);
        public SubjectListViewModel AddEditSubject(SubjectListViewModel subjectListViewModel);
        public SubjectListViewModel GetAllSubjectList(SubjectListViewModel subjectListViewModel);
        public SubjectAddViewModel DeleteSubject(SubjectAddViewModel subjectAddViewModel);
        public CourseAddViewModel AddCourse(CourseAddViewModel courseAddViewModel);
        public CourseAddViewModel UpdateCourse(CourseAddViewModel courseAddViewModel);
        public CourseAddViewModel DeleteCourse(CourseAddViewModel courseAddViewModel);
        public CourseListViewModel GetAllCourseList(CourseListViewModel courseListViewModel);
        public CourseSectionAddViewModel AddCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseSectionViewModel GetAllCourseSection(CourseSectionViewModel courseSectionViewModel);
        public CourseSectionAddViewModel UpdateCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseSectionAddViewModel DeleteCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseStandardForCourseViewModel GetAllCourseStandardForCourseSection(CourseStandardForCourseViewModel courseStandardForCourseViewModel);
        public DeleteScheduleViewModel DeleteSchedule(DeleteScheduleViewModel deleteScheduleViewModel);
        public SearchCourseSectionViewModel SearchCourseSectionForSchedule(SearchCourseSectionViewModel searchCourseSectionViewModel);
    }
}
