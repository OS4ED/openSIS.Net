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

using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.SchoolPeriod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class SchoolPeriodRepository: ISchoolPeriodRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public SchoolPeriodRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Adding School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel AddSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {

            //int? MasterSchoolPeriodId = Utility.GetMaxPK(this.context, new Func<SchoolPeriods, int>(x => x.PeriodId));
            //int? MasterSchoolPeriodId = 1;

            //var schoolPeriodData = this.context?.SchoolPeriods.Where(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId).OrderByDescending(x => x.PeriodId).FirstOrDefault();

            //if (schoolPeriodData != null)
            //{
            //    MasterSchoolPeriodId = schoolPeriodData.PeriodId + 1;
            //}

            //schoolPeriod.tableSchoolPeriods.PeriodId = (int)MasterSchoolPeriodId;           
            //schoolPeriod.tableSchoolPeriods.LastUpdated = DateTime.UtcNow;
            //this.context?.SchoolPeriods.Add(schoolPeriod.tableSchoolPeriods);
            //this.context?.SaveChanges();
            //schoolPeriod._failure = false;
            return schoolPeriod;
        }

        /// <summary>
        /// Update the School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel UpdateSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            return null;
            //try
            //{
            //    var schoolPeriodUpdate = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);

            //    schoolPeriod.tableSchoolPeriods.LastUpdated = DateTime.UtcNow;
            //    this.context.Entry(schoolPeriodUpdate).CurrentValues.SetValues(schoolPeriod.tableSchoolPeriods);
            //    this.context?.SaveChanges();
            //    schoolPeriod._failure = false;
            //    return schoolPeriod;
            //}
            //catch (Exception ex)
            //{
            //    schoolPeriod.tableSchoolPeriods = null;
            //    schoolPeriod._failure = true;
            //    schoolPeriod._message = NORECORDFOUND;
            //    return schoolPeriod;
            //}
        }

        /// <summary>
        /// View School Period By Id
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel ViewSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            return null;
            //try
            //{
            //    SchoolPeriodAddViewModel schoolPeriodView = new SchoolPeriodAddViewModel();
            //    var schoolPeriodById = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);
            //    if (schoolPeriodById != null)
            //    {
            //        schoolPeriodView.tableSchoolPeriods = schoolPeriodById;
            //        return schoolPeriodView;
            //    }
            //    else
            //    {
            //        schoolPeriodView._failure = true;
            //        schoolPeriodView._message = NORECORDFOUND;
            //        return schoolPeriodView;
            //    }
            //}
            //catch (Exception es)
            //{

            //    throw;
            //}
        }
        /// <summary>
        /// Delete School Period
        /// </summary>
        /// <param name="schoolPeriod"></param>
        /// <returns></returns>
        public SchoolPeriodAddViewModel DeleteSchoolPeriod(SchoolPeriodAddViewModel schoolPeriod)
        {
            //try
            //{
            //    var schoolPeriodDelete = this.context?.SchoolPeriods.FirstOrDefault(x => x.TenantId == schoolPeriod.tableSchoolPeriods.TenantId && x.SchoolId == schoolPeriod.tableSchoolPeriods.SchoolId && x.PeriodId == schoolPeriod.tableSchoolPeriods.PeriodId);

            //    this.context?.SchoolPeriods.Remove(schoolPeriodDelete);
            //    this.context?.SaveChanges();
            //    schoolPeriod._failure = false;
            //    schoolPeriod._message = "Deleted";
            //}

            //catch (Exception es)
            //{
            //    schoolPeriod._failure = true;
            //    schoolPeriod._message = es.Message;
            //}
            return schoolPeriod;
        }

    }
}
