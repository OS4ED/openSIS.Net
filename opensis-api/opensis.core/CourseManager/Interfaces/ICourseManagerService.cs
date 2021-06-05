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

using opensis.data.ViewModels.CourseManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.CourseManager.Interfaces
{
    public interface ICourseManagerService
    {
        //public ProgramAddViewModel AddProgram(ProgramAddViewModel programAddViewModel);
        public ProgramListViewModel GetAllProgram(ProgramListViewModel programListViewModel);
        public ProgramListViewModel AddEditProgram(ProgramListViewModel programListViewModel);
        public ProgramAddViewModel DeleteProgram(ProgramAddViewModel programAddViewModel);
        //public SubjectAddViewModel AddSubject(SubjectAddViewModel subjectAddViewModel);
        public SubjectListViewModel AddEditSubject(SubjectListViewModel subjectListViewModel);
        public SubjectListViewModel GetAllSubjectList(SubjectListViewModel subjectListViewModel);
        public SubjectAddViewModel DeleteSubject(SubjectAddViewModel subjectAddViewModel);
        public CourseAddViewModel AddCourse(CourseAddViewModel courseAddViewModel);
        public CourseAddViewModel UpdateCourse(CourseAddViewModel courseAddViewModel);
        public CourseAddViewModel DeleteCourse(CourseAddViewModel courseAddViewModel);
        public CourseListViewModel GetAllCourseList(CourseListViewModel courseListViewModel);
        public CourseSectionAddViewModel AddCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseSectionViewModel GetAllCourseSection(CourseSectionViewModel courseSectionViewModel);
        public CourseSectionAddViewModel UpdateCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseSectionAddViewModel DeleteCourseSection(CourseSectionAddViewModel courseSectionAddViewModel);
        public CourseStandardForCourseViewModel GetAllCourseStandardForCourse(CourseStandardForCourseViewModel courseStandardForCourseViewModel);
        //public DeleteScheduleViewModel DeleteSchedule(DeleteScheduleViewModel deleteScheduleViewModel);
        public SearchCourseSectionViewModel SearchCourseSectionForSchedule(SearchCourseSectionViewModel searchCourseSectionViewModel);
        public StaffListViewModel GetAllStaffScheduleInCourseSection(StaffListViewModel staffListViewModel);

        public SearchCourseScheduleViewModel SearchCourseForSchedule(SearchCourseScheduleViewModel searchCourseScheduleViewModel);
    }
}

