using opensis.core.helper;
using opensis.core.StudentEffortGrade.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentEffortGrade.Services
{
    public class StudentEffortGradeService : IStudentEffortGradeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentEffortGradeRepository studentEffortGradeRepository;
        public StudentEffortGradeService(IStudentEffortGradeRepository studentEffortGradeRepository)
        {
            this.studentEffortGradeRepository = studentEffortGradeRepository;
        }

        /// <summary>
        /// Add Update Student Effort Grade
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentEffortGradeListModel._tenantName + studentEffortGradeListModel._userName, studentEffortGradeListModel._token))
                {
                    studentEffortGradeList = this.studentEffortGradeRepository.AddUpdateStudentEffortGrade(studentEffortGradeListModel);
                }
                else
                {
                    studentEffortGradeList._failure = true;
                    studentEffortGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentEffortGradeList._failure = true;
                studentEffortGradeList._message = es.Message;
            }
            return studentEffortGradeList;
        }

        /// <summary>
        /// Get All Student Effort Grade List
        /// </summary>
        /// <param name="studentEffortGradeListModel"></param>
        /// <returns></returns>
        public StudentEffortGradeListModel GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentEffortGradeListModel._tenantName + studentEffortGradeListModel._userName, studentEffortGradeListModel._token))
                {
                    studentEffortGradeList = this.studentEffortGradeRepository.GetAllStudentEffortGradeList(studentEffortGradeListModel);
                }
                else
                {
                    studentEffortGradeList._failure = true;
                    studentEffortGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentEffortGradeList._failure = true;
                studentEffortGradeList._message = es.Message;
            }
            return studentEffortGradeList;
        }
    }
}
