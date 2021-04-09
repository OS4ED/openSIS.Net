using opensis.core.helper;
using opensis.core.StudentEnrollmentCodes.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StudentEnrollmentCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentEnrollmentCodes.Services
{
    public class StudentEnrollmentCodeService: IStudentEnrollmentCodeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentEnrollmentCodeRepository studentEnrollmentCodeRepository;
        public StudentEnrollmentCodeService(IStudentEnrollmentCodeRepository studentEnrollmentCodeRepository)
        {
            this.studentEnrollmentCodeRepository = studentEnrollmentCodeRepository;
        }
        //Required for Unit Testing
        public StudentEnrollmentCodeService() { }

        /// <summary>
        /// Add StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel SaveStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAdd = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeAdd = this.studentEnrollmentCodeRepository.AddStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);

            }
            else
            {
                studentEnrollmentCodeAdd._failure = true;
                studentEnrollmentCodeAdd._message = TOKENINVALID;
            }
            return studentEnrollmentCodeAdd;
        }

        /// <summary>
        /// View By Id StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel ViewStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeView = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeView = this.studentEnrollmentCodeRepository.ViewStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);

            }
            else
            {
                studentEnrollmentCodeView._failure = true;
                studentEnrollmentCodeView._message = TOKENINVALID;
            }
            return studentEnrollmentCodeView;
        }

        /// <summary>
        /// Delete StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel DeleteStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeDel = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeDel = this.studentEnrollmentCodeRepository.DeleteStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);             
            }
            else
            {
                studentEnrollmentCodeDel._failure = true;
                studentEnrollmentCodeDel._message = TOKENINVALID;
            }
            return studentEnrollmentCodeDel;
        }

        /// <summary>
        /// Update StudentEnrollmentCode
        /// </summary>
        /// <param name="studentEnrollmentCodeAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeAddViewModel UpdateStudentEnrollmentCode(StudentEnrollmentCodeAddViewModel studentEnrollmentCodeAddViewModel)
        {
            StudentEnrollmentCodeAddViewModel studentEnrollmentCodeUpdate = new StudentEnrollmentCodeAddViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeAddViewModel._tenantName + studentEnrollmentCodeAddViewModel._userName, studentEnrollmentCodeAddViewModel._token))
            {
                studentEnrollmentCodeUpdate = this.studentEnrollmentCodeRepository.UpdateStudentEnrollmentCode(studentEnrollmentCodeAddViewModel);
            }
            else
            {
                studentEnrollmentCodeUpdate._failure = true;
                studentEnrollmentCodeUpdate._message = TOKENINVALID;
            }
            return studentEnrollmentCodeUpdate;
        }
        /// <summary>
        /// Get All Student Enrollment Code
        /// </summary>
        /// <param name="studentEnrollmentCodeListView"></param>
        /// <returns></returns>
        public StudentEnrollmentCodeListViewModel GetAllStudentEnrollmentCode(StudentEnrollmentCodeListViewModel studentEnrollmentCodeListView)
        {
            StudentEnrollmentCodeListViewModel studentEnrollmentCodeList = new StudentEnrollmentCodeListViewModel();
            if (TokenManager.CheckToken(studentEnrollmentCodeListView._tenantName + studentEnrollmentCodeListView._userName, studentEnrollmentCodeListView._token))
            {
                studentEnrollmentCodeList = this.studentEnrollmentCodeRepository.GetAllStudentEnrollmentCode(studentEnrollmentCodeListView);
                
            }
            else
            {
                studentEnrollmentCodeList._failure = true;
                studentEnrollmentCodeList._message = TOKENINVALID;
               
            }
            return studentEnrollmentCodeList;
        }


    }
}
