using opensis.core.helper;
using opensis.core.Room.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Room.Services
{
    public class RoomRegister : IRoomRegisterService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IRoomRepository roomRepository;
        public RoomRegister(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public RoomRegister() { }   

        /// <summary>
        /// Add Room
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        public RoomAddViewModel SaveRoom(RoomAddViewModel rooms)
        {
            RoomAddViewModel RoomAddViewModel = new RoomAddViewModel();
            if (TokenManager.CheckToken(rooms._tenantName + rooms._userName, rooms._token))
            {

                RoomAddViewModel = this.roomRepository.AddRoom(rooms);                
                return RoomAddViewModel;

            }
            else
            {
                RoomAddViewModel._failure = true;
                RoomAddViewModel._message = TOKENINVALID;
                return RoomAddViewModel;
            }

        }
        /// <summary>
        /// Get Room By Id
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public RoomAddViewModel ViewRoom(RoomAddViewModel room)
        {
            RoomAddViewModel roomAddViewModel = new RoomAddViewModel();
            if (TokenManager.CheckToken(room._tenantName + room._userName, room._token))
            {
                roomAddViewModel = this.roomRepository.ViewRoom(room);
                //return getAllSection();
                return roomAddViewModel;

            }
            else
            {
                roomAddViewModel._failure = true;
                roomAddViewModel._message = TOKENINVALID;
                return roomAddViewModel;
            }

        }
        /// <summary>
        /// Update Room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public RoomAddViewModel UpdateRoom(RoomAddViewModel room)
        {
            RoomAddViewModel RoomAddViewModel = new RoomAddViewModel();
            if (TokenManager.CheckToken(room._tenantName + room._userName, room._token))
            {
                RoomAddViewModel = this.roomRepository.UpdateRoom(room);                
                return RoomAddViewModel;
            }
            else
            {
                RoomAddViewModel._failure = true;
                RoomAddViewModel._message = TOKENINVALID;
                return RoomAddViewModel;
            }

        }
        /// <summary>
        /// Get All Room
        /// </summary>
        /// <param name="roomList"></param>
        /// <returns></returns>
        public RoomListModel GetAllRoom(RoomListModel roomList)
        {
            RoomListModel roomlListModel = new RoomListModel();
            try
            {
                if (TokenManager.CheckToken(roomList._tenantName + roomList._userName, roomList._token))
                {
                    roomlListModel = this.roomRepository.GetAllRooms(roomList);
                }
                else
                {
                    roomlListModel._failure = true;
                    roomlListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                roomlListModel._failure = true;
                roomlListModel._message = es.Message;
            }

            return roomlListModel;
        }
        /// <summary>
        /// Delete Room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public RoomAddViewModel DeleteRoom(RoomAddViewModel room)
        {
            RoomAddViewModel roomListdelete = new RoomAddViewModel();
            try
            {
                if (TokenManager.CheckToken(room._tenantName + room._userName, room._token))
                {
                    roomListdelete = this.roomRepository.DeleteRoom(room);
                }
                else
                {
                    roomListdelete._failure = true;
                    roomListdelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                roomListdelete._failure = true;
                roomListdelete._message = es.Message;
            }

            return roomListdelete;
        }

    }
}
