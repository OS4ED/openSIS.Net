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
    public partial class StudentFinalGrade
    {
       
        public StudentFinalGrade()
        {
            StudentFinalGradeComments = new HashSet<StudentFinalGradeComments>();
            StudentFinalGradeStandard = new HashSet<StudentFinalGradeStandard>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public long StudentFinalGradeSrlno { get; set; }
        public bool? BasedOnStandardGrade { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public int? GradeId { get; set; }
        public int? GradeScaleId { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? CalendarId { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public bool? IsPercent { get; set; }
        public decimal? PercentMarks { get; set; }
        public string GradeObtained { get; set; }
        public string TeacherComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Quarters Quarters { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual StudentMaster StudentMaster { get; set; }
        public virtual ICollection<StudentFinalGradeComments> StudentFinalGradeComments { get; set; }
        public virtual ICollection<StudentFinalGradeStandard> StudentFinalGradeStandard { get; set; }
    
}
}
