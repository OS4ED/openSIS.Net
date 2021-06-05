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

using opensis.core.helper;
using opensis.core.StaffPortal.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StaffPortal;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StaffPortal.Services
{
    public class StaffPortalService : IStaffPortalService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStaffPortalRepository staffPortalRepository;
        public StaffPortalService(IStaffPortalRepository staffPortalRepository)
        {
            this.staffPortalRepository = staffPortalRepository;
        }
        public StaffPortalService() { }

        public ScheduledCourseSectionViewModel MissingAttendanceListForCourseSection(PageResult pageResult)
        {
            ScheduledCourseSectionViewModel missingAttendanceView = new ScheduledCourseSectionViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    missingAttendanceView = this.staffPortalRepository.MissingAttendanceListForCourseSection(pageResult);
                }
                else
                {
                    missingAttendanceView._failure = true;
                    missingAttendanceView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                missingAttendanceView._failure = true;
                missingAttendanceView._message = es.Message;
            }
            return missingAttendanceView;
        }

        /// <summary>
        /// Update Online Class Room URL & Password In CourseSection
        /// </summary>
        /// <param name="courseSectionUpdateViewModel"></param>
        /// <returns></returns>
        public CourseSectionUpdateViewModel UpdateOnlineClassRoomURLInCourseSection(CourseSectionUpdateViewModel courseSectionUpdateViewModel)
        {
            CourseSectionUpdateViewModel courseSectionUpdate = new CourseSectionUpdateViewModel();
            try
            {
                if (TokenManager.CheckToken(courseSectionUpdateViewModel._tenantName + courseSectionUpdateViewModel._userName, courseSectionUpdateViewModel._token))
                {
                    courseSectionUpdate = this.staffPortalRepository.UpdateOnlineClassRoomURLInCourseSection(courseSectionUpdateViewModel);
                }
                else
                {
                    courseSectionUpdate._failure = true;
                    courseSectionUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                courseSectionUpdate._failure = true;
                courseSectionUpdate._message = es.Message;
            }
            return courseSectionUpdate;
        }
    }
}
