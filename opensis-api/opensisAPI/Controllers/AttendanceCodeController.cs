using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.AttendanceCode.Interfaces;
using opensis.data.ViewModels.AttendanceCodes;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/AttendanceCode")]
    [ApiController]
    public class AttendanceCodeController : ControllerBase
    {
        private IAttendanceCodeRegisterService _attendanceCodeRegisterService;
        public AttendanceCodeController(IAttendanceCodeRegisterService attendanceCodeRegisterService)
        {
            _attendanceCodeRegisterService = attendanceCodeRegisterService;
        }
        [HttpPost("addAttendanceCode")]
        public ActionResult<AttendanceCodeAddViewModel> AddAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeAdd = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeAdd = _attendanceCodeRegisterService.SaveAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeAdd._failure = true;
                attendanceCodeAdd._message = es.Message;
            }
            return attendanceCodeAdd;
        }
        [HttpPost("viewAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> ViewAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeView = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeView = _attendanceCodeRegisterService.ViewAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeView._failure = true;
                attendanceCodeView._message = es.Message;
            }
            return attendanceCodeView;
        }
        [HttpPut("updateAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> UpdateAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeUpdate = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeUpdate = _attendanceCodeRegisterService.UpdateAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeUpdate._failure = true;
                attendanceCodeUpdate._message = es.Message;
            }
            return attendanceCodeUpdate;
        }
        [HttpPost("getAllAttendanceCode")]

        public ActionResult<AttendanceCodeListViewModel> GetAllAttendanceCode(AttendanceCodeListViewModel attendanceCodeListViewModel)
        {
            AttendanceCodeListViewModel attendanceCodeList = new AttendanceCodeListViewModel();
            try
            {
                if (attendanceCodeListViewModel.SchoolId > 0)
                {
                    attendanceCodeList = _attendanceCodeRegisterService.GetAllAttendanceCode(attendanceCodeListViewModel);
                }
                else
                {
                    attendanceCodeList._token = attendanceCodeListViewModel._token;
                    attendanceCodeList._tenantName = attendanceCodeListViewModel._tenantName;
                    attendanceCodeList._failure = true;
                    attendanceCodeList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                attendanceCodeList._message = es.Message;
                attendanceCodeList._failure = true;
            }
            return attendanceCodeList;
        }
        [HttpPost("deleteAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> DeleteAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodelDelete = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodelDelete = _attendanceCodeRegisterService.DeleteAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodelDelete._failure = true;
                attendanceCodelDelete._message = es.Message;
            }
            return attendanceCodelDelete;
        }

        [HttpPost("addAttendanceCodeCategories")]
        public ActionResult<AttendanceCodeCategoriesAddViewModel> AddAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAdd = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesAdd = _attendanceCodeRegisterService.SaveAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesAdd._failure = true;
                attendanceCodeCategoriesAdd._message = es.Message;
            }
            return attendanceCodeCategoriesAdd;
        }
        [HttpPost("viewAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> ViewAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesView = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesView = _attendanceCodeRegisterService.ViewAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesView._failure = true;
                attendanceCodeCategoriesView._message = es.Message;
            }
            return attendanceCodeCategoriesView;
        }
        [HttpPut("updateAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> UpdateAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesUpdate = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesUpdate = _attendanceCodeRegisterService.UpdateAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesUpdate._failure = true;
                attendanceCodeCategoriesUpdate._message = es.Message;
            }
            return attendanceCodeCategoriesUpdate;
        }
        [HttpPost("getAllAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesListViewModel> GetAllAttendanceCodeCategories(AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesListViewModel)
        {
            AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesList = new AttendanceCodeCategoriesListViewModel();
            try
            {
                if (attendanceCodeCategoriesListViewModel.SchoolId > 0)
                {
                    attendanceCodeCategoriesList = _attendanceCodeRegisterService.GetAllAttendanceCodeCategories(attendanceCodeCategoriesListViewModel);
                }
                else
                {
                    attendanceCodeCategoriesList._token = attendanceCodeCategoriesListViewModel._token;
                    attendanceCodeCategoriesList._tenantName = attendanceCodeCategoriesListViewModel._tenantName;
                    attendanceCodeCategoriesList._failure = true;
                    attendanceCodeCategoriesList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesList._message = es.Message;
                attendanceCodeCategoriesList._failure = true;
            }
            return attendanceCodeCategoriesList;
        }
        [HttpPost("deleteAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> DeleteAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategorieslDelete = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategorieslDelete = _attendanceCodeRegisterService.DeleteAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategorieslDelete._failure = true;
                attendanceCodeCategorieslDelete._message = es.Message;
            }
            return attendanceCodeCategorieslDelete;
        }


        [HttpPost("updateAttendanceCodeSortOrder")]
        public ActionResult<AttendanceCodeSortOrderModel> UpdateAttendanceCodeSortOrder(AttendanceCodeSortOrderModel attendanceCodeSortOrderModel)
        {
            AttendanceCodeSortOrderModel attendanceCodeSortOrderUpdate = new AttendanceCodeSortOrderModel();
            try
            {
                attendanceCodeSortOrderUpdate = _attendanceCodeRegisterService.UpdateAttendanceCodeSortOrder(attendanceCodeSortOrderModel);
            }
            catch (Exception es)
            {
                attendanceCodeSortOrderUpdate._failure = true;
                attendanceCodeSortOrderUpdate._message = es.Message;
            }
            return attendanceCodeSortOrderUpdate;
        }
    }
}
