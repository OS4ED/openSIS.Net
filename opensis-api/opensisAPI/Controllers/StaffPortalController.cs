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
using opensis.core.StaffPortal.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.StaffPortal;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StaffPortal")]
    [ApiController]
    public class StaffPortalController : ControllerBase
    {
        private IStaffPortalService _staffPortalService;
        public StaffPortalController(IStaffPortalService staffPortalService)
        {
            _staffPortalService = staffPortalService;
        }

        [HttpPost("missingAttendanceListForCourseSection")]
        public ActionResult<ScheduledCourseSectionViewModel> MissingAttendanceListForCourseSection(PageResult pageResult)
        {
            ScheduledCourseSectionViewModel staffMissingAttendanceListView = new ScheduledCourseSectionViewModel();
            try
            {
                staffMissingAttendanceListView = _staffPortalService.MissingAttendanceListForCourseSection(pageResult);
            }
            catch (Exception ex)
            {

                staffMissingAttendanceListView._message = ex.Message;
                staffMissingAttendanceListView._failure = true;
            }
            return staffMissingAttendanceListView;
        }

        [HttpPut("updateOnlineClassRoomURLInCourseSection")]
        public ActionResult<CourseSectionUpdateViewModel> UpdateOnlineClassRoomURLInCourseSection(CourseSectionUpdateViewModel courseSectionUpdateViewModel)
        {
            CourseSectionUpdateViewModel courseSectionUpdate = new CourseSectionUpdateViewModel();
            try
            {
                courseSectionUpdate = _staffPortalService.UpdateOnlineClassRoomURLInCourseSection(courseSectionUpdateViewModel);
            }
            catch (Exception ex)
            {

                courseSectionUpdate._message = ex.Message;
                courseSectionUpdate._failure = true;
            }
            return courseSectionUpdate;
        }
    }
}
