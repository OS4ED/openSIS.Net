/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

using Microsoft.EntityFrameworkCore;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class StudentEffortGradeRepository : IStudentEffortGradeRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StudentEffortGradeRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Update Student Effort Grade
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    List<StudentEffortGradeMaster> studentEffortGradeList = new List<StudentEffortGradeMaster>();

                    long Id = 1;
                    var StudentEffortDetails = this.context?.StudentEffortGradeDetail.ToList();

                    if (StudentEffortDetails.Count > 0)
                    {
                        Id = StudentEffortDetails.OrderByDescending(s => s.Id).FirstOrDefault().Id + 1;
                    }

                    int? YrMarkingPeriodId = 0;
                    int? SmstrMarkingPeriodId = 0;
                    int? QtrMarkingPeriodId = 0;

                    if (studentEffortGradeListModel.MarkingPeriodId != null)
                    {
                        var markingPeriodid = studentEffortGradeListModel.MarkingPeriodId.Split("_", StringSplitOptions.RemoveEmptyEntries);

                        if (markingPeriodid.First() == "2")
                        {
                            QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                        if (markingPeriodid.First() == "1")
                        {
                            SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                        if (markingPeriodid.First() == "0")
                        {
                            YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                    }

                    if (studentEffortGradeListModel.studentEffortGradeList.Count > 0)
                    {
                        foreach (var studentEffortGrade in studentEffortGradeListModel.studentEffortGradeList)
                        {
                            if (studentEffortGrade.StudentEffortGradeDetail.ToList().Count() > 0 || studentEffortGrade.StudentEffortGradeDetail != null)
                            {
                                foreach (var studentEffortGradeDetaildata in studentEffortGrade.StudentEffortGradeDetail)
                                {
                                    studentEffortGradeDetaildata.Id = Id;
                                    Id++;
                                }
                            }
                        }

                        var studentEffortGradeData = new List<StudentEffortGradeMaster>();

                        studentEffortGradeData = this.context?.StudentEffortGradeMaster.Where(e => e.SchoolId == studentEffortGradeListModel.SchoolId && e.TenantId == studentEffortGradeListModel.TenantId && e.CalendarId == studentEffortGradeListModel.CalendarId && e.CourseId == studentEffortGradeListModel.CourseId && e.CourseSectionId == studentEffortGradeListModel.CourseSectionId).ToList();

                        if (studentEffortGradeData.Count > 0)
                        {
                            var containStudentEffortGradeSrlno = studentEffortGradeData.Select(x => x.StudentEffortGradeSrlno).Distinct().ToList();

                            List<long> studentEffortGradeSrlnos = new List<long> { };
                            studentEffortGradeSrlnos = containStudentEffortGradeSrlno;

                            var studentEffortGradeDetailsData = this.context?.StudentEffortGradeDetail.Where(e => e.SchoolId == studentEffortGradeListModel.SchoolId && e.TenantId == studentEffortGradeListModel.TenantId && (studentEffortGradeSrlnos == null || (studentEffortGradeSrlnos.Contains(e.StudentEffortGradeSrlno)))).ToList();

                            if (studentEffortGradeDetailsData.Count > 0)
                            {
                                this.context?.StudentEffortGradeDetail.RemoveRange(studentEffortGradeDetailsData);
                            }
                            this.context?.StudentEffortGradeMaster.RemoveRange(studentEffortGradeData);
                            this.context?.SaveChanges();

                            long? studentEffortGradeSrlno = 1;

                            var studentFinalGradeSrlnoData = this.context?.StudentEffortGradeMaster.Where(x => x.SchoolId == studentEffortGradeListModel.SchoolId && x.TenantId == studentEffortGradeListModel.TenantId).OrderByDescending(x => x.StudentEffortGradeSrlno).FirstOrDefault();

                            if (studentFinalGradeSrlnoData != null)
                            {
                                studentEffortGradeSrlno = studentFinalGradeSrlnoData.StudentEffortGradeSrlno + 1;
                            }

                            foreach (var studentEffortGrade in studentEffortGradeListModel.studentEffortGradeList)
                            {
                                var studentEffortGradeUpdate = new StudentEffortGradeMaster()
                                {
                                    TenantId = studentEffortGradeListModel.TenantId,
                                    SchoolId = studentEffortGradeListModel.SchoolId,
                                    StudentId = studentEffortGrade.StudentId,
                                    CourseId = studentEffortGradeListModel.CourseId,
                                    CourseSectionId = studentEffortGradeListModel.CourseSectionId,
                                    AcademicYear = studentEffortGradeListModel.AcademicYear,
                                    CalendarId = studentEffortGradeListModel.CalendarId,
                                    YrMarkingPeriodId = (YrMarkingPeriodId > 0) ? YrMarkingPeriodId : null,
                                    SmstrMarkingPeriodId = (SmstrMarkingPeriodId > 0) ? SmstrMarkingPeriodId : null,
                                    QtrMarkingPeriodId = (QtrMarkingPeriodId > 0) ? QtrMarkingPeriodId : null,
                                    UpdatedBy = studentEffortGradeListModel.CreatedOrUpdatedBy,
                                    UpdatedOn = DateTime.UtcNow,
                                    StudentEffortGradeSrlno = (long)studentEffortGradeSrlno,
                                    TeacherComment = studentEffortGrade.TeacherComment,
                                    StudentEffortGradeDetail = studentEffortGrade.StudentEffortGradeDetail?.Select(c =>
                                    {
                                        c.UpdatedBy = studentEffortGradeListModel.CreatedOrUpdatedBy;
                                        c.UpdatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentEffortGradeList.Add(studentEffortGradeUpdate);
                                studentEffortGradeSrlno++;
                            }
                            studentEffortGradeListModel._message = "Student Effort Grade Updated Succsesfully.";
                        }
                        else
                        {
                            long? studentEffortGradeSrlno = 1;

                            var studentEffortGradeSrlnoData = this.context?.StudentEffortGradeMaster.Where(x => x.SchoolId == studentEffortGradeListModel.SchoolId && x.TenantId == studentEffortGradeListModel.TenantId).OrderByDescending(x => x.StudentEffortGradeSrlno).FirstOrDefault();

                            if (studentEffortGradeSrlnoData != null)
                            {
                                studentEffortGradeSrlno = studentEffortGradeSrlnoData.StudentEffortGradeSrlno + 1;
                            }

                            foreach (var studentEffortGrade in studentEffortGradeListModel.studentEffortGradeList)
                            {
                                var studentEffortGradeAdd = new StudentEffortGradeMaster()
                                {
                                    TenantId = studentEffortGradeListModel.TenantId,
                                    SchoolId = studentEffortGradeListModel.SchoolId,
                                    StudentId = studentEffortGrade.StudentId,
                                    CourseId = studentEffortGradeListModel.CourseId,
                                    CourseSectionId = studentEffortGradeListModel.CourseSectionId,
                                    AcademicYear = studentEffortGradeListModel.AcademicYear,
                                    CalendarId = studentEffortGradeListModel.CalendarId,
                                    YrMarkingPeriodId = (YrMarkingPeriodId > 0) ? YrMarkingPeriodId : null,
                                    SmstrMarkingPeriodId = (SmstrMarkingPeriodId > 0) ? SmstrMarkingPeriodId : null,
                                    QtrMarkingPeriodId = (QtrMarkingPeriodId > 0) ? QtrMarkingPeriodId : null,
                                    CreatedBy = studentEffortGradeListModel.CreatedOrUpdatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    StudentEffortGradeSrlno = (long)studentEffortGradeSrlno,
                                    TeacherComment = studentEffortGrade.TeacherComment,
                                    StudentEffortGradeDetail = studentEffortGrade.StudentEffortGradeDetail?.Select(c =>
                                    {
                                        c.CreatedBy = studentEffortGradeListModel.CreatedOrUpdatedBy;
                                        c.CreatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentEffortGradeList.Add(studentEffortGradeAdd);
                                studentEffortGradeSrlno++;
                            }
                            studentEffortGradeListModel._message = "Student Effort Grade Added succsesfully.";
                        }
                        this.context?.StudentEffortGradeMaster.AddRange(studentEffortGradeList);
                        this.context?.SaveChanges();
                        transaction.Commit();
                        studentEffortGradeListModel._failure = false;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentEffortGradeListModel._failure = true;
                    studentEffortGradeListModel._message = es.Message;
                }
            }
            return studentEffortGradeListModel;
        }

        /// <summary>
        /// Get All Student Effort Grade List
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                int? YrMarkingPeriodId = 0;
                int? SmstrMarkingPeriodId = 0;
                int? QtrMarkingPeriodId = 0;

                if (studentEffortGradeListModel.MarkingPeriodId != null)
                {

                    var markingPeriodid = studentEffortGradeListModel.MarkingPeriodId.Split("_", StringSplitOptions.RemoveEmptyEntries);

                    if (markingPeriodid.First() == "2")
                    {
                        QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                    if (markingPeriodid.First() == "1")
                    {
                        SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                    if (markingPeriodid.First() == "0")
                    {
                        YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                }

                var studentEffortGradeData = new List<StudentEffortGradeMaster>();

                studentEffortGradeData = this.context?.StudentEffortGradeMaster.Include(x => x.StudentEffortGradeDetail).Where(e => e.SchoolId == studentEffortGradeListModel.SchoolId && e.TenantId == studentEffortGradeListModel.TenantId && e.CalendarId == studentEffortGradeListModel.CalendarId && e.CourseId == studentEffortGradeListModel.CourseId && e.CourseSectionId == studentEffortGradeListModel.CourseSectionId).ToList();

                if (studentEffortGradeData.Count > 0)
                {
                    studentEffortGradeList.studentEffortGradeList = studentEffortGradeData;
                    studentEffortGradeList.TenantId = studentEffortGradeListModel.TenantId;
                    studentEffortGradeList.SchoolId = studentEffortGradeListModel.SchoolId;
                    studentEffortGradeList.CourseId = studentEffortGradeListModel.CourseId;
                    studentEffortGradeList.CourseSectionId = studentEffortGradeListModel.CourseSectionId;
                    studentEffortGradeList.CalendarId = studentEffortGradeListModel.CalendarId;
                    studentEffortGradeList.MarkingPeriodId = studentEffortGradeListModel.MarkingPeriodId;
                    studentEffortGradeList.AcademicYear = studentEffortGradeListModel.AcademicYear;
                    studentEffortGradeList.CreatedOrUpdatedBy = studentEffortGradeListModel.CreatedOrUpdatedBy;
                    studentEffortGradeList._userName = studentEffortGradeListModel._userName;
                    studentEffortGradeList._tenantName = studentEffortGradeListModel._tenantName;
                    studentEffortGradeList._token = studentEffortGradeListModel._token;
                    studentEffortGradeList._failure = false;
                }
                else
                {
                    studentEffortGradeList.studentEffortGradeList = studentEffortGradeData;
                    studentEffortGradeList._failure = true;
                    studentEffortGradeList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentEffortGradeList._message = es.Message;
                studentEffortGradeList._failure = true;
            }
            return studentEffortGradeList;
        }
    }
}
