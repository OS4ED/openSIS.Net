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
    public partial class Language
    {
        public Language()
        {
            StaffMasterFirstLanguageNavigation = new HashSet<StaffMaster>();
            StaffMasterSecondLanguageNavigation = new HashSet<StaffMaster>();
            StaffMasterThirdLanguageNavigation = new HashSet<StaffMaster>();
            StudentMasterFirstLanguage = new HashSet<StudentMaster>();
            StudentMasterSecondLanguage = new HashSet<StudentMaster>();
            StudentMasterThirdLanguage = new HashSet<StudentMaster>();
            UserMaster = new HashSet<UserMaster>();
        }

        public int LangId { get; set; }
        public string Lcid { get; set; }
        public string Locale { get; set; }
        public string LanguageCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }


        public virtual ICollection<UserMaster> UserMaster { get; set; }
        public virtual ICollection<StudentMaster> StudentMasterFirstLanguage { get; set; }
        public virtual ICollection<StudentMaster> StudentMasterSecondLanguage { get; set; }
        public virtual ICollection<StudentMaster> StudentMasterThirdLanguage { get; set; }
        public virtual ICollection<StaffMaster> StaffMasterFirstLanguageNavigation { get; set; }
        public virtual ICollection<StaffMaster> StaffMasterSecondLanguageNavigation { get; set; }
        public virtual ICollection<StaffMaster> StaffMasterThirdLanguageNavigation { get; set; }
    }
}
