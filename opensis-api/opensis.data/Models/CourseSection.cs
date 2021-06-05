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
    public partial class CourseSection
    {
        public CourseSection()
        {
            StaffCoursesectionSchedule = new HashSet<StaffCoursesectionSchedule>();
            StudentCoursesectionSchedule = new HashSet<StudentCoursesectionSchedule>();
            
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? GradeScaleId { get; set; }
        public string GradeScaleType { get; set; }
        public string CourseSectionName { get; set; }
        public int? CalendarId { get; set; }
        public int? AttendanceCategoryId { get; set; }
        public decimal? CreditHours { get; set; }
        public int? Seats { get; set; }
        public bool? AllowStudentConflict { get; set; }
        public bool? AllowTeacherConflict { get; set; }
        public bool? IsWeightedCourse { get; set; }
        public bool? AffectsClassRank { get; set; }
        public bool? AffectsHonorRoll { get; set; }
        public bool? OnlineClassRoom { get; set; }
        public string OnlineClassroomUrl { get; set; }
        public string OnlineClassroomPassword { get; set; }
        public bool? UseStandards { get; set; }
        public int? StandardGradeScaleId { get; set; }
        public bool? DurationBasedOnPeriod { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public string ScheduleType { get; set; }
        public string MeetingDays { get; set; }
        public bool? AttendanceTaken { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual AttendanceCodeCategories AttendanceCodeCategories { get; set; }
        public virtual Course Course { get; set; }
        public virtual GradeScale GradeScale { get; set; }
        public virtual Quarters Quarters { get; set; }
        public virtual SchoolCalendars SchoolCalendars { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual ICollection<StaffCoursesectionSchedule> StaffCoursesectionSchedule { get; set; }
        public virtual ICollection<StudentCoursesectionSchedule> StudentCoursesectionSchedule { get; set; }
        
    }
}
