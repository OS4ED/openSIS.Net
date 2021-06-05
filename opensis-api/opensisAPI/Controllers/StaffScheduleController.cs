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
using opensis.core.StaffSchedule.Interfaces;
using opensis.data.ViewModels.CourseManager;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StaffSchedule")]
    [ApiController]
    public class StaffScheduleController : ControllerBase
    {
        private IStaffScheduleService _staffScheduleService;
        public StaffScheduleController(IStaffScheduleService staffScheduleService)
        {
            _staffScheduleService = staffScheduleService;
        }
[HttpPost("staffScheduleViewForCourseSection")]
        public ActionResult<StaffScheduleViewModel> StaffScheduleViewForCourseSection(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.StaffScheduleViewForCourseSection(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }
        

        [HttpPost("addStaffCourseSectionSchedule")]
        public ActionResult<StaffScheduleViewModel> AddStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.AddStaffCourseSectionSchedule(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }

        [HttpPost("checkAvailabilityStaffCourseSectionSchedule")]
        public ActionResult<StaffScheduleViewModel> CheckAvailabilityStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.CheckAvailabilityStaffCourseSectionSchedule(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }

        [HttpPost("getAllScheduledCourseSectionForStaff")]
        public ActionResult<ScheduledCourseSectionViewModel> GetAllScheduledCourseSectionForStaff(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();
            try
            {
                scheduledCourseSectionView = _staffScheduleService.GetAllScheduledCourseSectionForStaff(scheduledCourseSectionViewModel);
            }
            catch (Exception es)
            {
                scheduledCourseSectionView._message = es.Message;
                scheduledCourseSectionView._failure = true;
            }
            return scheduledCourseSectionView;
        }

        [HttpPost("addStaffCourseSectionReSchedule")]
        public ActionResult<StaffScheduleViewModel> AddStaffCourseSectionReSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.AddStaffCourseSectionReSchedule(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }

        [HttpPost("checkAvailabilityStaffCourseSectionReSchedule")]
        public ActionResult<StaffListViewModel> checkAvailabilityStaffCourseSectionReSchedule(StaffListViewModel staffListViewModel)
        {
            StaffListViewModel staffListView = new StaffListViewModel();
            try
            {
                staffListView = _staffScheduleService.checkAvailabilityStaffCourseSectionReSchedule(staffListViewModel);
            }
            catch (Exception es)
            {
                staffListView._message = es.Message;
                staffListView._failure = true;
            }
            return staffListView;
        }

        [HttpPost("addStaffCourseSectionReScheduleByCourse")]
        public ActionResult<StaffListViewModel> AddStaffCourseSectionReScheduleByCourse(StaffListViewModel staffListViewModel)
        {
            StaffListViewModel staffListView = new StaffListViewModel();
            try
            {
                staffListView = _staffScheduleService.AddStaffCourseSectionReScheduleByCourse(staffListViewModel);
            }
            catch (Exception es)
            {
                staffListView._message = es.Message;
                staffListView._failure = true;
            }
            return staffListView;
        }
    }
}
