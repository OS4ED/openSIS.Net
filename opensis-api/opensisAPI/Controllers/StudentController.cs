using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.Student.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.Student;

namespace opensisAPI.Controllers
{

    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("addStudent")]
        public ActionResult<StudentAddViewModel> AddStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentAdd = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentAdd = _studentService.SaveStudent(student);
                }
                else
                {
                    studentAdd._token = student._token;
                    studentAdd._tenantName = student._tenantName;
                    studentAdd._failure = true;
                    studentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentAdd._failure = true;
                studentAdd._message = es.Message;
            }
            return studentAdd;
        }

        [HttpPut("updateStudent")]
        public ActionResult<StudentAddViewModel> UpdateStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentUpdate = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentUpdate = _studentService.UpdateStudent(student);
                }
                else
                {
                    studentUpdate._token = student._token;
                    studentUpdate._tenantName = student._tenantName;
                    studentUpdate._failure = true;
                    studentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentUpdate._failure = true;
                studentUpdate._message = es.Message;
            }
            return studentUpdate;
        }

        [HttpPost("getAllStudentList")]
        public ActionResult<StudentListModel> GetAllStudentList(PageResult pageResult)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.GetAllStudentList(pageResult);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("addStudentDocument")]
        public ActionResult<StudentDocumentAddViewModel> AddStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocoumentAdd = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocoumentAdd = _studentService.SaveStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocoumentAdd._token = studentDocumentAddViewModel._token;
                    studentDocoumentAdd._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocoumentAdd._failure = true;
                    studentDocoumentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentDocoumentAdd._failure = true;
                studentDocoumentAdd._message = es.Message;
            }
            return studentDocoumentAdd;
        }

        [HttpPut("updateStudentDocument")]

        public ActionResult<StudentDocumentAddViewModel> UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocumentUpdate = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocumentUpdate = _studentService.UpdateStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocumentUpdate._token = studentDocumentAddViewModel._token;
                    studentDocumentUpdate._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocumentUpdate._failure = true;
                    studentDocumentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentDocumentUpdate._failure = true;
                studentDocumentUpdate._message = es.Message;
            }
            return studentDocumentUpdate;
        }


        [HttpPost("getAllStudentDocumentsList")]

        public ActionResult<StudentDocumentListViewModel> GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentsListViewModel)
        {
            StudentDocumentListViewModel studentDocumentsList = new StudentDocumentListViewModel();
            try
            {
                studentDocumentsList = _studentService.GetAllStudentDocumentsList(studentDocumentsListViewModel);
            }
            catch (Exception es)
            {
                studentDocumentsList._message = es.Message;
                studentDocumentsList._failure = true;
            }
            return studentDocumentsList;
        }


        [HttpPost("deleteStudentDocument")]
        public ActionResult<StudentDocumentAddViewModel> DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocoumentdelete = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocoumentdelete = _studentService.DeleteStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocoumentdelete._token = studentDocumentAddViewModel._token;
                    studentDocoumentdelete._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocoumentdelete._failure = true;
                    studentDocoumentdelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentDocoumentdelete._failure = true;
                studentDocoumentdelete._message = es.Message;
            }
            return studentDocoumentdelete;
        }


        [HttpPost("addStudentLoginInfo")]
        public ActionResult<LoginInfoAddModel> AddStudentLoginInfo(LoginInfoAddModel login)
        {
            LoginInfoAddModel loginInfo = new LoginInfoAddModel();
            try
            {
                if (login.userMaster.SchoolId > 0)
                {
                    loginInfo = _studentService.AddStudentLoginInfo(login);
                }
                else
                {
                    loginInfo._token = login._token;
                    loginInfo._tenantName = login._tenantName;
                    loginInfo._failure = true;
                    loginInfo._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                loginInfo._failure = true;
                loginInfo._message = es.Message;
            }
            return loginInfo;
        }


        [HttpPost("addStudentComment")]
        public ActionResult<StudentCommentAddViewModel> AddStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentAdd = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentAdd = _studentService.SaveStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentAdd._token = studentCommentAddViewModel._token;
                    studentCommentAdd._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentAdd._failure = true;
                    studentCommentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentCommentAdd._failure = true;
                studentCommentAdd._message = es.Message;
            }
            return studentCommentAdd;
        }

        [HttpPut("updateStudentComment")]

        public ActionResult<StudentCommentAddViewModel> UpdateStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentUpdate = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentUpdate = _studentService.UpdateStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentUpdate._token = studentCommentAddViewModel._token;
                    studentCommentUpdate._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentUpdate._failure = true;
                    studentCommentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentCommentUpdate._failure = true;
                studentCommentUpdate._message = es.Message;
            }
            return studentCommentUpdate;
        }


        [HttpPost("getAllStudentCommentsList")]

        public ActionResult<StudentCommentListViewModel> GetAllStudentCommentsList(StudentCommentListViewModel studentCommentListViewModel)
        {
            StudentCommentListViewModel studentCommentsList = new StudentCommentListViewModel();
            try
            {
                studentCommentsList = _studentService.GetAllStudentCommentsList(studentCommentListViewModel);
            }
            catch (Exception es)
            {
                studentCommentsList._message = es.Message;
                studentCommentsList._failure = true;
            }
            return studentCommentsList;
        }


        [HttpPost("deleteStudentComment")]
        public ActionResult<StudentCommentAddViewModel> DeleteStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentDelete = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentDelete = _studentService.DeleteStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentDelete._token = studentCommentAddViewModel._token;
                    studentCommentDelete._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentDelete._failure = true;
                    studentCommentDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentCommentDelete._failure = true;
                studentCommentDelete._message = es.Message;
            }
            return studentCommentDelete;
        }

        //[HttpPost("viewStudent")]
        [HttpPost("viewStudent")]

        public ActionResult<StudentAddViewModel> ViewStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentView = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentView = _studentService.ViewStudent(student);
                }
                else
                {
                    studentView._token = student._token;
                    studentView._tenantName = student._tenantName;
                    studentView._failure = true;
                    studentView._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentView._failure = true;
                studentView._message = es.Message;
            }
            return studentView;
        }



        //[HttpPost("deleteStudent")]

        //public ActionResult<StudentAddViewModel> DeleteStudent(StudentAddViewModel student)
        //{
        //    StudentAddViewModel studentDelete = new StudentAddViewModel();
        //    try
        //    {
        //        if (student.studentMaster.SchoolId > 0)
        //        {
        //            studentDelete = _studentService.DeleteStudent(student);
        //        }
        //        else
        //        {
        //            studentDelete._token = student._token;
        //            studentDelete._tenantName = student._tenantName;
        //            studentDelete._failure = true;
        //            studentDelete._message = "Please enter valid scholl id";
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        studentDelete._failure = true;
        //        studentDelete._message = es.Message;
        //    }
        //    return studentDelete;
        //}


        [HttpPost("siblingSearch")]

        public ActionResult<SiblingSearchForStudentListModel> ViewStudentSiblingList(SiblingSearchForStudentListModel studentSiblingListViewModel)
        {
            SiblingSearchForStudentListModel studentSiblingsList = new SiblingSearchForStudentListModel();
            try
            {
                studentSiblingsList = _studentService.SearchSiblingForStudent(studentSiblingListViewModel);
            }
            catch (Exception es)
            {
                studentSiblingsList._message = es.Message;
                studentSiblingsList._failure = true;
            }
            return studentSiblingsList;
        }

        [HttpPost("associationSibling")]
        public ActionResult<SiblingAddUpdateForStudentModel> AssociationSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingAddUpdateForStudent = new SiblingAddUpdateForStudentModel();
            try
            {
                siblingAddUpdateForStudent = _studentService.AssociationSibling(siblingAddUpdateForStudentModel);
            }
            catch (Exception es)
            {
                siblingAddUpdateForStudent._failure = true;
                siblingAddUpdateForStudent._message = es.Message;
            }
            return siblingAddUpdateForStudent;
        }

        [HttpPost("viewSibling")]

        public ActionResult<StudentListModel> ViewSibling(StudentListModel studentListModel)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.ViewAllSibling(studentListModel);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("removeSibling")]
        public ActionResult<SiblingAddUpdateForStudentModel> RemoveSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingRemove = new SiblingAddUpdateForStudentModel();
            try
            {
                siblingRemove = _studentService.RemoveSibling(siblingAddUpdateForStudentModel);
            }
            catch (Exception es)
            {
                siblingRemove._message = es.Message;
                siblingRemove._failure = true;
            }
            return siblingRemove;
        }

        [HttpPost("checkStudentInternalId")]
        public ActionResult<CheckStudentInternalIdViewModel> CheckStudentInternalId(CheckStudentInternalIdViewModel checkStudentInternalIdViewModel)
        {
            CheckStudentInternalIdViewModel checkInternalId = new CheckStudentInternalIdViewModel();
            try
            {
                checkInternalId = _studentService.CheckStudentInternalId(checkStudentInternalIdViewModel);
            }
            catch (Exception es)
            {
                checkInternalId._message = es.Message;
                checkInternalId._failure = true;
            }
            return checkInternalId;
        }

        [HttpPost("addStudentEnrollment")]
        public ActionResult<StudentEnrollmentListModel> AddStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentAdd = new StudentEnrollmentListModel();
            try
            {

                if (studentEnrollmentListModel.studentEnrollments.Count > 0)
                {
                    studentEnrollmentAdd = _studentService.AddStudentEnrollment(studentEnrollmentListModel);
                }
                else
                {
                    studentEnrollmentAdd._token = studentEnrollmentListModel._token;
                    studentEnrollmentAdd._tenantName = studentEnrollmentListModel._tenantName;
                    studentEnrollmentAdd._failure = true;                    
                }
            }
            catch (Exception es)
            {

                studentEnrollmentAdd._failure = true;
                studentEnrollmentAdd._message = es.Message;
            }
            return studentEnrollmentAdd;
        }
        [HttpPost("getAllStudentEnrollment")]

        public ActionResult<StudentEnrollmentListViewModel> GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListModel)
        {
            StudentEnrollmentListViewModel studentEnrollmentList = new StudentEnrollmentListViewModel();
            try
            {
                    studentEnrollmentList = _studentService.GetAllStudentEnrollment(studentEnrollmentListModel);
            }
            catch (Exception es)
            {
                studentEnrollmentList._message = es.Message;
                studentEnrollmentList._failure = true;
            }
            return studentEnrollmentList;
        }

        [HttpPut("updateStudentEnrollment")]

        public ActionResult<StudentEnrollmentListModel> UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentUpdate = new StudentEnrollmentListModel();
            try
            {
                studentEnrollmentUpdate = _studentService.UpdateStudentEnrollment(studentEnrollmentListModel);            
            }
            catch (Exception es)
            {
                studentEnrollmentUpdate._failure = true;
                studentEnrollmentUpdate._message = es.Message;
            }
            return studentEnrollmentUpdate;
        }

        [HttpPut("addUpdateStudentPhoto")]
        public ActionResult<StudentAddViewModel> AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel)
        {
            StudentAddViewModel studentPhotoUpdate = new StudentAddViewModel();
            try
            {
                studentPhotoUpdate = _studentService.AddUpdateStudentPhoto(studentAddViewModel);
            }
            catch (Exception es)
            {
                studentPhotoUpdate._failure = true;
                studentPhotoUpdate._message = es.Message;
            }
            return studentPhotoUpdate;
        }

        //[HttpPost("searchStudentForSchedule")]

        //public ActionResult<SearchStudentViewModel> SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel)
        //{
        //    SearchStudentViewModel searchStudentView = new SearchStudentViewModel();
        //    try
        //    {
        //        searchStudentView = _studentService.SearchStudentForSchedule(searchStudentViewModel);
        //    }
        //    catch (Exception es)
        //    {
        //        searchStudentView._message = es.Message;
        //        searchStudentView._failure = true;
        //    }
        //    return searchStudentView;
        //}

        [HttpPost("searchStudentListForReenroll")]
        public ActionResult<StudentListModel> SearchStudentListForReenroll(PageResult pageResult)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.SearchStudentListForReenroll(pageResult);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("reenrollmentForStudent")]
        public ActionResult<StudentListModel> ReenrollmentForStudent(StudentListModel studentListModel)
        {
            StudentListModel studentReenrollment = new StudentListModel();
            try
            {
                studentReenrollment = _studentService.ReenrollmentForStudent(studentListModel);
            }
            catch (Exception es)
            {
                studentReenrollment._message = es.Message;
                studentReenrollment._failure = true;
            }
            return studentReenrollment;
        }

        [HttpPost("addStudentList")]
        public ActionResult<StudentListAddViewModel> AddStudentList(StudentListAddViewModel studentListAddViewModel)
        {
            StudentListAddViewModel studentListAdd = new StudentListAddViewModel();
            try
            {
                if (studentListAddViewModel.studentAddViewModelList.FirstOrDefault().studentMaster.SchoolId > 0)
                {
                    studentListAdd = _studentService.AddStudentList(studentListAddViewModel);
                }
                else
                {
                    studentListAdd._token = studentListAddViewModel._token;
                    studentListAdd._tenantName = studentListAddViewModel._tenantName;
                    studentListAdd._failure = true;
                    studentListAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentListAdd._failure = true;
                studentListAdd._message = es.Message;
            }
            return studentListAdd;
        }
    }
}
