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
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class SectionRepository : ISectionRepositiory
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public SectionRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }
        /// <summary>
        /// For Adding Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionAddViewModel AddSection(SectionAddViewModel section)
        {
            try
            {
                var checkSectionName = this.context?.Sections.Where(x => x.SchoolId == section.tableSections.SchoolId && x.TenantId == section.tableSections.TenantId && x.Name.ToLower() == section.tableSections.Name.ToLower()).FirstOrDefault();

                if (checkSectionName != null)
                {
                    section._failure = true;
                    section._message = "Section Name Already Exists";
                }
                else
                {
                    //int? MasterSectionId = Utility.GetMaxPK(this.context, new Func<Sections, int>(x => x.SectionId));
                    int? MasterSectionId = 1;

                    var SectionData = this.context?.Sections.Where(x => x.SchoolId == section.tableSections.SchoolId && x.TenantId == section.tableSections.TenantId).OrderByDescending(x => x.SectionId).FirstOrDefault();

                    if (SectionData != null)
                    {
                        MasterSectionId = SectionData.SectionId + 1;
                    }

                    section.tableSections.SectionId = (int)MasterSectionId;
                    section.tableSections.LastUpdated = DateTime.UtcNow;
                    this.context?.Sections.Add(section.tableSections);
                    this.context?.SaveChanges();
                    section._failure = false;
                    section._message = "Section Added Successfully";
                }                
            }
            catch(Exception es)
            {
                section._failure = true;
                section._message = es.Message;
            }            
            return section;
        }

       /// <summary>
       /// For Updating Section
       /// </summary>
       /// <param name="section"></param>
       /// <returns></returns>

        public SectionAddViewModel UpdateSection(SectionAddViewModel section)
        {
            try
            {
                var sectionUpdate = this.context?.Sections.FirstOrDefault(x => x.TenantId == section.tableSections.TenantId && x.SchoolId == section.tableSections.SchoolId && x.SectionId == section.tableSections.SectionId);
                if (sectionUpdate!=null)
                {
                    var checkSectionName = this.context?.Sections.Where(x => x.SchoolId == section.tableSections.SchoolId && x.TenantId == section.tableSections.TenantId && x.SectionId!=section.tableSections.SectionId && x.Name.ToLower() == section.tableSections.Name.ToLower()).FirstOrDefault();

                    if (checkSectionName != null)
                    {
                        section._failure = true;
                        section._message = "Section Name Already Exists";
                    }
                    else
                    {
                        section.tableSections.LastUpdated = DateTime.UtcNow;
                        this.context.Entry(sectionUpdate).CurrentValues.SetValues(section.tableSections);
                        this.context?.SaveChanges();
                        section._failure = false;
                        section._message = "Section Updated Successfully";
                    }                    
                }
                else
                {
                    section.tableSections = null;
                    section._failure = true;
                    section._message = NORECORDFOUND;
                }                
            }
            catch (Exception ex)
            {
                section._failure = true;
                section._message = ex.Message;
            }
            return section;
        }

        /// <summary>
        /// For Section View By ID
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>

        public SectionAddViewModel ViewSection(SectionAddViewModel section)        
        {
            try
            {
                SectionAddViewModel sectionView = new SectionAddViewModel();
                var sectionById = this.context?.Sections.FirstOrDefault(x => x.TenantId == section.tableSections.TenantId && x.SchoolId == section.tableSections.SchoolId && x.SectionId== section.tableSections.SectionId);
                if (sectionById != null)
                {
                    sectionView.tableSections = sectionById;
                    return sectionView;
                }
                else
                {
                    sectionView._failure = true;
                    sectionView._message = NORECORDFOUND;
                    return sectionView;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }

        /// <summary>
        /// For GetAllSection
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>

        public SectionListViewModel GetAllsection(SectionListViewModel section)
        {
            SectionListViewModel sectionList = new SectionListViewModel();
            try
            {
                var sectionAll = this.context?.Sections.Where(x => x.TenantId == section.TenantId && x.SchoolId == section.SchoolId).OrderBy(x => x.SortOrder).ToList();

                sectionList.tableSectionsList = sectionAll;
                sectionList._tenantName = section._tenantName;
                sectionList._token = section._token;

                if (sectionAll.Count > 0)
                {
                    sectionList._failure = false;
                }
                else
                {
                    sectionList._failure = true;
                    sectionList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                sectionList._message = es.Message;
                sectionList._failure = true;
                sectionList._tenantName = section._tenantName;
                sectionList._token = section._token;
            }
            return sectionList;

        }
        /// <summary>
        /// For Deleting Section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>

        public SectionAddViewModel DeleteSection(SectionAddViewModel section)
        {
            try
            {
                var sectionDel = this.context?.Sections.FirstOrDefault(x => x.TenantId == section.tableSections.TenantId && x.SchoolId == section.tableSections.SchoolId && x.SectionId == section.tableSections.SectionId);
                this.context?.Sections.Remove(sectionDel);
                this.context?.SaveChanges();
                section._failure = false;
                section._message = "Section Deleted Successfully";
            }
            catch (Exception es)
            {
                section._failure = true;
                section._message = es.Message;
            }
            return section;
        }

    }
}
