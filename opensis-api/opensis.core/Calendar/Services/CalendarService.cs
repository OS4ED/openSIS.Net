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

using opensis.core.Calender.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Calender.Services
{
    public class CalendarService : ICalendarService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ICalendarRepository calendarRepository;
        public CalendarService(ICalendarRepository calendarRepository)
        {
            this.calendarRepository = calendarRepository;
        }


        /// <summary>
        /// Add Calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel AddCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calenderAddViewModel = new CalendarAddViewModel();
            if (TokenManager.CheckToken(calendar._tenantName + calendar._userName, calendar._token))
            {

                calenderAddViewModel = this.calendarRepository.AddCalendar(calendar);
                return calenderAddViewModel;

            }
            else
            {
                calenderAddViewModel._failure = true;
                calenderAddViewModel._message = TOKENINVALID;
                return calenderAddViewModel;
            }

        }

        /// <summary>
        /// Get Calendar By Id
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel ViewCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calendarAddViewModel = new CalendarAddViewModel();
            if (TokenManager.CheckToken(calendar._tenantName + calendar._userName, calendar._token))
            {
                calendarAddViewModel = this.calendarRepository.ViewCalendar(calendar);
                
                return calendarAddViewModel;

            }
            else
            {
                calendarAddViewModel._failure = true;
                calendarAddViewModel._message = TOKENINVALID;
                return calendarAddViewModel;
            }

        }

        /// <summary>
        /// Update Calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel UpdateCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calendarAddViewModel = new CalendarAddViewModel();
            if (TokenManager.CheckToken(calendar._tenantName + calendar._userName, calendar._token))
            {
                calendarAddViewModel = this.calendarRepository.UpdateCalendar(calendar);
                return calendarAddViewModel;
            }
            else
            {
                calendarAddViewModel._failure = true;
                calendarAddViewModel._message = TOKENINVALID;
                return calendarAddViewModel;
            }

        }

        /// <summary>
        /// Get All Calendar
        /// </summary>
        /// <param name="calendarList"></param>
        /// <returns></returns>
        public CalendarListModel GetAllCalendar(CalendarListModel calendarList)
        {
            CalendarListModel calendarListModel = new CalendarListModel();
            try
            {
                if (TokenManager.CheckToken(calendarList._tenantName + calendarList._userName, calendarList._token))
                {
                    calendarListModel = this.calendarRepository.GetAllCalendar(calendarList);
                }
                else
                {
                    calendarListModel._failure = true;
                    calendarListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                calendarListModel._failure = true;
                calendarListModel._message = es.Message;
            }

            return calendarListModel;
        }

        /// <summary>
        /// Delete Calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel DeleteCalendar(CalendarAddViewModel calendar)
        {
            CalendarAddViewModel calendarDelete = new CalendarAddViewModel();
            try
            {
                if (TokenManager.CheckToken(calendar._tenantName + calendar._userName, calendar._token))
                {
                    calendarDelete = this.calendarRepository.DeleteCalendar(calendar);
                }
                else
                {
                    calendarDelete._failure = true;
                    calendarDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                calendarDelete._failure = true;
                calendarDelete._message = es.Message;
            }

            return calendarDelete;
        }
    }
}
