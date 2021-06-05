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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentEnrollmentCodes.Interfaces;
using opensis.data.ViewModels.StudentEnrollmentCodes;

namespace opensisAPI.Controllers
{

    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentEnrollmentCode")]
    [ApiController]
    public class StudentEnrollmentCodeController : ControllerBase
    {

        private IStudentEnrollmentCodeService _studentEnrollmentCodeService;
        public StudentEnrollmentCodeController(IStudentEnrollmentCodeService studentEnrollmentCodeService)
        {
            _studentEnrollmentCodeService = studentEnrollmentCodeService;
        }

        [HttpPost("addStudentEnrollmentCode")]
        public ActionResult<StudentEnrollmentCodeAddViewModel> AddStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAdd = new StudentEnrollmentCodeAddViewModel();
            try
            {
                studentEnrollmentCodeAdd = _studentEnrollmentCodeService.SaveStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            catch (Exception es)
            {
                studentEnrollmentCodeAdd._failure = true;
                studentEnrollmentCodeAdd._message = es.Message;
            }
            return studentEnrollmentCodeAdd;
        }

        [HttpPost("viewStudentEnrollmentCode")]

        public ActionResult<StudentEnrollmentCodeAddViewModel> ViewStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeView = new StudentEnrollmentCodeAddViewModel();
            try
            {
                studentEnrollmentCodeView = _studentEnrollmentCodeService.ViewStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            catch (Exception es)
            {
                studentEnrollmentCodeView._failure = true;
                studentEnrollmentCodeView._message = es.Message;
            }
            return studentEnrollmentCodeView;
        }

        [HttpPost("deleteStudentEnrollmentCode")]

        public ActionResult<StudentEnrollmentCodeAddViewModel> DeleteStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeDelete = new StudentEnrollmentCodeAddViewModel();
            try
            {
                studentEnrollmentCodeDelete = _studentEnrollmentCodeService.DeleteStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            catch (Exception es)
            {
                studentEnrollmentCodeDelete._failure = true;
                studentEnrollmentCodeDelete._message = es.Message;
            }
            return studentEnrollmentCodeDelete;
        }

        [HttpPut("updateStudentEnrollmentCode")]

        public ActionResult<StudentEnrollmentCodeAddViewModel> UpdateStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeUpdate = new StudentEnrollmentCodeAddViewModel();
            try
            {
                studentEnrollmentCodeUpdate = _studentEnrollmentCodeService.UpdateStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            catch (Exception es)
            {
                studentEnrollmentCodeUpdate._failure = true;
                studentEnrollmentCodeUpdate._message = es.Message;
            }
            return studentEnrollmentCodeUpdate;
        }

        [HttpPost("getAllStudentEnrollmentCode")]

        public ActionResult<StudentEnrollmentCodeListViewModel> GetAllStudentEnrollmentCode(StudentEnrollmentCodeListViewModel studentEnrollmentCodeListView)
        {
            StudentEnrollmentCodeListViewModel studentEnrollmentCodeList = new StudentEnrollmentCodeListViewModel();
            try
            {
                if (studentEnrollmentCodeListView.SchoolId > 0)
                {
                    studentEnrollmentCodeList = _studentEnrollmentCodeService.GetAllStudentEnrollmentCode(studentEnrollmentCodeListView);
                }
                else
                {
                    studentEnrollmentCodeList._token = studentEnrollmentCodeListView._token;
                    studentEnrollmentCodeList._tenantName = studentEnrollmentCodeListView._tenantName;
                    studentEnrollmentCodeList._failure = true;
                    studentEnrollmentCodeList._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentEnrollmentCodeList._failure = true;
                studentEnrollmentCodeList._message = es.Message;
            }
            return studentEnrollmentCodeList;
        }




    }
}
