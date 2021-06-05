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
using opensis.data.ViewModels.Staff;
using opensis.data.ViewModels.StaffSchedule;
using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentAttendances.Interfaces
{
    public interface IStudentAttendanceService
    {
        public StudentAttendanceAddViewModel SaveStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel);
        public StudentAttendanceAddViewModel GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel);
        public ScheduledCourseSectionViewModel SearchCourseSectionForStudentAttendance(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel);
        public StudentAttendanceAddViewModel AddUpdateStudentAttendanceForStudent360(StudentAttendanceAddViewModel studentAttendanceAddViewModel);
        public StaffListModel StaffListForMissingAttendance(PageResult pageResult);
        public ScheduledCourseSectionViewModel MissingAttendanceList(PageResult pageResult);
        public StudentAttendanceListViewModel GetAllStudentAttendanceListForAdministration(PageResult pageResult);
        public CourseSectionForAttendanceViewModel CourseSectionListForAttendanceAdministration(CourseSectionForAttendanceViewModel courseSectionForAttendanceViewModel);
        public StudentAttendanceListViewModel UpdateStudentsAttendanceForAdministration(StudentAttendanceListViewModel studentAttendanceListViewModel); 
    }
}
