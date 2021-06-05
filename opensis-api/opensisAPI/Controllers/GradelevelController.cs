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
using opensis.core.GradeLevel.Interfaces;
using opensis.data.ViewModels.GradeLevel;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Gradelevel")]
    [ApiController]
    public class GradeLevelController : ControllerBase
    {
        private IGradelevelService _gradelevelService;
        public GradeLevelController(IGradelevelService gradelevelService)
        {
            _gradelevelService = gradelevelService;
        }

        [HttpPost("addGradelevel")]
        public ActionResult<GradelevelViewModel> AddGradelevel(GradelevelViewModel gradelevel)
        {
            GradelevelViewModel gradelevelView = new GradelevelViewModel();
            try
            {
                if (gradelevel.tblGradelevel.SchoolId > 0)
                {
                    gradelevelView = _gradelevelService.AddGradelevel(gradelevel);
                }
                else
                {
                    gradelevelView._token = gradelevel._token;
                    gradelevelView._tenantName = gradelevel._tenantName;
                    gradelevelView._failure = true;
                    gradelevelView._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                gradelevelView._failure = true;
                gradelevelView._message = es.Message;
            }
            return gradelevelView;
        }
        [HttpPost("viewGradelevel")]

        public ActionResult<GradelevelViewModel> ViewGradelevel(GradelevelViewModel gradelevel)
        {
            GradelevelViewModel gradelevelView = new GradelevelViewModel();
            try
            {
                if (gradelevel.tblGradelevel.SchoolId > 0)
                {
                    gradelevelView = _gradelevelService.ViewGradelevel(gradelevel);
                }
                else
                {
                    gradelevelView._token = gradelevel._token;
                    gradelevelView._tenantName = gradelevel._tenantName;
                    gradelevelView._failure = true;
                    gradelevelView._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                gradelevelView._failure = true;
                gradelevelView._message = es.Message;
            }
            return gradelevelView;
        }

        [HttpPut("updateGradelevel")]

        public ActionResult<GradelevelViewModel> UpdateGradelevel(GradelevelViewModel gradelevel)
        {
            GradelevelViewModel gradelevelUpdate = new GradelevelViewModel();
            try
            {
                if (gradelevel.tblGradelevel.SchoolId > 0)
                {
                    gradelevelUpdate = _gradelevelService.UpdateGradelevel(gradelevel);
                }
                else
                {
                    gradelevelUpdate._token = gradelevel._token;
                    gradelevelUpdate._tenantName = gradelevel._tenantName;
                    gradelevelUpdate._failure = true;
                    gradelevelUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                gradelevelUpdate._failure = true;
                gradelevelUpdate._message = es.Message;
            }
            return gradelevelUpdate;
        }

        [HttpPost("getAllGradeLevels")]

        public ActionResult<GradelevelListViewModel> GetAllGradeLevels(GradelevelListViewModel gradelevel)
        {
            GradelevelListViewModel gradelevelList = new GradelevelListViewModel();
            try
            {
                if (gradelevel.SchoolId > 0)
                {
                    gradelevelList = _gradelevelService.GetAllGradeLevels(gradelevel);
                }
                else
                {
                    gradelevelList._token = gradelevel._token;
                    gradelevelList._tenantName = gradelevel._tenantName;
                    gradelevelList._failure = true;
                    gradelevelList._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                gradelevelList._failure = true;
                gradelevelList._message = es.Message;
            }
            return gradelevelList;
        }
        [HttpPost("deleteGradelevel")]

        public ActionResult<GradelevelViewModel> DeleteGradelevel(GradelevelViewModel gradelevel)
        {
            GradelevelViewModel gradelevelDelete = new GradelevelViewModel();
            try
            {
                if (gradelevel.tblGradelevel.SchoolId > 0)
                {
                    gradelevelDelete = _gradelevelService.DeleteGradelevel(gradelevel);
                }
                else
                {
                    gradelevelDelete._token = gradelevel._token;
                    gradelevelDelete._tenantName = gradelevel._tenantName;
                    gradelevelDelete._failure = true;
                    gradelevelDelete._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                gradelevelDelete._failure = true;
                gradelevelDelete._message = es.Message;
            }
            return gradelevelDelete;
        }
        [HttpPost("getAllGradeEquivalency")]

        public ActionResult<GradeEquivalencyListViewModel> GetAllGradeEquivalency(GradeEquivalencyListViewModel gradeEquivalencyList)
        {
            GradeEquivalencyListViewModel GradeEquivalencyList = new GradeEquivalencyListViewModel();
            try
            {
                GradeEquivalencyList = _gradelevelService.GetAllGradeEquivalency(gradeEquivalencyList);
            }
            catch (Exception es)
            {
                GradeEquivalencyList._failure = true;
                GradeEquivalencyList._message = es.Message;
            }
            return GradeEquivalencyList;
        }
    }
}
