﻿using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.SchoolPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class SchoolPeriodRepository: ISchoolPeriodRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "NO RECORD FOUND";
        public SchoolPeriodRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Adding School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel AddSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {

            int? MasterSchoolPeriodId = Utility.GetMaxPK(this.context, new Func<SchoolPeriods, int>(x => x.PeriodId));
            schoolPeriod.tableSchoolPeriods.PeriodId = (int)MasterSchoolPeriodId;
            
            schoolPeriod.tableSchoolPeriods.LastUpdated = DateTime.UtcNow;
            this.context?.SchoolPeriods.Add(schoolPeriod.tableSchoolPeriods);
            this.context?.SaveChanges();
            schoolPeriod._failure = false;
            return schoolPeriod;
        }

        /// <summary>
        /// Update the School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel UpdateSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            try
            {
                var schoolPeriodUpdate = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);

                schoolPeriodUpdate.TenantId = schoolPeriod.tableSchoolPeriods.TenantId;
                schoolPeriodUpdate.SchoolId = schoolPeriod.tableSchoolPeriods.SchoolId;
                schoolPeriodUpdate.PeriodId = schoolPeriod.tableSchoolPeriods.PeriodId;               
                schoolPeriodUpdate.AcademicYear = schoolPeriod.tableSchoolPeriods.AcademicYear;
                schoolPeriodUpdate.SortOrder = schoolPeriod.tableSchoolPeriods.SortOrder;
                schoolPeriodUpdate.Title = schoolPeriod.tableSchoolPeriods.Title;
                schoolPeriodUpdate.ShortName = schoolPeriod.tableSchoolPeriods.ShortName;
                schoolPeriodUpdate.Length = schoolPeriod.tableSchoolPeriods.Length;
                schoolPeriodUpdate.Block = schoolPeriod.tableSchoolPeriods.Block;
                schoolPeriodUpdate.IgnoreScheduling = schoolPeriod.tableSchoolPeriods.IgnoreScheduling;
                schoolPeriodUpdate.Attendance = schoolPeriod.tableSchoolPeriods.Attendance;
                schoolPeriodUpdate.RolloverId = schoolPeriod.tableSchoolPeriods.RolloverId;
                schoolPeriodUpdate.StartTime = schoolPeriod.tableSchoolPeriods.StartTime;
                schoolPeriodUpdate.EndTime = schoolPeriod.tableSchoolPeriods.EndTime;
                schoolPeriodUpdate.LastUpdated = schoolPeriod.tableSchoolPeriods.LastUpdated;
                schoolPeriodUpdate.UpdatedBy = schoolPeriod.tableSchoolPeriods.UpdatedBy;
                this.context?.SaveChanges();

                schoolPeriod._failure = false;
                return schoolPeriod;
            }
            catch (Exception ex)
            {
                schoolPeriod.tableSchoolPeriods = null;
                schoolPeriod._failure = true;
                schoolPeriod._message = NORECORDFOUND;
                return schoolPeriod;
            }
        }

        /// <summary>
        /// View School Period By Id
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel ViewSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            try
            {
                SchoolPeriodAddViewModel schoolPeriodView = new SchoolPeriodAddViewModel();
                var schoolPeriodById = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);
                if (schoolPeriodById != null)
                {
                    schoolPeriodView.tableSchoolPeriods = schoolPeriodById;
                    return schoolPeriodView;
                }
                else
                {
                    schoolPeriodView._failure = true;
                    schoolPeriodView._message = NORECORDFOUND;
                    return schoolPeriodView;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }
        /// <summary>
        /// Delete School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel DeleteSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            try
            {
                var schoolPeriodDelete = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);

                this.context?.SchoolPeriods.Remove(schoolPeriodDelete);
                this.context?.SaveChanges();
                schoolPeriod._failure = false;
                schoolPeriod._message = "Deleted";
            }

            catch (Exception es)
            {
                schoolPeriod._failure = true;
                schoolPeriod._message = es.Message;
            }
            return schoolPeriod;
        }

    }
}
