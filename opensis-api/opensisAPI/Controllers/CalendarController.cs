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
using Microsoft.AspNetCore.Mvc;
using opensis.core.Calender.Interfaces;
using opensis.data.ViewModels.Calendar;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private ICalendarService _calendarService;
        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpPost("addCalendar")]
        public ActionResult<CalendarAddViewModel> AddCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calendarAdd = new CalendarAddViewModel();
            try
            {
                calendarAdd = _calendarService.AddCalendar(calendar);
            }
            catch (Exception es)
            {
                calendarAdd._failure = true;
                calendarAdd._message = es.Message;
            }
            return calendarAdd;
        }

        [HttpPost("viewCalendar")]
        public ActionResult<CalendarAddViewModel> ViewCalendar(CalendarAddViewModel room)
        {
            CalendarAddViewModel viewCalendar = new CalendarAddViewModel();
            try
            {
                viewCalendar = _calendarService.ViewCalendar(room);
            }
            catch (Exception es)
            {
                viewCalendar._failure = true;
                viewCalendar._message = es.Message;
            }
            return viewCalendar;
        }

        [HttpPut("updateCalendar")]
        public ActionResult<CalendarAddViewModel> UpdateCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calendarUpdate = new CalendarAddViewModel();
            try
            {
                calendarUpdate = _calendarService.UpdateCalendar(calendar);
            }
            catch (Exception es)
            {
                calendarUpdate._failure = true;
                calendarUpdate._message = es.Message;
            }
            return calendarUpdate;
        }

        [HttpPost("getAllCalendar")]
        public ActionResult<CalendarListModel> GetAllCalendar(CalendarListModel calendarList)
        {
            CalendarListModel calendarListModel = new CalendarListModel();
            try
            {
                calendarListModel = _calendarService.GetAllCalendar(calendarList);
            }
            catch (Exception es)
            {
                calendarListModel._message = es.Message;
                calendarListModel._failure = true;
            }
            return calendarListModel;
        }

        [HttpPost("deleteCalendar")]
        public ActionResult<CalendarAddViewModel> DeleteCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel deleteCalendar = new CalendarAddViewModel();
            try
            {
                deleteCalendar = _calendarService.DeleteCalendar(calendar);
            }
            catch (Exception es)
            {
                deleteCalendar._message = es.Message;
                deleteCalendar._failure = true;
            }
            return deleteCalendar;
        }
    }
}
