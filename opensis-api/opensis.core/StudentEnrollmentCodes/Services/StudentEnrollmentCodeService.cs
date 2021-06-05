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
using opensis.core.StudentEnrollmentCodes.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StudentEnrollmentCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentEnrollmentCodes.Services
{
    public class StudentEnrollmentCodeService: IStudentEnrollmentCodeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentEnrollmentCodeRepository studentEnrollmentCodeRepository;
        public StudentEnrollmentCodeService(IStudentEnrollmentCodeRepository studentEnrollmentCodeRepository)
        {
            this.studentEnrollmentCodeRepository = studentEnrollmentCodeRepository;
        }
        //Required for Unit Testing
        public StudentEnrollmentCodeService() { }

        /// <summary>
        /// Add StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel SaveStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAdd = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeAdd = this.studentEnrollmentCodeRepository.AddStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);

            }
            else
            {
                studentEnrollmentCodeAdd._failure = true;
                studentEnrollmentCodeAdd._message = TOKENINVALID;
            }
            return studentEnrollmentCodeAdd;
        }

        /// <summary>
        /// View By Id StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel ViewStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeView = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeView = this.studentEnrollmentCodeRepository.ViewStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);

            }
            else
            {
                studentEnrollmentCodeView._failure = true;
                studentEnrollmentCodeView._message = TOKENINVALID;
            }
            return studentEnrollmentCodeView;
        }

        /// <summary>
        /// Delete StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel DeleteStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeDel = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeDel = this.studentEnrollmentCodeRepository.DeleteStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);             
            }
            else
            {
                studentEnrollmentCodeDel._failure = true;
                studentEnrollmentCodeDel._message = TOKENINVALID;
            }
            return studentEnrollmentCodeDel;
        }

        /// <summary>
        /// Update StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel UpdateStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeUpdate = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeUpdate = this.studentEnrollmentCodeRepository.UpdateStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            else
            {
                studentEnrollmentCodeUpdate._failure = true;
                studentEnrollmentCodeUpdate._message = TOKENINVALID;
            }
            return studentEnrollmentCodeUpdate;
        }
        /// <summary>
        /// Get All Student Enrollment Code
        /// </summary>
        /// <param name="studentEnrollmentCodeListView"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeListViewModel GetAllStudentEnrollmentCode(StudentEnrollmentCodeListViewModel studentEnrollmentCodeListView)
        {
            StudentEnrollmentCodeListViewModel studentEnrollmentCodeList = new StudentEnrollmentCodeListViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeListView._tenantName + studentEnrollmentCodeListView._userName, studentEnrollmentCodeListView._token))
            {
                studentEnrollmentCodeList = this.studentEnrollmentCodeRepository.GetAllStudentEnrollmentCode(studentEnrollmentCodeListView);
                
            }
            else
            {
                studentEnrollmentCodeList._failure = true;
                studentEnrollmentCodeList._message = TOKENINVALID;
               
            }
            return studentEnrollmentCodeList;
        }


    }
}
