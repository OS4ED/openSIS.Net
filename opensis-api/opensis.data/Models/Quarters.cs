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

using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class Quarters
    {
        public Quarters()
        {
            CourseSection = new HashSet<CourseSection>();
            ProgressPeriods = new HashSet<ProgressPeriods>();
            StaffCoursesectionSchedule = new HashSet<StaffCoursesectionSchedule>();
            StudentEffortGradeMaster = new HashSet<StudentEffortGradeMaster>();
            StudentFinalGrade = new HashSet<StudentFinalGrade>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int MarkingPeriodId { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? SemesterId { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public decimal? SortOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PostStartDate { get; set; }
        public DateTime? PostEndDate { get; set; }
        public bool? DoesGrades { get; set; }
        public bool? DoesExam { get; set; }
        public bool? DoesComments { get; set; }
        public int? RolloverId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual ICollection<CourseSection> CourseSection { get; set; }
        public virtual ICollection<ProgressPeriods> ProgressPeriods { get; set; }
        public virtual ICollection<StaffCoursesectionSchedule> StaffCoursesectionSchedule { get; set; }
        public virtual ICollection<StudentEffortGradeMaster> StudentEffortGradeMaster { get; set; }
        public virtual ICollection<StudentFinalGrade> StudentFinalGrade { get; set; }
    }
}
