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
using System.Text;
using NUnit.Framework;
using opensis.core.School.Interfaces;
using opensis.core.School.Services;
using opensis.data.Models;

namespace opensis.NunitTest.School.Services
{
    [TestFixture]
    public class Test_SchoolRegister
    {
       [Test]
        public void Test_IsMandatoryFieldsArePresent_Valid()
        {
            SchoolRegister srg = new SchoolRegister();
            //Schools schools = new Schools();
            //schools.tenant_id = "TenantA";
            //schools.school_name = "Test School";
            //Assert.AreEqual(true, srg.IsMandatoryFieldsArePresent(schools));
        }

        [TestCase("TenantA","TestSchool", true)]
        [TestCase("", "TestSchool", false)]
        [TestCase("TenantA", "", false)]

        public void Test_IsMandatoryFieldsArePresent_Multiple(string tenant, string schoolname, bool expectedresult)
        {
            SchoolRegister srg = new SchoolRegister();
            //Schools schools = new Schools();
            //schools.tenant_id =tenant;
            //schools.school_name = schoolname;
            //Assert.AreEqual(expectedresult, srg.IsMandatoryFieldsArePresent(schools));
        }
        [Test]
        public void Test_GetAllSchoolList()
        {
            SchoolRegister srg = new SchoolRegister();
            PageResult pr = new PageResult();
            pr.PageSize = 10;
            pr.PageNumber = 0;
            pr.TenantId =Guid.Parse("1e93c7bf-0fae-42bb-9e09-a1cedc8c0355");
            pr._tenantName = "opensisv2";
            pr._token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im9wZW5zaXN2MiIsIm5iZiI6MTYwMTgxNDIwOCwiZXhwIjoxNjAxODE2MDA4LCJpYXQiOjE2MDE4MTQyMDh9.ZHsm4jMC6S2yQvC9JejYhJCOOCQvwkxfed-mQMH9GAI";
            var data = srg.GetAllSchoolList(pr);
            Assert.AreEqual(true, data._failure);
           // Assert.AreEqual(true, srg.GetAllSchoolList(pr));
        }

    }
}
