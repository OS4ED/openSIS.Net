using opensis.data.ViewModels.MarkingPeriods;
using opensis.data.ViewModels.Quarter;
using opensis.data.ViewModels.SchoolYear;
using opensis.data.ViewModels.ProgressPeriod;
using opensis.data.ViewModels.Semester;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.MarkingPeriods.Interfaces
{
    public interface IMarkingPeriodService
    {
        public MarkingPeriod GetMarkingPeriod(MarkingPeriod markingPeriod);
        public SchoolYearsAddViewModel SaveSchoolYear(SchoolYearsAddViewModel schoolYear);
        public SchoolYearsAddViewModel ViewSchoolYear(SchoolYearsAddViewModel schoolYear);
        public SchoolYearsAddViewModel UpdateSchoolYear(SchoolYearsAddViewModel schoolYear);
        public SchoolYearsAddViewModel DeleteSchoolYear(SchoolYearsAddViewModel schoolYear);
        public QuarterAddViewModel SaveQuarter(QuarterAddViewModel quarter);
        public QuarterAddViewModel ViewQuarter(QuarterAddViewModel quarter);
        public QuarterAddViewModel UpdateQuarter(QuarterAddViewModel quarter);
        public QuarterAddViewModel DeleteQuarter(QuarterAddViewModel quarter);
        public SemesterAddViewModel SaveSemester(SemesterAddViewModel semester);
        public SemesterAddViewModel UpdateSemester(SemesterAddViewModel semester);
        public SemesterAddViewModel ViewSemester(SemesterAddViewModel semester);
        public SemesterAddViewModel DeleteSemester(SemesterAddViewModel semester);
        public ProgressPeriodAddViewModel SaveProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel UpdateProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel ViewProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel DeleteProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public DropDownViewModel GetAcademicYearList(DropDownViewModel downViewModel);
        public PeriodViewModel GetMarkingPeriodTitleList(PeriodViewModel dropdownModel);
        public MarkingPeriodListViewModel GetAllMarkingPeriodList(MarkingPeriodListViewModel markingPeriodListViewModel);
    }
}
