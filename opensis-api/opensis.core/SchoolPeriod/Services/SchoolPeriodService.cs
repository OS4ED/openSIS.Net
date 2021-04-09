using opensis.core.helper;
using opensis.core.SchoolPeriod.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.SchoolPeriod;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.SchoolPeriod.Services
{
    public class SchoolPeriodService: ISchoolPeriodService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ISchoolPeriodRepository schoolPeriodRepository;
        public SchoolPeriodService(ISchoolPeriodRepository schoolPeriodRepository)
        {
            this.schoolPeriodRepository = schoolPeriodRepository;
        }
        //Required for Unit Testing
        public SchoolPeriodService() { }

        /// <summary>
        /// Add School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel SaveSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodAddViewModel = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName+ schoolPeriod._userName, schoolPeriod._token))
            {

                schoolPeriodAddViewModel = this.schoolPeriodRepository.AddSchoolPeriod(schoolPeriod);

                return schoolPeriodAddViewModel;

            }
            else
            {
                schoolPeriodAddViewModel._failure = true;
                schoolPeriodAddViewModel._message = TOKENINVALID;
                return schoolPeriodAddViewModel;
            }
        }

        /// <summary>
        /// Update School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel UpdateSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodUpdate = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
            {
                schoolPeriodUpdate = this.schoolPeriodRepository.UpdateSchoolPeriod(schoolPeriod);

                return schoolPeriodUpdate;
            }
            else
            {
                schoolPeriodUpdate._failure = true;
                schoolPeriodUpdate._message = TOKENINVALID;
                return schoolPeriodUpdate;
            }

        }

        /// <summary>
        /// View School Period By Id
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel ViewSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodView = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
            {
                schoolPeriodView = this.schoolPeriodRepository.ViewSchoolPeriod(schoolPeriod);
                return schoolPeriodView;
            }
            else
            {
                schoolPeriodView._failure = true;
                schoolPeriodView._message = TOKENINVALID;
                return schoolPeriodView;
            }
        }

        /// <summary>
        /// Delete School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel DeleteSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodDelete = new SchoolPeriodAddViewModel();
            try
            {
                if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
                {
                    schoolPeriodDelete = this.schoolPeriodRepository.DeleteSchoolPeriod(schoolPeriod);
                }
                else
                {
                    schoolPeriodDelete._failure = true;
                    schoolPeriodDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                schoolPeriodDelete._failure = true;
                schoolPeriodDelete._message = es.Message;
            }

            return schoolPeriodDelete;
        }

    }
}
