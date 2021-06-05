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

using opensis.core.helper;
using opensis.core.StudentSchedule.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentSchedule.Services
{
    public class StudentScheduleService : IStudentScheduleService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentScheduleRepository studentScheduleRepository;
        public StudentScheduleService(IStudentScheduleRepository studentScheduleRepository)
        {
            this.studentScheduleRepository = studentScheduleRepository;
        }
        public StudentScheduleService() { }

        /// <summary>
        /// Add Student Course Section Schedule
        /// </summary>
        /// <param name="studentCourseSectionScheduleAddViewModel"></param>
        /// <returns></returns>
        public StudentCourseSectionScheduleAddViewModel AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(studentCourseSectionScheduleAddViewModel._tenantName + studentCourseSectionScheduleAddViewModel._userName, studentCourseSectionScheduleAddViewModel._token))
                {
                    studentCourseSectionScheduleAddModel = this.studentScheduleRepository.AddStudentCourseSectionSchedule(studentCourseSectionScheduleAddViewModel);
                }
                else
                {
                    studentCourseSectionScheduleAddModel._failure = true;
                    studentCourseSectionScheduleAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentCourseSectionScheduleAddModel._failure = true;
                studentCourseSectionScheduleAddModel._message = es.Message;
            }
            return studentCourseSectionScheduleAddModel;
        }

        /// <summary>
        /// Search Scheduled Student For Group Drop
        /// </summary>
        /// <param name="scheduleStudentListViewModel"></param>
        /// <returns></returns>
        public ScheduleStudentListViewModel SearchScheduledStudentForGroupDrop(PageResult pageResult)
        {
            ScheduleStudentListViewModel ScheduledStudentListView = new ScheduleStudentListViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    ScheduledStudentListView = this.studentScheduleRepository.SearchScheduledStudentForGroupDrop(pageResult);
                }
                else
                {
                    ScheduledStudentListView._failure = true;
                    ScheduledStudentListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ScheduledStudentListView._failure = true;
                ScheduledStudentListView._message = es.Message;
            }
            return ScheduledStudentListView;
        }

        public ScheduledStudentDropModel GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                if (TokenManager.CheckToken(scheduledStudentDropModel._tenantName + scheduledStudentDropModel._userName, scheduledStudentDropModel._token))
                {
                    ScheduledStudentDrop = this.studentScheduleRepository.GroupDropForScheduledStudent(scheduledStudentDropModel);
                }
                else
                {
                    ScheduledStudentDrop._failure = true;
                    ScheduledStudentDrop._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        public StudentScheduleReportViewModel StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel)
        {
            StudentScheduleReportViewModel studentScheduleReportView = new StudentScheduleReportViewModel();
            try
            {
                if (TokenManager.CheckToken(studentScheduleReportViewModel._tenantName + studentScheduleReportViewModel._userName, studentScheduleReportViewModel._token))
                {
                    studentScheduleReportView = this.studentScheduleRepository.StudentScheduleReport(studentScheduleReportViewModel);
                }
                else
                {
                    studentScheduleReportView._failure = true;
                    studentScheduleReportView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentScheduleReportView._failure = true;
                studentScheduleReportView._message = es.Message;
            }
            return studentScheduleReportView;
        }

        /// <summary>
        /// Delete Student Schedule Report
        /// </summary>
        /// <param name="studentCourseSectionScheduleAddViewModel"></param>
        /// <returns></returns>
        public StudentCourseSectionScheduleAddViewModel DeleteStudentScheduleReport(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleDeleteModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(studentCourseSectionScheduleAddViewModel._tenantName + studentCourseSectionScheduleAddViewModel._userName, studentCourseSectionScheduleAddViewModel._token))
                {
                    studentCourseSectionScheduleDeleteModel = this.studentScheduleRepository.DeleteStudentScheduleReport(studentCourseSectionScheduleAddViewModel);
                }
                else
                {
                    studentCourseSectionScheduleDeleteModel._failure = true;
                    studentCourseSectionScheduleDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentCourseSectionScheduleDeleteModel._failure = true;
                studentCourseSectionScheduleDeleteModel._message = es.Message;
            }
            return studentCourseSectionScheduleDeleteModel;
        }

        /// <summary>
        /// Schedule Courses For Student360
        /// </summary>
        /// <param name="student360ScheduleCourseSectionListViewModel"></param>
        /// <returns></returns>
        public Student360ScheduleCourseSectionListViewModel ScheduleCoursesForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel courseListView = new Student360ScheduleCourseSectionListViewModel();
            try
            {
                if (TokenManager.CheckToken(student360ScheduleCourseSectionListViewModel._tenantName + student360ScheduleCourseSectionListViewModel._userName, student360ScheduleCourseSectionListViewModel._token))
                {
                    courseListView = this.studentScheduleRepository.ScheduleCoursesForStudent360(student360ScheduleCourseSectionListViewModel);
                }
                else
                {
                    courseListView._failure = true;
                    courseListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                courseListView._failure = true;
                courseListView._message = es.Message;
            }
            return courseListView;
        }

        /// <summary>
        /// Drop Scheduled Course Section For Student360
        /// </summary>
        /// <param name="scheduledStudentDropModel"></param>
        /// <returns></returns>
        public ScheduledStudentDropModel DropScheduledCourseSectionForStudent360(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                if (TokenManager.CheckToken(scheduledStudentDropModel._tenantName + scheduledStudentDropModel._userName, scheduledStudentDropModel._token))
                {
                    ScheduledStudentDrop = this.studentScheduleRepository.DropScheduledCourseSectionForStudent360(scheduledStudentDropModel);
                }
                else
                {
                    ScheduledStudentDrop._failure = true;
                    ScheduledStudentDrop._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        public Student360ScheduleCourseSectionListViewModel ScheduleCourseSectionListForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel courseListView = new Student360ScheduleCourseSectionListViewModel();
            try
            {
                if (TokenManager.CheckToken(student360ScheduleCourseSectionListViewModel._tenantName + student360ScheduleCourseSectionListViewModel._userName, student360ScheduleCourseSectionListViewModel._token))
                {
                    courseListView = this.studentScheduleRepository.ScheduleCourseSectionListForStudent360(student360ScheduleCourseSectionListViewModel);
                }
                else
                {
                    courseListView._failure = true;
                    courseListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                courseListView._failure = true;
                courseListView._message = es.Message;
            }
            return courseListView;
        }
    }
}
