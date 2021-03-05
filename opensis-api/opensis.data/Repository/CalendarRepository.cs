using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class CalendarRepository : ICalendarRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public CalendarRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }


        /// <summary>
        /// Add calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel AddCalendar(CalendarAddViewModel calendar)
        {
            //int? calenderId = Utility.GetMaxPK(this.context, new Func<SchoolCalendars, int>(x => x.CalenderId));
            int? calenderId = 1;

            var calenderData = this.context?.SchoolCalendars.Where(x => x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId).OrderByDescending(x => x.CalenderId).FirstOrDefault();

            if (calenderData != null)
            {
                calenderId = calenderData.CalenderId + 1;
            }

            calendar.schoolCalendar.CalenderId = (int)calenderId;
            calendar.schoolCalendar.LastUpdated = DateTime.UtcNow;
            var checkDefaultCalendar = this.context?.SchoolCalendars.Where(x => x.AcademicYear == calendar.schoolCalendar.AcademicYear && x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId).ToList().Find(x => x.DefaultCalender == true);
            if (checkDefaultCalendar == null)
            {
                calendar.schoolCalendar.DefaultCalender = true;
            }
            if (calendar.schoolCalendar.DefaultCalender == true)
            {
                (from p in this.context?.SchoolCalendars
                 where p.TenantId == calendar.schoolCalendar.TenantId && p.SchoolId == calendar.schoolCalendar.SchoolId && p.AcademicYear == calendar.schoolCalendar.AcademicYear
                 select p).ToList().ForEach(x => x.DefaultCalender = false);
            }

            this.context?.SchoolCalendars.Add(calendar.schoolCalendar);
            this.context?.SaveChanges();
            calendar._failure = false;
            calendar._message = "Calendar Added Successfully";

            return calendar;
        }

        /// <summary>
        /// Get Calender By Id
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel ViewCalendar(CalendarAddViewModel calendar)
        {
            try
            {
                CalendarAddViewModel calendarAddViewModel = new CalendarAddViewModel();
                var calendarRepository = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId && x.CalenderId == calendar.schoolCalendar.CalenderId);
                if (calendarRepository != null)
                {
                    calendar.schoolCalendar = calendarRepository;
                    calendar._tenantName = calendar._tenantName;
                    calendar._failure = false;
                    return calendar;
                }
                else
                {
                    calendarAddViewModel._failure = true;
                    calendarAddViewModel._message = NORECORDFOUND;
                    return calendarAddViewModel;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }

        /// <summary>
        /// Update Calendar
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        public CalendarAddViewModel UpdateCalendar(CalendarAddViewModel calendar)
        {
            try
            {
                var calendarRepository = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId && x.CalenderId == calendar.schoolCalendar.CalenderId);

                var enrollmentCalendar = this.context?.StudentEnrollment.FirstOrDefault(x => x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId && x.CalenderId == calendar.schoolCalendar.CalenderId);

                if(enrollmentCalendar!= null && calendar.schoolCalendar.DefaultCalender==false)
                {
                    calendar.schoolCalendar = null;
                    calendar._failure = true;
                    calendar._message = "Default Calendar cannot be updated because it has enrollment.";
                    return calendar;
                }

                var checkDefaultCalendar = this.context?.SchoolCalendars.Where(x => x.AcademicYear == calendar.schoolCalendar.AcademicYear && x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId && x.CalenderId != calendar.schoolCalendar.CalenderId).ToList().Find(x => x.DefaultCalender == true);

                if (checkDefaultCalendar == null)
                {
                    calendar.schoolCalendar.DefaultCalender = true;
                }
                else
                {
                    var enrollmentDefaultCalendar = this.context?.StudentEnrollment.FirstOrDefault(x => x.TenantId == calendar.schoolCalendar.TenantId && x.SchoolId == calendar.schoolCalendar.SchoolId && x.CalenderId == checkDefaultCalendar.CalenderId);

                    if (enrollmentDefaultCalendar != null && calendar.schoolCalendar.DefaultCalender == true)
                    {
                        calendar.schoolCalendar = null;
                        calendar._failure = true;
                        calendar._message = "Existing Default Calendar cannot be updated because it has already enrollment.";
                        return calendar;
                    }
                    //calendarRepository.DefaultCalender = calendar.schoolCalendar.DefaultCalender;                        
                }
                
                if (calendar.schoolCalendar.DefaultCalender == true )
                {
                    (from p in this.context?.SchoolCalendars
                     where p.CalenderId != calendarRepository.CalenderId && p.AcademicYear == calendar.schoolCalendar.AcademicYear && p.TenantId == calendar.schoolCalendar.TenantId && p.SchoolId == calendar.schoolCalendar.SchoolId
                     select p).ToList().ForEach(x => x.DefaultCalender = false);

                }                
                calendar.schoolCalendar.LastUpdated = DateTime.Now;
                this.context.Entry(calendarRepository).CurrentValues.SetValues(calendar.schoolCalendar);
                this.context?.SaveChanges();

                this.context?.SaveChanges();
                calendar._failure = false;
                calendar._message = "Calendar Updated Successfully";
                return calendar;
            }
            catch (Exception ex)
            {
                calendar.schoolCalendar = null;
                calendar._failure = true;
                calendar._message = NORECORDFOUND;
                return calendar;
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
                var calendarRepository = this.context?.SchoolCalendars.Where(x => x.TenantId == calendarList.TenantId && x.SchoolId == calendarList.SchoolId && x.AcademicYear == calendarList.AcademicYear).OrderBy(x => x.Title).ToList();

                calendarListModel.CalendarList = calendarRepository;
                calendarListModel._tenantName = calendarList._tenantName;
                calendarListModel._token = calendarList._token;

                if (calendarRepository.Count > 0)
                {
                    calendarListModel._failure = false;
                }
                else
                {
                    calendarListModel._failure = true;
                    calendarListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                calendarListModel._message = es.Message;
                calendarListModel._failure = true;
                calendarListModel._tenantName = calendarList._tenantName;
                calendarListModel._token = calendarList._token;
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
            try
            {
                var calendarRepository = this.context?.SchoolCalendars.Where(x => x.CalenderId == calendar.schoolCalendar.CalenderId).ToList().OrderBy(x => x.CalenderId).LastOrDefault();
                if (calendarRepository != null)
                {
                    var eventsExist = this.context?.CalendarEvents.FirstOrDefault(x => x.TenantId == calendarRepository.TenantId && x.SchoolId == calendarRepository.SchoolId && x.CalendarId == calendarRepository.CalenderId);

                    var enrollmentExist = this.context?.StudentEnrollment.FirstOrDefault(x => x.TenantId == calendarRepository.TenantId && x.SchoolId == calendarRepository.SchoolId && x.CalenderId == calendarRepository.CalenderId);

                    if (eventsExist != null || enrollmentExist != null)
                    {
                        calendar._message = "Calendar cannot be deleted because it has an association.";
                        calendar._failure = true;
                    }
                    else
                    {
                        this.context?.SchoolCalendars.Remove(calendarRepository);
                        this.context?.SaveChanges();
                        calendar._failure = false;
                        calendar._message = "Calendar Deleted Successfully";
                    }
                }

                return calendar;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
