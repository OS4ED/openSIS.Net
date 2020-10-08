﻿using opensis.core.helper;
using opensis.core.MarkingPeriods.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.MarkingPeriods;
using opensis.data.ViewModels.Quarter;
using opensis.data.ViewModels.SchoolYear;
using opensis.data.ViewModels.ProgressPeriod;
using opensis.data.ViewModels.Semester;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.MarkingPeriods.Services
{
    public class MarkingPeriodService: IMarkingPeriodService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IMarkingperiodRepository markingperiodRepository;
        public MarkingPeriodService(IMarkingperiodRepository markingperiodRepository)
        {
            this.markingperiodRepository = markingperiodRepository;
        }
        public MarkingPeriod GetMarkingPeriod(MarkingPeriod markingPeriod)
        {
            MarkingPeriod markingPeriodModel = new MarkingPeriod();
            try
            {
                if (TokenManager.CheckToken(markingPeriod._tenantName, markingPeriod._token))
                {
                    markingPeriodModel = this.markingperiodRepository.GetMarkingPeriod(markingPeriod);
                }
                else
                {
                    markingPeriodModel._failure = true;
                    markingPeriodModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                markingPeriodModel._failure = true;
                markingPeriodModel._message = es.Message;
            }

            return markingPeriodModel;
        }
        public SchoolYearsAddViewModel SaveSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearAddViewModel = new SchoolYearsAddViewModel();
            if (TokenManager.CheckToken(schoolYear._tenantName, schoolYear._token))
            {

                schoolYearAddViewModel = this.markingperiodRepository.AddSchoolYear(schoolYear);
                return schoolYearAddViewModel;

            }
            else
            {
                schoolYearAddViewModel._failure = true;
                schoolYearAddViewModel._message = TOKENINVALID;
                return schoolYearAddViewModel;
            }

        }
        /// <summary>
        /// Get School Year By Id
        /// </summary>
        /// <param name="schoolYear"></param>
        /// <returns></returns>
        public SchoolYearsAddViewModel ViewSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearAddViewModel = new SchoolYearsAddViewModel();
            if (TokenManager.CheckToken(schoolYear._tenantName, schoolYear._token))
            {
                schoolYearAddViewModel = this.markingperiodRepository.ViewSchoolYear(schoolYear);
                return schoolYearAddViewModel;
            }
            else
            {
                schoolYearAddViewModel._failure = true;
                schoolYearAddViewModel._message = TOKENINVALID;
                return schoolYearAddViewModel;
            }

        }
        /// <summary>
        /// Update School Year
        /// </summary>
        /// <param name="schoolYear"></param>
        /// <returns></returns>
        public SchoolYearsAddViewModel UpdateSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearAddViewModel = new SchoolYearsAddViewModel();
            if (TokenManager.CheckToken(schoolYear._tenantName, schoolYear._token))
            {
                schoolYearAddViewModel = this.markingperiodRepository.UpdateSchoolYear(schoolYear);
                return schoolYearAddViewModel;
            }
            else
            {
                schoolYearAddViewModel._failure = true;
                schoolYearAddViewModel._message = TOKENINVALID;
                return schoolYearAddViewModel;
            }

        }

        /// <summary>
        /// Delete School Year
        /// </summary>
        /// <param name="schoolYear"></param>
        /// <returns></returns>
        public SchoolYearsAddViewModel DeleteSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearListdelete = new SchoolYearsAddViewModel();
            try
            {
                if (TokenManager.CheckToken(schoolYear._tenantName, schoolYear._token))
                {
                    schoolYearListdelete = this.markingperiodRepository.DeleteSchoolYear(schoolYear);
                }
                else
                {
                    schoolYearListdelete._failure = true;
                    schoolYearListdelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                schoolYearListdelete._failure = true;
                schoolYearListdelete._message = es.Message;
            }

            return schoolYearListdelete;
        }
        public QuarterAddViewModel SaveQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel QuarterAddViewModel = new QuarterAddViewModel();
            if (TokenManager.CheckToken(quarter._tenantName, quarter._token))
            {

                QuarterAddViewModel = this.markingperiodRepository.AddQuarter(quarter);
                return QuarterAddViewModel;

            }
            else
            {
                QuarterAddViewModel._failure = true;
                QuarterAddViewModel._message = TOKENINVALID;
                return QuarterAddViewModel;
            }
        }
        /// <summary>
        /// View Quarter By Id
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public QuarterAddViewModel ViewQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterAddViewModel = new QuarterAddViewModel();
            if (TokenManager.CheckToken(quarter._tenantName, quarter._token))
            {
                quarterAddViewModel = this.markingperiodRepository.ViewQuarter(quarter);
                return quarterAddViewModel;

            }
            else
            {
                quarterAddViewModel._failure = true;
                quarterAddViewModel._message = TOKENINVALID;
                return quarterAddViewModel;
            }

        }
        /// <summary>
        /// Update Quarter
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public QuarterAddViewModel UpdateQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterAddViewModel = new QuarterAddViewModel();
            if (TokenManager.CheckToken(quarter._tenantName, quarter._token))
            {
                quarterAddViewModel = this.markingperiodRepository.UpdateQuarter(quarter);
                return quarterAddViewModel;
            }
            else
            {
                quarterAddViewModel._failure = true;
                quarterAddViewModel._message = TOKENINVALID;
                return quarterAddViewModel;
            }
        }
        /// <summary>
        /// Delete Quarter
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public QuarterAddViewModel DeleteQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterListdelete = new QuarterAddViewModel();
            try
            {
                if (TokenManager.CheckToken(quarter._tenantName, quarter._token))
                {
                    quarterListdelete = this.markingperiodRepository.DeleteQuarter(quarter);
                }
                else
                {
                    quarterListdelete._failure = true;
                    quarterListdelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                quarterListdelete._failure = true;
                quarterListdelete._message = es.Message;
            }

            return quarterListdelete;
        }

        public SemesterAddViewModel SaveSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterAddViewModel = new SemesterAddViewModel();
            if (TokenManager.CheckToken(semester._tenantName, semester._token))
            {

                semesterAddViewModel = this.markingperiodRepository.AddSemester(semester);

                return semesterAddViewModel;

            }
            else
            {
                semesterAddViewModel._failure = true;
                semesterAddViewModel._message = TOKENINVALID;
                return semesterAddViewModel;
            }
        }
        public SemesterAddViewModel UpdateSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterUpdate = new SemesterAddViewModel();
            if (TokenManager.CheckToken(semester._tenantName, semester._token))
            {
                semesterUpdate = this.markingperiodRepository.UpdateSemester(semester);

                return semesterUpdate;
            }
            else
            {
                semesterUpdate._failure = true;
                semesterUpdate._message = TOKENINVALID;
                return semesterUpdate;
            }

        }
        public SemesterAddViewModel ViewSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterView = new SemesterAddViewModel();
            if (TokenManager.CheckToken(semester._tenantName, semester._token))
            {
                semesterView = this.markingperiodRepository.ViewSemester(semester);
                return semesterView;
            }
            else
            {
                semesterView._failure = true;
                semesterView._message = TOKENINVALID;
                return semesterView;
            }
        }

        public SemesterAddViewModel DeleteSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterDelete = new SemesterAddViewModel();
            try
            {
                if (TokenManager.CheckToken(semester._tenantName, semester._token))
                {
                    semesterDelete = this.markingperiodRepository.DeleteSemester(semester);
                }
                else
                {
                    semesterDelete._failure = true;
                    semesterDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                semesterDelete._failure = true;
                semesterDelete._message = es.Message;
            }

            return semesterDelete;
        }

        public ProgressPeriodAddViewModel SaveProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodAddViewModel = new ProgressPeriodAddViewModel();
            if (TokenManager.CheckToken(progressPeriod._tenantName, progressPeriod._token))
            {

                progressPeriodAddViewModel = this.markingperiodRepository.AddProgressPeriod(progressPeriod);

                return progressPeriodAddViewModel;

            }
            else
            {
                progressPeriodAddViewModel._failure = true;
                progressPeriodAddViewModel._message = TOKENINVALID;
                return progressPeriodAddViewModel;
            }
        }
        public ProgressPeriodAddViewModel UpdateProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressUpdate = new ProgressPeriodAddViewModel();
            if (TokenManager.CheckToken(progressPeriod._tenantName, progressPeriod._token))
            {
                progressUpdate = this.markingperiodRepository.UpdateProgressPeriod(progressPeriod);

                return progressUpdate;
            }
            else
            {
                progressUpdate._failure = true;
                progressUpdate._message = TOKENINVALID;
                return progressUpdate;
            }

        }
        public ProgressPeriodAddViewModel ViewProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodView = new ProgressPeriodAddViewModel();
            if (TokenManager.CheckToken(progressPeriod._tenantName, progressPeriod._token))
            {
                progressPeriodView = this.markingperiodRepository.ViewProgressPeriod(progressPeriod);
                return progressPeriodView;
            }
            else
            {
                progressPeriodView._failure = true;
                progressPeriodView._message = TOKENINVALID;
                return progressPeriodView;
            }
        }

        public ProgressPeriodAddViewModel DeleteProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodDelete = new ProgressPeriodAddViewModel();
            try
            {
                if (TokenManager.CheckToken(progressPeriod._tenantName, progressPeriod._token))
                {
                    progressPeriodDelete = this.markingperiodRepository.DeleteProgressPeriod(progressPeriod);
                }
                else
                {
                    progressPeriodDelete._failure = true;
                    progressPeriodDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                progressPeriodDelete._failure = true;
                progressPeriodDelete._message = es.Message;
            }

            return progressPeriodDelete;
        }
    }
}
