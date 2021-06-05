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
using System.ComponentModel.DataAnnotations.Schema;

namespace opensis.data.Models
{
    public partial class AllCourseSectionView
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSubject { get; set; }
        public string CourseProgram { get; set; }
        public decimal? AcademicYear { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; }
        public int? Seats { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public string ScheduleType { get; set; }
        public string FixedDays { get; set; }
        public int? FixedRoomId { get; set; }
        public int? FixedPeriodId { get; set; }
        public string VarDay { get; set; }
        public int? VarPeriodId { get; set; }
        public int? VarRoomId { get; set; }
        public DateTime? CalDate { get; set; }
        public int? CalPeriodId { get; set; }
        public int? CalRoomId { get; set; }
        public int? BlockPeriodId { get; set; }
        public int? BlockRoomId { get; set; }
        public bool? IsActive { get; set; }
        public string CourseGradeLevel { get; set; }
        public int? GradeScaleId { get; set; }
        public bool? AllowStudentConflict { get; set; }
        public bool? AllowTeacherConflict { get; set; }
        public int? CalendarId { get; set; }
        public bool? AttendanceTaken { get; set; }
        [NotMapped]
        public string StaffName { get; set; }
        public string CalDay { get; set; }
        public bool? TakeAttendanceVariable { get; set; }
        public bool? TakeAttendanceCalendar { get; set; }
        public bool? TakeAttendanceBlock { get; set; }
        [NotMapped]
        public int? AvailableSeat { get; set; }
        
    }
}
