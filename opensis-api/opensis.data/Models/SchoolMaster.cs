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
    public partial class SchoolMaster
    {
        public SchoolMaster()
        {
            AttendanceCodeCategories = new HashSet<AttendanceCodeCategories>();
            Block = new HashSet<Block>();
            BlockPeriod = new HashSet<BlockPeriod>();
            
            CourseSection = new HashSet<CourseSection>();
            CustomFields = new HashSet<CustomFields>();
            DpdownValuelist = new HashSet<DpdownValuelist>();
            FieldsCategory = new HashSet<FieldsCategory>();
            GradeScale = new HashSet<GradeScale>();
            Gradelevels = new HashSet<Gradelevels>();
            Membership = new HashSet<Membership>();
            PermissionGroup = new HashSet<PermissionGroup>();
            Quarters = new HashSet<Quarters>();
            SchoolCalendars = new HashSet<SchoolCalendars>();
            SchoolDetail = new HashSet<SchoolDetail>();
            SchoolPreference = new HashSet<SchoolPreference>();
            SchoolYears = new HashSet<SchoolYears>();
            SearchFilter = new HashSet<SearchFilter>();
            Semesters = new HashSet<Semesters>();
            StaffMaster = new HashSet<StaffMaster>();
            StudentCoursesectionSchedule = new HashSet<StudentCoursesectionSchedule>();
            StudentEnrollmentCode = new HashSet<StudentEnrollmentCode>();
            StudentMaster = new HashSet<StudentMaster>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public Guid SchoolGuid { get; set; }
        public string SchoolInternalId { get; set; }
        public string SchoolAltId { get; set; }
        public string SchoolStateId { get; set; }
        public string SchoolDistrictId { get; set; }
        public string SchoolLevel { get; set; }
        public string SchoolClassification { get; set; }
        public string SchoolName { get; set; }
        public string AlternateName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Division { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public DateTime? CurrentPeriodEnds { get; set; }
        public int? MaxApiChecks { get; set; }
        public string Features { get; set; }
        public int? PlanId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModifed { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        public virtual Plans Plans { get; set; }
        public virtual ICollection<AttendanceCodeCategories> AttendanceCodeCategories { get; set; }
        public virtual ICollection<Block> Block { get; set; }
        public virtual ICollection<BlockPeriod> BlockPeriod { get; set; }
        
        public virtual ICollection<CourseSection> CourseSection { get; set; }
        public virtual ICollection<CustomFields> CustomFields { get; set; }
        public virtual ICollection<DpdownValuelist> DpdownValuelist { get; set; }
        public virtual ICollection<FieldsCategory> FieldsCategory { get; set; }
        public virtual ICollection<GradeScale> GradeScale { get; set; }
        public virtual ICollection<Gradelevels> Gradelevels { get; set; }
        public virtual ICollection<Membership> Membership { get; set; }
        public virtual ICollection<PermissionGroup> PermissionGroup { get; set; }
        public virtual ICollection<Quarters> Quarters { get; set; }
        public virtual ICollection<SchoolCalendars> SchoolCalendars { get; set; }
        public virtual ICollection<SchoolDetail> SchoolDetail { get; set; }
        public virtual ICollection<SchoolPreference> SchoolPreference { get; set; }
        public virtual ICollection<SchoolYears> SchoolYears { get; set; }
        public virtual ICollection<SearchFilter> SearchFilter { get; set; }
        public virtual ICollection<Semesters> Semesters { get; set; }
        public virtual ICollection<StaffMaster> StaffMaster { get; set; }
        public virtual ICollection<StudentCoursesectionSchedule> StudentCoursesectionSchedule { get; set; }
        public virtual ICollection<StudentEnrollmentCode> StudentEnrollmentCode { get; set; }
        public virtual ICollection<StudentMaster> StudentMaster { get; set; }
    }
}
