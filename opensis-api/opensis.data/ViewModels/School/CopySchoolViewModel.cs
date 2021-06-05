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

namespace opensis.data.ViewModels.School
{
    public class CopySchoolViewModel : CommonFields
    {

        public SchoolMaster schoolMaster { get; set; }
        public Guid TenantId { get; set; }
        public int? FromSchoolId { get; set; }
        public bool? Periods { get; set; }           
        public bool? MarkingPeriods { get; set; }               
        public bool? Calendar { get; set; }
        public bool? Sections { get; set; }
        public bool? Rooms { get; set; }
        public bool? GradeLevels { get; set; }
       // public bool? Hierarchy { get; set; }
       // public bool? Preference { get; set; }
        public bool? SchoolFields { get; set; }
        public bool? StudentFields { get; set; }
        public bool? EnrollmentCodes { get; set; }
        public bool? StaffFields { get; set; }
        public bool? Subjets { get; set; }
        public bool? Programs { get; set; }
        public bool? Course { get; set; }      
        public bool? AttendanceCode { get; set; }
        public bool? ReportCardGrades { get; set; }
        public bool? ReportCardComments { get; set; }
        public bool? StandardGrades { get; set; }
        public bool? HonorRollSetup { get; set; }
        public bool? EffortGrades { get; set; }

        public bool? ProfilePermission { get; set; }

        public bool? SchoolLevel { get; set; }
        public bool? SchoolClassification { get; set; }
        public bool? Countries { get; set; }
        public bool? FemaleToiletType { get; set; }
        public bool? FemaleToiletAccessibility { get; set; }
        public bool? MaleToiletType { get; set; }
        public bool? MaleToiletAccessibility { get; set; }
        public bool? CommonToiletType { get; set; }
        public bool? CommonToiletAccessibility { get; set; }
        public bool? Race { get; set; }
        public bool? Ethnicity { get; set; }
        //public bool? Language { get; set; }

    }
}
