using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class StudentAttendanceRepository : IStudentAttendanceRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StudentAttendanceRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Student Attendance Add/Update
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>

        public StudentAttendanceAddViewModel AddUpdateStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            try
            {

                List<StudentAttendance> studentAttendance = new List<StudentAttendance>();

                if (studentAttendanceAddViewModel.studentAttendance.Count > 0)
                {
                    var attendanceDataExist = this.context?.StudentAttendance.Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId && x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId).ToList();

                    foreach (var studentAttendancedata in studentAttendanceAddViewModel.studentAttendance.ToList())
                    {
                        if (attendanceDataExist.Count > 0)
                        {

                            

                            var studentAttendanceUpdate = new StudentAttendance()
                            {
                                TenantId = studentAttendanceAddViewModel.TenantId,
                                SchoolId = studentAttendanceAddViewModel.SchoolId,
                                StudentId = studentAttendancedata.StudentId,
                                StaffId = studentAttendanceAddViewModel.StaffId,
                                CourseId = studentAttendanceAddViewModel.CourseId,
                                CourseSectionId = studentAttendanceAddViewModel.CourseSectionId,
                                AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                AttendanceCode = studentAttendancedata.AttendanceCode,
                                AttendanceDate = studentAttendanceAddViewModel.AttendanceDate,
                                Comments = studentAttendancedata.Comments,
                                UpdatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                UpdatedOn = DateTime.UtcNow,
                                BlockId = studentAttendancedata.BlockId,
                                PeriodId = studentAttendanceAddViewModel.PeriodId,
                            };
                            studentAttendance.Add(studentAttendanceUpdate);

                        }
                        else
                        {
                            var studentAttendanceAdd = new StudentAttendance()
                            {
                                TenantId = studentAttendanceAddViewModel.TenantId,
                                SchoolId = studentAttendanceAddViewModel.SchoolId,
                                StudentId = studentAttendancedata.StudentId,
                                StaffId = studentAttendanceAddViewModel.StaffId,
                                CourseId = studentAttendanceAddViewModel.CourseId,
                                CourseSectionId = studentAttendanceAddViewModel.CourseSectionId,
                                AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                AttendanceCode = studentAttendancedata.AttendanceCode,
                                AttendanceDate = studentAttendanceAddViewModel.AttendanceDate,
                                Comments = studentAttendancedata.Comments,
                                CreatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                CreatedOn = DateTime.UtcNow,
                                BlockId = studentAttendancedata.BlockId,
                                PeriodId = studentAttendanceAddViewModel.PeriodId,
                            };
                            studentAttendance.Add(studentAttendanceAdd);
                        }
                    }
                    if(attendanceDataExist.Count>0)
                    {
                        this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                        studentAttendanceAddViewModel._message = "Student Attendance updated succsesfully.";
                    }
                    else
                    {
                        studentAttendanceAddViewModel._message = "Student Attendance added succsesfully.";
                    }
                    this.context?.StudentAttendance.AddRange(studentAttendance);
                    this.context?.SaveChanges();
                    studentAttendanceAddViewModel._failure = false;
                    
                }


            }
            catch (Exception es)
            {
                studentAttendanceAddViewModel._failure = true;
                studentAttendanceAddViewModel._message = es.Message;
            }
            return studentAttendanceAddViewModel;
        }
    }
}
