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
using opensis.core.School.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.Notice;
using opensis.data.ViewModels.School;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/School")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private ISchoolRegisterService _schoolRegisterService;
        public SchoolController(ISchoolRegisterService schoolRegisterService)
        {
            _schoolRegisterService = schoolRegisterService;
        }
       
        [HttpPost("addSchool")]
        public ActionResult<SchoolAddViewModel> AddSchool(SchoolAddViewModel school)
        {
            SchoolAddViewModel schoolAdd = new SchoolAddViewModel();
            try
            {
                schoolAdd= _schoolRegisterService.SaveSchool(school);
            }
            catch (Exception es)
            {
                schoolAdd._failure = true;
                schoolAdd._message = es.Message;
            }
            return schoolAdd;
        }

        [HttpPut("updateSchool")]

        public ActionResult<SchoolAddViewModel> UpdateSchool(SchoolAddViewModel school)
        {
            SchoolAddViewModel schoolAdd = new SchoolAddViewModel();
            try
            {
                schoolAdd =  _schoolRegisterService.UpdateSchool(school);
            }
            catch (Exception es)
            {
                schoolAdd._failure = true;
                schoolAdd._message = es.Message;
            }
            return schoolAdd;
        }

        [HttpPost("viewSchool")]

        public ActionResult<SchoolAddViewModel> ViewSchool(SchoolAddViewModel school)
        {
            SchoolAddViewModel schoolAdd = new SchoolAddViewModel();
            try
            {
                schoolAdd= _schoolRegisterService.ViewSchool(school);
            }
            catch (Exception es)
            {
                schoolAdd._failure = true;
                schoolAdd._message = es.Message;
            }
            return schoolAdd;
        }

        


        [HttpPost("getAllSchools")]

        public ActionResult<SchoolListModel> GetAllSchools(SchoolListModel school)
        {
            
            SchoolListModel schoolList = new SchoolListModel();
            try
            {
                schoolList = _schoolRegisterService.GetAllSchools(school);
            }
            catch (Exception es)
            {
                schoolList._message = es.Message;
                schoolList._failure = true;
            }
            return schoolList;
        }

        [HttpPost("getAllSchoolList")]

        public ActionResult<SchoolListModel> GetAllSchoolList(PageResult pageResult)
        {
            SchoolListModel schoolList = new SchoolListModel();
            try
            {
                schoolList = _schoolRegisterService.GetAllSchoolList(pageResult);
            }
            catch (Exception es)
            {
                schoolList._message = es.Message;
                schoolList._failure = true;
            }
            return schoolList;
        }

        [HttpPost("checkSchoolInternalId")]
        public ActionResult<CheckSchoolInternalIdViewModel> CheckSchoolInternalId(CheckSchoolInternalIdViewModel checkSchoolInternalIdViewModel)
        {
            CheckSchoolInternalIdViewModel checkInternalId = new CheckSchoolInternalIdViewModel();
            try
            {
                checkInternalId = _schoolRegisterService.CheckSchoolInternalId(checkSchoolInternalIdViewModel);
            }
            catch (Exception es)
            {
                checkInternalId._message = es.Message;
                checkInternalId._failure = true;
            }
            return checkInternalId;
        }

        [HttpPost("studentEnrollmentSchoolList")]

        public ActionResult<SchoolListViewModel> StudentEnrollmentSchoolList(SchoolListViewModel schoolListViewModel)
        {

            SchoolListViewModel schoolListView = new SchoolListViewModel();
            try
            {
                schoolListView = _schoolRegisterService.StudentEnrollmentSchoolList(schoolListViewModel);
            }
            catch (Exception es)
            {
                schoolListView._message = es.Message;
                schoolListView._failure = true;
            }
            return schoolListView;
        }

        [HttpPut("addUpdateSchoolLogo")]
        public ActionResult<SchoolAddViewModel> AddUpdateSchoolLogo(SchoolAddViewModel schoolAddViewModel)
        {
            SchoolAddViewModel schoolLogoUpdate = new SchoolAddViewModel();
            try
            {
                schoolLogoUpdate = _schoolRegisterService.AddUpdateSchoolLogo(schoolAddViewModel);
            }
            catch (Exception es)
            {
                schoolLogoUpdate._failure = true;
                schoolLogoUpdate._message = es.Message;
            }
            return schoolLogoUpdate;
        }

        [HttpPost("copySchool")]
        public ActionResult<CopySchoolViewModel> CopySchool(CopySchoolViewModel copySchoolViewModel)
        {
            CopySchoolViewModel copySchool = new CopySchoolViewModel();
            try
            {
                copySchool = _schoolRegisterService.CopySchool(copySchoolViewModel);
            }
            catch (Exception es)
            {
                copySchool._failure = true;
                copySchool._message = es.Message;
            }
            return copySchool;
        }


    }
}