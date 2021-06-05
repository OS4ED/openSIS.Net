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
using System.Text;

namespace opensis.data.Models
{
   public partial class StudentEnrollment
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public Guid StudentGuid { get; set; }
        public int EnrollmentId { get; set; }
        public int? CalenderId { get; set; }
        public string RollingOption { get; set; }
        public string SchoolName { get; set; }
        public int? GradeId { get; set; }
        public string GradeLevelTitle { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string EnrollmentCode { get; set; }
        public DateTime? ExitDate { get; set; }
        public string ExitCode { get; set; }
        public int? TransferredSchoolId { get; set; }
        public string SchoolTransferred { get; set; }
        public string TransferredGrade { get; set; }
        
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public virtual Gradelevels Gradelevels { get; set; }
        public virtual StudentMaster StudentMaster { get; set; }

    }
}
