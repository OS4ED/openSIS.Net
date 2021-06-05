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

namespace opensis.data.ViewModels.ReportCard
{
    public class ReportCardViewModel : CommonFields
    {
        public ReportCardViewModel()
        {
            studentsReportCardViewModelList = new List<StudentsReportCardViewModel>();
            //courseCommentCategories = new List<CourseCommentCategory>();
            //teacherCommentList = new List<string>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string MarkingPeriods { get; set; }
        public bool? TeacherName { get; set; }
        public bool? TeacherComments { get; set; }
        public bool? Parcentage { get; set; }
        public bool? GPA { get; set; }
        public bool? YearToDateDailyAbsences { get; set; }
        public bool? DailyAbsencesThisMarkingPeriod { get; set; }
        public bool? OtherAttendanceCodeYearToDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public List<StudentsReportCardViewModel> studentsReportCardViewModelList { get; set; }
        public string ReportCardPdf { get; set; }
        //public string SchoolName { get; set; }
        //public string StreetAddress1 { get; set; }
        //public string StreetAddress2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string District { get; set; }
        //public string Zip { get; set; }
        //public string Country { get; set; }
        //public List<CourseCommentCategory> courseCommentCategories { get; set; }
        //public List<String> teacherCommentList { get; set; }

    }
}
