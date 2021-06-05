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
using opensis.core.SchoolPeriod.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.SchoolPeriod;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.SchoolPeriod.Services
{
    public class SchoolPeriodService: ISchoolPeriodService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ISchoolPeriodRepository schoolPeriodRepository;
        public SchoolPeriodService(ISchoolPeriodRepository schoolPeriodRepository)
        {
            this.schoolPeriodRepository = schoolPeriodRepository;
        }
        //Required for Unit Testing
        public SchoolPeriodService() { }

        /// <summary>
        /// Add School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel SaveSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodAddViewModel = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName+ schoolPeriod._userName, schoolPeriod._token))
            {

                schoolPeriodAddViewModel = this.schoolPeriodRepository.AddSchoolPeriod(schoolPeriod);

                return schoolPeriodAddViewModel;

            }
            else
            {
                schoolPeriodAddViewModel._failure = true;
                schoolPeriodAddViewModel._message = TOKENINVALID;
                return schoolPeriodAddViewModel;
            }
        }

        /// <summary>
        /// Update School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel UpdateSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodUpdate = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
            {
                schoolPeriodUpdate = this.schoolPeriodRepository.UpdateSchoolPeriod(schoolPeriod);

                return schoolPeriodUpdate;
            }
            else
            {
                schoolPeriodUpdate._failure = true;
                schoolPeriodUpdate._message = TOKENINVALID;
                return schoolPeriodUpdate;
            }

        }

        /// <summary>
        /// View School Period By Id
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel ViewSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodView = new SchoolPeriodAddViewModel();
            if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
            {
                schoolPeriodView = this.schoolPeriodRepository.ViewSchoolPeriod(schoolPeriod);
                return schoolPeriodView;
            }
            else
            {
                schoolPeriodView._failure = true;
                schoolPeriodView._message = TOKENINVALID;
                return schoolPeriodView;
            }
        }

        /// <summary>
        /// Delete School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel DeleteSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            SchoolPeriodAddViewModel schoolPeriodDelete = new SchoolPeriodAddViewModel();
            try
            {
                if (TokenManager.CheckToken(schoolPeriod._tenantName + schoolPeriod._userName, schoolPeriod._token))
                {
                    schoolPeriodDelete = this.schoolPeriodRepository.DeleteSchoolPeriod(schoolPeriod);
                }
                else
                {
                    schoolPeriodDelete._failure = true;
                    schoolPeriodDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                schoolPeriodDelete._failure = true;
                schoolPeriodDelete._message = es.Message;
            }

            return schoolPeriodDelete;
        }

    }
}
