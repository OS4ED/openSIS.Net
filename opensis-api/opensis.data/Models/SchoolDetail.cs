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

using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class SchoolDetail
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string Affiliation { get; set; }
        public string Associations { get; set; }
        public string Locale { get; set; }
        public string LowestGradeLevel { get; set; }
        public string HighestGradeLevel { get; set; }
        public DateTime? DateSchoolOpened { get; set; }
        public DateTime? DateSchoolClosed { get; set; }
        public bool? Status { get; set; }
        public string Gender { get; set; }
        public bool? Internet { get; set; }
        public bool? Electricity { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }
        public string NameOfPrincipal { get; set; }
        public string NameOfAssistantPrincipal { get; set; }
        public byte[] SchoolLogo { get; set; }
        public bool? RunningWater { get; set; }
        public string MainSourceOfDrinkingWater { get; set; }
        public bool? CurrentlyAvailable { get; set; }
        public string FemaleToiletType { get; set; }
        public short? TotalFemaleToilets { get; set; }
        public short? TotalFemaleToiletsUsable { get; set; }
        public string FemaleToiletAccessibility { get; set; }
        public string MaleToiletType { get; set; }
        public short? TotalMaleToilets { get; set; }
        public short? TotalMaleToiletsUsable { get; set; }
        public string MaleToiletAccessibility { get; set; }
        public string ComonToiletType { get; set; }
        public short? TotalCommonToilets { get; set; }
        public short? TotalCommonToiletsUsable { get; set; }
        public string CommonToiletAccessibility { get; set; }
        public bool? HandwashingAvailable { get; set; }
        public bool? SoapAndWaterAvailable { get; set; }
        public string HygeneEducation { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
    }
}
