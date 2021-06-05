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

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentSchedule.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentSchedule")]
    [ApiController]
    public class StudentScheduleController : ControllerBase
    {
        private IStudentScheduleService _studentScheduleService;
        public StudentScheduleController(IStudentScheduleService studentScheduleService)
        {
            _studentScheduleService = studentScheduleService;
        }

        [HttpPost("addStudentCourseSectionSchedule")]
        public ActionResult<StudentCourseSectionScheduleAddViewModel> AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel StudentCourseSectionScheduleAddModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                StudentCourseSectionScheduleAddModel = _studentScheduleService.AddStudentCourseSectionSchedule(studentCourseSectionScheduleAddViewModel);

            }
            catch (Exception es)
            {
                StudentCourseSectionScheduleAddModel._failure = true;
                StudentCourseSectionScheduleAddModel._message = es.Message;
            }
            return StudentCourseSectionScheduleAddModel;
        }

        [HttpPost("searchScheduledStudentForGroupDrop")]
        public ActionResult<ScheduleStudentListViewModel> SearchScheduledStudentForGroupDrop(PageResult pageResult)
        {
            ScheduleStudentListViewModel ScheduledStudentListView = new ScheduleStudentListViewModel();
            try
            {
                ScheduledStudentListView = _studentScheduleService.SearchScheduledStudentForGroupDrop(pageResult);
            }
            catch (Exception es)
            {
                ScheduledStudentListView._failure = true;
                ScheduledStudentListView._message = es.Message;
            }
            return ScheduledStudentListView;
        }

        [HttpPut("groupDropForScheduledStudent")]
        public ActionResult<ScheduledStudentDropModel> GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                ScheduledStudentDrop = _studentScheduleService.GroupDropForScheduledStudent(scheduledStudentDropModel);
            }
            catch (Exception es)
            {
                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        [HttpPost("studentScheduleReport")]
        public ActionResult<StudentScheduleReportViewModel> StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel)
        {
            StudentScheduleReportViewModel studentScheduleReportView = new StudentScheduleReportViewModel();
            try
            {
                studentScheduleReportView = _studentScheduleService.StudentScheduleReport(studentScheduleReportViewModel);
            }
            catch (Exception es)
            {
                studentScheduleReportView._failure = true;
                studentScheduleReportView._message = es.Message;
            }
            return studentScheduleReportView;
        }

        [HttpPost("deleteStudentScheduleReport")]
        public ActionResult<StudentCourseSectionScheduleAddViewModel> DeleteStudentScheduleReport(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel StudentCourseSectionScheduleDelete = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                StudentCourseSectionScheduleDelete = _studentScheduleService.DeleteStudentScheduleReport(studentCourseSectionScheduleAddViewModel);

            }
            catch (Exception es)
            {
                StudentCourseSectionScheduleDelete._failure = true;
                StudentCourseSectionScheduleDelete._message = es.Message;
            }
            return StudentCourseSectionScheduleDelete;
        }

        [HttpPost("scheduleCoursesForStudent360")]
        public ActionResult<Student360ScheduleCourseSectionListViewModel> ScheduleCoursesForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel courseListView = new Student360ScheduleCourseSectionListViewModel();
            try
            {
                courseListView = _studentScheduleService.ScheduleCoursesForStudent360(student360ScheduleCourseSectionListViewModel);

            }
            catch (Exception es)
            {
                courseListView._failure = true;
                courseListView._message = es.Message;
            }
            return courseListView;
        }

        [HttpPut("dropScheduledCourseSectionForStudent360")]
        public ActionResult<ScheduledStudentDropModel> DropScheduledCourseSectionForStudent360(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                ScheduledStudentDrop = _studentScheduleService.DropScheduledCourseSectionForStudent360(scheduledStudentDropModel);
            }
            catch (Exception es)
            {
                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        [HttpPost("scheduleCourseSectionListForStudent360")]
        public ActionResult<Student360ScheduleCourseSectionListViewModel> ScheduleCourseSectionListForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel courseListView = new Student360ScheduleCourseSectionListViewModel();
            try
            {
                courseListView = _studentScheduleService.ScheduleCourseSectionListForStudent360(student360ScheduleCourseSectionListViewModel);

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
