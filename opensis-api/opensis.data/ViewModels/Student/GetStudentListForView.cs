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

namespace opensis.data.ViewModels.Student
{
    public class GetStudentListForView
    {
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? StudentId { get; set; }
        public string AlternateId { get; set; }
        public string StudentInternalId { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public string PersonalEmail { get; set; }  
        public string GradeLevelTitle { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public Guid StudentGuid { get; set; }

    }
}
