using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.MarkingPeriods.Interfaces;
using opensis.data.ViewModels.MarkingPeriods;
using opensis.data.ViewModels.Quarter;
using opensis.data.ViewModels.SchoolYear;
using opensis.data.ViewModels.ProgressPeriod;
using opensis.data.ViewModels.Semester;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/MarkingPeriod")]
    [ApiController]
    public class MarkingPeriodController : ControllerBase
    {
        private IMarkingPeriodService _markingPeriodService;
        public MarkingPeriodController(IMarkingPeriodService markingPeriodService)
        {
            _markingPeriodService = markingPeriodService;
        }
        [HttpPost("getMarkingPeriod")]

        public ActionResult<MarkingPeriod> GetMarkingPeriod(MarkingPeriod markingPeriod)
        {
            MarkingPeriod markingPeriodModel = new MarkingPeriod();
            try
            {
                if (markingPeriod.SchoolId>0)
                {
                    markingPeriodModel = _markingPeriodService.GetMarkingPeriod(markingPeriod);
                }
                else
                {
                    markingPeriodModel.TenantId = markingPeriod.TenantId;
                    markingPeriodModel._token = markingPeriod._token;
                    markingPeriodModel._tenantName = markingPeriod._tenantName;
                    markingPeriodModel._failure = true;
                    markingPeriodModel._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                markingPeriodModel._failure = true;
                markingPeriodModel._message = es.Message;
            }
            return markingPeriodModel;
        }
        [HttpPost("addSchoolYear")]
        public ActionResult<SchoolYearsAddViewModel> AddSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearAdd = new SchoolYearsAddViewModel();
            try
            {
                if (schoolYear.tableSchoolYears.SchoolId > 0)
                {
                    schoolYearAdd = _markingPeriodService.SaveSchoolYear(schoolYear);
                }
                else
                {
                    schoolYearAdd._token = schoolYear._token;
                    schoolYearAdd._tenantName = schoolYear._tenantName;
                    schoolYearAdd._failure = true;
                    schoolYearAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                schoolYearAdd._failure = true;
                schoolYearAdd._message = es.Message;
            }
            return schoolYearAdd;
        }
        [HttpPost("viewSchoolYear")]

        public ActionResult<SchoolYearsAddViewModel> ViewSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel SchoolYearsView = new SchoolYearsAddViewModel();
            try
            {
                if (schoolYear.tableSchoolYears.SchoolId > 0)
                {
                    SchoolYearsView = _markingPeriodService.ViewSchoolYear(schoolYear);
                }
                else
                {
                    SchoolYearsView._token = schoolYear._token;
                    SchoolYearsView._tenantName = schoolYear._tenantName;
                    SchoolYearsView._failure = true;
                    SchoolYearsView._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                SchoolYearsView._failure = true;
                SchoolYearsView._message = es.Message;
            }
            return SchoolYearsView;
        }
        [HttpPut("updateSchoolYear")]

        public ActionResult<SchoolYearsAddViewModel> UpdateSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel SchoolYearsUpdate = new SchoolYearsAddViewModel();
            try
            {
                if (schoolYear.tableSchoolYears.SchoolId > 0)
                {
                    SchoolYearsUpdate = _markingPeriodService.UpdateSchoolYear(schoolYear);
                }
                else
                {
                    SchoolYearsUpdate._token = schoolYear._token;
                    SchoolYearsUpdate._tenantName = schoolYear._tenantName;
                    SchoolYearsUpdate._failure = true;
                    SchoolYearsUpdate._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                SchoolYearsUpdate._failure = true;
                SchoolYearsUpdate._message = es.Message;
            }
            return SchoolYearsUpdate;
        }
        [HttpPost("deleteSchoolYear")]

        public ActionResult<SchoolYearsAddViewModel> DeleteSchoolYear(SchoolYearsAddViewModel schoolYear)
        {
            SchoolYearsAddViewModel schoolYearlDelete = new SchoolYearsAddViewModel();
            try
            {
                if (schoolYear.tableSchoolYears.SchoolId > 0)
                {
                    schoolYearlDelete = _markingPeriodService.DeleteSchoolYear(schoolYear);
                }
                else
                {
                    schoolYearlDelete._token = schoolYear._token;
                    schoolYearlDelete._tenantName = schoolYear._tenantName;
                    schoolYearlDelete._failure = true;
                    schoolYearlDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                schoolYearlDelete._failure = true;
                schoolYearlDelete._message = es.Message;
            }
            return schoolYearlDelete;
        }
        [HttpPost("addQuarter")]
        public ActionResult<QuarterAddViewModel> AddQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterAdd = new QuarterAddViewModel();
            try
            {
                if (quarter.tableQuarter.SchoolId > 0)
                {
                    quarterAdd = _markingPeriodService.SaveQuarter(quarter);
                }
                else
                {
                    quarterAdd._token = quarter._token;
                    quarterAdd._tenantName = quarter._tenantName;
                    quarterAdd._failure = true;
                    quarterAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                quarterAdd._failure = true;
                quarterAdd._message = es.Message;
            }
            return quarterAdd;
        }

        [HttpPost("viewQuarter")]

        public ActionResult<QuarterAddViewModel> ViewQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterAdd = new QuarterAddViewModel();
            try
            {
                if (quarter.tableQuarter.SchoolId > 0)
                {
                    quarterAdd = _markingPeriodService.ViewQuarter(quarter);
                }
                else
                {
                    quarterAdd._token = quarter._token;
                    quarterAdd._tenantName = quarter._tenantName;
                    quarterAdd._failure = true;
                    quarterAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                quarterAdd._failure = true;
                quarterAdd._message = es.Message;
            }
            return quarterAdd;
        }

        [HttpPut("updateQuarter")]

        public ActionResult<QuarterAddViewModel> UpdateQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterAdd = new QuarterAddViewModel();
            try
            {
                if (quarter.tableQuarter.SchoolId > 0)
                {
                    quarterAdd = _markingPeriodService.UpdateQuarter(quarter);
                }
                else
                {
                    quarterAdd._token = quarter._token;
                    quarterAdd._tenantName = quarter._tenantName;
                    quarterAdd._failure = true;
                    quarterAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                quarterAdd._failure = true;
                quarterAdd._message = es.Message;
            }
            return quarterAdd;
        }

        [HttpPost("deleteQuarter")]

        public ActionResult<QuarterAddViewModel> DeleteQuarter(QuarterAddViewModel quarter)
        {
            QuarterAddViewModel quarterlDelete = new QuarterAddViewModel();
            try
            {
                if (quarter.tableQuarter.SchoolId > 0)
                {
                    quarterlDelete = _markingPeriodService.DeleteQuarter(quarter);
                }
                else
                {
                    quarterlDelete._token = quarter._token;
                    quarterlDelete._tenantName = quarter._tenantName;
                    quarterlDelete._failure = true;
                    quarterlDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                quarterlDelete._failure = true;
                quarterlDelete._message = es.Message;
            }
            return quarterlDelete;
        }
        [HttpPost("addSemester")]
        public ActionResult<SemesterAddViewModel> AddSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterAdd = new SemesterAddViewModel();
            try
            {
                if (semester.tableSemesters.SchoolId > 0)
                {
                    semesterAdd = _markingPeriodService.SaveSemester(semester);
                }
                else
                {
                    semesterAdd._token = semester._token;
                    semesterAdd._tenantName = semester._tenantName;
                    semesterAdd._failure = true;
                    semesterAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                semesterAdd._failure = true;
                semesterAdd._message = es.Message;
            }
            return semesterAdd;
        }

        [HttpPut("updateSemester")]

        public ActionResult<SemesterAddViewModel> UpdateSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterUpdate = new SemesterAddViewModel();
            try
            {
                if (semester.tableSemesters.SchoolId > 0)
                {
                    semesterUpdate = _markingPeriodService.UpdateSemester(semester);
                }
                else
                {
                    semesterUpdate._token = semester._token;
                    semesterUpdate._tenantName = semester._tenantName;
                    semesterUpdate._failure = true;
                    semesterUpdate._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                semesterUpdate._failure = true;
                semesterUpdate._message = es.Message;
            }
            return semesterUpdate;
        }


        [HttpPost("viewSemester")]

        public ActionResult<SemesterAddViewModel> ViewSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterView = new SemesterAddViewModel();
            try
            {
                if (semester.tableSemesters.SchoolId > 0)
                {
                    semesterView = _markingPeriodService.ViewSemester(semester);
                }
                else
                {
                    semesterView._token = semester._token;
                    semesterView._tenantName = semester._tenantName;
                    semesterView._failure = true;
                    semesterView._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                semesterView._failure = true;
                semesterView._message = es.Message;
            }
            return semesterView;
        }

        [HttpPost("deleteSemester")]

        public ActionResult<SemesterAddViewModel> DeleteSemester(SemesterAddViewModel semester)
        {
            SemesterAddViewModel semesterDelete = new SemesterAddViewModel();
            try
            {
                if (semester.tableSemesters.SchoolId > 0)
                {
                    semesterDelete = _markingPeriodService.DeleteSemester(semester);
                }
                else
                {
                    semesterDelete._token = semester._token;
                    semesterDelete._tenantName = semester._tenantName;
                    semesterDelete._failure = true;
                    semesterDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                semesterDelete._failure = true;
                semesterDelete._message = es.Message;
            }
            return semesterDelete;
        }

        [HttpPost("addProgressPeriod")]
        public ActionResult<ProgressPeriodAddViewModel> AddProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodAdd = new ProgressPeriodAddViewModel();
            try
            {
                if (progressPeriod.tableProgressPeriods.SchoolId > 0)
                {
                    progressPeriodAdd = _markingPeriodService.SaveProgressPeriod(progressPeriod);
                }
                else
                {
                    progressPeriodAdd._token = progressPeriod._token;
                    progressPeriodAdd._tenantName = progressPeriod._tenantName;
                    progressPeriodAdd._failure = true;
                    progressPeriodAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                progressPeriodAdd._failure = true;
                progressPeriodAdd._message = es.Message;
            }
            return progressPeriodAdd;
        }

        [HttpPut("updateProgressPeriod")]

        public ActionResult<ProgressPeriodAddViewModel> UpdateProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressUpdate = new ProgressPeriodAddViewModel();
            try
            {
                if (progressPeriod.tableProgressPeriods.SchoolId > 0)
                {
                    progressUpdate = _markingPeriodService.UpdateProgressPeriod(progressPeriod);
                }
                else
                {
                    progressUpdate._token = progressPeriod._token;
                    progressUpdate._tenantName = progressPeriod._tenantName;
                    progressUpdate._failure = true;
                    progressUpdate._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                progressUpdate._failure = true;
                progressUpdate._message = es.Message;
            }
            return progressUpdate;
        }

        [HttpPost("viewProgressPeriod")]

        public ActionResult<ProgressPeriodAddViewModel> ViewProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodView = new ProgressPeriodAddViewModel();
            try
            {
                if (progressPeriod.tableProgressPeriods.SchoolId > 0)
                {
                    progressPeriodView = _markingPeriodService.ViewProgressPeriod(progressPeriod);
                }
                else
                {
                    progressPeriodView._token = progressPeriod._token;
                    progressPeriodView._tenantName = progressPeriod._tenantName;
                    progressPeriodView._failure = true;
                    progressPeriodView._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                progressPeriodView._failure = true;
                progressPeriodView._message = es.Message;
            }
            return progressPeriodView;
        }

        [HttpPost("deleteProgressPeriod")]

        public ActionResult<ProgressPeriodAddViewModel> DeleteProgressPeriod(ProgressPeriodAddViewModel progressPeriod)
        {
            ProgressPeriodAddViewModel progressPeriodDelete = new ProgressPeriodAddViewModel();
            try
            {
                if (progressPeriod.tableProgressPeriods.SchoolId > 0)
                {
                    progressPeriodDelete = _markingPeriodService.DeleteProgressPeriod(progressPeriod);
                }
                else
                {
                    progressPeriodDelete._token = progressPeriod._token;
                    progressPeriodDelete._tenantName = progressPeriod._tenantName;
                    progressPeriodDelete._failure = true;
                    progressPeriodDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                progressPeriodDelete._failure = true;
                progressPeriodDelete._message = es.Message;
            }
            return progressPeriodDelete;
        }

        [HttpPost("getAcademicYearList")]

        public ActionResult<DropDownViewModel> GetAcademicYearList(DropDownViewModel downViewModel)
        {
            DropDownViewModel dropDownViewModel = new DropDownViewModel();
            try
            {
                if (downViewModel.SchoolId > 0)
                {
                    dropDownViewModel = _markingPeriodService.GetAcademicYearList(downViewModel);
                }
                else
                {
                    dropDownViewModel._token = downViewModel._token;
                    dropDownViewModel._tenantName = downViewModel._tenantName;
                    dropDownViewModel._failure = true;
                    dropDownViewModel._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                dropDownViewModel._failure = true;
                dropDownViewModel._message = es.Message;
            }
            return dropDownViewModel;
        }
        [HttpPost("getMarkingPeriodTitleList")]

        public ActionResult<PeriodViewModel> GetMarkingPeriodTitleList(PeriodViewModel downViewModel)
        {
            PeriodViewModel dropDownViewModel = new PeriodViewModel();
            try
            {
                if (downViewModel.SchoolId > 0)
                {
                    dropDownViewModel = _markingPeriodService.GetMarkingPeriodTitleList(downViewModel);
                }
                else
                {
                    dropDownViewModel._token = downViewModel._token;
                    dropDownViewModel._tenantName = downViewModel._tenantName;
                    dropDownViewModel._failure = true;
                    dropDownViewModel._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                dropDownViewModel._failure = true;
                dropDownViewModel._message = es.Message;
            }
            return dropDownViewModel;
        }

        [HttpPost("getAllMarkingPeriodList")]
        public ActionResult<MarkingPeriodListViewModel> GetAllMarkingPeriodList(MarkingPeriodListViewModel markingPeriodListViewModel)
        {
            MarkingPeriodListViewModel markingPeriodList = new MarkingPeriodListViewModel();
            try
            {
                if (markingPeriodListViewModel.SchoolId > 0)
                {
                    markingPeriodList = _markingPeriodService.GetAllMarkingPeriodList(markingPeriodListViewModel);
                }
                else
                {
                    markingPeriodList._token = markingPeriodListViewModel._token;
                    markingPeriodList._tenantName = markingPeriodListViewModel._tenantName;
                    markingPeriodList._failure = true;
                    markingPeriodList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                markingPeriodList._failure = true;
                markingPeriodList._message = es.Message;
            }
            return markingPeriodList;
        }
    }
}
