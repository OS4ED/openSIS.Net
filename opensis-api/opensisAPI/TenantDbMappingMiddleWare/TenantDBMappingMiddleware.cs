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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using opensis.data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace opensisAPI.TenantDbMappingMiddleWare
{
    public class TenantDBMappingMiddleware
    {
        private readonly RequestDelegate next;

        public TenantDBMappingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string[] urlParts = null;
            
            urlParts = httpContext.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

          //  urlParts = httpContext.Request.Host.Host.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);


            if (urlParts != null && urlParts.Any())
            {
                httpContext.RequestServices.GetService<IDbContextFactory>().TenantName = urlParts[0];
            }

            await this.next(httpContext);
        }
    }

    public static class TenantDBMappingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTenantDBMapper(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<TenantDBMappingMiddleware>();
        }
    }
}
