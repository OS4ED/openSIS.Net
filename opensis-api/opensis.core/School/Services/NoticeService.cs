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
using opensis.core.School.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.Notice;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.School.Services
{
    public class NoticeService : INoticeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";
        public INoticeRepository noticeRepository;
        public NoticeService(INoticeRepository noticeRepository)
        {
            this.noticeRepository = noticeRepository;
        }


        public NoticeAddViewModel SaveNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.AddNotice(notice);
                return noticeAddViewModel;
            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }

        public NoticeAddViewModel UpdateNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.UpdateNotice(notice);
                return noticeAddViewModel;
            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }
        public NoticeDeleteModel DeleteNotice(NoticeDeleteModel notice)
        {
            NoticeDeleteModel noticdeleteModel = new NoticeDeleteModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticdeleteModel = this.noticeRepository.DeleteNotice(notice);
                return noticdeleteModel;
            }
            else
            {
                noticdeleteModel._failure = true;
                noticdeleteModel._message = TOKENINVALID;
                return noticdeleteModel;
            }

        }
        public NoticeAddViewModel ViewNotice(NoticeAddViewModel notice)
        {
            NoticeAddViewModel noticeAddViewModel = new NoticeAddViewModel();
            if (TokenManager.CheckToken(notice._tenantName + notice._userName, notice._token))
            {
                noticeAddViewModel = this.noticeRepository.ViewNotice(notice);
                //return getAllSchools();
                return noticeAddViewModel;

            }
            else
            {
                noticeAddViewModel._failure = true;
                noticeAddViewModel._message = TOKENINVALID;
                return noticeAddViewModel;
            }

        }
        public NoticeListViewModel GetAllNotice(NoticeListViewModel noticeList)
        {
            NoticeListViewModel getAllNoticeList = new NoticeListViewModel();
            if (TokenManager.CheckToken(noticeList._tenantName + noticeList._userName, noticeList._token))
            {
                getAllNoticeList = this.noticeRepository.GetAllNotice(noticeList);
                return getAllNoticeList;
            }
            else
            {
                getAllNoticeList = null;
                getAllNoticeList._failure = true;
                getAllNoticeList._message = TOKENINVALID;
                return getAllNoticeList;
            }
        }
       
    }
}
