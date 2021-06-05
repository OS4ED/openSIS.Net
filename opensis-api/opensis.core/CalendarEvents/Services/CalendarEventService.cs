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

using opensis.core.CalendarEvents.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.CalendarEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.CalendarEvents.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ICalendarEventRepository calendarEventRepository;
        public CalendarEventService(ICalendarEventRepository calendarEventRepository)
        {
            this.calendarEventRepository = calendarEventRepository;
        }

        /// <summary>
        /// Add Calendar
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel AddCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel CalendarEventAddViewModel = new CalendarEventAddViewModel();
            if (TokenManager.CheckToken(calendarEvent._tenantName + calendarEvent._userName, calendarEvent._token))
            {

                CalendarEventAddViewModel = this.calendarEventRepository.AddCalendarEvent(calendarEvent);
                return CalendarEventAddViewModel;

            }
            else
            {
                CalendarEventAddViewModel._failure = true;
                CalendarEventAddViewModel._message = TOKENINVALID;
                return CalendarEventAddViewModel;
            }

        }

        /// <summary>
        /// Get Calendar Event By Id
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel ViewCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel CalendarEventAddViewModel = new CalendarEventAddViewModel();
            if (TokenManager.CheckToken(calendarEvent._tenantName + calendarEvent._userName, calendarEvent._token))
            {
                CalendarEventAddViewModel = this.calendarEventRepository.ViewCalendarEvent(calendarEvent);

                return CalendarEventAddViewModel;

            }
            else
            {
                CalendarEventAddViewModel._failure = true;
                CalendarEventAddViewModel._message = TOKENINVALID;
                return CalendarEventAddViewModel;
            }

        }

        /// <summary>
        /// Update Calendar Event
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel UpdateCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel CalendarEventAddViewModel = new CalendarEventAddViewModel();
            if (TokenManager.CheckToken(calendarEvent._tenantName + calendarEvent._userName, calendarEvent._token))
            {
                CalendarEventAddViewModel = this.calendarEventRepository.UpdateCalendarEvent(calendarEvent);
                return CalendarEventAddViewModel;
            }
            else
            {
                CalendarEventAddViewModel._failure = true;
                CalendarEventAddViewModel._message = TOKENINVALID;
                return CalendarEventAddViewModel;
            }

        }

        /// <summary>
        /// Get All Calendar Event List
        /// </summary>
        /// <param name="calendarEventList"></param>
        /// <returns></returns>
        public CalendarEventListViewModel GetAllCalendarEvent(CalendarEventListViewModel calendarEventList)
        {
            CalendarEventListViewModel calendarEventListModel = new CalendarEventListViewModel();
            try
            {
                if (TokenManager.CheckToken(calendarEventList._tenantName + calendarEventList._userName, calendarEventList._token))
                {
                    calendarEventListModel = this.calendarEventRepository.GetAllCalendarEvent(calendarEventList);
                }
                else
                {
                    calendarEventListModel._failure = true;
                    calendarEventListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                calendarEventListModel._failure = true;
                calendarEventListModel._message = es.Message;
            }

            return calendarEventListModel;
        }


        /// <summary>
        /// Delete Calendar Event
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel DeleteCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            CalendarEventAddViewModel deleteCalendarEvent = new CalendarEventAddViewModel();
            if (TokenManager.CheckToken(calendarEvent._tenantName + calendarEvent._userName, calendarEvent._token))
            {
                deleteCalendarEvent = this.calendarEventRepository.DeleteCalendarEvent(calendarEvent);
                return deleteCalendarEvent;
            }
            else
            {
                deleteCalendarEvent._failure = true;
                deleteCalendarEvent._message = TOKENINVALID;
                return deleteCalendarEvent;
            }

        }
    }
}
