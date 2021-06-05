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

using opensis.core.CustomField.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.CustomField;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.CustomField.Services
{
    public class CustomFieldService : ICustomFieldService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ICustomFieldRepository customFieldRepository;
        public CustomFieldService(ICustomFieldRepository customFieldRepository)
        {
            this.customFieldRepository = customFieldRepository;
        }

        //Required for Unit Testing
        public CustomFieldService() { }

        /// <summary>
        /// Add Custom Field
        /// </summary>
        /// <param name="customFieldAddViewModel"></param>
        /// <returns></returns>
        public CustomFieldAddViewModel SaveCustomField(CustomFieldAddViewModel customFieldAddViewModel)
        {
            CustomFieldAddViewModel customFieldAdd = new CustomFieldAddViewModel();
            if (TokenManager.CheckToken(customFieldAddViewModel._tenantName + customFieldAddViewModel._userName, customFieldAddViewModel._token))
            {
                customFieldAdd = this.customFieldRepository.AddCustomField(customFieldAddViewModel);
            }
            else
            {
                customFieldAdd._failure = true;
                customFieldAdd._message = TOKENINVALID;
            }
            return customFieldAdd;
        }

        /// <summary>
        /// View Custom Field By Id
        /// </summary>
        /// <param name="customFieldAddViewModel"></param>
        /// <returns></returns>

        //public CustomFieldAddViewModel ViewCustomField(CustomFieldAddViewModel customFieldAddViewModel)
        //{
        //    CustomFieldAddViewModel customFieldView = new CustomFieldAddViewModel();
        //    try
        //    {
        //        if (TokenManager.CheckToken(customFieldAddViewModel._tenantName, customFieldAddViewModel._token))
        //        {
        //            customFieldView = this.customFieldRepository.ViewCustomField(customFieldAddViewModel);
        //        }
        //        else
        //        {
        //            customFieldView._failure = true;
        //            customFieldView._message = TOKENINVALID;
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        customFieldView._failure = true;
        //        customFieldView._message = es.Message;
        //    }
        //    return customFieldView;
        //}

        /// <summary>
        /// Update Custom Field
        /// </summary>
        /// <param name="customFieldAddViewModel"></param>
        /// <returns></returns>
        public CustomFieldAddViewModel UpdateCustomField(CustomFieldAddViewModel customFieldAddViewModel)
        {
            CustomFieldAddViewModel customFieldUpdate = new CustomFieldAddViewModel();
            try
            {
                if (TokenManager.CheckToken(customFieldAddViewModel._tenantName + customFieldAddViewModel._userName, customFieldAddViewModel._token))
                {
                    customFieldUpdate = this.customFieldRepository.UpdateCustomField(customFieldAddViewModel);
                }
                else
                {
                    customFieldUpdate._failure = true;
                    customFieldUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                customFieldUpdate._failure = true;
                customFieldUpdate._message = es.Message;
            }
            return customFieldUpdate;
        }

        /// <summary>
        /// Delete Custom Field
        /// </summary>
        /// <param name="customFieldAddViewModel"></param>
        /// <returns></returns>
        public CustomFieldAddViewModel DeleteCustomField(CustomFieldAddViewModel customFieldAddViewModel)
        {
            CustomFieldAddViewModel customFieldDelete = new CustomFieldAddViewModel();
            try
            {
                if (TokenManager.CheckToken(customFieldAddViewModel._tenantName + customFieldAddViewModel._userName, customFieldAddViewModel._token))
                {
                    customFieldDelete = this.customFieldRepository.DeleteCustomField(customFieldAddViewModel);
                }
                else
                {
                    customFieldDelete._failure = true;
                    customFieldDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                customFieldDelete._failure = true;
                customFieldDelete._message = es.Message;
            }
            return customFieldDelete;
        }

        /// <summary>
        /// Add FieldsCategory
        /// </summary>
        /// <param name="fieldsCategoryAddViewModel"></param>
        /// <returns></returns>
        public FieldsCategoryAddViewModel SaveFieldsCategory(FieldsCategoryAddViewModel fieldsCategoryAddViewModel)
        {
            FieldsCategoryAddViewModel FieldsCategoryAddModel = new FieldsCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(fieldsCategoryAddViewModel._tenantName + fieldsCategoryAddViewModel._userName, fieldsCategoryAddViewModel._token))
                {

                    FieldsCategoryAddModel = this.customFieldRepository.AddFieldsCategory(fieldsCategoryAddViewModel);

                }
                else
                {
                    FieldsCategoryAddModel._failure = true;
                    FieldsCategoryAddModel._message = TOKENINVALID;

                }
            }
            catch (Exception es)
            {

                FieldsCategoryAddModel._failure = true;
                FieldsCategoryAddModel._message = es.Message;
            }
            return FieldsCategoryAddModel;
        }
        /// <summary>
        /// View By Id FieldsCategory
        /// </summary>
        /// <param name="fieldsCategoryAddViewModel"></param>
        /// <returns></returns>
        //public FieldsCategoryAddViewModel ViewFieldsCategory(FieldsCategoryAddViewModel fieldsCategoryAddViewModel)
        //{
        //    FieldsCategoryAddViewModel fieldsCategoryViewModel = new FieldsCategoryAddViewModel();
        //    try
        //    {
        //        if (TokenManager.CheckToken(fieldsCategoryAddViewModel._tenantName, fieldsCategoryAddViewModel._token))
        //        {
        //            fieldsCategoryViewModel = this.customFieldRepository.ViewFieldsCategory(fieldsCategoryAddViewModel);
        //        }
        //        else
        //        {
        //            fieldsCategoryViewModel._failure = true;
        //            fieldsCategoryViewModel._message = TOKENINVALID;
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        fieldsCategoryViewModel._failure = true;
        //        fieldsCategoryViewModel._message = es.Message;
        //    }
        //    return fieldsCategoryViewModel;
        //}
        /// <summary>
        /// Update FieldsCategory
        /// </summary>
        /// <param name="fieldsCategoryAddViewModel"></param>
        /// <returns></returns>
        public FieldsCategoryAddViewModel UpdateFieldsCategory(FieldsCategoryAddViewModel fieldsCategoryAddViewModel)
        {
            FieldsCategoryAddViewModel fieldsCategoryUpdateModel = new FieldsCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(fieldsCategoryAddViewModel._tenantName + fieldsCategoryAddViewModel._userName, fieldsCategoryAddViewModel._token))
                {
                    fieldsCategoryUpdateModel = this.customFieldRepository.UpdateFieldsCategory(fieldsCategoryAddViewModel);
                }
                else
                {
                    fieldsCategoryUpdateModel._failure = true;
                    fieldsCategoryUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                fieldsCategoryUpdateModel._failure = true;
                fieldsCategoryUpdateModel._message = es.Message;
            }

            return fieldsCategoryUpdateModel;
        }
        /// <summary>
        /// Delete FieldsCategory
        /// </summary>
        /// <param name="fieldsCategoryAddViewModel"></param>
        /// <returns></returns>
        public FieldsCategoryAddViewModel DeleteFieldsCategory(FieldsCategoryAddViewModel fieldsCategoryAddViewModel)
        {
            FieldsCategoryAddViewModel fieldsCategoryDeleteModel = new FieldsCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(fieldsCategoryAddViewModel._tenantName + fieldsCategoryAddViewModel._userName, fieldsCategoryAddViewModel._token))
                {
                    fieldsCategoryDeleteModel = this.customFieldRepository.DeleteFieldsCategory(fieldsCategoryAddViewModel);
                }
                else
                {
                    fieldsCategoryDeleteModel._failure = true;
                    fieldsCategoryDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                fieldsCategoryDeleteModel._failure = true;
                fieldsCategoryDeleteModel._message = es.Message;
            }

            return fieldsCategoryDeleteModel;
        }
        /// <summary>
        /// Get All FieldsCategory
        /// </summary>
        /// <param name="fieldsCategoryListViewModel"></param>
        /// <returns></returns>
        public FieldsCategoryListViewModel GetAllFieldsCategory(FieldsCategoryListViewModel fieldsCategoryListViewModel)
        {
            FieldsCategoryListViewModel fieldsCategoryListModel = new FieldsCategoryListViewModel();
            try
            {
                if (TokenManager.CheckToken(fieldsCategoryListViewModel._tenantName + fieldsCategoryListViewModel._userName, fieldsCategoryListViewModel._token))
                {
                    fieldsCategoryListModel = this.customFieldRepository.GetAllFieldsCategory(fieldsCategoryListViewModel);
                }
                else
                {
                    fieldsCategoryListModel._failure = true;
                    fieldsCategoryListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                fieldsCategoryListModel._failure = true;
                fieldsCategoryListModel._message = es.Message;
            }

            return fieldsCategoryListModel;
        }
        /// <summary>
        /// Update Custom Field Sort Order
        /// </summary>
        /// <param name="customFieldSortOrderModel"></param>
        /// <returns></returns>
        public CustomFieldSortOrderModel UpdateCustomFieldSortOrder(CustomFieldSortOrderModel customFieldSortOrderModel)
        {
            CustomFieldSortOrderModel customFieldSortOrderUpdateModel = new CustomFieldSortOrderModel();
            try
            {
                if (TokenManager.CheckToken(customFieldSortOrderModel._tenantName + customFieldSortOrderModel._userName, customFieldSortOrderModel._token))
                {
                    customFieldSortOrderUpdateModel = this.customFieldRepository.UpdateCustomFieldSortOrder(customFieldSortOrderModel);
                }
                else
                {
                    customFieldSortOrderUpdateModel._failure = true;
                    customFieldSortOrderUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                customFieldSortOrderUpdateModel._failure = true;
                customFieldSortOrderUpdateModel._message = es.Message;
            }

            return customFieldSortOrderUpdateModel;
        }


    }
}
