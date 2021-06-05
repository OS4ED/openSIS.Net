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
using opensis.core.CalendarEvents.Interfaces;
using opensis.data.ViewModels.CalendarEvents;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/CalendarEvent")]
    [ApiController]
    public class CalendarEventController : ControllerBase
    {
        private ICalendarEventService _calendarEventService;
        public CalendarEventController(ICalendarEventService calendarEventService)
        {
            _calendarEventService = calendarEventService;
        }


        [HttpPost("addCalendarEvent")]
        public ActionResult<CalendarEventAddViewModel> AddCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel calendarEventAdd = new CalendarEventAddViewModel();
            try
            {
                calendarEventAdd = _calendarEventService.AddCalendarEvent(calendarEvent);
            }
            catch (Exception es)
            {
                calendarEventAdd._failure = true;
                calendarEventAdd._message = es.Message;
            }
            return calendarEventAdd;
        }

        [HttpPost("viewCalendarEvent")]
        public ActionResult<CalendarEventAddViewModel> ViewCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel viewCalendar = new CalendarEventAddViewModel();
            try
            {
                viewCalendar = _calendarEventService.ViewCalendarEvent(calendarEvent);
            }
            catch (Exception es)
            {
                viewCalendar._failure = true;
                viewCalendar._message = es.Message;
            }
            return viewCalendar;
        }

        [HttpPut("updateCalendarEvent")]
        public ActionResult<CalendarEventAddViewModel> UpdateCalendarEvent(CalendarEventAddViewModel calendar)
        {
            CalendarEventAddViewModel calendarEventUpdate = new CalendarEventAddViewModel();
            try
            {
                calendarEventUpdate = _calendarEventService.UpdateCalendarEvent(calendar);
            }
            catch (Exception es)
            {
                calendarEventUpdate._failure = true;
                calendarEventUpdate._message = es.Message;
            }
            return calendarEventUpdate;
        }

        [HttpPost("getAllCalendarEvent")]
        public ActionResult<CalendarEventListViewModel> GetAllCalendarEvent(CalendarEventListViewModel calendarList)
        {
            CalendarEventListViewModel calendarEventListModel = new CalendarEventListViewModel();
            try
            {
                calendarEventListModel = _calendarEventService.GetAllCalendarEvent(calendarList);
            }
            catch (Exception es)
            {
                calendarEventListModel._message = es.Message;
                calendarEventListModel._failure = true;
            }
            return calendarEventListModel;
        }

        [HttpPost("deleteCalendarEvent")]
        public ActionResult<CalendarEventAddViewModel> DeleteCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel deleteCalendarEvent = new CalendarEventAddViewModel();
            try
            {
                deleteCalendarEvent = _calendarEventService.DeleteCalendarEvent(calendarEvent);
            }
            catch (Exception es)
            {
                deleteCalendarEvent._failure = true;
                deleteCalendarEvent._message = es.Message;
            }
            return deleteCalendarEvent;
        }
    }
}
