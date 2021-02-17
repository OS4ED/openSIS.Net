using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.CourseManager;

namespace opensis.data.Repository
{
    public class CourseManagerRepository : ICourseManagerRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "NO RECORD FOUND";
        public CourseManagerRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }
        /// <summary>
        /// Add Program
        /// </summary>
        /// <param name="programAddViewModel"></param>
        /// <returns></returns>
        //public ProgramAddViewModel AddProgram(ProgramAddViewModel programAddViewModel)
        //{
        //    //int? ProgramId = Utility.GetMaxPK(this.context, new Func<Programs, int>(x => x.ProgramId));

        //    int? ProgramId = 0;

        //    var programData = this.context?.Programs.Where(x => x.SchoolId == programAddViewModel.programs.SchoolId && x.TenantId == programAddViewModel.programs.TenantId).OrderByDescending(x => x.ProgramId).FirstOrDefault();

        //    if (programData != null)
        //    {
        //        ProgramId = programData.ProgramId + 1;
        //    }
        //    else
        //    {
        //        ProgramId = 1;
        //    }

        //    programAddViewModel.programs.ProgramId = (int)ProgramId;
        //    programAddViewModel.programs.CreatedOn = DateTime.UtcNow;
        //    this.context?.Programs.Add(programAddViewModel.programs);
        //    this.context?.SaveChanges();
        //    programAddViewModel._failure = false;

        //    return programAddViewModel;
        //}
        /// <summary>
        /// Get All Program
        /// </summary>
        /// <param name="programListViewModel"></param>
        /// <returns></returns>
        public ProgramListViewModel GetAllProgram(ProgramListViewModel programListViewModel)
        {
            ProgramListViewModel programListModel = new ProgramListViewModel();
            try
            {

                var programList = this.context?.Programs.Where(x => x.TenantId == programListViewModel.TenantId && x.SchoolId == programListViewModel.SchoolId).ToList();
                if (programList.Count > 0)
                {
                    programListModel.programList = programList;
                    programListModel._tenantName = programListViewModel._tenantName;
                    programListModel._token = programListViewModel._token;
                    programListModel._failure = false;
                }
                else
                {
                    programListModel.programList = null;
                    programListModel._tenantName = programListViewModel._tenantName;
                    programListModel._token = programListViewModel._token;
                    programListModel._failure = true;
                    programListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                programListModel._message = es.Message;
                programListModel._failure = true;
                programListModel._tenantName = programListViewModel._tenantName;
                programListModel._token = programListViewModel._token;
            }
            return programListModel;
        }
        /// <summary>
        /// Update Program
        /// </summary>
        /// <param name="programAddViewModel"></param>
        /// <returns></returns>
        public ProgramListViewModel AddEditProgram(ProgramListViewModel programListViewModel)
        {
            ProgramListViewModel programUpdateModel = new ProgramListViewModel();
            try
            {
                foreach (var programLists in programListViewModel.programList)
                {
                    if (programLists.ProgramId > 0)
                    {
                        var programUpdate = this.context?.Programs.FirstOrDefault(x => x.TenantId == programLists.TenantId && x.SchoolId == programLists.SchoolId && x.ProgramId == programLists.ProgramId);
                        if (programUpdate != null)
                        {
                            var program = this.context?.Programs.FirstOrDefault(x => x.TenantId == programLists.TenantId && x.SchoolId == programLists.SchoolId && x.ProgramId != programLists.ProgramId && x.ProgramName.ToLower() == programLists.ProgramName.ToLower());
                            if (program != null)
                            {
                                programUpdateModel._message = "Program Name Already Exists";
                                programUpdateModel._failure = true;
                                return programUpdateModel;
                            }
                            else
                            {
                                var courseList = this.context?.Course.Where(x => x.TenantId == programUpdate.TenantId && x.SchoolId == programUpdate.SchoolId && x.CourseProgram.ToLower() == programUpdate.ProgramName.ToLower()).ToList();

                                programLists.CreatedBy = programUpdate.CreatedBy;
                                programLists.CreatedOn = programUpdate.CreatedOn;
                                programLists.UpdatedOn = DateTime.Now;
                                this.context.Entry(programUpdate).CurrentValues.SetValues(programLists);
                                this.context?.SaveChanges();

                                if (courseList.Count > 0)
                                {
                                    courseList.ForEach(x => x.CourseProgram = programLists.ProgramName);
                                }
                            }
                        }
                    }
                    else
                    {
                        int? ProgramId = 1;

                        var programData = this.context?.Programs.Where(x => x.SchoolId == programLists.SchoolId && x.TenantId == programLists.TenantId).OrderByDescending(x => x.ProgramId).FirstOrDefault();

                        if (programData != null)
                        {
                            ProgramId = programData.ProgramId + 1;
                        }

                        var program = this.context?.Programs.Where(x => x.SchoolId == programLists.SchoolId && x.TenantId == programLists.TenantId && x.ProgramName.ToLower() == programLists.ProgramName.ToLower()).FirstOrDefault();
                        if (program != null)
                        {
                            programUpdateModel._failure = true;
                            programUpdateModel._message = "Program Name Already Exists";
                            return programUpdateModel;
                        }
                        programLists.ProgramId = (int)ProgramId;
                        programLists.CreatedOn = DateTime.UtcNow;
                        this.context?.Programs.AddRange(programLists);

                    }
                }
                this.context?.SaveChanges();
                programUpdateModel._message = "Program Updated Successfully";
                programUpdateModel._failure = false;
            }
            catch (Exception es)
            {

                programUpdateModel._message = es.Message;
                programUpdateModel._failure = true;
                programUpdateModel._tenantName = programListViewModel._tenantName;
                programUpdateModel._token = programListViewModel._token;
            }
            return programUpdateModel;
        }
        /// <summary>
        /// Delete Program
        /// </summary>
        /// <param name="programAddViewModel"></param>
        /// <returns></returns>
        public ProgramAddViewModel DeleteProgram(ProgramAddViewModel programAddViewModel)
        {
            try
            {
                var programDelete = this.context?.Programs.FirstOrDefault(x => x.TenantId == programAddViewModel.programs.TenantId && x.SchoolId == programAddViewModel.programs.SchoolId && x.ProgramId == programAddViewModel.programs.ProgramId);
                var CourseList = this.context?.Course.Where(e => e.CourseProgram.ToLower() == programDelete.ProgramName.ToLower() && e.SchoolId == programDelete.SchoolId && e.TenantId == programDelete.TenantId).ToList();

                if (CourseList.Count > 0)
                {
                    programAddViewModel._message = "It Has Associationship";
                    programAddViewModel._failure = true;
                }
                else
                {
                    this.context?.Programs.Remove(programDelete);
                    this.context?.SaveChanges();
                    programAddViewModel._failure = false;
                    programAddViewModel._message = "Deleted";
                }
            }
            catch (Exception es)
            {
                programAddViewModel._failure = true;
                programAddViewModel._message = es.Message;
            }
            return programAddViewModel;
        }

        /// <summary>
        /// Add Subject
        /// </summary>
        /// <param name="subjectAddViewModel"></param>
        /// <returns></returns>
        //public SubjectAddViewModel AddSubject(SubjectAddViewModel subjectAddViewModel)
        //{
        //    try
        //    {
        //        //int? MasterSubjectId = Utility.GetMaxPK(this.context, new Func<Subject, int>(x => x.SubjectId));
        //        int? MasterSubjectId = 1;

        //        var subjectData = this.context?.Subject.Where(x => x.SchoolId == subjectAddViewModel.subject.SchoolId && x.TenantId == subjectAddViewModel.subject.TenantId).OrderByDescending(x => x.SubjectId).FirstOrDefault();

        //        if (subjectData != null)
        //        {
        //            MasterSubjectId = subjectData.SubjectId + 1;
        //        }
        //        subjectAddViewModel.subject.SubjectId = (int)MasterSubjectId;
        //        subjectAddViewModel.subject.CreatedOn = DateTime.UtcNow;
        //        this.context?.Subject.Add(subjectAddViewModel.subject);
        //        this.context?.SaveChanges();
        //        subjectAddViewModel._failure = false;
        //    }
        //    catch (Exception es)
        //    {
        //        subjectAddViewModel._failure = true;
        //        subjectAddViewModel._message = es.Message;
        //    }
        //    return subjectAddViewModel;
        //}

        /// <summary>
        /// Add & Update Subject
        /// </summary>
        /// <param name="subjectListViewModel"></param>
        /// <returns></returns>
        public SubjectListViewModel AddEditSubject(SubjectListViewModel subjectListViewModel)
        {
            try
            {
                foreach (var subject in subjectListViewModel.subjectList)
                {
                    if (subject.SubjectId > 0)
                    {
                        var SubjectUpdate = this.context?.Subject.FirstOrDefault(x => x.TenantId == subject.TenantId && x.SchoolId == subject.SchoolId && x.SubjectId == subject.SubjectId);

                        if (SubjectUpdate != null)
                        {
                            var subjectName = this.context?.Subject.FirstOrDefault(x => x.TenantId == subject.TenantId && x.SchoolId == subject.SchoolId && x.SubjectName.ToLower() == subject.SubjectName.ToLower() && x.SubjectId != subject.SubjectId);

                            if (subjectName == null)
                            {
                                var sameSubjectNameExits = this.context?.Course.Where(x => x.SchoolId == SubjectUpdate.SchoolId && x.TenantId == SubjectUpdate.TenantId && x.CourseSubject.ToLower() == SubjectUpdate.SubjectName.ToLower()).ToList();

                                if (sameSubjectNameExits.Count > 0)
                                {
                                    sameSubjectNameExits.ForEach(x => x.CourseSubject = subject.SubjectName);
                                }

                                subject.CreatedBy = SubjectUpdate.CreatedBy;
                                subject.CreatedOn = SubjectUpdate.CreatedOn;
                                subject.UpdatedOn = DateTime.Now;
                                this.context.Entry(SubjectUpdate).CurrentValues.SetValues(subject);
                                this.context?.SaveChanges();
                            }
                            else
                            {
                                subjectListViewModel._failure = true;
                                subjectListViewModel._message = "Subject Name Already Exits";
                                return subjectListViewModel;
                            }
                        }
                        else
                        {
                            subjectListViewModel._failure = true;
                            subjectListViewModel._message = NORECORDFOUND;
                        }
                    }
                    else
                    {
                        var subjectName = this.context?.Subject.FirstOrDefault(x => x.TenantId == subject.TenantId && x.SchoolId == subject.SchoolId && x.SubjectName.ToLower() == subject.SubjectName.ToLower());

                        if (subjectName == null)
                        {
                            int? SubjectId = 1;

                            var subjectData = this.context?.Subject.Where(x => x.SchoolId == subject.SchoolId && x.TenantId == subject.TenantId).OrderByDescending(x => x.SubjectId).FirstOrDefault();

                            if (subjectData != null)
                            {
                                SubjectId = subjectData.SubjectId + 1;
                            }
                            subject.SubjectId = (int)SubjectId;
                            subject.CreatedOn = DateTime.UtcNow;
                            this.context?.Subject.Add(subject);
                        }
                        else
                        {
                            subjectListViewModel._failure = true;
                            subjectListViewModel._message = "Subject Name Already Exits";
                            return subjectListViewModel;
                        }
                    }
                }
                this.context?.SaveChanges();
                subjectListViewModel._failure = false;
                subjectListViewModel._message = "Subject Updated Successfully";
            }
            catch (Exception es)
            {
                subjectListViewModel._failure = true;
                subjectListViewModel._message = es.Message;
            }
            return subjectListViewModel;
        }

        /// <summary>
        /// Get All Subject List
        /// </summary>
        /// <param name="subjectListViewModel"></param>
        /// <returns></returns>
        public SubjectListViewModel GetAllSubjectList(SubjectListViewModel subjectListViewModel)
        {
            SubjectListViewModel subjectList = new SubjectListViewModel();
            try
            {
                var Subjectdata = this.context?.Subject.Where(x => x.TenantId == subjectListViewModel.TenantId && x.SchoolId == subjectListViewModel.SchoolId).ToList();
                if (Subjectdata.Count > 0)
                {
                    subjectList.subjectList = Subjectdata;
                    subjectList._tenantName = subjectListViewModel._tenantName;
                    subjectList._token = subjectListViewModel._token;
                    subjectList._failure = false;
                }
                else
                {
                    subjectList.subjectList = null;
                    subjectList._tenantName = subjectListViewModel._tenantName;
                    subjectList._token = subjectListViewModel._token;
                    subjectList._failure = true;
                    subjectList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                subjectList._message = es.Message;
                subjectList._failure = true;
                subjectList._tenantName = subjectListViewModel._tenantName;
                subjectList._token = subjectListViewModel._token;
            }
            return subjectList;
        }

        /// <summary>
        /// Delete Subject
        /// </summary>
        /// <param name="subjectAddViewModel"></param>
        /// <returns></returns>
        public SubjectAddViewModel DeleteSubject(SubjectAddViewModel subjectAddViewModel)
        {
            try
            {
                var subjectDelete = this.context?.Subject.FirstOrDefault(x => x.TenantId == subjectAddViewModel.subject.TenantId && x.SchoolId == subjectAddViewModel.subject.SchoolId && x.SubjectId == subjectAddViewModel.subject.SubjectId);

                if (subjectDelete != null)
                {
                    var courseExits = this.context?.Course.FirstOrDefault(x => x.TenantId == subjectDelete.TenantId && x.SchoolId == subjectDelete.SchoolId && x.CourseSubject == subjectDelete.SubjectName);
                    if (courseExits != null)
                    {
                        subjectAddViewModel._failure = true;
                        subjectAddViewModel._message = "Cannot delete because it has association.";
                    }
                    else
                    {
                        this.context?.Subject.Remove(subjectDelete);
                        this.context?.SaveChanges();
                        subjectAddViewModel._failure = false;
                        subjectAddViewModel._message = "Deleted Successfully";
                    }
                }
            }
            catch (Exception es)
            {
                subjectAddViewModel._failure = true;
                subjectAddViewModel._message = es.Message;
            }
            return subjectAddViewModel;
        }

        /// <summary>
        /// Add Course
        /// </summary>
        /// <param name="courseAddViewModel"></param>
        /// <returns></returns>
        public CourseAddViewModel AddCourse(CourseAddViewModel courseAddViewModel)
        {
            try
            {
                if (courseAddViewModel.ProgramId == 0)
                {
                    int? ProgramId = 1;

                    var programData = this.context?.Programs.Where(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId).OrderByDescending(x => x.ProgramId).FirstOrDefault();

                    if (programData != null)
                    {
                        ProgramId = programData.ProgramId + 1;
                    }

                    var programName = this.context?.Programs.FirstOrDefault(x => x.SchoolId == courseAddViewModel.course.SchoolId && x.TenantId == courseAddViewModel.course.TenantId && x.ProgramName.ToLower() == courseAddViewModel.course.CourseProgram.ToLower());
                    if (programName != null)
                    {
                        courseAddViewModel._failure = true;
                        courseAddViewModel._message = "Program Name Already Exists";
                        return courseAddViewModel;
                    }
                    var programAdd = new Programs() { TenantId = courseAddViewModel.course.TenantId, SchoolId = courseAddViewModel.course.SchoolId, ProgramId = (int)ProgramId, ProgramName = courseAddViewModel.course.CourseProgram, CreatedOn = DateTime.UtcNow, CreatedBy = courseAddViewModel.course.CreatedBy };
                    this.context?.Programs.Add(programAdd);
                }
                if (courseAddViewModel.SubjectId == 0)
                {
                    int? SubjectId = 1;

                    var subjectData = this.context?.Subject.Where(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId).OrderByDescending(x => x.SubjectId).FirstOrDefault();

                    if (subjectData != null)
                    {
                        SubjectId = subjectData.SubjectId + 1;
                    }

                    var subjectName = this.context?.Subject.FirstOrDefault(x => x.SchoolId == courseAddViewModel.course.SchoolId && x.TenantId == courseAddViewModel.course.TenantId && x.SubjectName.ToLower() == courseAddViewModel.course.CourseSubject.ToLower());
                    if (subjectName != null)
                    {
                        courseAddViewModel._failure = true;
                        courseAddViewModel._message = "Subject Name Already Exists";
                        return courseAddViewModel;
                    }
                    var subjectAdd = new Subject() { TenantId = courseAddViewModel.course.TenantId, SchoolId = courseAddViewModel.course.SchoolId, SubjectId = (int)SubjectId, SubjectName = courseAddViewModel.course.CourseSubject, CreatedOn = DateTime.UtcNow, CreatedBy = courseAddViewModel.course.CreatedBy };
                    this.context?.Subject.Add(subjectAdd);
                }

                var courseTitle = this.context?.Course.FirstOrDefault(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId && x.CourseTitle.ToLower() == courseAddViewModel.course.CourseTitle.ToLower());

                if (courseTitle == null)
                {
                    int? CourseId = 1;

                    var courseData = this.context?.Course.Where(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId).OrderByDescending(x => x.CourseId).FirstOrDefault();

                    if (courseData != null)
                    {
                        CourseId = courseData.CourseId + 1;
                    }
                    courseAddViewModel.course.CourseId = (int)CourseId;
                    courseAddViewModel.course.CreatedOn = DateTime.UtcNow;
                    courseAddViewModel.course.IsCourseActive = true;

                    if (courseAddViewModel.course.CourseStandard.ToList().Count > 0)
                    {
                        courseAddViewModel.course.CourseStandard.ToList().ForEach(x => x.CreatedOn = DateTime.UtcNow);
                    }

                    this.context?.Course.Add(courseAddViewModel.course);
                }
                else
                {
                    courseAddViewModel._failure = true;
                    courseAddViewModel._message = "Course Title Already Exits";
                    return courseAddViewModel;
                }
                this.context.SaveChanges();
                courseAddViewModel._failure = false;
            }
            catch (Exception es)
            {
                courseAddViewModel._failure = true;
                courseAddViewModel._message = es.Message;
            }
            return courseAddViewModel;
        }

        /// <summary>
        /// Update Course
        /// </summary>
        /// <param name="courseAddViewModel"></param>
        /// <returns></returns>
        public CourseAddViewModel UpdateCourse(CourseAddViewModel courseAddViewModel)
        {
            try
            {
                if (courseAddViewModel.ProgramId == 0)
                {
                    int? ProgramId = 1;

                    var programData = this.context?.Programs.Where(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId).OrderByDescending(x => x.ProgramId).FirstOrDefault();

                    if (programData != null)
                    {
                        ProgramId = programData.ProgramId + 1;
                    }

                    var programName = this.context?.Programs.FirstOrDefault(x => x.SchoolId == courseAddViewModel.course.SchoolId && x.TenantId == courseAddViewModel.course.TenantId && x.ProgramName.ToLower() == courseAddViewModel.course.CourseProgram.ToLower());

                    if (programName != null)
                    {
                        courseAddViewModel._failure = true;
                        courseAddViewModel._message = "Program Name Already Exists";
                        return courseAddViewModel;
                    }

                    var programAdd = new Programs() { TenantId = courseAddViewModel.course.TenantId, SchoolId = courseAddViewModel.course.SchoolId, ProgramId = (int)ProgramId, ProgramName = courseAddViewModel.course.CourseProgram, CreatedOn = DateTime.UtcNow, CreatedBy = courseAddViewModel.course.CreatedBy };
                    this.context?.Programs.Add(programAdd);
                }

                if (courseAddViewModel.SubjectId == 0)
                {
                    int? SubjectId = 1;

                    var subjectData = this.context?.Subject.Where(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId).OrderByDescending(x => x.SubjectId).FirstOrDefault();

                    if (subjectData != null)
                    {
                        SubjectId = subjectData.SubjectId + 1;
                    }

                    var subjectName = this.context?.Subject.FirstOrDefault(x => x.SchoolId == courseAddViewModel.course.SchoolId && x.TenantId == courseAddViewModel.course.TenantId && x.SubjectName.ToLower() == courseAddViewModel.course.CourseSubject.ToLower());
                    if (subjectName != null)
                    {
                        courseAddViewModel._failure = true;
                        courseAddViewModel._message = "Subject Name Already Exists";
                        return courseAddViewModel;
                    }

                    var subjectAdd = new Subject() { TenantId = courseAddViewModel.course.TenantId, SchoolId = courseAddViewModel.course.SchoolId, SubjectId = (int)SubjectId, SubjectName = courseAddViewModel.course.CourseSubject, CreatedOn = DateTime.UtcNow, CreatedBy = courseAddViewModel.course.CreatedBy };
                    this.context?.Subject.Add(subjectAdd);
                }

                var courseUpdate = this.context?.Course.Include(x => x.CourseStandard).FirstOrDefault(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId && x.CourseId == courseAddViewModel.course.CourseId);

                if (courseUpdate != null)
                {
                    var courseTitle = this.context?.Course.FirstOrDefault(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId && x.CourseTitle.ToLower() == courseAddViewModel.course.CourseTitle.ToLower() && x.CourseId != courseAddViewModel.course.CourseId);

                    if (courseTitle == null)
                    {
                        courseAddViewModel.course.CreatedBy = courseUpdate.CreatedBy;
                        courseAddViewModel.course.CreatedOn = courseUpdate.CreatedOn;
                        courseAddViewModel.course.UpdatedOn = DateTime.Now;
                        this.context.Entry(courseUpdate).CurrentValues.SetValues(courseAddViewModel.course);
                        courseAddViewModel._message = "Course Updated Successfully";

                        if (courseAddViewModel.course.CourseStandard.ToList().Count > 0)
                        {
                            this.context?.CourseStandard.RemoveRange(courseUpdate.CourseStandard);
                            courseAddViewModel.course.CourseStandard.ToList().ForEach(x => x.UpdatedOn = DateTime.UtcNow);
                            this.context?.CourseStandard.AddRange(courseAddViewModel.course.CourseStandard);
                        }
                    }
                    else
                    {
                        courseAddViewModel._failure = true;
                        courseAddViewModel._message = "Course Title Already Exits";
                        return courseAddViewModel;
                    }
                }
                this.context.SaveChanges();
                courseAddViewModel._failure = false;
            }
            catch (Exception es)
            {
                courseAddViewModel._failure = true;
                courseAddViewModel._message = es.Message;
            }
            return courseAddViewModel;
        }

        /// <summary>
        /// Delete Course
        /// </summary>
        /// <param name="courseAddViewModel"></param>
        /// <returns></returns>
        public CourseAddViewModel DeleteCourse(CourseAddViewModel courseAddViewModel)
        {
            try
            {
                var courseDelete = this.context?.Course.Include(x => x.CourseStandard).FirstOrDefault(x => x.TenantId == courseAddViewModel.course.TenantId && x.SchoolId == courseAddViewModel.course.SchoolId && x.CourseId == courseAddViewModel.course.CourseId);

                if (courseDelete != null)
                {
                    this.context?.CourseStandard.RemoveRange(courseDelete.CourseStandard);
                    this.context?.Course.Remove(courseDelete);
                    this.context?.SaveChanges();
                    courseAddViewModel._failure = false;
                    courseAddViewModel._message = "Deleted Successfully";
                }
                else
                {
                    courseAddViewModel._failure = true;
                    courseAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                courseAddViewModel._failure = true;
                courseAddViewModel._message = es.Message;
            }
            return courseAddViewModel;
        }

        /// <summary>
        /// Get All Course List
        /// </summary>
        /// <param name="courseListViewModel"></param>
        /// <returns></returns>
        public CourseListViewModel GetAllCourseList(CourseListViewModel courseListViewModel)
        {
            CourseListViewModel courseListModel = new CourseListViewModel();
            try
            {
                var courseRecords = this.context?.Course.Include(x=>x.CourseSection).Include(e => e.CourseStandard).ThenInclude(c => c.GradeUsStandard).Where(x => x.TenantId == courseListViewModel.TenantId && x.SchoolId == courseListViewModel.SchoolId).ToList();
                if (courseRecords.Count > 0)
                {
                    foreach(var course in courseRecords)
                    {
                        CourseViewModel courseViewModel = new CourseViewModel { course = course, CourseSectionCount = course.CourseSection.Count() };
                        courseListModel.courseViewModelList.Add(courseViewModel);
                    }                    
                    courseListModel.CourseCount = courseRecords.Count();
                    courseListModel._tenantName = courseListViewModel._tenantName;
                    courseListModel._token = courseListViewModel._token;
                    courseListModel._failure = false;
                }
                else
                {
                    courseListModel.courseViewModelList = null;
                    courseListModel.CourseCount = null;
                    courseListModel._tenantName = courseListViewModel._tenantName;
                    courseListModel._token = courseListViewModel._token;
                    courseListModel._failure = true;
                    courseListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                courseListModel._message = es.Message;
                courseListModel._failure = true;
                courseListModel._tenantName = courseListViewModel._tenantName;
                courseListModel._token = courseListViewModel._token;
            }
            return courseListModel;
        }
        /// <summary>
        /// Add Course Section
        /// </summary>
        /// <param name="courseSectionAddViewModel"></param>
        /// <returns></returns>
        public CourseSectionAddViewModel AddCourseSection(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var checkCourseSectionName = this.context?.CourseSection.Where(x => x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.CourseSectionName.ToLower() == courseSectionAddViewModel.courseSection.CourseSectionName.ToLower()).FirstOrDefault();

                    if (checkCourseSectionName != null)
                    {
                        courseSectionAddViewModel._failure = true;
                        courseSectionAddViewModel._message = "Course Section Name Already Exists";
                    }
                    else
                    {
                        if (!(bool)courseSectionAddViewModel.courseSection.DurationBasedOnPeriod)
                        {
                            var CalenderData = this.context?.SchoolCalendars.FirstOrDefault(x => x.CalenderId == courseSectionAddViewModel.courseSection.CalendarId);


                        if (CalenderData != null)
                        {
                            if (CalenderData.StartDate >= courseSectionAddViewModel.courseSection.DurationStartDate || CalenderData.EndDate <= courseSectionAddViewModel.courseSection.DurationEndDate)
                            {
                                courseSectionAddViewModel._message = "Start Date And End Date of Course Section Should Be Between Start Date And End Date of School Calender";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }
                        }
                    }

                        int? CourseSectionId = 1;

                        var CourseSectionData = this.context?.CourseSection.Where(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId).OrderByDescending(x => x.CourseSectionId).FirstOrDefault();


                        if (CourseSectionData != null)
                        {
                            CourseSectionId = CourseSectionData.CourseSectionId + 1;
                        }

                        if (courseSectionAddViewModel.MarkingPeriodId != null)
                        {
                            string[] markingPeriodID = courseSectionAddViewModel.MarkingPeriodId.Split("_");

                            if (markingPeriodID.First() == "0")
                            {
                                courseSectionAddViewModel.courseSection.YrMarkingPeriodId = Convert.ToInt32(markingPeriodID[1]);
                            }

                            if (markingPeriodID.First() == "1")
                            {
                                courseSectionAddViewModel.courseSection.SmstrMarkingPeriodId = Convert.ToInt32(markingPeriodID[1]);
                            }

                            if (markingPeriodID.First() == "2")
                            {
                                courseSectionAddViewModel.courseSection.QtrMarkingPeriodId = Convert.ToInt32(markingPeriodID[1]);
                            }
                        }
                        courseSectionAddViewModel._message = "Course Section Added Successfully.";
                        
                            switch (courseSectionAddViewModel.courseSection.ScheduleType.ToLower())
                            {
                                case "fixedschedule":

                                    if (AddFixedSchedule(courseSectionAddViewModel, CourseSectionId)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "variableschedule":

                                    if (AddVariableSchedule(courseSectionAddViewModel, CourseSectionId)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "calendarschedule":

                                    if (AddCalendarSchedule(courseSectionAddViewModel, CourseSectionId)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "blockschedule":

                                    if (AddBlockSchedule(courseSectionAddViewModel , CourseSectionId)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;
                                
                            default:
                                courseSectionAddViewModel._failure = true;
                                courseSectionAddViewModel._message = "Please Provide a Valid Schedule Type.";
                                return courseSectionAddViewModel;                                
                        }
                        
                        courseSectionAddViewModel.courseSection.CourseSectionId = (int)CourseSectionId;
                        courseSectionAddViewModel.courseSection.CreatedOn = DateTime.UtcNow;
                        this.context?.CourseSection.Add(courseSectionAddViewModel.courseSection);
                        this.context?.SaveChanges();
                        courseSectionAddViewModel._failure = false;
                        transaction.Commit();
                    }                    
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    courseSectionAddViewModel._message = es.Message;
                    courseSectionAddViewModel._failure = true;
                }

                return courseSectionAddViewModel;
            }
        }

        /// <summary>
        /// Get All CourseSection
        /// </summary>
        /// <param name="courseSectionViewModel"></param>
        /// <returns></returns>
        public CourseSectionViewModel GetAllCourseSection(CourseSectionViewModel courseSectionViewModel)
        {
            CourseSectionViewModel courseSectionView = new CourseSectionViewModel();
            try
            {
                var courseSectionData = this.context?.CourseSection.Include(x => x.Course).Include(x => x.AttendanceCodeCategories).Include(x => x.GradeScale).Include(x => x.SchoolCalendars).Include(x => x.SchoolYears).Include(x => x.Semesters).Include(x => x.Quarters).Where(x => x.TenantId == courseSectionViewModel.TenantId && x.SchoolId == courseSectionViewModel.SchoolId && x.CourseId == courseSectionViewModel.CourseId).ToList().Select(cs => new CourseSection
                {
                    TenantId = cs.TenantId,
                    SchoolId = cs.SchoolId,
                    CourseId = cs.CourseId,
                    CourseSectionId = cs.CourseSectionId,
                    GradeScaleId = cs.GradeScaleId,
                    CourseSectionName = cs.CourseSectionName,
                    CalendarId = cs.CalendarId,
                    AttendanceCategoryId = cs.AttendanceCategoryId,
                    CreditHours = cs.CreditHours,
                    Seats = cs.Seats,
                    IsWeightedCourse = cs.IsWeightedCourse,
                    AffectsClassRank = cs.AffectsClassRank,
                    AffectsHonorRoll = cs.AffectsHonorRoll,
                    OnlineClassRoom = cs.OnlineClassRoom,
                    OnlineClassroomUrl = cs.OnlineClassroomUrl,
                    OnlineClassroomPassword = cs.OnlineClassroomPassword,
                    StandardGradeScaleId = cs.StandardGradeScaleId,
                    UseStandards = cs.UseStandards,
                    DurationBasedOnPeriod = cs.DurationBasedOnPeriod,
                    DurationStartDate = cs.DurationStartDate,
                    DurationEndDate = cs.DurationEndDate,
                    ScheduleType = cs.ScheduleType,
                    MeetingDays = cs.MeetingDays,
                    AttendanceTaken = cs.AttendanceTaken,
                    IsActive = cs.IsActive,
                    CreatedBy = cs.CreatedBy,
                    CreatedOn = cs.CreatedOn,
                    UpdatedBy = cs.UpdatedBy,
                    UpdatedOn = cs.UpdatedOn,
                    QtrMarkingPeriodId = cs.QtrMarkingPeriodId,
                    SmstrMarkingPeriodId = cs.SmstrMarkingPeriodId,
                    YrMarkingPeriodId = cs.YrMarkingPeriodId,
                    Course = new Course { CourseTitle = cs.Course.CourseTitle },
                    GradeScale = new GradeScale { GradeScaleName = cs.GradeScale.GradeScaleName },
                    AttendanceCodeCategories =cs.AttendanceCodeCategories != null ? new AttendanceCodeCategories { Title = cs.AttendanceCodeCategories.Title } : null,
                    SchoolCalendars = cs.SchoolCalendars != null ? new SchoolCalendars { TenantId = cs.SchoolCalendars.TenantId, SchoolId = cs.SchoolCalendars.SchoolId, CalenderId = cs.SchoolCalendars.CalenderId, Title = cs.SchoolCalendars.Title, StartDate = cs.SchoolCalendars.StartDate, EndDate = cs.SchoolCalendars.EndDate, AcademicYear = cs.SchoolCalendars.AcademicYear, DefaultCalender = cs.SchoolCalendars.DefaultCalender, Days = cs.SchoolCalendars.Days, RolloverId = cs.SchoolCalendars.RolloverId, VisibleToMembershipId = cs.SchoolCalendars.VisibleToMembershipId, LastUpdated = cs.SchoolCalendars.LastUpdated, UpdatedBy = cs.SchoolCalendars.UpdatedBy } : null,
                    Quarters = cs.Quarters != null ? new Quarters { Title = cs.Quarters.Title, StartDate = cs.Quarters.StartDate, EndDate = cs.Quarters.EndDate, } : null,
                    Semesters = cs.Semesters != null ? new Semesters { Title = cs.Semesters.Title, StartDate = cs.Semesters.StartDate, EndDate = cs.Semesters.EndDate, } : null,
                    SchoolYears = cs.SchoolYears != null ? new SchoolYears { Title = cs.SchoolYears.Title, StartDate = cs.SchoolYears.StartDate, EndDate = cs.SchoolYears.EndDate, } : null,

                });

                if (courseSectionData.Count() > 0)
                {
                    string markingId = null;
                    foreach (var courseSection in courseSectionData)
                    {
                        if (courseSection.YrMarkingPeriodId != null)
                        {
                            markingId = "0" + "_" + courseSection.YrMarkingPeriodId;
                        }
                        if (courseSection.SmstrMarkingPeriodId != null)
                        {
                            markingId = "1" + "_" + courseSection.SmstrMarkingPeriodId;
                        }
                        if (courseSection.QtrMarkingPeriodId != null)
                        {
                            markingId = "2" + "_" + courseSection.QtrMarkingPeriodId;
                        }

                        if (courseSection.ScheduleType.ToLower() == "Fixed Schedule (1)".ToLower())
                        {
                            var fixedScheduleData = this.context?.CourseFixedSchedule.Include(f => f.Rooms).Include(f => f.BlockPeriod).Where(x => x.TenantId == courseSection.TenantId && x.SchoolId == courseSection.SchoolId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId).Select(s => new CourseFixedSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, RoomId = s.RoomId, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).FirstOrDefault();
                            if (fixedScheduleData != null)
                            {
                                courseSection.ScheduleType = "Fixed Schedule";
                                GetCourseSectionForView getFixedSchedule = new GetCourseSectionForView
                                {
                                    courseFixedSchedule = fixedScheduleData,
                                    courseSection = courseSection,
                                    MarkingPeriod = markingId

                                };
                                courseSectionView.getCourseSectionForView.Add(getFixedSchedule);
                            }
                        }

                        if (courseSection.ScheduleType.ToLower() == "Variable Schedule (2)".ToLower())
                        {
                            var variableScheduleData = this.context?.CourseVariableSchedule.Where(x => x.TenantId == courseSection.TenantId && x.SchoolId == courseSection.SchoolId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId).Include(f => f.Rooms).Include(f => f.BlockPeriod).Select(s => new CourseVariableSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, Day = s.Day, RoomId = s.RoomId, TakeAttendance = s.TakeAttendance, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();

                            if (variableScheduleData.Count > 0)
                            {
                                courseSection.ScheduleType = "Variable Schedule";
                                GetCourseSectionForView getVariableSchedule = new GetCourseSectionForView
                                {
                                    courseVariableSchedule = variableScheduleData,
                                    courseSection = courseSection,
                                    MarkingPeriod = markingId
                                };
                                courseSectionView.getCourseSectionForView.Add(getVariableSchedule);
                            }
                        }

                        if (courseSection.ScheduleType.ToLower() == "Calendar Schedule (3)".ToLower())
                        {
                            var calendarScheduleData = this.context?.CourseCalendarSchedule.Where(x => x.TenantId == courseSection.TenantId && x.SchoolId == courseSection.SchoolId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId).Include(f => f.Rooms).Include(f => f.BlockPeriod).Select(s => new CourseCalendarSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, Date = s.Date, RoomId = s.RoomId, TakeAttendance = s.TakeAttendance, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();
                            if (calendarScheduleData.Count > 0)
                            {
                                courseSection.ScheduleType = "Calendar Schedule";
                                GetCourseSectionForView getCalendarSchedule = new GetCourseSectionForView
                                {
                                    courseCalendarSchedule = calendarScheduleData,
                                    courseSection = courseSection,
                                    MarkingPeriod = markingId
                                };
                                courseSectionView.getCourseSectionForView.Add(getCalendarSchedule);
                            }
                        }

                        if (courseSection.ScheduleType.ToLower() == "Block Schedule (4)".ToLower())
                        {
                            var blockScheduleData = this.context?.CourseBlockSchedule.Where(x => x.TenantId == courseSection.TenantId && x.SchoolId == courseSection.SchoolId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId).Include(f => f.Rooms).Include(f => f.BlockPeriod).Select(s => new CourseBlockSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, RoomId = s.RoomId, PeriodId = s.PeriodId, BlockId = s.BlockId, TakeAttendance = s.TakeAttendance, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();
                            if (blockScheduleData.Count > 0)
                            {
                                courseSection.ScheduleType = "Block/Rotating Schedule";
                                GetCourseSectionForView getBlockSchedule = new GetCourseSectionForView
                                {
                                    courseBlockSchedule = blockScheduleData,
                                    courseSection = courseSection,
                                    MarkingPeriod = markingId
                                };
                                courseSectionView.getCourseSectionForView.Add(getBlockSchedule);
                            }
                        }
                    }
                    courseSectionView.TenantId = courseSectionViewModel.TenantId;
                    courseSectionView.SchoolId = courseSectionViewModel.SchoolId;
                    courseSectionView.CourseId = courseSectionViewModel.CourseId;
                    courseSectionView._tenantName = courseSectionViewModel._tenantName;
                    courseSectionView._token = courseSectionViewModel._token;
                    courseSectionView._failure = false;
                }
                else
                {
                    courseSectionView._failure = true;
                    courseSectionView._message = NORECORDFOUND;
                    courseSectionView._tenantName = courseSectionViewModel._tenantName;
                    courseSectionView._token = courseSectionViewModel._token;
                }
            }
            catch (Exception es)
            {
                courseSectionView._failure = true;
                courseSectionView._message = es.Message;
            }
            return courseSectionView;
        }




        private CourseSectionAddViewModel AddFixedSchedule(CourseSectionAddViewModel courseSectionAddViewModel , int? CourseSectionId)
        {
            try
            {
                if (courseSectionAddViewModel.courseFixedSchedule != null)
                {
                    string[] meetingDays = courseSectionAddViewModel.courseSection.MeetingDays.Split("|");
                    string message = null;

                    foreach (var meetingDay in meetingDays)
                    { 
                       var courseSectionList = this.context?.CourseSection.
                                    Join(this.context?.CourseFixedSchedule,
                                    cs => cs.CourseSectionId, cf => cf.CourseSectionId,
                                    (cs, cfs) => new { cs, cfs }).Where(c=>c.cs.TenantId==courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId==courseSectionAddViewModel.courseSection.SchoolId && c.cfs.RoomId== courseSectionAddViewModel.courseFixedSchedule.RoomId && c.cfs.PeriodId==courseSectionAddViewModel.courseFixedSchedule.PeriodId && c.cs.MeetingDays.Contains(meetingDay)).ToList();


                        if (courseSectionList.Count>0)
                        {
                            courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }

                        Days days = new Days();
                        var Day = Enum.GetName(days.GetType(), Convert.ToInt32(meetingDay));
                        
                        var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == Day.ToLower() && e.SchoolId==courseSectionAddViewModel.courseSection.SchoolId && e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.RoomId==courseSectionAddViewModel.courseFixedSchedule.RoomId && e.PeriodId==courseSectionAddViewModel.courseFixedSchedule.PeriodId).ToList();

                        if (variableScheduleDataList.Count > 0)
                        {
                            courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }

                        var calenderScheduleList = this.context?.CourseCalendarSchedule.Where(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId== courseSectionAddViewModel.courseFixedSchedule.RoomId && e.PeriodId==courseSectionAddViewModel.courseFixedSchedule.PeriodId).ToList();

                        if (calenderScheduleList.Count > 0)
                        {
                            foreach (var calenderSchedule in calenderScheduleList)
                            {
                                var calenderDay = calenderSchedule.Date.Value.DayOfWeek.ToString();

                                if (calenderDay != null)
                                {
                                    int dayValue = (int)Enum.Parse(typeof(Days), calenderDay);                                   

                                    if (dayValue.ToString() == meetingDay)
                                    {
                                        if (message != null)
                                        {
                                            message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")" + "," + message;
                                        }
                                        else
                                        {
                                            message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")";
                                        }
                                        courseSectionAddViewModel._message = message + "is associate with Course Calender Schedule";
                                    }
                                }
                            }
                        }
                    }

                    var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseSectionAddViewModel.courseFixedSchedule.TenantId && e.SchoolId == courseSectionAddViewModel.courseFixedSchedule.SchoolId && e.RoomId == courseSectionAddViewModel.courseFixedSchedule.RoomId)?.Capacity;

                    if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                    {
                        courseSectionAddViewModel._message = "Invalid Seat Capacity";
                        courseSectionAddViewModel._failure = true;
                        return courseSectionAddViewModel;
                    }
                    else
                    {
                        int? fixedscheduleSerial = 1;

                        var CourseSectionfixedscheduleData = this.context?.CourseFixedSchedule.Where(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId).OrderByDescending(x => x.Serial).FirstOrDefault();

                        if (CourseSectionfixedscheduleData != null)
                        {
                            fixedscheduleSerial = CourseSectionfixedscheduleData.Serial + 1;
                        }

                        var courseFixedSchedule = new CourseFixedSchedule()
                        {
                            TenantId = courseSectionAddViewModel.courseSection.TenantId,
                            SchoolId = courseSectionAddViewModel.courseSection.SchoolId,
                            CourseId = courseSectionAddViewModel.courseSection.CourseId,
                            CourseSectionId = (int)CourseSectionId,
                            GradeScaleId = courseSectionAddViewModel.courseSection.GradeScaleId,
                            Serial = (int)fixedscheduleSerial,
                            RoomId = courseSectionAddViewModel.courseFixedSchedule.RoomId,
                            PeriodId = courseSectionAddViewModel.courseFixedSchedule.PeriodId,
                            CreatedBy = courseSectionAddViewModel.courseFixedSchedule.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            BlockId= courseSectionAddViewModel.courseFixedSchedule.BlockId
                        };

                        this.context?.CourseFixedSchedule.Add(courseFixedSchedule);
                        courseSectionAddViewModel.courseSection.ScheduleType = "Fixed Schedule (1)";
                        courseSectionAddViewModel._failure = false;
                    }                    
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel AddVariableSchedule(CourseSectionAddViewModel courseSectionAddViewModel , int? CourseSectionId)
        {
            try
            {
                List<CourseVariableSchedule> courseVariableScheduleList = new List<CourseVariableSchedule>();
                string message = null;

                if (courseSectionAddViewModel.courseVariableScheduleList.Count > 0)
                {
                    var variableScheduleData = this.context?.CourseVariableSchedule.FirstOrDefault(s => s.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && s.TenantId == courseSectionAddViewModel.courseSection.TenantId);


                    int? variablescheduleSerial = 1;

                    var CourseSectionvariablescheduleData = this.context?.CourseVariableSchedule.Where(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId).OrderByDescending(x => x.Serial).FirstOrDefault();

                    if (CourseSectionvariablescheduleData != null)
                    {
                        variablescheduleSerial = CourseSectionvariablescheduleData.Serial + 1;
                    }

                    foreach (var courseVariableSchedules in courseSectionAddViewModel.courseVariableScheduleList)
                    {
                        Days days = new Days();

                        var Day = Enum.GetName(days.GetType(), Convert.ToInt32(courseVariableSchedules.Day));

                        var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == Day.ToLower() && e.TenantId== courseSectionAddViewModel.courseSection.TenantId && e.SchoolId== courseSectionAddViewModel.courseSection.SchoolId && e.RoomId== courseVariableSchedules.RoomId && e.PeriodId== courseVariableSchedules.PeriodId).ToList();

                        if (variableScheduleDataList.Count > 0)
                        {
                            courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }

                        var courseSectionList = this.context?.CourseSection.
                                    Join(this.context?.CourseFixedSchedule,
                                    cs => cs.CourseSectionId, cf => cf.CourseSectionId,
                                    (cs, cf) => new { cs, cf }).Where(c => c.cs.TenantId == courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && c.cf.RoomId == courseVariableSchedules.RoomId && c.cf.PeriodId == courseVariableSchedules.PeriodId && c.cs.MeetingDays.Contains(courseVariableSchedules.Day)).ToList();

                        if (courseSectionList.Count > 0)
                        {
                            courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }

                        var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseVariableSchedules.TenantId && e.SchoolId == courseVariableSchedules.SchoolId && e.RoomId == courseVariableSchedules.RoomId)?.Capacity;

                        if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                        {
                            courseSectionAddViewModel._message = "Invalid Seat Capacity";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }
                        else
                        {
                            var calenderScheduleList = this.context?.CourseCalendarSchedule.Where(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId== courseVariableSchedules.RoomId && e.PeriodId== courseVariableSchedules.PeriodId).ToList();
                            
                            if (calenderScheduleList.Count>0)
                            {
                                
                                foreach (var calenderSchedule in calenderScheduleList)
                                {
                                    var calenderDay = calenderSchedule.Date.Value.DayOfWeek.ToString();

                                    if (calenderDay != null)
                                    {
                                        int dayValue = (int)Enum.Parse(typeof(Days), calenderDay);

                                        if (dayValue.ToString() == courseVariableSchedules.Day)
                                        {
                                            if (message != null)
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")" + "," + message;
                                            }
                                            else
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")";
                                            }
                                            courseSectionAddViewModel._message = message + "is associate with Course Calender Schedule";
                                        }
                                    }
                                }
                            
                            }

                            var courseeVariableSchedule = new CourseVariableSchedule()
                            {
                                TenantId = courseSectionAddViewModel.courseSection.TenantId,
                                SchoolId = courseSectionAddViewModel.courseSection.SchoolId,
                                CourseId = courseSectionAddViewModel.courseSection.CourseId,
                                CourseSectionId = (int)CourseSectionId,
                                GradeScaleId = courseSectionAddViewModel.courseSection.GradeScaleId,
                                Serial = (int)variablescheduleSerial,
                                Day = Day,
                                PeriodId = courseVariableSchedules.PeriodId,
                                RoomId = courseVariableSchedules.RoomId,
                                TakeAttendance = courseVariableSchedules.TakeAttendance,
                                CreatedBy = courseSectionAddViewModel.courseSection.CreatedBy,
                                CreatedOn = DateTime.UtcNow,
                                BlockId= courseVariableSchedules.BlockId

                            };
                            courseVariableScheduleList.Add(courseeVariableSchedule);
                            variablescheduleSerial++;
                        }
                    }
                    this.context?.CourseVariableSchedule.AddRange(courseVariableScheduleList);
                    courseSectionAddViewModel.courseSection.ScheduleType = "Variable Schedule (2)";
                    courseSectionAddViewModel._failure = false;
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel AddCalendarSchedule(CourseSectionAddViewModel courseSectionAddViewModel , int? CourseSectionId)
        {
            try
            {
                List<CourseCalendarSchedule> courseCalendarScheduleList = new List<CourseCalendarSchedule>();                

                if (courseSectionAddViewModel.courseCalendarScheduleList.Count > 0)
                {
                    int? calendarscheduleSerial = 1;

                    var CourseSectioncalendarscheduleData = this.context?.CourseCalendarSchedule.Where(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId).OrderByDescending(x => x.Serial).FirstOrDefault();

                    if (CourseSectioncalendarscheduleData != null)
                    {
                        calendarscheduleSerial = CourseSectioncalendarscheduleData.Serial + 1;
                    }

                    foreach (var courseCalendarSchedule in courseSectionAddViewModel.courseCalendarScheduleList)
                    {
                        var calenderDay = courseCalendarSchedule.Date.Value.DayOfWeek.ToString();
                        
                        if (calenderDay != null)
                        {
                            int dayValue = (int)Enum.Parse(typeof(Days), calenderDay);

                            var courseSectionList = this.context?.CourseSection.
                                Join(this.context?.CourseFixedSchedule,
                                cs => cs.CourseSectionId, cf => cf.CourseSectionId,
                                (cs, cf) => new { cs, cf }).Where(c => c.cs.TenantId == courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && c.cf.RoomId == courseCalendarSchedule.RoomId && c.cf.PeriodId == courseCalendarSchedule.PeriodId && c.cs.MeetingDays.Contains(dayValue.ToString())).ToList();

                        if (courseSectionList.Count > 0)
                        {                            
                            courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }
                    }

                            var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == calenderDay.ToLower() && e.TenantId==courseSectionAddViewModel.courseSection.TenantId && e.SchoolId==courseSectionAddViewModel.courseSection.SchoolId && e.RoomId== courseCalendarSchedule.RoomId && e.PeriodId== courseCalendarSchedule.PeriodId).ToList();

                            if (variableScheduleDataList.Count > 0)
                            {                            
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var calenderScheduleDataList = this.context?.CourseCalendarSchedule.Where(e => e.Date == courseCalendarSchedule.Date && e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId== courseCalendarSchedule.RoomId && e.PeriodId== courseCalendarSchedule.PeriodId).ToList();

                            if (calenderScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Calendar Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                        var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId == courseCalendarSchedule.RoomId)?.Capacity;

                            if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                            {
                                courseSectionAddViewModel._message = "Invalid Seat Capacity";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }
                            else
                            {
                                var courseCalenderSchedule = new CourseCalendarSchedule()
                                {
                                    TenantId = courseSectionAddViewModel.courseSection.TenantId,
                                    SchoolId = courseSectionAddViewModel.courseSection.SchoolId,
                                    CourseId = courseSectionAddViewModel.courseSection.CourseId,
                                    CourseSectionId = (int)CourseSectionId,
                                    GradeScaleId = courseSectionAddViewModel.courseSection.GradeScaleId,
                                    Serial = (int)calendarscheduleSerial,
                                    BlockId = courseCalendarSchedule.BlockId,
                                    Date= courseCalendarSchedule.Date,
                                    PeriodId = courseCalendarSchedule.PeriodId,
                                    RoomId = courseCalendarSchedule.RoomId,
                                    TakeAttendance = courseCalendarSchedule.TakeAttendance,
                                    CreatedBy = courseSectionAddViewModel.courseSection.CreatedBy,
                                    CreatedOn = DateTime.UtcNow
                                };
                                courseCalendarScheduleList.Add(courseCalenderSchedule);
                                calendarscheduleSerial++;
                            }                        
                    }
                    this.context?.CourseCalendarSchedule.AddRange(courseCalendarScheduleList);
                    courseSectionAddViewModel.courseSection.ScheduleType = "Calendar Schedule (3)";
                    courseSectionAddViewModel._failure = false;
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel AddBlockSchedule(CourseSectionAddViewModel courseSectionAddViewModel , int? CourseSectionId)
        {
            try
            {
                List<CourseBlockSchedule> courseBlockScheduleList = new List<CourseBlockSchedule>();

                if (courseSectionAddViewModel.courseBlockScheduleList.Count > 0)
                {

                    int? blockscheduleSerial = 1;

                    var CourseSectionblockscheduleData = this.context?.CourseBlockSchedule.Where(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId).OrderByDescending(x => x.Serial).FirstOrDefault();

                    if (CourseSectionblockscheduleData != null)
                    {
                        blockscheduleSerial = CourseSectionblockscheduleData.Serial + 1;
                    }

                    foreach (var courseBlockSchedules in courseSectionAddViewModel.courseBlockScheduleList)
                    {


                        var blockScheduleData = this.context?.CourseBlockSchedule.Where(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.BlockId == courseBlockSchedules.BlockId && e.RoomId == courseBlockSchedules.RoomId && e.PeriodId == courseBlockSchedules.PeriodId).ToList();
                        
                        if (blockScheduleData.Count>0)
                        {
                            courseSectionAddViewModel._failure = true;
                            courseSectionAddViewModel._message = "Room is not available for this block and period";
                            return courseSectionAddViewModel;
                        }

                        var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId == courseBlockSchedules.RoomId)?.Capacity;

                        if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                        {
                            courseSectionAddViewModel._message = "Invalid Seat Capacity";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }
                        else
                        {
                            var courseBlockSchedule = new CourseBlockSchedule()
                            {
                                TenantId = courseSectionAddViewModel.courseSection.TenantId,
                                SchoolId = courseSectionAddViewModel.courseSection.SchoolId,
                                CourseId = courseSectionAddViewModel.courseSection.CourseId,
                                CourseSectionId = (int)CourseSectionId,
                                GradeScaleId = courseSectionAddViewModel.courseSection.GradeScaleId,
                                Serial = (int)blockscheduleSerial,
                                BlockId = courseBlockSchedules.BlockId,
                                PeriodId = courseBlockSchedules.PeriodId,
                                RoomId = courseBlockSchedules.RoomId,
                                TakeAttendance = courseBlockSchedules.TakeAttendance,
                                CreatedBy = courseSectionAddViewModel.courseSection.CreatedBy,
                                CreatedOn = DateTime.UtcNow
                            };
                            courseBlockScheduleList.Add(courseBlockSchedule);
                            blockscheduleSerial++;
                        }
                    }
                    this.context?.CourseBlockSchedule.AddRange(courseBlockScheduleList);
                    courseSectionAddViewModel.courseSection.ScheduleType = "Block Schedule (4)";
                    courseSectionAddViewModel._failure = false;
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        /// <summary>
        /// Update Course Section
        /// </summary>
        /// <param name="courseSectionAddViewModel"></param>
        /// <returns></returns>
        public CourseSectionAddViewModel UpdateCourseSection(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var courseSectionUpdate = this.context?.CourseSection.FirstOrDefault(x => x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && x.CourseId == courseSectionAddViewModel.courseSection.CourseId && x.CourseSectionId == courseSectionAddViewModel.courseSection.CourseSectionId);

                    if (courseSectionUpdate != null)
                    {
                        if (!(bool)courseSectionAddViewModel.courseSection.DurationBasedOnPeriod)
                        {
                            var CalenderData = this.context?.SchoolCalendars.FirstOrDefault(x => x.CalenderId == courseSectionAddViewModel.courseSection.CalendarId);

                            if (CalenderData != null)
                            {
                                if (CalenderData.StartDate >= courseSectionAddViewModel.courseSection.DurationStartDate || CalenderData.EndDate <= courseSectionAddViewModel.courseSection.DurationEndDate)
                                {
                                    courseSectionAddViewModel._message = "Start Date And End Date of Course Section Should Be Between Start Date And End Date of School Calender";
                                    courseSectionAddViewModel._failure = true;
                                    return courseSectionAddViewModel;
                                }
                            }
                        }

                        var courseSectionNameExists = this.context?.CourseSection.FirstOrDefault(x => x.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && x.TenantId == courseSectionAddViewModel.courseSection.TenantId && x.CourseSectionId != courseSectionAddViewModel.courseSection.CourseSectionId && x.CourseSectionName.ToLower() == courseSectionAddViewModel.courseSection.CourseSectionName.ToLower());
                        if (courseSectionNameExists != null)
                        {
                            courseSectionAddViewModel._failure = true;
                            courseSectionAddViewModel._message = "Course Section Name Already Exists";
                            return courseSectionAddViewModel;
                        }

                        courseSectionAddViewModel._message = "Course Section Updated Successfully";

                        if (courseSectionAddViewModel.courseSection.ScheduleType != null)
                        {
                            switch (courseSectionAddViewModel.courseSection.ScheduleType.ToLower())
                            {
                                case "fixedschedule":
                                    if (UpdateFixedSchedule(courseSectionAddViewModel)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "variableschedule":
                                    if (UpdateVariableSchedule(courseSectionAddViewModel)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "calendarschedule":
                                    if (UpdateCalendarSchedule(courseSectionAddViewModel)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;

                                case "blockschedule":
                                    if (UpdateBlockSchedule(courseSectionAddViewModel)._failure)
                                    {
                                        return courseSectionAddViewModel;
                                    }
                                    break;
                                default:
                                    courseSectionAddViewModel._failure = true;
                                    courseSectionAddViewModel._message = "Please Provide a Valid Schedule Type.";
                                    return courseSectionAddViewModel;
                            }

                            if (courseSectionAddViewModel.MarkingPeriodId != null)
                            {
                                var markingPeriodid = courseSectionAddViewModel.MarkingPeriodId.Split("_", StringSplitOptions.RemoveEmptyEntries);

                                if (markingPeriodid.First() == "2")
                                {
                                    courseSectionAddViewModel.courseSection.QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                                }
                                if (markingPeriodid.First() == "1")
                                {
                                    courseSectionAddViewModel.courseSection.SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                                }
                                if (markingPeriodid.First() == "0")
                                {
                                    courseSectionAddViewModel.courseSection.YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                                }
                            }
                            courseSectionAddViewModel.courseSection.CreatedBy = courseSectionUpdate.CreatedBy;
                            courseSectionAddViewModel.courseSection.CreatedOn = courseSectionUpdate.CreatedOn;
                            courseSectionAddViewModel.courseSection.UpdatedOn = DateTime.UtcNow;
                            this.context.Entry(courseSectionUpdate).CurrentValues.SetValues(courseSectionAddViewModel.courseSection);
                            this.context?.SaveChanges();
                            courseSectionAddViewModel._failure = false;                            
                        }
                    }
                    else
                    {
                        courseSectionAddViewModel._failure = true;
                        courseSectionAddViewModel._message = NORECORDFOUND;
                    }
                    transaction.Commit();
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    courseSectionAddViewModel._failure = true;
                    courseSectionAddViewModel._message = es.Message;
                }
            }
            return courseSectionAddViewModel;
        }


        private CourseSectionAddViewModel UpdateFixedSchedule(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            try
            {
                if (courseSectionAddViewModel.courseFixedSchedule != null)
                {
                    var fixedScheduleDataUpdate = this.context?.CourseFixedSchedule.Where(x => x.TenantId == courseSectionAddViewModel.courseFixedSchedule.TenantId && x.SchoolId == courseSectionAddViewModel.courseFixedSchedule.SchoolId && x.CourseId == courseSectionAddViewModel.courseFixedSchedule.CourseId && x.CourseSectionId == courseSectionAddViewModel.courseFixedSchedule.CourseSectionId && x.Serial == courseSectionAddViewModel.courseFixedSchedule.Serial).FirstOrDefault();

                    if (fixedScheduleDataUpdate != null)
                    {
                        var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseSectionAddViewModel.courseFixedSchedule.TenantId && e.SchoolId == courseSectionAddViewModel.courseFixedSchedule.SchoolId && e.RoomId == courseSectionAddViewModel.courseFixedSchedule.RoomId && e.IsActive == true)?.Capacity;
                        
                        if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                        {
                            courseSectionAddViewModel._message = "Invalid Seat Capacity";
                            courseSectionAddViewModel._failure = true;
                            return courseSectionAddViewModel;
                        }

                        string[] meetingDays = courseSectionAddViewModel.courseSection.MeetingDays.Split("|");
                        string message = null;
                        
                        foreach (var meetingDay in meetingDays)
                        {
                            var courseSectionList = this.context?.CourseSection.
                            Join(this.context?.CourseFixedSchedule,
                             cs => cs.CourseSectionId, cfs => cfs.CourseSectionId,
                            (cs, cfs) => new { cs, cfs }).Where(c => c.cs.TenantId == courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && c.cfs.RoomId == courseSectionAddViewModel.courseFixedSchedule.RoomId && c.cfs.PeriodId == courseSectionAddViewModel.courseFixedSchedule.PeriodId && c.cs.CourseSectionId != courseSectionAddViewModel.courseSection.CourseSectionId && c.cfs.Serial != courseSectionAddViewModel.courseFixedSchedule.Serial && c.cs.MeetingDays.Contains(meetingDay)).ToList();

                            if (courseSectionList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            Days days = new Days();
                            var Day = Enum.GetName(days.GetType(), Convert.ToInt32(meetingDay));

                            var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == Day.ToLower() && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.PeriodId == courseSectionAddViewModel.courseFixedSchedule.PeriodId && e.RoomId == courseSectionAddViewModel.courseFixedSchedule.RoomId).ToList();

                            if (variableScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var calenderScheduleList = this.context?.CourseCalendarSchedule.Where(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId == courseSectionAddViewModel.courseFixedSchedule.RoomId && e.PeriodId == courseSectionAddViewModel.courseFixedSchedule.PeriodId).ToList();

                            if (calenderScheduleList.Count > 0)
                            {
                                foreach (var calenderSchedule in calenderScheduleList)
                                {
                                    var calenderDay = calenderSchedule.Date.Value.DayOfWeek.ToString();

                                    if (calenderDay != null)
                                    {
                                        int calenderDayValue = (int)Enum.Parse(typeof(Days), calenderDay);
                                        if (calenderDayValue.ToString() == meetingDay)
                                        {
                                            if (message != null)
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")" + "," + message;
                                            }
                                            else
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")";
                                            }
                                            courseSectionAddViewModel._message = message + "is associate with Course Calender Schedule";
                                        }
                                    }
                                }
                            }
                        }
                        courseSectionAddViewModel.courseFixedSchedule.CreatedBy = fixedScheduleDataUpdate.CreatedBy;
                        courseSectionAddViewModel.courseFixedSchedule.CreatedOn = fixedScheduleDataUpdate.CreatedOn;
                        courseSectionAddViewModel.courseFixedSchedule.UpdatedOn = DateTime.UtcNow;
                        this.context.Entry(fixedScheduleDataUpdate).CurrentValues.SetValues(courseSectionAddViewModel.courseFixedSchedule);
                    }
                    courseSectionAddViewModel._failure = false;
                    courseSectionAddViewModel.courseSection.ScheduleType = "Fixed Schedule (1)";
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel UpdateVariableSchedule(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            try
            {
                if (courseSectionAddViewModel.courseVariableScheduleList.Count > 0)
                {
                    foreach (var courseVariableSchedules in courseSectionAddViewModel.courseVariableScheduleList)
                    {
                        var variableScheduleDataUpdate = this.context?.CourseVariableSchedule.Where(x => x.TenantId == courseVariableSchedules.TenantId && x.SchoolId == courseVariableSchedules.SchoolId && x.CourseId == courseVariableSchedules.CourseId && x.CourseSectionId == courseVariableSchedules.CourseSectionId && x.Serial == courseVariableSchedules.Serial).FirstOrDefault();

                        if (variableScheduleDataUpdate != null)
                        {
                            var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseVariableSchedules.TenantId && e.SchoolId == courseVariableSchedules.SchoolId && e.RoomId == courseVariableSchedules.RoomId && e.IsActive == true)?.Capacity;
                            if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                            {
                                courseSectionAddViewModel._message = "Invalid Seat Capacity";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            Days days = new Days();
                            var Day = Enum.GetName(days.GetType(), Convert.ToInt32(courseVariableSchedules.Day));

                            var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == Day.ToLower() && e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.Serial != courseVariableSchedules.Serial && e.PeriodId == courseVariableSchedules.PeriodId && e.RoomId == courseVariableSchedules.RoomId).ToList();

                            if (variableScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var courseSectionList = this.context?.CourseSection.
                                                    Join(this.context?.CourseFixedSchedule,
                                                    cs => cs.CourseSectionId, cfs => cfs.CourseSectionId,
                                                   (cs, cfs) => new { cs, cfs }).Where(c => c.cs.TenantId == courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && c.cfs.RoomId == courseVariableSchedules.RoomId && c.cfs.PeriodId == courseVariableSchedules.PeriodId && c.cs.CourseSectionId != courseSectionAddViewModel.courseSection.CourseSectionId && c.cs.MeetingDays.Contains(courseVariableSchedules.Day)).ToList();

                            if (courseSectionList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var calenderScheduleList = this.context?.CourseCalendarSchedule.Where(e => e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.RoomId == courseVariableSchedules.RoomId && e.PeriodId == courseVariableSchedules.PeriodId).ToList();

                            if (calenderScheduleList.Count > 0)
                            {
                                string message = null;
                                
                                foreach (var calenderSchedule in calenderScheduleList)
                                {
                                    var calenderDay = calenderSchedule.Date.Value.DayOfWeek.ToString();
                                    if (calenderDay != null)
                                    {
                                        int calenderDayValue = (int)Enum.Parse(typeof(Days), calenderDay);
                                        if (calenderDayValue.ToString() == courseVariableSchedules.Day)
                                        {
                                            if (message != null)
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")" + "," + message;
                                            }
                                            else
                                            {
                                                message = calenderSchedule.Date.Value.Date.ToString("yyyy-MM-dd") + "(" + calenderDay + ")";
                                            }
                                            courseSectionAddViewModel._message = message + "is associate with Course Calender Schedule";
                                        }
                                    }
                                }
                            }
                            courseVariableSchedules.Day = Day;
                            courseVariableSchedules.CreatedBy = variableScheduleDataUpdate.CreatedBy;
                            courseVariableSchedules.CreatedOn = variableScheduleDataUpdate.CreatedOn;
                            courseVariableSchedules.UpdatedOn = DateTime.UtcNow;
                            this.context.Entry(variableScheduleDataUpdate).CurrentValues.SetValues(courseVariableSchedules);
                        }
                    }
                    courseSectionAddViewModel._failure = false;
                    courseSectionAddViewModel.courseSection.ScheduleType = "Variable Schedule (2)";
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel UpdateCalendarSchedule(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            try
            {
                if (courseSectionAddViewModel.courseCalendarScheduleList.Count > 0)
                {
                    foreach (var courseCalendarSchedule in courseSectionAddViewModel.courseCalendarScheduleList)
                    {
                        var calenderScheduleDataUpdate = this.context?.CourseCalendarSchedule.Where(x => x.TenantId == courseCalendarSchedule.TenantId && x.SchoolId == courseCalendarSchedule.SchoolId && x.CourseId == courseCalendarSchedule.CourseId && x.CourseSectionId == courseCalendarSchedule.CourseSectionId && x.Serial == courseCalendarSchedule.Serial).FirstOrDefault();

                        if (calenderScheduleDataUpdate != null)
                        {
                            var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseCalendarSchedule.TenantId && e.SchoolId == courseCalendarSchedule.SchoolId && e.RoomId == courseCalendarSchedule.RoomId && e.IsActive == true)?.Capacity;
                            if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                            {
                                courseSectionAddViewModel._message = "Invalid Seat Capacity";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var calenderDay = courseCalendarSchedule.Date.Value.DayOfWeek.ToString();

                            if (calenderDay != null)
                            {
                                int calenderDayValue = (int)Enum.Parse(typeof(Days), calenderDay);

                                var courseSectionList = this.context?.CourseSection.
                              Join(this.context?.CourseFixedSchedule,
                               cs => cs.CourseSectionId, cfs => cfs.CourseSectionId,
                              (cs, cfs) => new { cs, cfs }).Where(c => c.cs.TenantId == courseSectionAddViewModel.courseSection.TenantId && c.cs.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && c.cfs.RoomId == courseCalendarSchedule.RoomId && c.cfs.PeriodId == courseCalendarSchedule.PeriodId && c.cs.CourseSectionId != courseSectionAddViewModel.courseSection.CourseSectionId && c.cs.MeetingDays.Contains(calenderDayValue.ToString())).ToList();

                                if (courseSectionList.Count > 0)
                                {
                                    courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Fixed Scheduling.";
                                    courseSectionAddViewModel._failure = true;
                                    return courseSectionAddViewModel;
                                }
                            }

                            var variableScheduleDataList = this.context?.CourseVariableSchedule.Where(e => e.Day.ToLower() == calenderDay.ToLower() && e.PeriodId == courseCalendarSchedule.PeriodId && e.RoomId == courseCalendarSchedule.RoomId && e.SchoolId == courseCalendarSchedule.SchoolId && e.TenantId == courseCalendarSchedule.TenantId).ToList();

                            if (variableScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Variable Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var calenderScheduleDataList = this.context?.CourseCalendarSchedule.Where(e => e.Date == courseCalendarSchedule.Date && e.TenantId == courseSectionAddViewModel.courseSection.TenantId && e.SchoolId == courseSectionAddViewModel.courseSection.SchoolId && e.Serial != courseCalendarSchedule.Serial && e.PeriodId == courseCalendarSchedule.PeriodId && e.RoomId == courseCalendarSchedule.RoomId).ToList();

                            if (calenderScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this period due to already booked for Calendar Scheduling.";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;

                            }

                            courseCalendarSchedule.CreatedBy = calenderScheduleDataUpdate.CreatedBy;
                            courseCalendarSchedule.CreatedOn = calenderScheduleDataUpdate.CreatedOn;
                            courseCalendarSchedule.UpdatedOn = DateTime.UtcNow;
                            this.context.Entry(calenderScheduleDataUpdate).CurrentValues.SetValues(courseCalendarSchedule);
                        }
                    }
                    courseSectionAddViewModel.courseSection.ScheduleType = "Calendar Schedule (3)";
                    courseSectionAddViewModel._failure = false;
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }

        private CourseSectionAddViewModel UpdateBlockSchedule(CourseSectionAddViewModel courseSectionAddViewModel)
        {
            try
            {
                if (courseSectionAddViewModel.courseBlockScheduleList.Count > 0)
                {
                    foreach (var courseBlockSchedule in courseSectionAddViewModel.courseBlockScheduleList)
                    {
                        var blockScheduleDataUpdate = this.context?.CourseBlockSchedule.Where(x => x.TenantId == courseBlockSchedule.TenantId && x.SchoolId == courseBlockSchedule.SchoolId && x.CourseId == courseBlockSchedule.CourseId && x.CourseSectionId == courseBlockSchedule.CourseSectionId && x.Serial == courseBlockSchedule.Serial).FirstOrDefault();

                        if (blockScheduleDataUpdate != null)
                        {
                            var roomCapacity = this.context?.Rooms.FirstOrDefault(e => e.TenantId == courseBlockSchedule.TenantId && e.SchoolId == courseBlockSchedule.SchoolId && e.RoomId == courseBlockSchedule.RoomId && e.IsActive == true)?.Capacity;
                            if (roomCapacity < courseSectionAddViewModel.courseSection.Seats)
                            {
                                courseSectionAddViewModel._message = "Invalid Seat Capacity";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            var blockScheduleDataList = this.context?.CourseBlockSchedule.Where(x => x.TenantId == courseBlockSchedule.TenantId && x.SchoolId == courseBlockSchedule.SchoolId && x.BlockId == courseBlockSchedule.BlockId && x.PeriodId == courseBlockSchedule.PeriodId && x.RoomId == courseBlockSchedule.RoomId && x.Serial != courseBlockSchedule.Serial).ToList();

                            if (blockScheduleDataList.Count > 0)
                            {
                                courseSectionAddViewModel._message = "Room is not available for this block and period";
                                courseSectionAddViewModel._failure = true;
                                return courseSectionAddViewModel;
                            }

                            courseBlockSchedule.CreatedBy = blockScheduleDataUpdate.CreatedBy;
                            courseBlockSchedule.CreatedOn = blockScheduleDataUpdate.CreatedOn;
                            courseBlockSchedule.UpdatedOn = DateTime.UtcNow;
                            this.context.Entry(blockScheduleDataUpdate).CurrentValues.SetValues(courseBlockSchedule);
                        }
                    }
                    courseSectionAddViewModel.courseSection.ScheduleType = "Block Schedule (4)";
                    courseSectionAddViewModel._failure = false;
                }
            }
            catch (Exception es)
            {
                courseSectionAddViewModel._failure = true;
                courseSectionAddViewModel._message = es.Message;
            }
            return courseSectionAddViewModel;
        }
    }
}
