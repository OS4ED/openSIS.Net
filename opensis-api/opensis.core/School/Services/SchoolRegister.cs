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

using Microsoft.EntityFrameworkCore;
using opensis.core.helper;
using opensis.core.School.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opensis.core.School.Services
{
    public class SchoolRegister : ISchoolRegisterService
    {

        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";
       
        public ISchoolRepository schoolRepository;
        public SchoolRegister(ISchoolRepository schoolRepository)
        {
            this.schoolRepository = schoolRepository;
        }

        //Required for Unit Testing
        public SchoolRegister() { }
        
        public SchoolListModel GetAllSchools(SchoolListModel school)
        {
            logger.Info("Method getAllSchools called.");
            SchoolListModel schoolList = new SchoolListModel();
            try
            {
                if (TokenManager.CheckToken(school._tenantName + school._userName, school._token))
                {
                    schoolList = this.schoolRepository.GetAllSchools(school);
                    schoolList._message = SUCCESS;
                    schoolList._failure = false;
                    logger.Info("Method getAllSchools end with success.");
                }

                else
                {
                    schoolList._failure = true;
                    schoolList._message = TOKENINVALID;
                    return schoolList;
                }
            }
            catch (Exception ex)
            {
                schoolList._message = ex.Message;
                schoolList._failure = true;
                logger.Error("Method getAllSchools end with error :" + ex.Message);
            }


            return schoolList;
        }


        public SchoolListModel GetAllSchoolList(PageResult pageResult)
        {
            logger.Info("Method getAllSchoolList called.");
            SchoolListModel schoolList = new SchoolListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName+pageResult._userName, pageResult._token))
                {
                    schoolList = this.schoolRepository.GetAllSchoolList(pageResult);
                    schoolList._message = SUCCESS;
                    schoolList._failure = false;
                    logger.Info("Method getAllSchoolList end with success.");
                }

                else
                {
                    schoolList._failure = true;
                    schoolList._message = TOKENINVALID;
                    return schoolList;
                }
            }
            catch (Exception ex)
            {
                schoolList._message = ex.Message;
                schoolList._failure = true;
                logger.Error("Method getAllSchools end with error :" + ex.Message);
            }


            return schoolList;

            
        }

        
        public SchoolAddViewModel UpdateSchool(SchoolAddViewModel schools)
        {
            SchoolAddViewModel SchoolAddViewModel = new SchoolAddViewModel();
            if (TokenManager.CheckToken(schools._tenantName + schools._userName, schools._token))
            {
                SchoolAddViewModel =  this.schoolRepository.UpdateSchool(schools);
                //return getAllSchools();
                return SchoolAddViewModel;
            }
            else
            {
                SchoolAddViewModel._failure = true;
                SchoolAddViewModel._message = TOKENINVALID;
                return SchoolAddViewModel;
            }

        }

        public SchoolAddViewModel SaveSchool(SchoolAddViewModel schools)
        {
            SchoolAddViewModel SchoolAddViewModel = new SchoolAddViewModel();
            if (TokenManager.CheckToken(schools._tenantName + schools._userName, schools._token))
            {
                
                    SchoolAddViewModel = this.schoolRepository.AddSchool(schools);
                    //return getAllSchools();
                    return SchoolAddViewModel;
               
            }
            else
            {
                SchoolAddViewModel._failure = true;
                SchoolAddViewModel._message = TOKENINVALID;
                return SchoolAddViewModel;
            }

        }
        public SchoolAddViewModel ViewSchool(SchoolAddViewModel schools)
        {
            SchoolAddViewModel SchoolAddViewModel = new SchoolAddViewModel();
            if (TokenManager.CheckToken(schools._tenantName + schools._userName, schools._token))
            {
                SchoolAddViewModel =  this.schoolRepository.ViewSchool(schools);
                //return getAllSchools();
                return SchoolAddViewModel;

            }
            else
            {
                SchoolAddViewModel._failure = true;
                SchoolAddViewModel._message = TOKENINVALID;
                return SchoolAddViewModel;
            }

        }

        /// <summary>
        /// Check School InternalId Exist or Not
        /// </summary>
        /// <param name="checkSchoolInternalIdViewModel"></param>
        /// <returns></returns>
        public CheckSchoolInternalIdViewModel CheckSchoolInternalId(CheckSchoolInternalIdViewModel checkSchoolInternalIdViewModel)
        {
            CheckSchoolInternalIdViewModel checkInternalId = new CheckSchoolInternalIdViewModel();
            if (TokenManager.CheckToken(checkSchoolInternalIdViewModel._tenantName + checkSchoolInternalIdViewModel._userName, checkSchoolInternalIdViewModel._token))
            {
                checkInternalId = this.schoolRepository.CheckSchoolInternalId(checkSchoolInternalIdViewModel);
            }
            else
            {
                checkInternalId._failure = true;
                checkInternalId._message = TOKENINVALID;
            }
            return checkInternalId;
        }

        //public bool IsMandatoryFieldsArePresent(Schools schools)
        //{
        //    bool isvalid = false;
        //    if (schools.tenant_id != "" && schools.school_name != "")
        //    {
        //        isvalid = true;
        //    }

        //    return isvalid;
        //}
        /// <summary>
        /// Student Enrollment School List
        /// </summary>
        /// <param name="schoolListViewModel"></param>
        /// <returns></returns>
        public SchoolListViewModel StudentEnrollmentSchoolList(SchoolListViewModel schoolListViewModel)
        {
            logger.Info("Method studentEnrollmentSchoolList called.");
            SchoolListViewModel schoolListView = new SchoolListViewModel();
            try
            {
                if (TokenManager.CheckToken(schoolListViewModel._tenantName + schoolListViewModel._userName, schoolListViewModel._token))
                {
                    schoolListView = this.schoolRepository.StudentEnrollmentSchoolList(schoolListViewModel);
                    schoolListView._message = SUCCESS;
                    schoolListView._failure = false;
                    logger.Info("Method StudentEnrollmentSchoolList end with success.");
                }
                else
                {
                    schoolListView._failure = true;
                    schoolListView._message = TOKENINVALID;
                }
            }
            catch (Exception ex)
            {
                schoolListView._message = ex.Message;
                schoolListView._failure = true;
                logger.Error("Method StudentEnrollmentSchoolList end with error :" + ex.Message);
            }
            return schoolListView;
        }

        /// <summary>
        /// Add Update School Logo
        /// </summary>
        /// <param name="schoolAddViewModel"></param>
        /// <returns></returns>
        public SchoolAddViewModel AddUpdateSchoolLogo(SchoolAddViewModel schoolAddViewModel)
        {
            SchoolAddViewModel schoolLogoUpdate = new SchoolAddViewModel();
            try
            {               
                if (TokenManager.CheckToken(schoolAddViewModel._tenantName + schoolAddViewModel._userName, schoolAddViewModel._token))
                {
                    schoolLogoUpdate = this.schoolRepository.AddUpdateSchoolLogo(schoolAddViewModel);
                }
                else
                {
                    schoolLogoUpdate._failure = true;
                    schoolLogoUpdate._message = TOKENINVALID;
                }                
            }
            catch (Exception es)
            {
                schoolLogoUpdate._message = es.Message;
                schoolLogoUpdate._failure = true;
            }
            return schoolLogoUpdate;
        }

        /// <summary>
        /// Copy School
        /// </summary>
        /// <param name="copySchoolViewModel"></param>
        /// <returns></returns>
        public CopySchoolViewModel CopySchool(CopySchoolViewModel copySchoolViewModel)
        {
            CopySchoolViewModel copySchool = new CopySchoolViewModel();
            try
            {
                if (TokenManager.CheckToken(copySchoolViewModel._tenantName + copySchoolViewModel._userName, copySchoolViewModel._token))
                {
                    copySchool = this.schoolRepository.CopySchool(copySchoolViewModel);
                }
                else
                {
                    copySchool._failure = true;
                    copySchool._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                copySchool._message = es.Message;
                copySchool._failure = true;
            }
            return copySchool;
        }

    }
}
