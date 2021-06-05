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
using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.School.Services
{
    public class MembershipService : IMembershipService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";
        public IMembershipRepository membershipRepository;
        public MembershipService(IMembershipRepository membershipRepository)
        {
            this.membershipRepository = membershipRepository;
        }
        public GetAllMembersList GetAllMembersForNotice(GetAllMembersList allMembersList)
        {
            GetAllMembersList getAllMembers = new GetAllMembersList();

            if (TokenManager.CheckToken(allMembersList._tenantName + allMembersList._userName, allMembersList._token))
            {
                getAllMembers = this.membershipRepository.GetAllMemberList(allMembersList);
                return getAllMembers;
            }
            else
            {
                getAllMembers._failure = true;
                getAllMembers._message = TOKENINVALID;
                return getAllMembers;
            }
        }

        /// <summary>
        /// Add Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel AddMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipAddModel = new MembershipAddViewModel();
            try
            {
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipAddModel = this.membershipRepository.AddMembership(membershipAddViewModel);
                }
                else
                {
                    MembershipAddModel._failure = true;
                    MembershipAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                MembershipAddModel._failure = true;
                MembershipAddModel._message = es.Message;
            }  
            return MembershipAddModel;
        }

        /// <summary>
        /// Update Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel UpdateMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipUpdateModel = new MembershipAddViewModel();
            try
            { 
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipUpdateModel = this.membershipRepository.UpdateMembership(membershipAddViewModel);                    
                }
                else
                {
                    MembershipUpdateModel._failure = true;
                    MembershipUpdateModel._message = TOKENINVALID;                    
                }
            }
            catch (Exception es)
            {
                MembershipUpdateModel._failure = true;
                MembershipUpdateModel._message = es.Message;
            }
            return MembershipUpdateModel;
        }

        /// <summary>
        /// Delete Membership
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel DeleteMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipDeleteModel = new MembershipAddViewModel();
            try
            {
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipDeleteModel = this.membershipRepository.DeleteMembership(membershipAddViewModel);
                }
                else
                {
                    MembershipDeleteModel._failure = true;
                    MembershipDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                MembershipDeleteModel._failure = true;
                MembershipDeleteModel._message = es.Message;
            }
            return MembershipDeleteModel;
        }
    }
}
