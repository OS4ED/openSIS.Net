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
    public partial class UserMaster
    {
        public UserMaster()
        {
            SearchFilter = new HashSet<SearchFilter>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public int LangId { get; set; }
        public int MembershipId { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Language Lang { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual UserSecretQuestions UserSecretQuestions { get; set; }
        public virtual ICollection<SearchFilter> SearchFilter { get; set; }
    }
}
