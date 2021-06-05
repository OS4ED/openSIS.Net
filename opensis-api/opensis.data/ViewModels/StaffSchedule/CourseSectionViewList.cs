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

namespace opensis.data.ViewModels.StaffSchedule
{
    public class CourseSectionViewList
    {
        public CourseSectionViewList()
        {
            courseVariableSchedule = new List<CourseVariableSchedule>();
            courseCalendarSchedule = new List<CourseCalendarSchedule>();
            courseBlockSchedule = new List<CourseBlockSchedule>();
        }
        public int? CourseSectionId { get; set; }
        public int? CourseId { get; set; }
        public int? CalendarId { get; set; }
        public string CourseTitle { get; set; }
        public int? GradeScaleId { get; set; }
        public int? StandardGradeScaleId { get; set; }
        public string CourseSectionName { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public string ScheduleType { get; set; }
        public string CourseGradeLevel { get; set; }
        public bool? AttendanceTaken { get; set; }
        public string MeetingDays { get; set; }
        public string ScheduledStaff { get; set; }
        public string WeekDays { get; set; }
        public bool? TakeAttendanceForFixedSchedule { get; set; }
        public bool? ConflictCourseSection { get; set; }
        public int? AttendanceCategoryId { get; set; }
        public string GradeScaleType { get; set; }
        public bool? UsedStandard { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string StaffFirstGivenName { get; set; }
        public string StaffMiddleName { get; set; }
        public string StaffLastFamilyName { get; set; }
        public string PeriodTitle { get; set; }
        public int? BlockId { get; set; }
        public int? PeriodId { get; set; }
       
        public CourseFixedSchedule courseFixedSchedule { get; set; }
        public List<CourseVariableSchedule> courseVariableSchedule { get; set; }
        public List<CourseCalendarSchedule> courseCalendarSchedule { get; set; }
        public List<CourseBlockSchedule> courseBlockSchedule { get; set; }
    }
}
