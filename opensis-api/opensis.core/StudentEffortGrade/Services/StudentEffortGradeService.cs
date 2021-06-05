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
using opensis.core.StudentEffortGrade.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentEffortGrade.Services
{
    public class StudentEffortGradeService : IStudentEffortGradeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentEffortGradeRepository studentEffortGradeRepository;
        public StudentEffortGradeService(IStudentEffortGradeRepository studentEffortGradeRepository)
        {
            this.studentEffortGradeRepository = studentEffortGradeRepository;
        }

        /// <summary>
        /// Add Update Student Effort Grade
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentEffortGradeListModel._tenantName + studentEffortGradeListModel._userName, studentEffortGradeListModel._token))
                {
                    studentEffortGradeList = this.studentEffortGradeRepository.AddUpdateStudentEffortGrade(studentEffortGradeListModel);
                }
                else
                {
                    studentEffortGradeList._failure = true;
                    studentEffortGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentEffortGradeList._failure = true;
                studentEffortGradeList._message = es.Message;
            }
            return studentEffortGradeList;
        }

        /// <summary>
        /// Get All Student Effort Grade List
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentEffortGradeListModel._tenantName + studentEffortGradeListModel._userName, studentEffortGradeListModel._token))
                {
                    studentEffortGradeList = this.studentEffortGradeRepository.GetAllStudentEffortGradeList(studentEffortGradeListModel);
                }
                else
                {
                    studentEffortGradeList._failure = true;
                    studentEffortGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentEffortGradeList._failure = true;
                studentEffortGradeList._message = es.Message;
            }
            return studentEffortGradeList;
        }
    }
}
