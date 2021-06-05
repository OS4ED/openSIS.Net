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

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentEffortGrade.Interfaces;
using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentEffortGrade")]
    [ApiController]
    public class StudentEffortGradeController : ControllerBase
    {
        private IStudentEffortGradeService _studentEffortGradeService;
        public StudentEffortGradeController(IStudentEffortGradeService studentEffortGradeService)
        {
            _studentEffortGradeService = studentEffortGradeService;
        }

        [HttpPost("addUpdateStudentEffortGrade")]
        public ActionResult<StudentEffortGradeListModel> AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeAdd = new StudentEffortGradeListModel();
            try
            {
                studentEffortGradeAdd = _studentEffortGradeService.AddUpdateStudentEffortGrade(studentEffortGradeListModel);
            }
            catch (Exception ex)
            {
                studentEffortGradeAdd._message = ex.Message;
                studentEffortGradeAdd._failure = true;
            }
            return studentEffortGradeAdd;
        }

        [HttpPost("getAllStudentEffortGradeList")]
        public ActionResult<StudentEffortGradeListModel> GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                studentEffortGradeList = _studentEffortGradeService.GetAllStudentEffortGradeList(studentEffortGradeListModel);
            }
            catch (Exception ex)
            {

                studentEffortGradeList._message = ex.Message;
                studentEffortGradeList._failure = true;
            }
            return studentEffortGradeList;
        }
    }
}
