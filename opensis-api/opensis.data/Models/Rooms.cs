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
    public partial class Rooms
    {
        public Rooms()
        {
            CourseBlockSchedule = new HashSet<CourseBlockSchedule>();
            CourseCalendarSchedule = new HashSet<CourseCalendarSchedule>();
            CourseFixedSchedule = new HashSet<CourseFixedSchedule>();
            CourseVariableSchedule = new HashSet<CourseVariableSchedule>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int RoomId { get; set; }
        public string Title { get; set; }
        public int? Capacity { get; set; }
        public string Description { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public bool? IsActive { get; set; }
        public virtual ICollection<CourseBlockSchedule> CourseBlockSchedule { get; set; }
        public virtual ICollection<CourseCalendarSchedule> CourseCalendarSchedule { get; set; }
        public virtual ICollection<CourseFixedSchedule> CourseFixedSchedule { get; set; }
        public virtual ICollection<CourseVariableSchedule> CourseVariableSchedule { get; set; }
    }
}
