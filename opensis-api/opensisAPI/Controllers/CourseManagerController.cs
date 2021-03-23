using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.CourseManager.Interfaces;
using opensis.data.ViewModels.CourseManager;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/CourseManager")]
    [ApiController]
    public class CourseManagerController : ControllerBase
    {
        private ICourseManagerService _courseManagerService;
        public CourseManagerController(ICourseManagerService courseManagerService)
        {
            _courseManagerService = courseManagerService;
        }

        //[HttpPost("addProgram")]
        //public ActionResult<ProgramAddViewModel> AddProgram(ProgramAddViewModel programAddViewModel)
        //{
        //    ProgramAddViewModel programAdd = new ProgramAddViewModel();
        //    try
        //    {
        //        programAdd = _courseManagerService.AddProgram(programAddViewModel);
        //    }
        //    catch (Exception es)
        //    {
        //        programAdd._failure = true;
        //        programAdd._message = es.Message;
        //    }
        //    return programAdd;
        //}
        
        [HttpPost("getAllProgram")]
        public ActionResult<ProgramListViewModel> GetAllProgram(ProgramListViewModel programListViewModel)
        {
            ProgramListViewModel programList = new ProgramListViewModel();
            try
            {
                programList = _courseManagerService.GetAllProgram(programListViewModel);               
                
            }
            catch (Exception es)
            {
                programList._message = es.Message;
                programList._failure = true;
            }
            return programList;
        }
        
        [HttpPut("addEditProgram")]
        public ActionResult<ProgramListViewModel> AddEditProgram(ProgramListViewModel programListViewModel)
        {
            ProgramListViewModel programUpdate = new ProgramListViewModel();
            try
            {
                programUpdate = _courseManagerService.AddEditProgram(programListViewModel);
            }
            catch (Exception es)
            {
                programUpdate._failure = true;
                programUpdate._message = es.Message;
            }
            return programUpdate;
        }
        
        [HttpPost("deleteProgram")]
        public ActionResult<ProgramAddViewModel> DeleteProgram(ProgramAddViewModel programAddViewModel)
        {
            ProgramAddViewModel programDelete = new ProgramAddViewModel();
            try
            {
                programDelete = _courseManagerService.DeleteProgram(programAddViewModel);
            }
            catch (Exception es)
            {
                programDelete._failure = true;
                programDelete._message = es.Message;
            }
            return programDelete;
        }

        //[HttpPost("addSubject")]
        //public ActionResult<SubjectAddViewModel> AddSubject(SubjectAddViewModel subjectAddViewModel)
        //{
        //    SubjectAddViewModel subjectAdd = new SubjectAddViewModel();
        //    try
        //    {
        //        subjectAdd = _courseManagerService.AddSubject(subjectAddViewModel);
        //    }
        //    catch (Exception es)
        //    {
        //        subjectAdd._failure = true;
        //        subjectAdd._message = es.Message;
        //    }
        //    return subjectAdd;
        //}

        [HttpPut("addEditSubject")]
        public ActionResult<SubjectListViewModel> AddEditSubject(SubjectListViewModel subjectListViewModel)
        {
            SubjectListViewModel subjectAddUpdate = new SubjectListViewModel();
            try
            {
                subjectAddUpdate = _courseManagerService.AddEditSubject(subjectListViewModel);
            }
            catch (Exception es)
            {
                subjectAddUpdate._failure = true;
                subjectAddUpdate._message = es.Message;
            }
            return subjectAddUpdate;
        }

        [HttpPost("getAllSubjectList")]
        public ActionResult<SubjectListViewModel> GetAllSubjectList(SubjectListViewModel subjectListViewModel)
        {
            SubjectListViewModel subjectList = new SubjectListViewModel();
            try
            {
                subjectList = _courseManagerService.GetAllSubjectList(subjectListViewModel);
            }
            catch (Exception es)
            {
                subjectList._message = es.Message;
                subjectList._failure = true;
            }
            return subjectList;
        }

        [HttpPost("deleteSubject")]
        public ActionResult<SubjectAddViewModel> DeleteSubject(SubjectAddViewModel subjectAddViewModel)
        {
            SubjectAddViewModel subjectDelete = new SubjectAddViewModel();
            try
            {
                if (subjectAddViewModel.subject.SchoolId > 0)
                {
                    subjectDelete = _courseManagerService.DeleteSubject(subjectAddViewModel);
                }
                else
                {
                    subjectDelete._token = subjectAddViewModel._token;
                    subjectDelete._tenantName = subjectAddViewModel._tenantName;
                    subjectDelete._failure = true;
                    subjectDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                subjectDelete._failure = true;
                subjectDelete._message = es.Message;
            }
            return subjectDelete;
        }

        [HttpPost("addCourse")]
        public ActionResult<CourseAddViewModel> AddCourse(CourseAddViewModel courseAddViewModel)
        {
            CourseAddViewModel courseAdd = new CourseAddViewModel();
            try
            {
                courseAdd = _courseManagerService.AddCourse(courseAddViewModel);
            }
            catch (Exception es)
            {
                courseAdd._failure = true;
                courseAdd._message = es.Message;
            }
            return courseAdd;
        }

        [HttpPut("updateCourse")]
        public ActionResult<CourseAddViewModel> UpdateCourse(CourseAddViewModel courseAddViewModel)
        {
            CourseAddViewModel courseUpdate = new CourseAddViewModel();
            try
            {
                courseUpdate = _courseManagerService.UpdateCourse(courseAddViewModel);
            }
            catch (Exception es)
            {
                courseUpdate._failure = true;
                courseUpdate._message = es.Message;
            }
            return courseUpdate;
        }

        [HttpPost("deleteCourse")]
        public ActionResult<CourseAddViewModel> DeleteCourse(CourseAddViewModel courseAddViewModel)
        {
            CourseAddViewModel courseDelete = new CourseAddViewModel();
            try
            {
                courseDelete = _courseManagerService.DeleteCourse(courseAddViewModel);
            }
            catch (Exception es)
            {
                courseDelete._failure = true;
                courseDelete._message = es.Message;
            }
            return courseDelete;
        }

        [HttpPost("getAllCourseList")]
        public ActionResult<CourseListViewModel> GetAllCourseList(CourseListViewModel courseListViewModel)
        {
            CourseListViewModel courseList = new CourseListViewModel();
            try
            {
                courseList = _courseManagerService.GetAllCourseList(courseListViewModel);

            }
            catch (Exception es)
            {
                courseList._message = es.Message;
                courseList._failure = true;
            }
            return courseList;
        }

        [HttpPost("addCourseSection")]
        public ActionResult<CourseSectionAddViewModel> AddCourseSection(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            CourseSectionAddViewModel courseSectionAdd = new CourseSectionAddViewModel();
            try
            {
                courseSectionAdd = _courseManagerService.AddCourseSection(courseSectionAddViewModel);
            }
            catch (Exception es)
            {
                courseSectionAdd._failure = true;
                courseSectionAdd._message = es.Message;
            }
            return courseSectionAdd;
        }

        [HttpPost("getAllCourseSection")]
        public ActionResult<CourseSectionViewModel> GetAllCourseSection(CourseSectionViewModel courseSectionViewModel)
        {
            CourseSectionViewModel courseSectionView = new CourseSectionViewModel();
            try
            {
                courseSectionView = _courseManagerService.GetAllCourseSection(courseSectionViewModel);
            }
            catch (Exception es)
            {
                courseSectionView._failure = true;
                courseSectionView._message = es.Message;
            }
            return courseSectionView;
        }

        [HttpPut("updateCourseSection")]
        public ActionResult<CourseSectionAddViewModel> UpdateCourseSection(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            CourseSectionAddViewModel courseSectionUpdate = new CourseSectionAddViewModel();
            try
            {
                courseSectionUpdate = _courseManagerService.UpdateCourseSection(courseSectionAddViewModel);
            }
            catch (Exception es)
            {
                courseSectionUpdate._failure = true;
                courseSectionUpdate._message = es.Message;
            }
            return courseSectionUpdate;
        }

        [HttpPost("deleteCourseSection")]
        public ActionResult<CourseSectionAddViewModel> DeleteCourseSection(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            CourseSectionAddViewModel courseSectionDelete = new CourseSectionAddViewModel();
            try
            {
                courseSectionDelete = _courseManagerService.DeleteCourseSection(courseSectionAddViewModel);
            }
            catch (Exception es)
            {
                courseSectionDelete._failure = true;
                courseSectionDelete._message = es.Message;
            }
            return courseSectionDelete;
        }

        /// <summary>
        /// Get All CourseStandard For Course
        /// </summary>
        /// <param name="courseStandardForCourseViewModel"></param>
        /// <returns></returns>
        [HttpPost("getAllCourseStandardForCourseSection")]
        public ActionResult<CourseStandardForCourseViewModel> GetAllCourseStandardForCourseSection(CourseStandardForCourseViewModel courseStandardForCourseViewModel)
        {
            CourseStandardForCourseViewModel courseStandardForCourseView = new CourseStandardForCourseViewModel();
            try
            {
                courseStandardForCourseView = _courseManagerService.GetAllCourseStandardForCourseSection(courseStandardForCourseViewModel);
            }
            catch (Exception es)
            {
                courseStandardForCourseView._failure = true;
                courseStandardForCourseView._message = es.Message;
            }
            return courseStandardForCourseView;
        }

        [HttpPost("deleteSchedule")]
        public ActionResult<DeleteScheduleViewModel> DeleteSchedule(DeleteScheduleViewModel deleteScheduleViewModel)
        {
            DeleteScheduleViewModel deleteSchedule = new DeleteScheduleViewModel();
            try
            {
                deleteSchedule = _courseManagerService.DeleteSchedule(deleteScheduleViewModel);
            }
            catch (Exception es)
            {
                deleteSchedule._failure = true;
                deleteSchedule._message = es.Message;
            }
            return deleteSchedule;
        }

        [HttpPost("searchCourseSectionForSchedule")]
        public ActionResult<SearchCourseSectionViewModel> SearchCourseSectionForSchedule(SearchCourseSectionViewModel searchCourseSectionViewModel)
        {
            SearchCourseSectionViewModel searchCourseSection = new SearchCourseSectionViewModel();
            try
            {
                searchCourseSection = _courseManagerService.SearchCourseSectionForSchedule(searchCourseSectionViewModel);
            }
            catch (Exception es)
            {
                searchCourseSection._failure = true;
                searchCourseSection._message = es.Message;
            }
            return searchCourseSection;
        }
    }
}
