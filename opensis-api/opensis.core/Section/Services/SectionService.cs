﻿/***********************************************************************************
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

using opensis.core.Section.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.Section;
using System;
using System.Collections.Generic;
using System.Text;
using opensis.data.Models;

namespace opensis.core.Section.Services
{
    public class SectionService: ISectionService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ISectionRepositiory sectionRepository;
        public SectionService(ISectionRepositiory sectionRepository)
        {
            this.sectionRepository = sectionRepository;
        }
        //Required for Unit Testing
        public SectionService() { }

        /// <summary>
        /// Service layer For Adding Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionAddViewModel SaveSection(SectionAddViewModel section)
        {
            SectionAddViewModel sectionAddViewModel = new SectionAddViewModel();
            if (TokenManager.CheckToken(section._tenantName + section._userName, section._token))
            {

                sectionAddViewModel = this.sectionRepository.AddSection(section);
               
                return sectionAddViewModel;

            }
            else
            {
                sectionAddViewModel._failure = true;
                sectionAddViewModel._message = TOKENINVALID;
                return sectionAddViewModel;
            }
        }
        /// <summary>
        /// Service layer For Editing/Updating Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>

        public SectionAddViewModel UpdateSection(SectionAddViewModel section)
        {
            SectionAddViewModel sectionUpdate = new SectionAddViewModel();
            if (TokenManager.CheckToken(section._tenantName + section._userName, section._token))
            {
                sectionUpdate = this.sectionRepository.UpdateSection(section);
              
                return sectionUpdate;
            }
            else
            {
                sectionUpdate._failure = true;
                sectionUpdate._message = TOKENINVALID;
                return sectionUpdate;
            }

        }

        /// <summary>
        /// Service layer For View Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionAddViewModel ViewSection(SectionAddViewModel section)
        {
            SectionAddViewModel sectionView = new SectionAddViewModel();
            if (TokenManager.CheckToken(section._tenantName + section._userName, section._token))
            {
                sectionView = this.sectionRepository.ViewSection(section);

                return sectionView;

            }
            else
            {
                sectionView._failure = true;
                sectionView._message = TOKENINVALID;
                return sectionView;
            }

        }
        /// <summary>
        /// Service layer For GetAll Section
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public SectionListViewModel GetAllsection(SectionListViewModel section)
        {
            SectionListViewModel sectionList = new SectionListViewModel();
            try
            {
                if (TokenManager.CheckToken(section._tenantName + section._userName, section._token))
                {
                    sectionList = this.sectionRepository.GetAllsection(section);
                }
                else
                {
                    sectionList._failure = true;
                    sectionList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                sectionList._failure = true;
                sectionList._message = es.Message;
            }

            return sectionList;
        }

        /// <summary>
        /// Service Layer For Deleting Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionAddViewModel DeleteSection(SectionAddViewModel section)
        {
            SectionAddViewModel sectionDelete = new SectionAddViewModel();
            try
            {
                if (TokenManager.CheckToken(section._tenantName + section._userName, section._token))
                {
                    sectionDelete = this.sectionRepository.DeleteSection(section);
                }
                else
                {
                    sectionDelete._failure = true;
                    sectionDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                sectionDelete._failure = true;
                sectionDelete._message = es.Message;
            }

            return sectionDelete;
        }


    }
}
