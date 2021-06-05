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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentAttendances.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.Staff;
using opensis.data.ViewModels.StaffSchedule;
using opensis.data.ViewModels.StudentAttendances;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentAttendance")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private IStudentAttendanceService _studentAttendanceService;
        public StudentAttendanceController(IStudentAttendanceService studentAttendanceService)
        {
            _studentAttendanceService = studentAttendanceService;
        }

        [HttpPost("addUpdateStudentAttendance")]
        public ActionResult<StudentAttendanceAddViewModel> AddUpdateStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        { 
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            try
            {
                studentAttendanceAdd = _studentAttendanceService.SaveStudentAttendance(studentAttendanceAddViewModel);
            }
            catch (Exception ex)
            {

                studentAttendanceAdd._message = ex.Message;
                studentAttendanceAdd._failure = true;
            }

            return studentAttendanceAdd;
        }

        [HttpPost("getAllStudentAttendanceList")]
        public ActionResult<StudentAttendanceAddViewModel> GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceView = new StudentAttendanceAddViewModel();
            try
            {
                studentAttendanceView = _studentAttendanceService.GetAllStudentAttendanceList(studentAttendanceAddViewModel);
            }
            catch (Exception ex)
            {

                studentAttendanceView._message = ex.Message;
                studentAttendanceView._failure = true;
            }
            return studentAttendanceView;
        }

        [HttpPost("searchCourseSectionForStudentAttendance")]
        public ActionResult<ScheduledCourseSectionViewModel> SearchCourseSectionForStudentAttendance(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();
            try
            {
                scheduledCourseSectionView = _studentAttendanceService.SearchCourseSectionForStudentAttendance(scheduledCourseSectionViewModel);
            }
            catch (Exception ex)
            {

                scheduledCourseSectionView._message = ex.Message;
                scheduledCourseSectionView._failure = true;
            }
            return scheduledCourseSectionView;
        }

        [HttpPost("addUpdateStudentAttendanceForStudent360")]
        public ActionResult<StudentAttendanceAddViewModel> AddUpdateStudentAttendanceForStudent360(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            try
            {
                studentAttendanceAdd = _studentAttendanceService.AddUpdateStudentAttendanceForStudent360(studentAttendanceAddViewModel);
            }
            catch (Exception ex)
            {

                studentAttendanceAdd._message = ex.Message;
                studentAttendanceAdd._failure = true;
            }

            return studentAttendanceAdd;
        }

        [HttpPost("staffListForMissingAttendance")]
        public ActionResult<StaffListModel> StaffListForMissingAttendance(PageResult pageResult)
        {
            StaffListModel staffListView = new StaffListModel();
            try
            {
                staffListView = _studentAttendanceService.StaffListForMissingAttendance(pageResult);
            }
            catch (Exception ex)
            {

                staffListView._message = ex.Message;
                staffListView._failure = true;
            }

            return staffListView;
        }

        [HttpPost("missingAttendanceList")]
        public ActionResult<ScheduledCourseSectionViewModel> MissingAttendanceList(PageResult pageResult)
        {
            ScheduledCourseSectionViewModel staffListView = new ScheduledCourseSectionViewModel();
            try
            {
                staffListView = _studentAttendanceService.MissingAttendanceList(pageResult);
            }
            catch (Exception ex)
            {

                staffListView._message = ex.Message;
                staffListView._failure = true;
            }
            return staffListView;
        }

        [HttpPost("getAllStudentAttendanceListForAdministration")]
        public ActionResult<StudentAttendanceListViewModel> GetAllStudentAttendanceListForAdministration(PageResult pageResult)
        {
            StudentAttendanceListViewModel studentAttendanceList = new StudentAttendanceListViewModel();
            try
            {
                studentAttendanceList = _studentAttendanceService.GetAllStudentAttendanceListForAdministration(pageResult);
            }
            catch (Exception ex)
            {

                studentAttendanceList._message = ex.Message;
                studentAttendanceList._failure = true;
            }
            return studentAttendanceList;
        }

        [HttpPost("courseSectionListForAttendanceAdministration")]
        public ActionResult<CourseSectionForAttendanceViewModel> CourseSectionListForAttendanceAdministration(CourseSectionForAttendanceViewModel courseSectionForAttendanceViewModel)
        {
            CourseSectionForAttendanceViewModel courseSectionList = new CourseSectionForAttendanceViewModel();
            try
            {
                courseSectionList = _studentAttendanceService.CourseSectionListForAttendanceAdministration(courseSectionForAttendanceViewModel);
            }
            catch (Exception ex)
            {

                courseSectionList._message = ex.Message;
                courseSectionList._failure = true;
            }
            return courseSectionList;
        }

        [HttpPost("updateStudentsAttendanceForAdministration")]
        public ActionResult<StudentAttendanceListViewModel> UpdateStudentsAttendanceForAdministration(StudentAttendanceListViewModel studentAttendanceListViewModel)
        {
            StudentAttendanceListViewModel studentAttendanceList = new StudentAttendanceListViewModel();
            try
            {
                studentAttendanceList = _studentAttendanceService.UpdateStudentsAttendanceForAdministration(studentAttendanceListViewModel);
            }
            catch (Exception ex)
            {

                studentAttendanceList._message = ex.Message;
                studentAttendanceList._failure = true;
            }
            return studentAttendanceList;
        }
    }
}
