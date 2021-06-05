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
