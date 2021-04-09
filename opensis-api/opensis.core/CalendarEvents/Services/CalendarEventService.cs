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
