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
using opensis.core.ParentInfo.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.ParentInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.ParentInfo.Services
{
    public class ParentInfoRegister : IParentInfoRegisterService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IParentInfoRepository parentInfoRepository;
        public ParentInfoRegister(IParentInfoRepository parentInfoRepository)
        {
            this.parentInfoRepository = parentInfoRepository;
        }
        public ParentInfoRegister() { }

        /// <summary>
        /// Add Parent For Student
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentForStudent(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfoAddModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName + parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {

                    ParentInfoAddModel = this.parentInfoRepository.AddParentForStudent(parentInfoAddViewModel);

                }
                else
                {
                    ParentInfoAddModel._failure = true;
                    ParentInfoAddModel._message = TOKENINVALID;

                }
            }
            catch (Exception es)
            {

                ParentInfoAddModel._failure = true;
                ParentInfoAddModel._message = es.Message;
            }
            return ParentInfoAddModel;

        }
        /// <summary>
        /// View Parent List For Student
        /// </summary>
        /// <param name="parentInfoList"></param>
        /// <returns></returns>
        public ParentInfoListModel ViewParentListForStudent(ParentInfoListModel parentInfoList)
        {
            ParentInfoListModel parentInfoViewListModel = new ParentInfoListModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoList._tenantName + parentInfoList._userName, parentInfoList._token))
                {
                    parentInfoViewListModel = this.parentInfoRepository.ViewParentListForStudent(parentInfoList);
                }
                else
                {
                    parentInfoViewListModel._failure = true;
                    parentInfoViewListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
{
                parentInfoViewListModel._failure = true;
                parentInfoViewListModel._message = es.Message;
}

            return parentInfoViewListModel;
        }
        /// <summary>
        /// Update parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel UpdateParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoUpdateModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName + parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {
                    parentInfoUpdateModel = this.parentInfoRepository.UpdateParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    parentInfoUpdateModel._failure = true;
                    parentInfoUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentInfoUpdateModel._failure = true;
                parentInfoUpdateModel._message = es.Message;
            }

            return parentInfoUpdateModel;
        }
        /// <summary>
        /// Get All parent Info
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView GetAllParentInfoList(PageResult pageResult)
        {
            logger.Info("Method getAllParentInfoList called.");
            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName+pageResult._userName, pageResult._token))
                {
                    parentInfoList = this.parentInfoRepository.GetAllParentInfoList(pageResult);
                    if (parentInfoList.parentInfoForView.Count > 0)
                    {
                        parentInfoList._message = SUCCESS;
                        parentInfoList._failure = false;
                    }
                    else
                    {
                        parentInfoList._message = "NO RECORD FOUND";
                        parentInfoList._failure = true;
                    }
                    logger.Info("Method getAllParentInfoList end with success.");
                }

                else
                {
                    parentInfoList._failure = true;
                    parentInfoList._message = TOKENINVALID;
                    return parentInfoList;
                }
            }
            catch (Exception ex)
            {
                parentInfoList._message = ex.Message;
                parentInfoList._failure = true;
                logger.Error("Method getAllParentInfoList end with error :" + ex.Message);
            }


            return parentInfoList;


        }
        /// <summary>
        /// Delete parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel DeleteParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfodelete = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName + parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {
                    ParentInfodelete = this.parentInfoRepository.DeleteParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    ParentInfodelete._failure = true;
                    ParentInfodelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                ParentInfodelete._failure = true;
                ParentInfodelete._message = es.Message;
            }

            return ParentInfodelete;
        }
        /// <summary>
        /// Search Parent Info For Student
        /// </summary>
        /// <param name="getAllParentInfoListForView"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView SearchParentInfoForStudent(GetAllParentInfoListForView getAllParentInfoListForView)
        {
            logger.Info("Method SearchParentInfoForStudent called.");
            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                if (TokenManager.CheckToken(getAllParentInfoListForView._tenantName + getAllParentInfoListForView._userName, getAllParentInfoListForView._token))
                {
                    parentInfoList = this.parentInfoRepository.SearchParentInfoForStudent(getAllParentInfoListForView);
                    parentInfoList._message = SUCCESS;
                    parentInfoList._failure = false;
                    logger.Info("Method SearchParentInfoForStudent end with success.");
                }
                else
                {
                    parentInfoList._failure = true;
                    parentInfoList._message = TOKENINVALID;
                    return parentInfoList;
                }
            }
            catch (Exception ex)
            {
                parentInfoList._message = ex.Message;
                parentInfoList._failure = true;
                logger.Error("Method SearchParentInfoForStudent end with error :" + ex.Message);
            }
            return parentInfoList;
        }
        /// <summary>
        /// View Parent Info By Id
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel ViewParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            logger.Info("Method viewParentInfo called.");
            ParentInfoAddViewModel parentInfoViewModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName+ parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {
                    parentInfoViewModel = this.parentInfoRepository.ViewParentInfo(parentInfoAddViewModel);
                    parentInfoViewModel._message = SUCCESS;
                    parentInfoViewModel._failure = false;
                    logger.Info("Method viewParentInfo end with success.");

                }
                else
                {
                    parentInfoViewModel._failure = true;
                    parentInfoViewModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentInfoViewModel._message = es.Message;
                parentInfoViewModel._failure = true;
                logger.Error("Method viewParentInfo end with error :" + es.Message);
            }            
            return parentInfoViewModel;
        }
        /// <summary>
        /// Add Parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfoAddModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName + parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {
                    ParentInfoAddModel = this.parentInfoRepository.AddParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    ParentInfoAddModel._failure = true;
                    ParentInfoAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ParentInfoAddModel._failure = true;
                ParentInfoAddModel._message = es.Message;
            }
            return ParentInfoAddModel;
        }

        /// <summary>
        /// Remove Associated Parent
        /// </summary>
        /// <param name="parentInfoDeleteViewModel"></param>
        /// <returns></returns>
        public ParentInfoDeleteViewModel RemoveAssociatedParent(ParentInfoDeleteViewModel parentInfoDeleteViewModel)
        {
            ParentInfoDeleteViewModel parentAssociationshipDelete = new ParentInfoDeleteViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoDeleteViewModel._tenantName + parentInfoDeleteViewModel._userName, parentInfoDeleteViewModel._token))
                {
                    parentAssociationshipDelete = this.parentInfoRepository.RemoveAssociatedParent(parentInfoDeleteViewModel);
                }
                else
                {
                    parentAssociationshipDelete._failure = true;
                    parentAssociationshipDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentAssociationshipDelete._failure = true;
                parentAssociationshipDelete._message = es.Message;
            }

            return parentAssociationshipDelete;
        }

        /// <summary>
        /// Add Update Parent Photo
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddUpdateParentPhoto(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentPhotoUpdate = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName + parentInfoAddViewModel._userName, parentInfoAddViewModel._token))
                {
                    parentPhotoUpdate = this.parentInfoRepository.AddUpdateParentPhoto(parentInfoAddViewModel);
                }
                else
                {
                    parentPhotoUpdate._failure = true;
                    parentPhotoUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentPhotoUpdate._message = es.Message;
                parentPhotoUpdate._failure = true;
            }
            return parentPhotoUpdate;
        }
    }
}
