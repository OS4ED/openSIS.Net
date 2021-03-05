using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.CalendarEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class CalendarEventRepository : ICalendarEventRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";

        public CalendarEventRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }


        /// <summary>
        /// Add new Calendar Event
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel AddCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {

            //int? eventId = Utility.GetMaxPK(this.context, new Func<CalendarEvents, int>(x => x.EventId));
            int? eventId = 1;

            var eventData = this.context?.CalendarEvents.Where(x => x.TenantId == calendarEvent.schoolCalendarEvent.TenantId && x.SchoolId == calendarEvent.schoolCalendarEvent.SchoolId).OrderByDescending(x => x.EventId).FirstOrDefault();

            if (eventData != null)
            {
                eventId = eventData.EventId + 1;
            }

            calendarEvent.schoolCalendarEvent.EventId = (int)eventId;
            calendarEvent.schoolCalendarEvent.LastUpdated = DateTime.UtcNow;
            this.context?.CalendarEvents.Add(calendarEvent.schoolCalendarEvent);
            this.context?.SaveChanges();
            calendarEvent._failure = false;
            calendarEvent._message = "Calendar Event Added Successfully";
            return calendarEvent;
        }


        /// <summary>
        /// Get Calender Event By Id
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel ViewCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            try
            {
                CalendarEventAddViewModel calendarEventAddViewModel = new CalendarEventAddViewModel();
                var calendarEventRepository = this.context?.CalendarEvents.FirstOrDefault(x => x.TenantId == calendarEvent.schoolCalendarEvent.TenantId && x.SchoolId == calendarEvent.schoolCalendarEvent.SchoolId && x.EventId == calendarEvent.schoolCalendarEvent.EventId);
                if (calendarEventRepository != null)
                {
                    calendarEventAddViewModel.schoolCalendarEvent = calendarEventRepository;
                    calendarEventAddViewModel._tenantName = calendarEvent._tenantName;
                    calendarEventAddViewModel._failure = false;
                    return calendarEventAddViewModel;
                }
                else
                {
                    calendarEventAddViewModel._failure = true;
                    calendarEventAddViewModel._message = NORECORDFOUND;
                    return calendarEventAddViewModel;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }

        /// <summary>
        /// Update Calendar Event
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel UpdateCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            try
            {
                var calendarEventRepository = this.context?.CalendarEvents.FirstOrDefault(x => x.TenantId == calendarEvent.schoolCalendarEvent.TenantId && x.SchoolId == calendarEvent.schoolCalendarEvent.SchoolId && x.EventId == calendarEvent.schoolCalendarEvent.EventId);
                
                calendarEvent.schoolCalendarEvent.LastUpdated = DateTime.Now;
                this.context.Entry(calendarEventRepository).CurrentValues.SetValues(calendarEvent.schoolCalendarEvent);
                this.context?.SaveChanges();
                calendarEvent._failure = false;
                calendarEvent._message = "Calendar Event Updated Successfully";
                return calendarEvent;
            }
            catch (Exception ex)
            {
                calendarEvent.schoolCalendarEvent = null;
                calendarEvent._failure = true;
                calendarEvent._message = NORECORDFOUND;
                return calendarEvent;
            }
        }
        /// <summary>
        /// Get All Calendar Event
        /// </summary>
        /// <param name="calendarEventList"></param>
        /// <returns></returns>
        public CalendarEventListViewModel GetAllCalendarEvent(CalendarEventListViewModel calendarEventList)
        {
            CalendarEventListViewModel calendarEventListViewModel = new CalendarEventListViewModel();
            try
            {
                var eventList = this.context?.CalendarEvents.Where(x => x.TenantId == calendarEventList.TenantId && x.SchoolId == calendarEventList.SchoolId && x.AcademicYear == calendarEventList.AcademicYear && ((x.CalendarId == calendarEventList.CalendarId && x.SystemWideEvent == false) || x.SystemWideEvent == true)).OrderBy(x => x.Title).ToList();

                calendarEventListViewModel.calendarEventList = eventList;
                calendarEventListViewModel._tenantName = calendarEventList._tenantName;
                calendarEventListViewModel._token = calendarEventList._token;

                if (eventList.Count > 0)
                {
                    calendarEventListViewModel._failure = false;
                }
                else
                {
                    calendarEventListViewModel._failure = true;
                    calendarEventListViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                calendarEventListViewModel._message = es.Message;
                calendarEventListViewModel._failure = true;
                calendarEventListViewModel._tenantName = calendarEventList._tenantName;
                calendarEventListViewModel._token = calendarEventList._token;
            }
            return calendarEventListViewModel;

        }


        /// <summary>
        /// Delete Calendar Event
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        public CalendarEventAddViewModel DeleteCalendarEvent(CalendarEventAddViewModel calendarEvent)
        {
            try
            {
                var calendarEventRepository = this.context?.CalendarEvents.Where(x => x.EventId == calendarEvent.schoolCalendarEvent.EventId).ToList().OrderBy(x => x.EventId).LastOrDefault();
                if (calendarEventRepository != null)
                {
                    this.context?.CalendarEvents.Remove(calendarEventRepository);
                    this.context?.SaveChanges();
                    calendarEvent._failure = false;
                    calendarEvent._message = "Calendar Event Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                calendarEvent._message = ex.Message;
                calendarEvent._failure = true;
            }
            return calendarEvent;
        }

    }
}
