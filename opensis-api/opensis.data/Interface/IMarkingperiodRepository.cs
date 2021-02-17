using opensis.data.ViewModels.MarkingPeriods;
using opensis.data.ViewModels.Quarter;
using opensis.data.ViewModels.SchoolYear;
using opensis.data.ViewModels.ProgressPeriod;
using opensis.data.ViewModels.Semester;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IMarkingperiodRepository
    {
        public MarkingPeriod GetMarkingPeriod(MarkingPeriod markingPeriod);
        public SchoolYearsAddViewModel AddSchoolYear(SchoolYearsAddViewModel schoolYears);
        public SchoolYearsAddViewModel ViewSchoolYear(SchoolYearsAddViewModel schoolYears);
        public SchoolYearsAddViewModel UpdateSchoolYear(SchoolYearsAddViewModel schoolYears);
        public SchoolYearsAddViewModel DeleteSchoolYear(SchoolYearsAddViewModel schoolYears);
        public QuarterAddViewModel AddQuarter(QuarterAddViewModel quarters);
        public QuarterAddViewModel ViewQuarter(QuarterAddViewModel quarter);
        public QuarterAddViewModel UpdateQuarter(QuarterAddViewModel quarters);
        public QuarterAddViewModel DeleteQuarter(QuarterAddViewModel quarter);
        SemesterAddViewModel AddSemester(SemesterAddViewModel semester);
        SemesterAddViewModel UpdateSemester(SemesterAddViewModel semester);
        SemesterAddViewModel ViewSemester(SemesterAddViewModel semester);
        SemesterAddViewModel DeleteSemester(SemesterAddViewModel semester);

        ProgressPeriodAddViewModel AddProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel UpdateProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel ViewProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public ProgressPeriodAddViewModel DeleteProgressPeriod(ProgressPeriodAddViewModel progressPeriod);
        public DropDownViewModel GetAcademicYearList(DropDownViewModel downViewModel);
        public PeriodViewModel GetMarkingPeriodTitleList(PeriodViewModel periodViewModel);
        public MarkingPeriodListViewModel GetAllMarkingPeriodList(MarkingPeriodListViewModel markingPeriodListViewModel);
    }
}
