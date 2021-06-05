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
using opensis.core.StudentAttendances.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Staff;
using opensis.data.ViewModels.StaffSchedule;
using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentAttendances.Services
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentAttendanceRepository studentAttendanceRepository;
        public StudentAttendanceService(IStudentAttendanceRepository studentAttendanceRepository)
        {
            this.studentAttendanceRepository = studentAttendanceRepository;
        }
        public StudentAttendanceService() { }

        /// <summary>
        /// Student Attendance Add/Update
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>

        public StudentAttendanceAddViewModel SaveStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            if (TokenManager.CheckToken(studentAttendanceAddViewModel._tenantName + studentAttendanceAddViewModel._userName, studentAttendanceAddViewModel._token))
            {
                studentAttendanceAdd = this.studentAttendanceRepository.AddUpdateStudentAttendance(studentAttendanceAddViewModel);
            }
            else
            {
                studentAttendanceAdd._failure = true;
                studentAttendanceAdd._message = TOKENINVALID;
            }
            return studentAttendanceAdd;
        }
        /// <summary>
        /// Get All Student Attendance List
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>
        public StudentAttendanceAddViewModel GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceView = new StudentAttendanceAddViewModel();

            if (TokenManager.CheckToken(studentAttendanceAddViewModel._tenantName + studentAttendanceAddViewModel._userName, studentAttendanceAddViewModel._token))
            {
                studentAttendanceView = this.studentAttendanceRepository.GetAllStudentAttendanceList(studentAttendanceAddViewModel);
            }
            else
            {
                studentAttendanceView._failure = true;
                studentAttendanceView._message = TOKENINVALID;
            }
            return studentAttendanceView;
        }

        /// <summary>
        /// Search Course Section For Student Attendance
        /// </summary>
        /// <param name="scheduledCourseSectionViewModel"></param>
        /// <returns></returns>
        public ScheduledCourseSectionViewModel SearchCourseSectionForStudentAttendance(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();

            if (TokenManager.CheckToken(scheduledCourseSectionViewModel._tenantName + scheduledCourseSectionViewModel._userName, scheduledCourseSectionViewModel._token))
            {
                scheduledCourseSectionView = this.studentAttendanceRepository.SearchCourseSectionForStudentAttendance(scheduledCourseSectionViewModel);
            }
            else
            {
                scheduledCourseSectionView._failure = true;
                scheduledCourseSectionView._message = TOKENINVALID;
            }
            return scheduledCourseSectionView;
        }

        /// <summary>
        /// Add Update Student Attendance For Student360
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>
        public StudentAttendanceAddViewModel AddUpdateStudentAttendanceForStudent360(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            try
            {               
                if (TokenManager.CheckToken(studentAttendanceAddViewModel._tenantName + studentAttendanceAddViewModel._userName, studentAttendanceAddViewModel._token))
                {
                    studentAttendanceAdd = this.studentAttendanceRepository.AddUpdateStudentAttendanceForStudent360(studentAttendanceAddViewModel);
                }
                else
                {
                    studentAttendanceAdd._failure = true;
                    studentAttendanceAdd._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentAttendanceAdd._failure = true;
                studentAttendanceAdd._message = es.Message;
            }
            return studentAttendanceAdd;
        }

        /// <summary>
        /// Staff List For Missing Attendance
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StaffListModel StaffListForMissingAttendance(PageResult pageResult)
        {
            StaffListModel staffListView = new StaffListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    staffListView = this.studentAttendanceRepository.StaffListForMissingAttendance(pageResult);
                }
                else
                {
                    staffListView._failure = true;
                    staffListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                staffListView._failure = true;
                staffListView._message = es.Message;
            }
            return staffListView;
        }

        /// <summary>
        /// Missing Attendance List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public ScheduledCourseSectionViewModel MissingAttendanceList(PageResult pageResult)
        {
            ScheduledCourseSectionViewModel missingAttendanceView = new ScheduledCourseSectionViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    missingAttendanceView = this.studentAttendanceRepository.MissingAttendanceList(pageResult);
                }
                else
                {
                    missingAttendanceView._failure = true;
                    missingAttendanceView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                missingAttendanceView._failure = true;
                missingAttendanceView._message = es.Message;
            }
            return missingAttendanceView;
        }

        /// <summary>
        /// Get All Student Attendance List Administration
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StudentAttendanceListViewModel GetAllStudentAttendanceListForAdministration(PageResult pageResult)
        {
            StudentAttendanceListViewModel studentAttendanceList = new StudentAttendanceListViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    studentAttendanceList = this.studentAttendanceRepository.GetAllStudentAttendanceListForAdministration(pageResult);
                }
                else
                {
                    studentAttendanceList._failure = true;
                    studentAttendanceList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentAttendanceList._failure = true;
                studentAttendanceList._message = es.Message;
            }
            return studentAttendanceList;
        }

        public CourseSectionForAttendanceViewModel CourseSectionListForAttendanceAdministration(CourseSectionForAttendanceViewModel courseSectionForAttendanceViewModel)
        {
            CourseSectionForAttendanceViewModel courseSectionList = new CourseSectionForAttendanceViewModel();
            try
            {
                if (TokenManager.CheckToken(courseSectionForAttendanceViewModel._tenantName + courseSectionForAttendanceViewModel._userName, courseSectionForAttendanceViewModel._token))
                {
                    courseSectionList = this.studentAttendanceRepository.CourseSectionListForAttendanceAdministration(courseSectionForAttendanceViewModel);
                }
                else
                {
                    courseSectionList._failure = true;
                    courseSectionList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                courseSectionList._failure = true;
                courseSectionList._message = es.Message;
            }
            return courseSectionList;
        }

        public StudentAttendanceListViewModel UpdateStudentsAttendanceForAdministration(StudentAttendanceListViewModel studentAttendanceListViewModel)
        {
            StudentAttendanceListViewModel studentAttendanceList = new StudentAttendanceListViewModel();
            try
            {
                if (TokenManager.CheckToken(studentAttendanceListViewModel._tenantName + studentAttendanceListViewModel._userName, studentAttendanceListViewModel._token))
                {
                    studentAttendanceList = this.studentAttendanceRepository.UpdateStudentsAttendanceForAdministration(studentAttendanceListViewModel);
                }
                else
                {
                    studentAttendanceList._failure = true;
                    studentAttendanceList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentAttendanceList._failure = true;
                studentAttendanceList._message = es.Message;
            }
            return studentAttendanceList;
        }
    }
}
