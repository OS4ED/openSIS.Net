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

using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class Student360ScheduleCourseSectionForView
    {        
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSectionName { get; set; }
        public DateTime? CourseSectionDurationStartDate { get; set; }
        public DateTime? CourseSectionDurationEndDate { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
        public string DayOfWeek { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDropped { get; set; }
        public bool? IsAssociationship { get; set; }
        public bool? AttendanceTaken { get; set; }
        public string WeekDays { get; set; }

        public CourseFixedSchedule courseFixedSchedule { get; set; }
        public List<CourseVariableSchedule> courseVariableScheduleList { get; set; }
        public List<CourseCalendarSchedule> courseCalendarScheduleList { get; set; }
        public List<CourseBlockSchedule> courseBlockScheduleList { get; set; }
        public List<StaffMaster> staffMasterList { get; set; }
        public List<StudentAttendance> studentAttendanceList { get; set; }
        public AttendanceCodeCategories AttendanceCodeCategories { get; set; }
        public  SchoolYears SchoolYears { get; set; }
        public  Semesters Semesters { get; set; }
        public  Quarters Quarters { get; set; }
    }
}
