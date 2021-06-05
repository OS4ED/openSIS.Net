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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
using opensis.core.Room.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.Room;
using opensis.data.ViewModels.School;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomRegisterService _roomRegisterService;
        public RoomController(IRoomRegisterService roomRegisterService)
        {
            _roomRegisterService = roomRegisterService;
        }

        [HttpPost("addRoom")]
        public ActionResult<RoomAddViewModel> AddRoom(RoomAddViewModel room)
        {
            RoomAddViewModel roomAdd = new RoomAddViewModel();
            try
            {
                if (room.tableRoom.SchoolId > 0)
                {
                    roomAdd = _roomRegisterService.SaveRoom(room);
                }
                else
                {
                    roomAdd._token = room._token;
                    roomAdd._tenantName = room._tenantName;
                    roomAdd._failure = true;
                    roomAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                roomAdd._failure = true;
                roomAdd._message = es.Message;
            }
            return roomAdd;
        }
        [HttpPost("viewRoom")]

        public ActionResult<RoomAddViewModel> ViewRoom(RoomAddViewModel room)
        {
            RoomAddViewModel roomAdd = new RoomAddViewModel();
            try
            {
                if (room.tableRoom.SchoolId > 0)
                {
                    roomAdd = _roomRegisterService.ViewRoom(room);
                }
                else
                {
                    roomAdd._token = room._token;
                    roomAdd._tenantName = room._tenantName;
                    roomAdd._failure = true;
                    roomAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                roomAdd._failure = true;
                roomAdd._message = es.Message;
            }
            return roomAdd;
        }
        [HttpPut("updateRoom")]

        public ActionResult<RoomAddViewModel> UpdateRoom(RoomAddViewModel room)
        {
            RoomAddViewModel RoomAdd = new RoomAddViewModel();
            try
            {
                if (room.tableRoom.SchoolId > 0)
                {
                    RoomAdd = _roomRegisterService.UpdateRoom(room);
                }
                else
                {
                    RoomAdd._token = room._token;
                    RoomAdd._tenantName = room._tenantName;
                    RoomAdd._failure = true;
                    RoomAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                RoomAdd._failure = true;
                RoomAdd._message = es.Message;
            }
            return RoomAdd;
        }
        [HttpPost("getAllRoom")]

        public ActionResult<RoomListModel> GetAllRoom(RoomListModel room)
        {
            RoomListModel roomList = new RoomListModel();
            try
            {
                if (room.SchoolId > 0)
                {
                    roomList = _roomRegisterService.GetAllRoom(room);
                }
                else
                {
                    roomList._token = room._token;
                    roomList._tenantName = room._tenantName;
                    roomList._failure = true;
                    roomList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                roomList._message = es.Message;
                roomList._failure = true;
            }
            return roomList;
        }
        [HttpPost("deleteRoom")]

        public ActionResult<RoomAddViewModel> DeleteRoom(RoomAddViewModel room)
        {
            RoomAddViewModel roomlDelete = new RoomAddViewModel();
            try
            {
                if (room.tableRoom.SchoolId > 0)
                {
                    roomlDelete = _roomRegisterService.DeleteRoom(room);
                }
                else
                {
                    roomlDelete._token = room._token;
                    roomlDelete._tenantName = room._tenantName;
                    roomlDelete._failure = true;
                    roomlDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                roomlDelete._failure = true;
                roomlDelete._message = es.Message;
            }
            return roomlDelete;
        }
    }
}