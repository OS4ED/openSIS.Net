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
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ParentInfos
{
     public class GetParentInfoForView
    {
        public GetParentInfoForView()
        {
            getStudentForView = new List<GetStudentForView>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int ParentId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }        
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string LoginEmail { get; set; }
        public string UserProfile { get; set; }
        public bool IsPortalUser { get; set; }
        public bool IsCustodian { get; set; }
        public string FullAddress { get; set; }
        public List<GetStudentForView> getStudentForView { get; set; }
        public string[] students { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool StudentAddressSame { get; set; }
        public string StudentList { get; set; }
    }
}
