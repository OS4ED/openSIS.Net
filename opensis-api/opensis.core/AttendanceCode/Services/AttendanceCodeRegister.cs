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

using opensis.core.AttendanceCode.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.AttendanceCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.AttendanceCode.Services
{
    public class AttendanceCodeRegister : IAttendanceCodeRegisterService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IAttendanceCodeRepository attendanceCodeRepository;
        public AttendanceCodeRegister(IAttendanceCodeRepository attendanceCodeRepository)
        {
            this.attendanceCodeRepository = attendanceCodeRepository;
        }
        public AttendanceCodeRegister() { }
        /// <summary>
        /// Save AttendanceCode
        /// </summary>
        /// <param name="attendanceCodeAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeAddViewModel SaveAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel AttendanceCodeAddModel = new AttendanceCodeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeAddViewModel._tenantName+ attendanceCodeAddViewModel._userName, attendanceCodeAddViewModel._token))
                {

                    AttendanceCodeAddModel = this.attendanceCodeRepository.AddAttendanceCode(attendanceCodeAddViewModel);                

                }
                else
                {
                    AttendanceCodeAddModel._failure = true;
                    AttendanceCodeAddModel._message = TOKENINVALID;
                    
                }
            }
            catch (Exception es)
            {

                AttendanceCodeAddModel._failure = true;
                AttendanceCodeAddModel._message = es.Message;
            }
            return AttendanceCodeAddModel;

        }
        /// <summary>
        /// Get AttendanceCode By Id
        /// </summary>
        /// <param name="attendanceCodeAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeAddViewModel ViewAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeViewModel = new AttendanceCodeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeAddViewModel._tenantName + attendanceCodeAddViewModel._userName, attendanceCodeAddViewModel._token))
                {
                    attendanceCodeViewModel = this.attendanceCodeRepository.ViewAttendanceCode(attendanceCodeAddViewModel);
                }
                else
                {
                    attendanceCodeViewModel._failure = true;
                    attendanceCodeViewModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeViewModel._failure = true;
                attendanceCodeViewModel._message = es.Message;
            }
            return attendanceCodeViewModel;
        }
        /// <summary>
        /// Update AttendanceCode
        /// </summary>
        /// <param name="attendanceCodeAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeAddViewModel UpdateAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeUpdateModel = new AttendanceCodeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeAddViewModel._tenantName + attendanceCodeAddViewModel._userName, attendanceCodeAddViewModel._token))
                {
                    attendanceCodeUpdateModel = this.attendanceCodeRepository.UpdateAttendanceCode(attendanceCodeAddViewModel);
                }
                else
                {
                    attendanceCodeUpdateModel._failure = true;
                    attendanceCodeUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeUpdateModel._failure = true;
                attendanceCodeUpdateModel._message = es.Message;
            }

            return attendanceCodeUpdateModel;
        }
        /// <summary>
        /// Get All AttendanceCode
        /// </summary>
        /// <param name="attendanceCodeListViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeListViewModel GetAllAttendanceCode(AttendanceCodeListViewModel attendanceCodeListViewModel)
        {
            AttendanceCodeListViewModel attendanceCodeListModel = new AttendanceCodeListViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeListViewModel._tenantName + attendanceCodeListViewModel._userName, attendanceCodeListViewModel._token))
                {
                    attendanceCodeListModel = this.attendanceCodeRepository.GetAllAttendanceCode(attendanceCodeListViewModel);
                }
                else
                {
                    attendanceCodeListModel._failure = true;
                    attendanceCodeListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeListModel._failure = true;
                attendanceCodeListModel._message = es.Message;
            }

            return attendanceCodeListModel;
        }
        /// <summary>
        /// Delete AttendanceCode
        /// </summary>
        /// <param name="attendanceCodeAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeAddViewModel DeleteAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeDeleteModel = new AttendanceCodeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeAddViewModel._tenantName + attendanceCodeAddViewModel._userName, attendanceCodeAddViewModel._token))
                {
                    attendanceCodeDeleteModel = this.attendanceCodeRepository.DeleteAttendanceCode(attendanceCodeAddViewModel);
                }
                else
                {
                    attendanceCodeDeleteModel._failure = true;
                    attendanceCodeDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeDeleteModel._failure = true;
                attendanceCodeDeleteModel._message = es.Message;
            }

            return attendanceCodeDeleteModel;
        }
        /// <summary>
        /// Add  AttendanceCodeCategories
        /// </summary>
        /// <param name="attendanceCodeCategoriesAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeCategoriesAddViewModel SaveAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel AttendanceCodeCategoriesAddModel = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeCategoriesAddViewModel._tenantName + attendanceCodeCategoriesAddViewModel._userName, attendanceCodeCategoriesAddViewModel._token))
                {

                    AttendanceCodeCategoriesAddModel = this.attendanceCodeRepository.AddAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);                 

                }
                else
                {
                    AttendanceCodeCategoriesAddModel._failure = true;
                    AttendanceCodeCategoriesAddModel._message = TOKENINVALID;                    
                }
            }
            catch (Exception es)
            {

                AttendanceCodeCategoriesAddModel._failure = true;
                AttendanceCodeCategoriesAddModel._message = es.Message;
            }
            return AttendanceCodeCategoriesAddModel;
        }
        /// <summary>
        /// Get  AttendanceCodeCategories By Id
        /// </summary>
        /// <param name="attendanceCodeCategoriesAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeCategoriesAddViewModel ViewAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesViewModel = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeCategoriesAddViewModel._tenantName + attendanceCodeCategoriesAddViewModel._userName, attendanceCodeCategoriesAddViewModel._token))
                {
                    attendanceCodeCategoriesViewModel = this.attendanceCodeRepository.ViewAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
                }
                else
                {
                    attendanceCodeCategoriesViewModel._failure = true;
                    attendanceCodeCategoriesViewModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesViewModel._failure = true;
                attendanceCodeCategoriesViewModel._message = es.Message;
            }
            return attendanceCodeCategoriesViewModel;
        }
        /// <summary>
        /// Update  AttendanceCodeCategories
        /// </summary>
        /// <param name="attendanceCodeCategoriesAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeCategoriesAddViewModel UpdateAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesUpdateModel = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeCategoriesAddViewModel._tenantName + attendanceCodeCategoriesAddViewModel._userName, attendanceCodeCategoriesAddViewModel._token))
                {
                    attendanceCodeCategoriesUpdateModel = this.attendanceCodeRepository.UpdateAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
                }
                else
                {
                    attendanceCodeCategoriesUpdateModel._failure = true;
                    attendanceCodeCategoriesUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesUpdateModel._failure = true;
                attendanceCodeCategoriesUpdateModel._message = es.Message;
            }

            return attendanceCodeCategoriesUpdateModel;
        }
        /// <summary>
        /// Get All AttendanceCodeCategories
        /// </summary>
        /// <param name="attendanceCodeCategoriesListViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeCategoriesListViewModel GetAllAttendanceCodeCategories(AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesListViewModel)
        {
            AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesListModel = new AttendanceCodeCategoriesListViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeCategoriesListViewModel._tenantName + attendanceCodeCategoriesListViewModel._userName, attendanceCodeCategoriesListViewModel._token))
                {
                    attendanceCodeCategoriesListModel = this.attendanceCodeRepository.GetAllAttendanceCodeCategories(attendanceCodeCategoriesListViewModel);
                }
                else
                {
                    attendanceCodeCategoriesListModel._failure = true;
                    attendanceCodeCategoriesListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesListModel._failure = true;
                attendanceCodeCategoriesListModel._message = es.Message;
            }

            return attendanceCodeCategoriesListModel;
        }
        /// <summary>
        /// Delete AttendanceCodeCategories
        /// </summary>
        /// <param name="attendanceCodeCategoriesAddViewModel"></param>
        /// <returns></returns>
        public AttendanceCodeCategoriesAddViewModel DeleteAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesDeleteModel = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeCategoriesAddViewModel._tenantName + attendanceCodeCategoriesAddViewModel._userName, attendanceCodeCategoriesAddViewModel._token))
                {
                    attendanceCodeCategoriesDeleteModel = this.attendanceCodeRepository.DeleteAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
                }
                else
                {
                    attendanceCodeCategoriesDeleteModel._failure = true;
                    attendanceCodeCategoriesDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesDeleteModel._failure = true;
                attendanceCodeCategoriesDeleteModel._message = es.Message;
            }

            return attendanceCodeCategoriesDeleteModel;
        }

        /// <summary>
        /// Update Attendance Code Sort Order
        /// </summary>
        /// <param name="attendanceCodeSortOrderModel"></param>
        /// <returns></returns>
        public AttendanceCodeSortOrderModel UpdateAttendanceCodeSortOrder(AttendanceCodeSortOrderModel attendanceCodeSortOrderModel)
        {
            AttendanceCodeSortOrderModel AttendanceCodeSortOrderUpdate = new AttendanceCodeSortOrderModel();
            try
            {
                if (TokenManager.CheckToken(attendanceCodeSortOrderModel._tenantName + attendanceCodeSortOrderModel._userName, attendanceCodeSortOrderModel._token))
                {
                    AttendanceCodeSortOrderUpdate = this.attendanceCodeRepository.UpdateAttendanceCodeSortOrder(attendanceCodeSortOrderModel);
                }
                else
                {
                    AttendanceCodeSortOrderUpdate._failure = true;
                    AttendanceCodeSortOrderUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                AttendanceCodeSortOrderUpdate._failure = true;
                AttendanceCodeSortOrderUpdate._message = es.Message;
            }

            return AttendanceCodeSortOrderUpdate;
        }

    }
}
