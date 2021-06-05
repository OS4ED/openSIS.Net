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

using JSReport;
using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opensis.data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StudentRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentAddViewModel AddStudent(StudentAddViewModel student)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    //int? MasterStudentId = Utility.GetMaxPK(this.context, new Func<StudentMaster, int>(x => x.StudentId));
                    int? MasterStudentId = 1;

                    var studentData = this.context?.StudentMaster.Where(x => x.SchoolId == student.studentMaster.SchoolId && x.TenantId == student.studentMaster.TenantId).OrderByDescending(x => x.StudentId).FirstOrDefault();

                    if (studentData != null)
                    {
                        MasterStudentId = studentData.StudentId + 1;
                    }

                    student.studentMaster.StudentId = (int)MasterStudentId;
                    Guid GuidId = Guid.NewGuid();
                    var GuidIdExist = this.context?.StudentMaster.FirstOrDefault(x => x.StudentGuid == GuidId);
                    if (GuidIdExist != null)
                    {
                        student._failure = true;
                        student._message = "Guid is already exist, Please try again.";
                        return student;
                    }
                    student.studentMaster.StudentGuid = GuidId;
                    student.studentMaster.IsActive = true;
                    student.studentMaster.EnrollmentType = "Internal";

                    if (!string.IsNullOrEmpty(student.studentMaster.StudentInternalId))
                    {
                        bool checkInternalID = CheckInternalID(student.studentMaster.TenantId, student.studentMaster.StudentInternalId, student.studentMaster.SchoolId);
                        if (checkInternalID == false)
                        {
                            student.studentMaster = null;
                            student.fieldsCategoryList = null;
                            student._failure = true;
                            student._message = "Student InternalID Already Exist";
                            return student;
                        }
                    }
                    else
                    {
                        student.studentMaster.StudentInternalId = MasterStudentId.ToString();
                    }

                    var schoolName = this.context?.SchoolMaster.Where(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId).Select(s => s.SchoolName).FirstOrDefault();

                    //Insert data into Enrollment table
                    int? calenderId = null;
                    string enrollmentCode = null;

                    var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.AcademicYear.ToString() == student.AcademicYear && x.DefaultCalender == true);

                    if (defaultCalender != null)
                    {
                        calenderId = defaultCalender.CalenderId;
                    }

                    var enrollmentType = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Type.ToLower() == "Add".ToLower());

                    if (enrollmentType != null)
                    {
                        enrollmentCode = enrollmentType.Title;
                    }

                    var gradeLevel = this.context?.Gradelevels.Where(x => x.SchoolId == student.studentMaster.SchoolId).OrderBy(x => x.GradeId).FirstOrDefault();

                    int? gradeId = null;
                    if (gradeLevel != null)
                    {
                        gradeId = gradeLevel.GradeId;
                    }

                    var StudentEnrollmentData = new StudentEnrollment() { TenantId = student.studentMaster.TenantId, SchoolId = student.studentMaster.SchoolId, StudentId = student.studentMaster.StudentId, EnrollmentId = 1, SchoolName = schoolName, RollingOption = "Next grade at current school", EnrollmentCode = enrollmentCode, CalenderId = calenderId, GradeLevelTitle = (gradeLevel != null) ? gradeLevel.Title : null, EnrollmentDate = DateTime.UtcNow, StudentGuid = GuidId, IsActive = true, GradeId = gradeId };

                    //Add student portal access
                    if (!string.IsNullOrWhiteSpace(student.PasswordHash) && !string.IsNullOrWhiteSpace(student.LoginEmail))
                    {
                        UserMaster userMaster = new UserMaster();

                        var decrypted = Utility.Decrypt(student.PasswordHash);
                        string passwordHash = Utility.GetHashedPassword(decrypted);

                        var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == student.LoginEmail);

                        if (loginInfo == null)
                        {
                            var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Profile == "Student");

                            userMaster.SchoolId = student.studentMaster.SchoolId;
                            userMaster.TenantId = student.studentMaster.TenantId;
                            userMaster.UserId = student.studentMaster.StudentId;
                            userMaster.LangId = 1;
                            userMaster.MembershipId = membership.MembershipId;
                            userMaster.EmailAddress = student.LoginEmail;
                            userMaster.PasswordHash = passwordHash;
                            userMaster.Name = student.studentMaster.FirstGivenName;
                            userMaster.LastUpdated = DateTime.UtcNow;
                            userMaster.IsActive = student.PortalAccess;
                            student.studentMaster.StudentPortalId = student.LoginEmail;
                            this.context?.UserMaster.Add(userMaster);
                            this.context?.SaveChanges();
                        }
                        else
                        {
                            student.studentMaster = null;
                            student.fieldsCategoryList = null;
                            student._failure = true;
                            student._message = "Student Login Email Already Exist";
                            return student;
                        }
                    }

                    this.context?.StudentMaster.Add(student.studentMaster);
                    this.context?.StudentEnrollment.Add(StudentEnrollmentData);
                    this.context?.SaveChanges();

                    if (student.fieldsCategoryList != null && student.fieldsCategoryList.ToList().Count > 0)
                    {
                        var fieldsCategory = student.fieldsCategoryList.FirstOrDefault(x => x.CategoryId == student.SelectedCategoryId);
                        if (fieldsCategory != null)
                        {
                            foreach (var customFields in fieldsCategory.CustomFields.ToList())
                            {
                                if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                {
                                    customFields.CustomFieldsValue.FirstOrDefault().Module = "Student";
                                    customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                    customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                    customFields.CustomFieldsValue.FirstOrDefault().SchoolId = student.studentMaster.SchoolId;
                                    customFields.CustomFieldsValue.FirstOrDefault().TargetId = student.studentMaster.StudentId;
                                    this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                    this.context?.SaveChanges();
                                }
                            }

                        }
                    }

                    student._failure = false;
                    student._message = "Student Added Successfully";
                    transaction.Commit();
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    student._failure = true;
                    student._message = es.Message;
                }
            }
            return student;
        }
        private bool CheckInternalID(Guid TenantId, string InternalID,int SchoolId)
        {
            if (InternalID != null && InternalID != "")
            {
                var checkInternalId = this.context?.StudentMaster.Where(x => x.TenantId == TenantId && x.StudentInternalId == InternalID && x.SchoolId == SchoolId).ToList();
                if (checkInternalId.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update Student
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentAddViewModel UpdateStudent(StudentAddViewModel student)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var checkInternalId = this.context?.StudentMaster.Where(x => x.TenantId == student.studentMaster.TenantId && x.StudentInternalId == student.studentMaster.StudentInternalId && x.StudentInternalId != null && x.StudentId != student.studentMaster.StudentId ).ToList();
                    if(checkInternalId.Count()>0)
                    {
                        student.studentMaster = null;
                        student.fieldsCategoryList = null;
                        student._failure = true;
                        student._message = "Student InternalID Already Exist";
                    }
                    else
                    {
                        var studentUpdate = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.StudentId == student.studentMaster.StudentId);

                        if(string.IsNullOrEmpty(student.studentMaster.StudentInternalId))
                        {
                            student.studentMaster.StudentInternalId = studentUpdate.StudentInternalId;
                        }

                        //Add or Update student portal access
                        if (studentUpdate.StudentPortalId != null)
                        {
                            if (!string.IsNullOrWhiteSpace(student.LoginEmail))
                            {
                                if (studentUpdate.StudentPortalId != student.LoginEmail)
                                {
                                    var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == student.LoginEmail);

                                    if (loginInfo != null)
                                    {
                                        student.studentMaster = null;
                                        student.fieldsCategoryList = null;
                                        student._failure = true;
                                        student._message = "Student Login Email Already Exist";
                                        return student;
                                    }
                                    else
                                    {
                                        var loginInfoData = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == studentUpdate.StudentPortalId);

                                        loginInfoData.EmailAddress = student.LoginEmail;
                                        loginInfoData.IsActive = student.PortalAccess;

                                        this.context?.UserMaster.Add(loginInfoData);
                                        this.context?.SaveChanges();

                                        //Update StudentPortalId in Studentmaster table.
                                        //studentUpdate.StudentPortalId = student.LoginEmail;
                                        student.studentMaster.StudentPortalId = student.LoginEmail;
                                    }
                                }
                                else
                                {
                                    var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == studentUpdate.StudentPortalId);

                                    if (loginInfo != null)
                                    {
                                        loginInfo.IsActive = student.PortalAccess;
                                    }

                                    this.context?.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(student.LoginEmail) && !string.IsNullOrWhiteSpace(student.PasswordHash))
                            {
                                var decrypted = Utility.Decrypt(student.PasswordHash);
                                string passwordHash = Utility.GetHashedPassword(decrypted);

                                UserMaster userMaster = new UserMaster();

                                var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == student.LoginEmail);

                                if (loginInfo == null)
                                {
                                    var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Profile == "Student");

                                    userMaster.SchoolId = student.studentMaster.SchoolId;
                                    userMaster.TenantId = student.studentMaster.TenantId;
                                    userMaster.UserId = student.studentMaster.StudentId;
                                    userMaster.LangId = 1;
                                    userMaster.MembershipId = membership.MembershipId;
                                    userMaster.EmailAddress = student.LoginEmail;
                                    userMaster.PasswordHash = passwordHash;
                                    userMaster.Name = student.studentMaster.FirstGivenName;
                                    userMaster.LastUpdated = DateTime.UtcNow;
                                    userMaster.IsActive = student.PortalAccess;

                                    this.context?.UserMaster.Add(userMaster);
                                    this.context?.SaveChanges();


                                    //Update StudentPortalId in Studentmaster table.
                                    //studentUpdate.StudentPortalId = student.LoginEmail;
                                    student.studentMaster.StudentPortalId = student.LoginEmail;
                                }
                                else
                                {
                                    student.studentMaster = null;
                                    student.fieldsCategoryList = null;
                                    student._failure = true;
                                    student._message = "Student Login Email Already Exist";
                                    return student;
                                }
                            }
                        }

                        student.studentMaster.Associationship = studentUpdate.Associationship;
                        student.studentMaster.EnrollmentType = studentUpdate.EnrollmentType;
                        student.studentMaster.IsActive = studentUpdate.IsActive;
                        student.studentMaster.StudentGuid = studentUpdate.StudentGuid;
                        student.studentMaster.LastUpdated = DateTime.UtcNow;
                        this.context.Entry(studentUpdate).CurrentValues.SetValues(student.studentMaster);
                        this.context?.SaveChanges();

                        

                        this.context?.SaveChanges();

                        if (student.fieldsCategoryList != null && student.fieldsCategoryList.ToList().Count > 0)
                        {
                            var fieldsCategory = student.fieldsCategoryList.FirstOrDefault(x => x.CategoryId == student.SelectedCategoryId);
                            if (fieldsCategory != null)
                            {
                                foreach (var customFields in fieldsCategory.CustomFields.ToList())
                                {
                                    var customFieldValueData = this.context?.CustomFieldsValue.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.CategoryId == customFields.CategoryId && x.FieldId == customFields.FieldId && x.Module == "Student" && x.TargetId == student.studentMaster.StudentId);
                                    if (customFieldValueData != null)
                                    {
                                        this.context?.CustomFieldsValue.RemoveRange(customFieldValueData);
                                    }
                                    if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                    {
                                        customFields.CustomFieldsValue.FirstOrDefault().Module = "Student";
                                        customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                        customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                        customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                        customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                        customFields.CustomFieldsValue.FirstOrDefault().SchoolId = student.studentMaster.SchoolId;
                                        customFields.CustomFieldsValue.FirstOrDefault().TargetId = student.studentMaster.StudentId;
                                        this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                        this.context?.SaveChanges();
                                    }
                                }
                            }
                        }

                        student._failure = false;
                        student._message = "Student Updated Successfully";
                    }
                    transaction.Commit();
                   
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    student.studentMaster = null;
                    student._failure = true;
                    student._message = ex.Message;

                }
            }
            return student;

        }

        /// <summary>
        /// Get All Student With Pagination,sorting,searching
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StudentListModel GetAllStudentList(PageResult pageResult)
        {

            StudentListModel studentListModel = new StudentListModel();
            IQueryable<StudentMaster> transactionIQ = null;


            //var studentDataList = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.IsActive != false);

            var studentDataList = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Include(x => x.Sections).Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && (pageResult.IncludeInactive == false || pageResult.IncludeInactive == null ? x.IsActive!=false : true)).AsNoTracking().Select(e => new StudentMaster
            {
                TenantId = e.TenantId,
                SchoolId = e.SchoolId,
                StudentId = e.StudentId,
                FirstGivenName = e.FirstGivenName,
                MiddleName = e.MiddleName,
                LastFamilyName = e.LastFamilyName,
                AlternateId = e.AlternateId,
                StudentInternalId = e.StudentInternalId,
                MobilePhone = e.MobilePhone,
                HomePhone = e.HomePhone,
                PersonalEmail = e.PersonalEmail,
                SchoolEmail = e.SchoolEmail,
                StudentGuid = e.StudentGuid,
                AdmissionNumber=e.AdmissionNumber,
                RollNumber=e.RollNumber,
                Dob=e.Dob,
                Gender=e.Gender,
                Race=e.Race,
                Ethnicity=e.Ethnicity,
                MaritalStatus=e.MaritalStatus,
                CountryOfBirth=e.CountryOfBirth,
                Nationality=e.Nationality,
                FirstLanguage=e.FirstLanguage,
                SecondLanguage=e.SecondLanguage,
                ThirdLanguage=e.ThirdLanguage,
                HomeAddressLineOne = e.HomeAddressLineOne,
                HomeAddressLineTwo = e.HomeAddressLineTwo,
                HomeAddressCity = e.HomeAddressCity,
                HomeAddressCountry = e.HomeAddressCountry,
                HomeAddressState = e.HomeAddressState,
                HomeAddressZip = e.HomeAddressZip,
                BusNo=e.BusNo,
                FirstLanguageId=e.FirstLanguageId,
                SecondLanguageId=e.SecondLanguageId,
                ThirdLanguageId=e.ThirdLanguageId,
                SectionId=e.SectionId,
                IsActive=e.IsActive,
                Sections = e.Sections,
                StudentEnrollment = e.StudentEnrollment.Where(d => d.IsActive == true).Select(s => new StudentEnrollment
                {
                    EnrollmentDate = s.EnrollmentDate,
                    GradeLevelTitle = s.GradeLevelTitle,
                    TenantId = s.TenantId,
                    SchoolId = s.SchoolId,
                    StudentId = s.StudentId,
                    EnrollmentId = s.EnrollmentId, 
                    StudentGuid = s.StudentGuid,
                    GradeId=s.GradeId,
                }).ToList()
            });

            try
            {
                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = studentDataList;
                }
                else
                {
                    string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        transactionIQ = studentDataList.Where(x => x.FirstGivenName != null && x.FirstGivenName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.MiddleName != null && x.MiddleName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.LastFamilyName != null && x.LastFamilyName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.StudentInternalId != null && x.StudentInternalId.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.AlternateId != null && x.AlternateId.Contains(Columnvalue) ||
                                                                    x.HomePhone != null && x.HomePhone.Contains(Columnvalue) ||
                                                                    x.MobilePhone != null && x.MobilePhone.Contains(Columnvalue) ||
                                                                    x.PersonalEmail != null && x.PersonalEmail.Contains(Columnvalue) ||
                                                                    x.SchoolEmail != null && x.SchoolEmail.Contains(Columnvalue));

                        //for GradeLevel Searching
                        var gradeLevelFilter = studentDataList.AsNoTracking().ToList().Where(x => x.StudentEnrollment.ToList().Count > 0 ? x.StudentEnrollment.FirstOrDefault().GradeLevelTitle.ToLower().Contains(Columnvalue.ToLower()) : string.Empty.Contains(Columnvalue)).AsQueryable();

                        if (gradeLevelFilter.ToList().Count > 0)
                        {
                            transactionIQ = transactionIQ.AsNoTracking().ToList().Concat(gradeLevelFilter).AsQueryable();
                        
                        }

                        //searching for section name
                        var sectionId = this.context?.Sections.Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.Name.ToLower().Contains(Columnvalue.ToLower())).Select(x => x.SectionId).ToList();

                        if (sectionId.ToList().Count > 0)
                        {
                            var sectionSearchData = studentDataList.Where(x => x.SchoolId == pageResult.SchoolId && sectionId.Contains((int)x.SectionId));

                            if (sectionSearchData.ToList().Count > 0)
                            {
                                transactionIQ = transactionIQ.AsNoTracking().ToList().Concat(sectionSearchData).AsQueryable();
                                transactionIQ = transactionIQ.GroupBy(x => x.StudentId).Select(g => g.First());
                            }
                        }
                    }
                    else
                    {
                        if (pageResult.FilterParams.Any(x => x.ColumnName.ToLower() == "gradeid"))
                        {
                            var filterData = pageResult.FilterParams.Where(x => x.ColumnName.ToLower() == "gradeid").ToList();
                            if (filterData.Count > 0)
                            {
                                var filterValues = filterData.Select(x => x.FilterValue).ToList();

                                var gradeLevelData = studentDataList.AsNoTracking().ToList().Where(x =>filterValues.Contains(x.StudentEnrollment.FirstOrDefault().GradeId.ToString())).AsQueryable();

                                if (gradeLevelData.ToList().Count() > 0)
                                {
                                    transactionIQ = gradeLevelData.AsNoTracking().ToList().AsQueryable();
                                    var indexValue = pageResult.FilterParams.FindIndex(x => x.ColumnName.ToLower() == "gradeid");
                                    pageResult.FilterParams.RemoveAt(indexValue);
                                    if (pageResult.FilterParams.Count() > 0)
                                    {
                                        transactionIQ = Utility.FilteredData(pageResult.FilterParams, transactionIQ).AsQueryable();
                                    }
                                }
                            }
                            //else
                            //{
                            //    var filterValue = Convert.ToInt32(pageResult.FilterParams.Where(x => x.ColumnName.ToLower() == "gradeid").Select(x => x.FilterValue).FirstOrDefault());

                            //    var gradeLevelData = studentDataList.AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().GradeId == filterValue).AsQueryable();

                            //    if (gradeLevelData.ToList().Count() > 0)
                            //    {
                            //        transactionIQ = gradeLevelData.AsNoTracking().ToList().AsQueryable();
                            //        var indexValue = pageResult.FilterParams.FindIndex(x => x.ColumnName.ToLower() == "gradeid");
                            //        pageResult.FilterParams.RemoveAt(indexValue);
                            //        if (pageResult.FilterParams.Count() > 0)
                            //        {
                            //            transactionIQ = Utility.FilteredData(pageResult.FilterParams, transactionIQ).AsQueryable();
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            transactionIQ = Utility.FilteredData(pageResult.FilterParams, studentDataList).AsQueryable();
                        }
                    }  
                }

                if (pageResult.DobStartDate != null && pageResult.DobEndDate != null)
                {
                    var filterInDateRange = transactionIQ.AsNoTracking().ToList().Where(x => x.Dob >= pageResult.DobStartDate && x.Dob <= pageResult.DobEndDate).AsQueryable();
                    if (filterInDateRange.ToList().Count() > 0)
                    {
                        transactionIQ = filterInDateRange;
                    }
                    else
                    {
                        transactionIQ = null;
                    }
                }

                if (pageResult.FullName != null)
                {
                    var studentName = pageResult.FullName.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (studentName.Length > 1)
                    {
                        var firstName = studentName.First();
                        var lastName = studentName.Last();
                        pageResult.FullName = null;

                        if (pageResult.FullName == null)
                        {
                            var nameSearch = transactionIQ.AsNoTracking().ToList().Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.FirstGivenName.StartsWith(firstName.ToString()) && x.LastFamilyName.StartsWith(lastName.ToString())).AsQueryable();

                            transactionIQ = nameSearch;
                        }
                    }
                    else
                    {
                        var nameSearch = transactionIQ.AsNoTracking().ToList().Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && (x.FirstGivenName.StartsWith(pageResult.FullName) || x.LastFamilyName.StartsWith(pageResult.FullName))).AsQueryable();

                        transactionIQ = nameSearch;
                    }
                }

                if (pageResult.SortingModel != null)
                {
                    switch (pageResult.SortingModel.SortColumn.ToLower())
                    {
                        //For GradeLevel Sorting
                        case "gradeleveltitle":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {
                               
                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderBy(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().GradeLevelTitle : null).AsQueryable();
                            }
                            else
                            {
                             
                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderByDescending(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().GradeLevelTitle : null).AsQueryable();
                            }
                            break;

                        default:
                            transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                            break;
                    }
                    
                }

                if (transactionIQ != null)
                {
                    int? totalCount = transactionIQ.AsNoTracking().Count();
                    if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                    {
                        transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                    }
                    studentListModel.studentMaster = transactionIQ.ToList();
                    studentListModel.TotalCount = totalCount;
                }
                else
                {
                    studentListModel.TotalCount = 0;
                }
                
                studentListModel.TenantId = pageResult.TenantId;
                studentListModel.SchoolId = pageResult.SchoolId;
                studentListModel.PageNumber = pageResult.PageNumber;
                studentListModel._pageSize = pageResult.PageSize;
                studentListModel._tenantName = pageResult._tenantName;
                studentListModel._token = pageResult._token;
                studentListModel._failure = false;
            }
            catch (Exception es)
            {
                studentListModel._message = es.Message;
                studentListModel._failure = true;
                studentListModel._tenantName = pageResult._tenantName;
                studentListModel._token = pageResult._token;
            }
            return studentListModel;
        }

        /// <summary>
        /// Add StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel AddStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            try
            {
                int? MasterDocumentId = 0;

                if (studentDocumentAddViewModel.studentDocuments != null && studentDocumentAddViewModel.studentDocuments.ToList().Count > 0)
                {
                    MasterDocumentId = Utility.GetMaxPK(this.context, new Func<StudentDocuments, int>(x => x.DocumentId));

                    foreach (var studentDocument in studentDocumentAddViewModel.studentDocuments.ToList())
                    {
                        studentDocument.DocumentId = (int)MasterDocumentId;
                        studentDocument.UploadedOn = DateTime.UtcNow;
                        this.context?.StudentDocuments.Add(studentDocument);
                        MasterDocumentId++;
                    }

                    this.context?.SaveChanges();
                }

                studentDocumentAddViewModel._failure = false;
                studentDocumentAddViewModel._message = "Student Document Added Successfully";
            }
            catch (Exception es)
            {
                studentDocumentAddViewModel._failure = true;
                studentDocumentAddViewModel._message = es.Message;
            }
            return studentDocumentAddViewModel;
        }

        /// <summary>
        /// Update StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            try
            {
                var studentDocumentUpdate = this.context?.StudentDocuments.FirstOrDefault(x => x.TenantId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().TenantId && x.SchoolId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId && x.StudentId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().StudentId && x.DocumentId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().DocumentId);

                studentDocumentAddViewModel.studentDocuments.FirstOrDefault().UploadedOn = DateTime.UtcNow;
                this.context.Entry(studentDocumentUpdate).CurrentValues.SetValues(studentDocumentAddViewModel.studentDocuments.FirstOrDefault());
                this.context?.SaveChanges();
                studentDocumentAddViewModel._failure = false;
                studentDocumentAddViewModel._message = "Student Document Updated Successfully";
            }
            catch (Exception es)
            {
                studentDocumentAddViewModel._failure = true;
                studentDocumentAddViewModel._message = es.Message;
            }
            return studentDocumentAddViewModel;
        }

        /// <summary>
        /// Get All StudentDocuments List
        /// </summary>
        /// <param name="studentDocumentListViewModel"></param>
        /// <returns></returns>
        public StudentDocumentListViewModel GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentListViewModel)
        {
            StudentDocumentListViewModel studentDocumentsList = new StudentDocumentListViewModel();
            try
            {
                var StudentDocumentsAll = this.context?.StudentDocuments.Where(x => x.TenantId == studentDocumentListViewModel.TenantId && x.SchoolId == studentDocumentListViewModel.SchoolId && x.StudentId == studentDocumentListViewModel.StudentId).OrderByDescending(x=>x.DocumentId).ToList();

                studentDocumentsList.studentDocumentsList = StudentDocumentsAll;
                studentDocumentsList._tenantName = studentDocumentListViewModel._tenantName;
                studentDocumentsList._token = studentDocumentListViewModel._token;
                studentDocumentsList.TenantId = studentDocumentListViewModel.TenantId;
                studentDocumentsList.SchoolId = studentDocumentListViewModel.SchoolId;
                studentDocumentsList.StudentId = studentDocumentListViewModel.StudentId;

                if (StudentDocumentsAll.Count > 0)
                { 
                    studentDocumentsList._failure = false;
                }
                else
                {
                    studentDocumentsList._failure = true;
                    studentDocumentsList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentDocumentsList._message = es.Message;
                studentDocumentsList._failure = true;
                studentDocumentsList._tenantName = studentDocumentListViewModel._tenantName;
                studentDocumentsList._token = studentDocumentListViewModel._token;
            }
            return studentDocumentsList;
        }

        /// <summary>
        /// Delete StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            try
            {
                var studentDocumentDelete = this.context?.StudentDocuments.FirstOrDefault(x => x.TenantId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().TenantId && x.SchoolId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId && x.StudentId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().StudentId && x.DocumentId == studentDocumentAddViewModel.studentDocuments.FirstOrDefault().DocumentId);
                this.context?.StudentDocuments.Remove(studentDocumentDelete);
                this.context?.SaveChanges();
                studentDocumentAddViewModel._failure = false;
                studentDocumentAddViewModel._message = "Student Document Deleted Successfully";
            }
            catch (Exception es)
            {
                studentDocumentAddViewModel._failure = true;
                studentDocumentAddViewModel._message = es.Message;
            }
            return studentDocumentAddViewModel;
        }
        /// <summary>
        /// Add Student Login Info
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public LoginInfoAddModel AddStudentLoginInfo(LoginInfoAddModel login)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(login.userMaster.PasswordHash) && !string.IsNullOrWhiteSpace(login.userMaster.EmailAddress))
                {
                    var decrypted = Utility.Decrypt(login.userMaster.PasswordHash);
                    string passwordHash = Utility.GetHashedPassword(decrypted);

                    var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == login.userMaster.TenantId && x.SchoolId == login.userMaster.SchoolId && x.EmailAddress == login.userMaster.EmailAddress);

                    if (loginInfo == null)
                    {
                        var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == login.userMaster.TenantId && x.SchoolId == login.userMaster.SchoolId && x.Profile == "Student");

                        login.userMaster.UserId = login.StudentId;
                        login.userMaster.LangId = 1;
                        login.userMaster.MembershipId = membership.MembershipId;
                        login.userMaster.PasswordHash = passwordHash;
                        login.userMaster.LastUpdated = DateTime.UtcNow;
                        login.userMaster.IsActive = true;

                        if (login.userMaster.UserSecretQuestions != null)
                        {
                            login.userMaster.UserSecretQuestions.UserId = login.StudentId;
                            login.userMaster.UserSecretQuestions.LastUpdated = DateTime.UtcNow;
                        }

                        this.context?.UserMaster.Add(login.userMaster);
                        this.context?.SaveChanges();

                        //Update StudentPortalId in Studentmaster table.
                        var student = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == login.userMaster.TenantId && x.SchoolId == login.userMaster.SchoolId && x.StudentId == login.StudentId);
                        student.StudentPortalId = login.userMaster.EmailAddress;

                        this.context?.SaveChanges();
                    }
                }
                login._failure = false;
                login._message = "Student Login Info Added Succesfully";
            }
            catch (Exception es)
            {
                login._failure = true;
                login._message = es.Message;
            }

            return login;
        }
        /// <summary>
        /// Add Student Comment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel AddStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            try
            {
                int? MasterCommentId = Utility.GetMaxPK(this.context, new Func<StudentComments, int>(x => x.CommentId));
                studentCommentAddViewModel.studentComments.CommentId = (int)MasterCommentId;
                studentCommentAddViewModel.studentComments.LastUpdated = DateTime.UtcNow;
                this.context?.StudentComments.Add(studentCommentAddViewModel.studentComments);
                this.context?.SaveChanges();
                studentCommentAddViewModel._failure = false;
                studentCommentAddViewModel._message = "Student Comment Added Successfully";
            }
            catch (Exception es)
            {
                studentCommentAddViewModel._failure = true;
                studentCommentAddViewModel._message = es.Message;
            }
            return studentCommentAddViewModel;
        }
        /// <summary>
        /// Update Student Comment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel UpdateStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            try
            {
                var studentCommentUpdate = this.context?.StudentComments.FirstOrDefault(x => x.TenantId == studentCommentAddViewModel.studentComments.TenantId && x.SchoolId == studentCommentAddViewModel.studentComments.SchoolId && x.StudentId == studentCommentAddViewModel.studentComments.StudentId && x.CommentId == studentCommentAddViewModel.studentComments.CommentId);

                studentCommentAddViewModel.studentComments.LastUpdated = DateTime.UtcNow;
                this.context.Entry(studentCommentUpdate).CurrentValues.SetValues(studentCommentAddViewModel.studentComments);
                this.context?.SaveChanges();
                studentCommentAddViewModel._failure = false;
                studentCommentAddViewModel._message = "Student Comment Updated Successfully";
            }
            catch (Exception es)
            {
                studentCommentAddViewModel._failure = true;
                studentCommentAddViewModel._message = es.Message;
            }
            return studentCommentAddViewModel;
        }
        /// <summary>
        /// Get All Student Comments List
        /// </summary>
        /// <param name="studentCommentListViewModel"></param>
        /// <returns></returns>
        public StudentCommentListViewModel GetAllStudentCommentsList(StudentCommentListViewModel studentCommentListViewModel)
        {
            StudentCommentListViewModel studentCommentsList = new StudentCommentListViewModel();
            try
            {

                var StudentCommentsAll = this.context?.StudentComments.Where(x => x.TenantId == studentCommentListViewModel.TenantId && x.SchoolId == studentCommentListViewModel.SchoolId && x.StudentId == studentCommentListViewModel.StudentId).OrderByDescending(x => x.CommentId).ToList();

                studentCommentsList.studentCommentsList = StudentCommentsAll;
                studentCommentsList._tenantName = studentCommentListViewModel._tenantName;
                studentCommentsList.TenantId = studentCommentListViewModel.TenantId;
                studentCommentsList.SchoolId = studentCommentListViewModel.SchoolId;
                studentCommentsList.StudentId = studentCommentListViewModel.StudentId;
                studentCommentsList._token = studentCommentListViewModel._token;

                if (StudentCommentsAll.Count > 0)
                {
                    studentCommentsList._failure = false;
                }
                else
                {
                    studentCommentsList._failure = true;
                    studentCommentsList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentCommentsList._message = es.Message;
                studentCommentsList._failure = true;
                studentCommentsList._tenantName = studentCommentListViewModel._tenantName;
                studentCommentsList._token = studentCommentListViewModel._token;
            }
            return studentCommentsList;
        }
        /// <summary>
        /// Delete Student Comment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel DeleteStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            try
            {
                var studentCommentDelete = this.context?.StudentComments.FirstOrDefault(x => x.TenantId == studentCommentAddViewModel.studentComments.TenantId && x.SchoolId == studentCommentAddViewModel.studentComments.SchoolId && x.StudentId == studentCommentAddViewModel.studentComments.StudentId && x.CommentId == studentCommentAddViewModel.studentComments.CommentId);
                this.context?.StudentComments.Remove(studentCommentDelete);
                this.context?.SaveChanges();
                studentCommentAddViewModel._failure = false;
                studentCommentAddViewModel._message = "Student Comment Deleted Successfully";
            }
            catch (Exception es)
            {
                studentCommentAddViewModel._failure = true;
                studentCommentAddViewModel._message = es.Message;
            }
            return studentCommentAddViewModel;
        }


        /// <summary>
        /// Add Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentAddView"></param>
        /// <returns></returns>
        public StudentEnrollmentListModel AddStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            try
            {
                int? EnrollmentId = null;
                EnrollmentId = Utility.GetMaxPK(this.context, new Func<StudentEnrollment, int>(x => x.EnrollmentId));
                foreach (var studentEnrollment in studentEnrollmentListModel.studentEnrollments)
                {                  
                    
                    studentEnrollment.EnrollmentId = (int)EnrollmentId;
                    studentEnrollment.CalenderId = studentEnrollmentListModel.CalenderId;
                    studentEnrollment.RollingOption = studentEnrollmentListModel.RollingOption;
                    studentEnrollment.LastUpdated = DateTime.UtcNow;
                    this.context?.StudentEnrollment.AddRange(studentEnrollment);
                    EnrollmentId++;
                }
                this.context?.SaveChanges();
                studentEnrollmentListModel._failure = false;
                studentEnrollmentListModel._message = "Student Enrollment Added Successfully";
            }
            catch (Exception es)
            {
                studentEnrollmentListModel._failure = true;
                studentEnrollmentListModel._message = es.Message;
            }

            return studentEnrollmentListModel;
        }
        /// <summary>
        /// Update Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentListModel"></param>
        /// <returns></returns>
        public StudentEnrollmentListModel UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var studentMasterData = this.context?.StudentMaster.Where(x => x.TenantId == studentEnrollmentListModel.TenantId && x.SchoolId == studentEnrollmentListModel.SchoolId && x.StudentGuid == studentEnrollmentListModel.StudentGuid).FirstOrDefault();
                    if (studentMasterData != null)
                    {
                        studentMasterData.SectionId = studentEnrollmentListModel.SectionId;
                        studentMasterData.EstimatedGradDate = studentEnrollmentListModel.EstimatedGradDate;
                        studentMasterData.Eligibility504 = studentEnrollmentListModel.Eligibility504;
                        studentMasterData.EconomicDisadvantage = studentEnrollmentListModel.EconomicDisadvantage;
                        studentMasterData.FreeLunchEligibility = studentEnrollmentListModel.FreeLunchEligibility;
                        studentMasterData.SpecialEducationIndicator = studentEnrollmentListModel.SpecialEducationIndicator;
                        studentMasterData.LepIndicator = studentEnrollmentListModel.LepIndicator;
                        this.context.SaveChanges();
                    }

                    int? EnrollmentId = 1;
                    //EnrollmentId = Utility.GetMaxPK(this.context, new Func<StudentEnrollment, int>(x => x.EnrollmentId));

                    var studentEnrollmentData = this.context?.StudentEnrollment.Where(x=>x.StudentGuid==studentEnrollmentListModel.StudentGuid).OrderByDescending(x => x.EnrollmentId).FirstOrDefault();

                    if (studentEnrollmentData != null)
                    {
                        EnrollmentId = studentEnrollmentData.EnrollmentId + 1;
                    }

                    foreach (var studentEnrollmentList in studentEnrollmentListModel.studentEnrollments)
                    {
                        //Update Existing Enrollment Data
                        if (studentEnrollmentList.EnrollmentId > 0) 
                        {
                            var studentEnrollmentUpdate = this.context?.StudentEnrollment.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.StudentId == studentEnrollmentList.StudentId && x.EnrollmentId == studentEnrollmentList.EnrollmentId);
                            if (studentEnrollmentUpdate != null)
                            {
                                StudentEnrollment studentEnrollment = new StudentEnrollment();
                                if (studentEnrollmentList.ExitCode != null)
                                {
                                    //This block for Roll Over,Drop (Transfer),Enroll (Transfer)
                                    var studentExitCode = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.EnrollmentCode.ToString() == studentEnrollmentList.ExitCode); //fetching enrollemnt code type 

                                    if (studentExitCode.Type.ToLower() == "Drop (Transfer)".ToLower())
                                    {     
                                        //This block for student drop(transfer) & enroll(transfer) new school

                                        //update student's existing enrollment details 
                                        studentEnrollmentUpdate.ExitCode = studentExitCode.Title;
                                        studentEnrollmentUpdate.ExitDate = studentEnrollmentList.ExitDate;
                                        studentEnrollmentUpdate.TransferredGrade = studentEnrollmentList.TransferredGrade;
                                        studentEnrollmentUpdate.TransferredSchoolId = studentEnrollmentList.TransferredSchoolId;
                                        studentEnrollmentUpdate.SchoolTransferred = studentEnrollmentList.SchoolTransferred;
                                        studentEnrollmentUpdate.LastUpdated = DateTime.UtcNow;
                                        studentEnrollmentUpdate.UpdatedBy = studentEnrollmentList.UpdatedBy;
                                        studentEnrollmentUpdate.IsActive = false;

                                        //fetching enrollment code where student enroll(transfer).
                                        var studentTransferIn = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.TransferredSchoolId && x.Type.ToLower() == "Enroll (Transfer)".ToLower());

                                        if(studentTransferIn != null)
                                        {
                                            //fetching student details from studentMaster table for the new school if exist previously
                                            var checkStudentAlreadyExistInTransferredSchool = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.TransferredSchoolId && x.StudentGuid == studentEnrollmentListModel.StudentGuid);

                                            if (checkStudentAlreadyExistInTransferredSchool != null)
                                            {
                                                //fetching student details from studentMaster table
                                                var studentData = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentListModel.TenantId && x.SchoolId == studentEnrollmentListModel.SchoolId && x.StudentId == studentEnrollmentListModel.StudentId);

                                                //fetching all student's active details from studentMaster table
                                                var otherSchoolEnrollment = this.context?.StudentMaster.Where(x => x.TenantId == studentData.TenantId && x.StudentGuid == studentData.StudentGuid).ToList();

                                                if (otherSchoolEnrollment.Count > 0)
                                                {
                                                    foreach (var enrollmentData in otherSchoolEnrollment)
                                                    {
                                                        //this loop for update student's IsActive details and make it false for previos school
                                                        enrollmentData.IsActive = false;
                                                        this.context?.SaveChanges();
                                                    }
                                                }
                                                checkStudentAlreadyExistInTransferredSchool.IsActive = true;
                                                checkStudentAlreadyExistInTransferredSchool.EnrollmentType = "Internal";
                                                this.context?.SaveChanges();

                                                studentEnrollmentList.StudentId = (int)checkStudentAlreadyExistInTransferredSchool.StudentId;
                                            }
                                            else
                                            {
                                                //fetching student details from studentMaster table
                                                var studentData = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentListModel.TenantId && x.SchoolId == studentEnrollmentListModel.SchoolId && x.StudentId == studentEnrollmentListModel.StudentId);
                                                if (studentData != null)
                                                {
                                                    //fetching all student's active details from studentMaster table
                                                    var otherSchoolEnrollment = this.context?.StudentMaster.Where(x => x.TenantId == studentData.TenantId && x.StudentGuid == studentData.StudentGuid).ToList();
                                                    if (otherSchoolEnrollment.Count > 0)
                                                    {
                                                        foreach (var enrollmentData in otherSchoolEnrollment)
                                                        {
                                                            //this loop for update student's IsActive details and make it false for previos school
                                                            enrollmentData.IsActive = false;
                                                            this.context?.SaveChanges();
                                                        }
                                                    }

                                                    //generate StudentId where student enroll(Transfer) & save data
                                                    int? MasterStudentId = 0;

                                                    var studentDataForTransferredSchool = this.context?.StudentMaster.Where(x => x.SchoolId == studentEnrollmentList.TransferredSchoolId && x.TenantId == studentEnrollmentList.TenantId).OrderByDescending(x => x.StudentId).FirstOrDefault();

                                                    if (studentDataForTransferredSchool != null)
                                                    {
                                                        MasterStudentId = studentDataForTransferredSchool.StudentId + 1;
                                                    }
                                                    else
                                                    {
                                                        MasterStudentId = 1;
                                                    }

                                                    //studentData.SchoolId = (int)studentEnrollmentList.TransferredSchoolId;
                                                    //studentData.StudentId = (int)MasterStudentId;
                                                    //studentData.EnrollmentType = "Internal";
                                                    //studentData.IsActive = true;
                                                    //studentData.LastUpdated = DateTime.UtcNow;                                                   
                                                    //this.context?.StudentMaster.Add(studentData);
                                                    
                                                    StudentMaster studentMaster = new StudentMaster() { TenantId = studentData.TenantId, SchoolId = (int)studentEnrollmentList.TransferredSchoolId, StudentId = (int)MasterStudentId, AlternateId = studentData.AlternateId, DistrictId = studentData.DistrictId, StateId = studentData.StateId, AdmissionNumber = studentData.AdmissionNumber, RollNumber = studentData.RollNumber, Salutation = studentData.Salutation, FirstGivenName = studentData.FirstGivenName, MiddleName = studentData.MiddleName, LastFamilyName = studentData.LastFamilyName, Suffix = studentData.Suffix, PreferredName = studentData.PreferredName, PreviousName = studentData.PreviousName, SocialSecurityNumber = studentData.SocialSecurityNumber, OtherGovtIssuedNumber = studentData.OtherGovtIssuedNumber, StudentPhoto = studentData.StudentPhoto, Dob = studentData.Dob, Gender = studentData.Gender, Race = studentData.Race, Ethnicity = studentData.Ethnicity, MaritalStatus = studentData.MaritalStatus, CountryOfBirth = studentData.CountryOfBirth, Nationality = studentData.Nationality, FirstLanguageId = studentData.FirstLanguageId, SecondLanguageId = studentData.SecondLanguageId, ThirdLanguageId = studentData.ThirdLanguageId, HomePhone = studentData.HomePhone, MobilePhone = studentData.MobilePhone, PersonalEmail = studentData.PersonalEmail, SchoolEmail = studentData.SchoolEmail, Twitter = studentData.Twitter, Facebook = studentData.Facebook, Instagram = studentData.Instagram, Youtube = studentData.Youtube, Linkedin = studentData.Linkedin, HomeAddressLineOne = studentData.HomeAddressLineOne, HomeAddressLineTwo = studentData.HomeAddressLineTwo, HomeAddressCountry = studentData.HomeAddressCountry, HomeAddressState = studentData.HomeAddressState, HomeAddressCity = studentData.HomeAddressCity, HomeAddressZip = studentData.HomeAddressZip, BusNo = studentData.BusNo, SchoolBusPickUp = studentData.SchoolBusPickUp, SchoolBusDropOff = studentData.SchoolBusDropOff, MailingAddressSameToHome = studentData.MailingAddressSameToHome, MailingAddressLineOne = studentData.MailingAddressLineOne, MailingAddressLineTwo = studentData.MailingAddressLineTwo, MailingAddressCountry = studentData.MailingAddressCountry, MailingAddressState = studentData.MailingAddressState, MailingAddressCity = studentData.MailingAddressCity, MailingAddressZip = studentData.MailingAddressZip, StudentPortalId = studentData.StudentPortalId, AlertDescription = studentData.AlertDescription, CriticalAlert = studentData.CriticalAlert, Dentist = studentData.Dentist, DentistPhone = studentData.DentistPhone, InsuranceCompany = studentData.InsuranceCompany, InsuranceCompanyPhone = studentData.InsuranceCompanyPhone, MedicalFacility = studentData.MedicalFacility, MedicalFacilityPhone = studentData.MedicalFacilityPhone, PolicyHolder = studentData.PolicyHolder, PolicyNumber = studentData.PolicyNumber, PrimaryCarePhysician = studentData.PrimaryCarePhysician, PrimaryCarePhysicianPhone = studentData.PrimaryCarePhysicianPhone, Vision = studentData.Vision, VisionPhone = studentData.VisionPhone, Associationship = studentData.Associationship, EconomicDisadvantage = studentData.EconomicDisadvantage, Eligibility504 = studentData.Eligibility504, EstimatedGradDate = studentData.EstimatedGradDate, FreeLunchEligibility = studentData.FreeLunchEligibility, LepIndicator = studentData.LepIndicator, SectionId = null, SpecialEducationIndicator = studentData.SpecialEducationIndicator, StudentInternalId = studentData.StudentInternalId, LastUpdated = DateTime.UtcNow, UpdatedBy = studentData.UpdatedBy, EnrollmentType = "Internal", IsActive = true, StudentGuid = studentData.StudentGuid };
                                                    
                                                    this.context?.StudentMaster.Add(studentMaster);
                                                    
                                                    //Student Protal Access
                                                    if (studentData.StudentPortalId != null)
                                                    {
                                                        var userMasterData = this.context?.UserMaster.FirstOrDefault(x => x.EmailAddress == studentData.StudentPortalId && x.TenantId == studentData.TenantId);
                                                        if (userMasterData != null)
                                                        {
                                                            userMasterData.IsActive = false;
                                                            UserMaster userMaster = new UserMaster();
                                                            userMaster.TenantId = studentData.TenantId;
                                                            userMaster.SchoolId = (int)studentEnrollmentList.TransferredSchoolId;
                                                            userMaster.UserId = (int)MasterStudentId;
                                                            userMaster.Name = userMasterData.Name;
                                                            userMaster.EmailAddress = userMasterData.EmailAddress;
                                                            userMaster.PasswordHash = userMasterData.PasswordHash;
                                                            userMaster.LangId = userMasterData.LangId;
                                                            var membershipsId = this.context?.Membership.Where(x => x.SchoolId == (int)studentEnrollmentList.TransferredSchoolId && x.TenantId == studentEnrollmentList.TenantId && x.Profile == "Student").Select(x => x.MembershipId).FirstOrDefault();
                                                            userMaster.MembershipId = (int)membershipsId;
                                                            userMaster.LastUpdated = DateTime.UtcNow;
                                                            userMaster.UpdatedBy = studentEnrollmentList.UpdatedBy;
                                                            userMaster.IsActive = true;
                                                            this.context?.UserMaster.Add(userMaster);
                                                        }
                                                    }
                                                    this.context?.SaveChanges();

                                                    studentEnrollmentList.TenantId = studentEnrollmentList.TenantId;
                                                    studentEnrollmentList.SchoolId = (int)studentEnrollmentList.SchoolId;
                                                    studentEnrollmentList.StudentId = studentEnrollmentUpdate.StudentId;
                                                    studentEnrollmentList.EnrollmentId = (int)EnrollmentId;
                                                    studentEnrollmentList.EnrollmentDate = null;
                                                    studentEnrollmentList.EnrollmentCode = null;
                                                    studentEnrollmentList.ExitCode = studentExitCode.Title;
                                                    studentEnrollmentList.ExitDate = studentEnrollmentList.ExitDate;
                                                    studentEnrollmentList.SchoolName = studentEnrollmentUpdate.SchoolName;
                                                    studentEnrollmentList.SchoolTransferred = studentEnrollmentList.SchoolTransferred;
                                                    studentEnrollmentList.TransferredSchoolId = studentEnrollmentList.TransferredSchoolId;
                                                    studentEnrollmentList.GradeLevelTitle = null;
                                                    studentEnrollmentList.TransferredGrade = studentEnrollmentList.TransferredGrade;
                                                    studentEnrollmentList.CalenderId = studentEnrollmentUpdate.CalenderId;
                                                    studentEnrollmentList.RollingOption = studentEnrollmentListModel.RollingOption;
                                                    studentEnrollmentList.LastUpdated = DateTime.UtcNow;
                                                    studentEnrollmentList.IsActive = false;
                                                    this.context?.StudentEnrollment.AddRange(studentEnrollmentList);
                                                    this.context?.SaveChanges();
                                                    EnrollmentId++;

                                                    studentEnrollmentList.StudentId = (int)MasterStudentId;
                                                }
                                            }

                                            //fetch default calender for enroll(transfer) school and save details in StudentEnrollment table.
                                            int? calenderId = null;

                                            var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.TransferredSchoolId && x.AcademicYear == studentEnrollmentListModel.AcademicYear && x.DefaultCalender == true);

                                            if (defaultCalender != null)
                                            {
                                                calenderId = defaultCalender.CalenderId;
                                            }


                                            var transferredGradeId = this.context?.Gradelevels.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == (int)studentEnrollmentList.TransferredSchoolId && x.Title.ToLower() == studentEnrollmentList.TransferredGrade.ToLower());
                                            
                                            studentEnrollmentList.TenantId = studentEnrollmentList.TenantId;
                                            studentEnrollmentList.SchoolId = (int)studentEnrollmentList.TransferredSchoolId;
                                            studentEnrollmentList.EnrollmentId = (int)EnrollmentId;
                                            studentEnrollmentList.EnrollmentDate = studentEnrollmentList.ExitDate;
                                            studentEnrollmentList.EnrollmentCode = studentTransferIn.Title;
                                            studentEnrollmentList.ExitCode = null;
                                            studentEnrollmentList.ExitDate = null;
                                            studentEnrollmentList.SchoolName = studentEnrollmentList.SchoolTransferred;
                                            studentEnrollmentList.SchoolTransferred = null;
                                            studentEnrollmentList.TransferredSchoolId = null;
                                            studentEnrollmentList.GradeLevelTitle = studentEnrollmentList.TransferredGrade;
                                            studentEnrollmentList.GradeId = transferredGradeId.GradeId;
                                            studentEnrollmentList.TransferredGrade = null;
                                            studentEnrollmentList.CalenderId = calenderId;
                                            studentEnrollmentList.RollingOption = studentEnrollmentListModel.RollingOption;
                                            studentEnrollmentList.LastUpdated = DateTime.UtcNow;
                                            studentEnrollmentList.IsActive = true;
                                            this.context?.StudentEnrollment.AddRange(studentEnrollmentList);
                                            EnrollmentId++;
                                        }
                                    }
                                    else
                                    {
                                        //This block save data for student's Roll over or Drop in same grade details
                                        studentEnrollmentUpdate.ExitCode = studentExitCode.Title;
                                        studentEnrollmentUpdate.ExitDate = studentEnrollmentList.ExitDate;
                                        studentEnrollmentUpdate.TransferredGrade = studentEnrollmentList.GradeLevelTitle;                                
                                        studentEnrollmentUpdate.LastUpdated = DateTime.UtcNow;
                                        studentEnrollmentUpdate.UpdatedBy = studentEnrollmentList.UpdatedBy;
                                        studentEnrollmentUpdate.IsActive = false;

                                        studentEnrollment.EnrollmentId = (int)EnrollmentId;
                                        studentEnrollment.TenantId = studentEnrollmentList.TenantId;
                                        studentEnrollment.SchoolId = (int)studentEnrollmentList.SchoolId;
                                        studentEnrollment.StudentId = studentEnrollmentList.StudentId;
                                        studentEnrollment.CalenderId = studentEnrollmentListModel.CalenderId;
                                        studentEnrollment.SchoolName = studentEnrollmentList.SchoolName;
                                        studentEnrollment.EnrollmentDate = studentEnrollmentList.ExitDate;
                                        studentEnrollment.EnrollmentCode = studentExitCode.Title;
                                        studentEnrollment.GradeLevelTitle = studentEnrollmentList.GradeLevelTitle;
                                        studentEnrollment.GradeId = studentEnrollmentList.GradeId;
                                        studentEnrollment.RollingOption = studentEnrollmentListModel.RollingOption;
                                        studentEnrollment.UpdatedBy = studentEnrollmentList.UpdatedBy;
                                        studentEnrollment.LastUpdated = DateTime.UtcNow;
                                        studentEnrollment.StudentGuid = studentEnrollmentUpdate.StudentGuid;
                                        studentEnrollment.IsActive = true;
                                        this.context?.StudentEnrollment.Add(studentEnrollment);

                                        if (studentExitCode.Type.ToLower() == "Drop".ToLower() && studentEnrollmentListModel.RollingOption.ToLower() == "Do not enroll after this school year".ToLower())
                                        {
                                            this.context?.StudentMaster.Where(x => x.StudentGuid == studentEnrollmentList.StudentGuid).ToList().ForEach(x => x.IsActive = false);
                                            this.context?.SaveChanges();
                                        }

                                        EnrollmentId++;
                                    }                                  
                                }
                                else
                                {
                                    //This block for update existing enrollment details only
                                    var studentEnrollmentCode = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.EnrollmentCode.ToString() == studentEnrollmentList.EnrollmentCode);

                                    studentEnrollmentUpdate.EnrollmentCode = studentEnrollmentCode.Title;
                                    studentEnrollmentUpdate.EnrollmentDate = studentEnrollmentList.EnrollmentDate;
                                    studentEnrollmentUpdate.GradeLevelTitle = studentEnrollmentList.GradeLevelTitle;
                                    studentEnrollmentUpdate.GradeId = studentEnrollmentList.GradeId;
                                    studentEnrollmentUpdate.RollingOption = studentEnrollmentListModel.RollingOption;
                                    studentEnrollmentUpdate.CalenderId = studentEnrollmentListModel.CalenderId;
                                    studentEnrollmentUpdate.LastUpdated = DateTime.UtcNow;
                                    studentEnrollmentUpdate.UpdatedBy = studentEnrollmentList.UpdatedBy;
                                }
                            }
                        }
                        else
                        {
                            //fetching student details from studentMaster table for the new school if exist previously
                            var checkStudentAlreadyExistInTransferredSchool = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.StudentGuid == studentEnrollmentListModel.StudentGuid);

                            if (checkStudentAlreadyExistInTransferredSchool != null)
                            {
                                checkStudentAlreadyExistInTransferredSchool.IsActive = true;
                                checkStudentAlreadyExistInTransferredSchool.EnrollmentType = "External";
                                this.context?.SaveChanges();

                                studentEnrollmentList.StudentId = (int)checkStudentAlreadyExistInTransferredSchool.StudentId;
                            }
                            else
                            {
                                //This block for student new enrollment in another school as external school
                                var studentData = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentListModel.TenantId && x.SchoolId == studentEnrollmentListModel.SchoolId && x.StudentId == studentEnrollmentListModel.StudentId);
                                if (studentData != null)
                                {
                                    int? MasterStudentId = 0;

                                    var studentDataForNewSchool = this.context?.StudentMaster.Where(x => x.SchoolId == studentEnrollmentList.SchoolId && x.TenantId == studentEnrollmentList.TenantId).OrderByDescending(x => x.StudentId).FirstOrDefault();

                                    if (studentDataForNewSchool != null)
                                    {
                                        MasterStudentId = studentDataForNewSchool.StudentId + 1;
                                    }
                                    else
                                    {
                                        MasterStudentId = 1;
                                    }

                                    StudentMaster studentMaster = new StudentMaster() { TenantId = studentData.TenantId, SchoolId = studentEnrollmentList.SchoolId, StudentId = (int)MasterStudentId, AlternateId = studentData.AlternateId, DistrictId = studentData.DistrictId, StateId = studentData.StateId, AdmissionNumber = studentData.AdmissionNumber, RollNumber = studentData.RollNumber, Salutation = studentData.Salutation, FirstGivenName = studentData.FirstGivenName, MiddleName = studentData.MiddleName, LastFamilyName = studentData.LastFamilyName, Suffix = studentData.Suffix, PreferredName = studentData.PreferredName, PreviousName = studentData.PreviousName, SocialSecurityNumber = studentData.SocialSecurityNumber, OtherGovtIssuedNumber = studentData.OtherGovtIssuedNumber, StudentPhoto = studentData.StudentPhoto, Dob = studentData.Dob, Gender = studentData.Gender, Race = studentData.Race, Ethnicity = studentData.Ethnicity, MaritalStatus = studentData.MaritalStatus, CountryOfBirth = studentData.CountryOfBirth, Nationality = studentData.Nationality, FirstLanguageId = studentData.FirstLanguageId, SecondLanguageId = studentData.SecondLanguageId, ThirdLanguageId = studentData.ThirdLanguageId, HomePhone = studentData.HomePhone, MobilePhone = studentData.MobilePhone, PersonalEmail = studentData.PersonalEmail, SchoolEmail = studentData.SchoolEmail, Twitter = studentData.Twitter, Facebook = studentData.Facebook, Instagram = studentData.Instagram, Youtube = studentData.Youtube, Linkedin = studentData.Linkedin, HomeAddressLineOne = studentData.HomeAddressLineOne, HomeAddressLineTwo = studentData.HomeAddressLineTwo, HomeAddressCountry = studentData.HomeAddressCountry, HomeAddressState = studentData.HomeAddressState, HomeAddressCity = studentData.HomeAddressCity, HomeAddressZip = studentData.HomeAddressZip, BusNo = studentData.BusNo, SchoolBusPickUp = studentData.SchoolBusPickUp, SchoolBusDropOff = studentData.SchoolBusDropOff, MailingAddressSameToHome = studentData.MailingAddressSameToHome, MailingAddressLineOne = studentData.MailingAddressLineOne, MailingAddressLineTwo = studentData.MailingAddressLineTwo, MailingAddressCountry = studentData.MailingAddressCountry, MailingAddressState = studentData.MailingAddressState, MailingAddressCity = studentData.MailingAddressCity, MailingAddressZip = studentData.MailingAddressZip, StudentPortalId = studentData.StudentPortalId, AlertDescription = studentData.AlertDescription, CriticalAlert = studentData.CriticalAlert, Dentist = studentData.Dentist, DentistPhone = studentData.DentistPhone, InsuranceCompany = studentData.InsuranceCompany, InsuranceCompanyPhone = studentData.InsuranceCompanyPhone, MedicalFacility = studentData.MedicalFacility, MedicalFacilityPhone = studentData.MedicalFacilityPhone, PolicyHolder = studentData.PolicyHolder, PolicyNumber = studentData.PolicyNumber, PrimaryCarePhysician = studentData.PrimaryCarePhysician, PrimaryCarePhysicianPhone = studentData.PrimaryCarePhysicianPhone, Vision = studentData.Vision, VisionPhone = studentData.VisionPhone, Associationship = studentData.Associationship, EconomicDisadvantage = studentData.EconomicDisadvantage, Eligibility504 = studentData.Eligibility504, EstimatedGradDate = studentData.EstimatedGradDate, FreeLunchEligibility = studentData.FreeLunchEligibility, LepIndicator = studentData.LepIndicator, SectionId = null, SpecialEducationIndicator = studentData.SpecialEducationIndicator, StudentInternalId = studentData.StudentInternalId, LastUpdated = DateTime.UtcNow, UpdatedBy = studentData.UpdatedBy, EnrollmentType = "External", IsActive = true, StudentGuid = studentData.StudentGuid };

                                    //studentData.SchoolId = studentEnrollmentList.SchoolId;
                                    //studentData.StudentId = (int)MasterStudentId;
                                    //studentData.EnrollmentType = "External";
                                    //studentData.IsActive = true;
                                    //studentData.LastUpdated = DateTime.UtcNow;
                                    //this.context?.StudentMaster.Add(studentData);
                                    this.context?.StudentMaster.Add(studentMaster);
                                    this.context?.SaveChanges();

                                    studentEnrollmentList.StudentId = (int)MasterStudentId;
                                }
                            }

                            var studentEnrollmentCode = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.EnrollmentCode.ToString() == studentEnrollmentList.EnrollmentCode);

                            int? calenderId = null;

                            var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == studentEnrollmentList.TenantId && x.SchoolId == studentEnrollmentList.SchoolId && x.AcademicYear == studentEnrollmentListModel.AcademicYear && x.DefaultCalender == true);

                            if (defaultCalender != null)
                            {
                                calenderId = defaultCalender.CalenderId;
                            }

                            studentEnrollmentList.TenantId = studentEnrollmentList.TenantId;
                            studentEnrollmentList.SchoolId = studentEnrollmentList.SchoolId;
                            
                            studentEnrollmentList.EnrollmentId = (int)EnrollmentId;
                            studentEnrollmentList.EnrollmentDate = studentEnrollmentList.EnrollmentDate;
                            studentEnrollmentList.EnrollmentCode = studentEnrollmentCode.Title;
                            studentEnrollmentList.CalenderId = calenderId;
                            studentEnrollmentList.RollingOption = studentEnrollmentListModel.RollingOption;
                            studentEnrollmentList.LastUpdated = DateTime.UtcNow;
                            studentEnrollmentList.IsActive = true;
                            this.context?.StudentEnrollment.AddRange(studentEnrollmentList);
                            EnrollmentId++;
                        }
                    }
                    this.context?.SaveChanges();
                    transaction.Commit();
                    studentEnrollmentListModel._failure = false;
                    studentEnrollmentListModel._message = "Student Enrollment Updated Successfully";
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentEnrollmentListModel._failure = true;
                    studentEnrollmentListModel._message = es.Message;
                }
                return studentEnrollmentListModel;
            }
        }
        /// <summary>
        /// Get All Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentListViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentListViewModel GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListViewModel)
        {
            StudentEnrollmentListViewModel studentEnrollmentListView = new StudentEnrollmentListViewModel();
            try
            {
                //fetch default calender id
                int? calenderId = null;

                var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == studentEnrollmentListViewModel.TenantId && x.SchoolId == studentEnrollmentListViewModel.SchoolId && x.AcademicYear.ToString() == studentEnrollmentListViewModel.AcademicYear && x.DefaultCalender == true);

                if (defaultCalender != null)
                {
                    calenderId = defaultCalender.CalenderId;
                }

                var studentEnrollmentList = this.context?.StudentEnrollment.Where(x => x.TenantId == studentEnrollmentListViewModel.TenantId && x.StudentGuid == studentEnrollmentListViewModel.StudentGuid).OrderByDescending(x => x.EnrollmentId).ToList();

                if (studentEnrollmentList.Count > 0)
                {
                    var studentEnrollment = studentEnrollmentList.Select(y => new StudentEnrollmentListForView
                    {
                        TenantId = y.TenantId,
                        SchoolId = y.SchoolId,
                        StudentId = y.StudentId,
                        GradeLevelTitle = y.GradeLevelTitle,
                        RollingOption = y.RollingOption,
                        SchoolName = y.SchoolName,
                        LastUpdated = y.LastUpdated,
                        SchoolTransferred = y.SchoolTransferred,
                        TransferredGrade = y.TransferredGrade,
                        TransferredSchoolId = y.TransferredSchoolId,
                        UpdatedBy = y.UpdatedBy,
                        AcademicYear = this.context?.SchoolCalendars.FirstOrDefault(z => z.CalenderId == y.CalenderId && z.SchoolId == y.SchoolId)?.AcademicYear,
                        CalenderId = y.CalenderId,
                        EnrollmentCode = y.EnrollmentCode,
                        EnrollmentId = y.EnrollmentId,
                        EnrollmentDate = y.EnrollmentDate,
                        ExitCode = y.ExitCode,
                        ExitDate = y.ExitDate,
                        StudentGuid = y.StudentGuid,
                        EnrollmentType = this.context?.StudentEnrollmentCode.FirstOrDefault(s => s.TenantId == y.TenantId && s.SchoolId == y.SchoolId && s.Title == y.EnrollmentCode)?.Type,
                        ExitType = this.context?.StudentEnrollmentCode.FirstOrDefault(s => s.TenantId == y.TenantId && s.SchoolId == y.SchoolId && s.Title == y.ExitCode)?.Type,
                        Type = this.context?.StudentMaster.FirstOrDefault(s => s.TenantId == y.TenantId && s.SchoolId == y.SchoolId && s.StudentId == y.StudentId)?.EnrollmentType,
                        GradeId = y.GradeId
                    }).ToList();
                    studentEnrollmentListView.studentEnrollmentListForView = studentEnrollment;
                    studentEnrollmentListView.RollingOption = "Next Grade at Current School";

                    var studentMasterData = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentEnrollmentListViewModel.TenantId && x.SchoolId == studentEnrollmentListViewModel.SchoolId && x.StudentGuid == studentEnrollmentListViewModel.StudentGuid);

                    if(studentMasterData != null)
                    {
                        studentEnrollmentListView.SectionId = studentMasterData.SectionId;
                        studentEnrollmentListView.EstimatedGradDate = studentMasterData.EstimatedGradDate;
                        studentEnrollmentListView.Eligibility504 = studentMasterData.Eligibility504;
                        studentEnrollmentListView.EconomicDisadvantage = studentMasterData.EconomicDisadvantage;
                        studentEnrollmentListView.FreeLunchEligibility = studentMasterData.FreeLunchEligibility;
                        studentEnrollmentListView.SpecialEducationIndicator = studentMasterData.SpecialEducationIndicator;
                        studentEnrollmentListView.LepIndicator = studentMasterData.LepIndicator;
                    }  
                    
                    studentEnrollmentListView._failure = false;
                }
                else
                {
                    studentEnrollmentListView._failure = true;
                    studentEnrollmentListView._message = NORECORDFOUND;
                }
                studentEnrollmentListView.StudentId = studentEnrollmentListViewModel.StudentId;
                studentEnrollmentListView._tenantName = studentEnrollmentListViewModel._tenantName;
                studentEnrollmentListView._token = studentEnrollmentListViewModel._token;
                studentEnrollmentListView.TenantId = studentEnrollmentListViewModel.TenantId;
                studentEnrollmentListView.CalenderId = calenderId;
                studentEnrollmentListView.AcademicYear = studentEnrollmentListViewModel.AcademicYear;
            }
            catch (Exception es)
            {
                studentEnrollmentListView._message = es.Message;
                studentEnrollmentListView._failure = true;
                studentEnrollmentListView._tenantName = studentEnrollmentListViewModel._tenantName;
                studentEnrollmentListView._token = studentEnrollmentListViewModel._token;
            }
            return studentEnrollmentListView;
        }
       

        /// <summary>
        /// View Student By Id
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>

        public StudentAddViewModel ViewStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentView = new StudentAddViewModel();
            try
            {

                var studentData = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.StudentId == student.studentMaster.StudentId);
                if (studentData != null)
                {
                    studentView.studentMaster = studentData;
                    studentView.CurrentGradeLevel = this.context?.StudentEnrollment.Where(x => x.TenantId == studentData.TenantId && x.StudentGuid == studentData.StudentGuid && x.IsActive == true).Select(x => x.GradeLevelTitle).FirstOrDefault();

                    if (studentData.StudentPortalId != null)
                    {
                        var userMasterData = this.context?.UserMaster.FirstOrDefault(x => x.EmailAddress == studentData.StudentPortalId);

                        if (userMasterData != null)
                        {
                            studentView.LoginEmail = userMasterData.EmailAddress;
                            studentView.PortalAccess = userMasterData.IsActive;
                        }
                    }
                   var customFields = this.context?.FieldsCategory.Where(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Module == "Student").OrderByDescending(x=>x.IsSystemCategory).ThenBy(x=>x.SortOrder)
                        .Select(y => new FieldsCategory
                        {
                            TenantId = y.TenantId,
                            SchoolId = y.SchoolId,
                            CategoryId = y.CategoryId,
                            IsSystemCategory = y.IsSystemCategory,
                            Search = y.Search,
                            Title = y.Title,
                            Module = y.Module,
                            SortOrder = y.SortOrder,
                            Required = y.Required,
                            Hide = y.Hide,
                            LastUpdate = y.LastUpdate,
                            UpdatedBy = y.UpdatedBy,
                            CustomFields = y.CustomFields.Where(x => x.SystemField != true).Select(z => new CustomFields
                            {
                                TenantId = z.TenantId,
                                SchoolId = z.SchoolId,
                                CategoryId = z.CategoryId,
                                FieldId = z.FieldId,
                                Module = z.Module,
                                Type = z.Type,
                                Search = z.Search,
                                Title = z.Title,
                                SortOrder = z.SortOrder,
                                SelectOptions = z.SelectOptions,
                                SystemField = z.SystemField,
                                Required = z.Required,
                                DefaultSelection = z.DefaultSelection,
                                LastUpdate = z.LastUpdate,
                                UpdatedBy = z.UpdatedBy,
                                CustomFieldsValue = z.CustomFieldsValue.Where(w => w.TargetId == student.studentMaster.StudentId).ToList()
                            }).OrderByDescending(x=>x.SystemField).ThenBy(x=>x.SortOrder).ToList()
                        }).ToList();
                    studentView.fieldsCategoryList = customFields;
                    studentView._tenantName = student._tenantName;
                    studentView._token = student._token;
                }
                else
                {
                    studentView._tenantName = student._tenantName;
                    studentView._token = student._token;
                    studentView._failure = true;
                    studentView._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentView._tenantName = student._tenantName;
                studentView._token = student._token;
                studentView._failure = true;
                studentView._message = es.Message;
            }
            return studentView;
        }

        //    /// <summary>
        //    /// Delete Student
        //    /// </summary>
        //    /// <param name="student"></param>
        //    /// <returns></returns>

        //    public StudentAddViewModel DeleteStudent(StudentAddViewModel student)
        //    {
        //        try
        //        {
        //            var studentDel = this.context?.StudentEnrollment.FirstOrDefault(x => x.TenantId == student.studentEnrollment.TenantId && x.SchoolId == student.studentEnrollment.SchoolId && x.StudentId == student.studentEnrollment.StudentId);
        //            this.context?.StudentEnrollment.Remove(studentDel);
        //            this.context?.SaveChanges();
        //            student._failure = false;
        //            student._message = "Deleted";
        //        }
        //        catch (Exception es)
        //        {
        //            student._failure = true;
        //            student._message = es.Message;
        //        }
        //        return student;
        //    }

        private static string ToFullAddress(string Address1, string Address2, string City, string State, string Country, string Zip)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(Address1))
            {


                return address == null
                      ? null
                      : $"{Address1?.Trim()}{(!string.IsNullOrWhiteSpace(Address2) ? $", {Address2?.Trim()}" : string.Empty)}, {City?.Trim()}, {State?.Trim()} {Zip?.Trim()}";
            }
            return address;
        }

        /// <summary>
        /// Search Sibling For Student
        /// </summary>
        /// <param name="studentSiblingListViewModel"></param>
        /// <returns></returns>
        public SiblingSearchForStudentListModel SearchSiblingForStudent(SiblingSearchForStudentListModel studentSiblingListViewModel)
        {
            SiblingSearchForStudentListModel StudentSiblingList = new SiblingSearchForStudentListModel();
            try
            {
                int resultData;
                var studentData = (from student in this.context?.StudentMaster
                                   join enrollment in this.context?.StudentEnrollment on student.StudentId equals enrollment.StudentId
                                   where student.SchoolId == enrollment.SchoolId && student.TenantId == enrollment.TenantId && enrollment.GradeLevelTitle != null && enrollment.IsActive == true
                                   select new
                                   {
                                       student.TenantId,
                                       student.SchoolId,
                                       student.StudentId,
                                       student.HomeAddressLineOne,
                                       student.HomeAddressLineTwo,
                                       student.HomeAddressCountry,
                                       student.HomeAddressState,
                                       student.HomeAddressCity,
                                       student.HomeAddressZip,
                                       student.StudentInternalId,
                                       student.FirstGivenName,
                                       student.MiddleName,
                                       student.LastFamilyName,
                                       student.Dob,
                                       enrollment.GradeLevelTitle
                                   });
                if (studentData != null && studentData.Count() > 0)
                {
                    var StudentSibling = studentData.Where(x => x.FirstGivenName == studentSiblingListViewModel.FirstGivenName && x.LastFamilyName == studentSiblingListViewModel.LastFamilyName && x.TenantId == studentSiblingListViewModel.TenantId && (studentSiblingListViewModel.GradeLevelTitle == null || (x.GradeLevelTitle == studentSiblingListViewModel.GradeLevelTitle)) && (studentSiblingListViewModel.SchoolId == null || (x.SchoolId == studentSiblingListViewModel.SchoolId)) && (studentSiblingListViewModel.Dob == null || (x.Dob == studentSiblingListViewModel.Dob)) && (studentSiblingListViewModel.StudentInternalId == null || (x.StudentInternalId.ToLower().Trim() == studentSiblingListViewModel.StudentInternalId.ToLower().Trim()))).ToList();
                    if (StudentSibling.Count > 0)
                    {
                        var siblingsOfStudent = StudentSibling.Select(s => new GetStudentForView
                        {
                            FirstGivenName = s.FirstGivenName,
                            LastFamilyName = s.LastFamilyName,
                            Dob = s.Dob,
                            StudentId = s.StudentId,
                            StudentInternalId = s.StudentInternalId,
                            SchoolId = s.SchoolId,
                            TenantId = s.TenantId,
                            SchoolName = this.context?.SchoolMaster.Where(x => x.SchoolId == s.SchoolId)?.Select(e => e.SchoolName).FirstOrDefault(),
                            Address = ToFullAddress(s.HomeAddressLineOne, s.HomeAddressLineTwo,
                        int.TryParse(s.HomeAddressCity, out resultData) == true ? this.context.City.Where(x => x.Id == Convert.ToInt32(s.HomeAddressCity)).FirstOrDefault().Name : s.HomeAddressCity,
                        int.TryParse(s.HomeAddressState, out resultData) == true ? this.context.State.Where(x => x.Id == Convert.ToInt32(s.HomeAddressState)).FirstOrDefault().Name : s.HomeAddressState,
                        int.TryParse(s.HomeAddressCountry, out resultData) == true ? this.context.Country.Where(x => x.Id == Convert.ToInt32(s.HomeAddressCountry)).FirstOrDefault().Name : string.Empty, s.HomeAddressZip),
                            GradeLevelTitle = s.GradeLevelTitle
                        }).ToList();
                        StudentSiblingList.getStudentForView = siblingsOfStudent;
                        StudentSiblingList._tenantName = studentSiblingListViewModel._tenantName;
                        StudentSiblingList._token = studentSiblingListViewModel._token;
                        StudentSiblingList._failure = false;
                    }
                    else
                    {
                        StudentSiblingList._failure = true;
                        StudentSiblingList._message = NORECORDFOUND;
                    }
                }
                else
                {
                    StudentSiblingList._failure = true;
                    StudentSiblingList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                StudentSiblingList._message = es.Message;
                StudentSiblingList._failure = true;
                StudentSiblingList._tenantName = studentSiblingListViewModel._tenantName;
                StudentSiblingList._token = studentSiblingListViewModel._token;
            }
            return StudentSiblingList;
        }

        /// <summary>
        /// Association Sibling
        /// </summary>
        /// <param name="siblingAddUpdateForStudentModel"></param>
        /// <returns></returns>
        public SiblingAddUpdateForStudentModel AssociationSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingAddUpdateForStudent = new SiblingAddUpdateForStudentModel();
            try
            {
                if (siblingAddUpdateForStudentModel.studentMaster.StudentId > 0)
                {
                    var studentAssociateTo = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == siblingAddUpdateForStudentModel.studentMaster.StudentId && x.SchoolId == siblingAddUpdateForStudentModel.studentMaster.SchoolId && x.TenantId == siblingAddUpdateForStudentModel.studentMaster.TenantId);

                    var studentAssociateBy = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == siblingAddUpdateForStudentModel.StudentId && x.SchoolId == siblingAddUpdateForStudentModel.SchoolId && x.TenantId == siblingAddUpdateForStudentModel.TenantId);

                    if (studentAssociateTo != null && studentAssociateBy != null)
                    {
                        if (studentAssociateTo.Associationship != null)
                        {
                            studentAssociateTo.Associationship = studentAssociateTo.Associationship + " | " + siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.SchoolId + "#" + siblingAddUpdateForStudentModel.StudentId;
                        }
                        else
                        {
                            studentAssociateTo.Associationship = siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.SchoolId + "#" + siblingAddUpdateForStudentModel.StudentId;
                        }

                        if (studentAssociateBy.Associationship != null)
                        {
                            studentAssociateBy.Associationship = studentAssociateBy.Associationship + " | " + siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.studentMaster.SchoolId + "#" + siblingAddUpdateForStudentModel.studentMaster.StudentId;
                        }
                        else
                        {
                            studentAssociateBy.Associationship = siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.studentMaster.SchoolId + "#" + siblingAddUpdateForStudentModel.studentMaster.StudentId;
                        }

                        this.context?.SaveChanges();
                        siblingAddUpdateForStudentModel._failure = false;
                        siblingAddUpdateForStudentModel._message = "Sibling Added Successfully";
                    }
                    else
                    {
                        siblingAddUpdateForStudentModel._failure = true;
                        siblingAddUpdateForStudentModel._message = "Please Select a Valid Student";
                    }
                }
            }
            catch (Exception es)
            {
                siblingAddUpdateForStudentModel._message = es.Message;
                siblingAddUpdateForStudentModel._failure = true;
                siblingAddUpdateForStudentModel._tenantName = siblingAddUpdateForStudentModel._tenantName;
                siblingAddUpdateForStudentModel._token = siblingAddUpdateForStudentModel._token;
            }
            return siblingAddUpdateForStudentModel;
        }

        /// <summary>
        /// View All Sibling
        /// </summary>
        /// <param name="studentListModel"></param>
        /// <returns></returns>
        public StudentListModel ViewAllSibling(StudentListModel studentListModel)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                var Associationship = studentListModel.TenantId + "#" + studentListModel.SchoolId + "#" + studentListModel.StudentId;
                var studentAssociationship = this.context?.StudentMaster.Where(x => x.Associationship.Contains(Associationship)).Include(x=>x.SchoolMaster).Include(x=>x.StudentEnrollment).ToList();
                foreach(var studentData in studentAssociationship)
                {
                    studentData.StudentEnrollment = studentData.StudentEnrollment.Where(x => x.IsActive == true && x.SchoolId == studentData.SchoolId && x.TenantId == studentData.TenantId && x.StudentId == studentData.StudentId).ToList();
                }
                if (studentAssociationship.Count > 0)
                {
                    studentList.studentMaster = studentAssociationship;
                    studentList._tenantName = studentListModel._tenantName;
                    studentList._token = studentListModel._token;
                    studentList._failure = false;
                }
                else
                {
                    studentList._failure = true;
                    studentList._message = NORECORDFOUND;                    
                }
            }
            catch (Exception es)
            {
                studentList.studentMaster = null;
                studentList._message = es.Message;
                studentList._failure = true;
                studentList._tenantName = studentListModel._tenantName;
                studentList._token = studentListModel._token;
            }
            return studentList;
        }

        /// <summary>
        /// Remove Sibling
        /// </summary>
        /// <param name="siblingAddUpdateForStudentModel"></param>
        /// <returns></returns>
        public SiblingAddUpdateForStudentModel RemoveSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            try
            {
                string StudentAssociateToAfterDel;
                string StudentAssociateByAfterDel;
                var StudentAssociateTo = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == siblingAddUpdateForStudentModel.studentMaster.StudentId && x.SchoolId == siblingAddUpdateForStudentModel.studentMaster.SchoolId);
                var StudentAssociateBy = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == siblingAddUpdateForStudentModel.StudentId && x.SchoolId == siblingAddUpdateForStudentModel.SchoolId);
                var StudentAssociateToDataDel = siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.studentMaster.SchoolId + "#" + siblingAddUpdateForStudentModel.studentMaster.StudentId;
                var StudentAssociateByDataDel = siblingAddUpdateForStudentModel.studentMaster.TenantId + "#" + siblingAddUpdateForStudentModel.SchoolId + "#" + siblingAddUpdateForStudentModel.StudentId;

                if (StudentAssociateTo != null && StudentAssociateTo.Associationship != null)
                {
                    var AssociationshipToData = StudentAssociateTo.Associationship;

                    string[] StudentAssociateToWithSiblings = AssociationshipToData.Split(" | ", StringSplitOptions.RemoveEmptyEntries);

                    StudentAssociateToWithSiblings = StudentAssociateToWithSiblings.Where(w => w != StudentAssociateByDataDel).ToArray();

                    if (StudentAssociateToWithSiblings.Count() > 1)
                    {
                        StudentAssociateToAfterDel = string.Join(" | ", StudentAssociateToWithSiblings);
                    }
                    else if (StudentAssociateToWithSiblings.Count() == 1)
                    {
                        StudentAssociateToAfterDel = string.Concat(StudentAssociateToWithSiblings);
                    }
                    else
                    {
                        StudentAssociateToAfterDel = null;
                    }
                    StudentAssociateTo.Associationship = StudentAssociateToAfterDel;
                }

                if (StudentAssociateBy != null && StudentAssociateBy.Associationship != null)
                {
                    var AssociationshipByData = StudentAssociateBy.Associationship;

                    string[] StudentAssociateByWithSiblings = AssociationshipByData.Split(" | ", StringSplitOptions.RemoveEmptyEntries);

                    StudentAssociateByWithSiblings = StudentAssociateByWithSiblings.Where(w => w != StudentAssociateToDataDel).ToArray();

                    if (StudentAssociateByWithSiblings.Count() > 1)
                    {
                        StudentAssociateByAfterDel = string.Join(" | ", StudentAssociateByWithSiblings);
                    }
                    else if (StudentAssociateByWithSiblings.Count() == 1)
                    {
                        StudentAssociateByAfterDel = string.Concat(StudentAssociateByWithSiblings);
                    }
                    else
                    {
                        StudentAssociateByAfterDel = null;
                    }
                    StudentAssociateBy.Associationship = StudentAssociateByAfterDel;
                    
                }
                this.context?.SaveChanges();
                siblingAddUpdateForStudentModel._message = "Sibling Deleted Successfully";
            }

            catch (Exception es)
            {
                siblingAddUpdateForStudentModel._failure = true;
                siblingAddUpdateForStudentModel._message = es.Message;
            }
            return siblingAddUpdateForStudentModel;
        }

        /// <summary>
        ///  Check Student InternalId Exist or Not
        /// </summary>
        /// <param name="checkStudentInternalIdViewModel"></param>
        /// <returns></returns>
        public CheckStudentInternalIdViewModel CheckStudentInternalId(CheckStudentInternalIdViewModel checkStudentInternalIdViewModel)
        {
            var checkInternalId = this.context?.StudentMaster.Where(x => x.TenantId == checkStudentInternalIdViewModel.TenantId && x.SchoolId == checkStudentInternalIdViewModel.SchoolId && x.StudentInternalId == checkStudentInternalIdViewModel.StudentInternalId).ToList();
            if(checkInternalId.Count()>0)
            {
                checkStudentInternalIdViewModel.IsValidInternalId = false;
                checkStudentInternalIdViewModel._message = "Student Internal Id Already Exist";
            }
            else
            {
                checkStudentInternalIdViewModel.IsValidInternalId = true;
                checkStudentInternalIdViewModel._message = "Student Internal Id Is Valid";
            }
            return checkStudentInternalIdViewModel;
        }

        /// <summary>
        /// Add or Update Student Photo
        /// </summary>
        /// <param name="studentAddViewModel"></param>
        /// <returns></returns>
        public StudentAddViewModel AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel)
        {
            try
            {
                var studentUpdate = this.context?.StudentMaster.FirstOrDefault(x => x.TenantId == studentAddViewModel.studentMaster.TenantId && x.SchoolId == studentAddViewModel.studentMaster.SchoolId && x.StudentId == studentAddViewModel.studentMaster.StudentId);
                if (studentUpdate != null)
                {
                    studentUpdate.StudentPhoto = studentAddViewModel.studentMaster.StudentPhoto;
                    this.context?.SaveChanges();
                    studentAddViewModel._message = "Student Photo Updated Successfully";
                }
                else
                {
                    studentAddViewModel._failure = true;
                    studentAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentAddViewModel._failure = true;
                studentAddViewModel._message = es.Message;
            }
            return studentAddViewModel;
        }

        //public SearchStudentViewModel SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel)
        //{
        //    SearchStudentViewModel searchStudentView = new SearchStudentViewModel();
        //    IQueryable<StudentMaster> transactionIQ = null;
        //    try
        //    {
        //        var studentDataList = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Where(x => x.TenantId == searchStudentViewModel.TenantId && x.SchoolId == searchStudentViewModel.SchoolId && (searchStudentViewModel.StudentId == null || (x.StudentId == searchStudentViewModel.StudentId)) && (searchStudentViewModel.AlternateId == null || (x.AlternateId == searchStudentViewModel.AlternateId)) && (searchStudentViewModel.SectionId == null || (x.SectionId == searchStudentViewModel.SectionId)) && (searchStudentViewModel.FirstLanguageId == null || (x.FirstLanguageId == searchStudentViewModel.FirstLanguageId))).Select(e => new StudentMaster
        //        {
        //            TenantId = e.TenantId,
        //            SchoolId = e.SchoolId,
        //            StudentId = e.StudentId,
        //            FirstGivenName = e.FirstGivenName,
        //            MiddleName = e.MiddleName,
        //            LastFamilyName = e.LastFamilyName,
        //            AlternateId = e.AlternateId,
        //            SectionId = e.SectionId,
        //            FirstLanguage = e.FirstLanguage,

        //            StudentEnrollment = e.StudentEnrollment.Where(d => d.IsActive == true).Select(s => new StudentEnrollment
        //            {
        //                GradeId = s.GradeId,

        //            }).ToList()
        //        });



        //        if (searchStudentViewModel.GradeId!=null)
        //        {
        //            var filterGradeLevelTitle = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Where(x => x.StudentEnrollment.FirstOrDefault().GradeId == searchStudentViewModel.GradeId).AsQueryable();
        //            if (filterGradeLevelTitle.ToList().Count() > 0)
        //            {
        //                transactionIQ = filterGradeLevelTitle;
        //            }
        //        }

        //        if (searchStudentViewModel.StudentName != null)
        //        {
        //            var name = searchStudentViewModel.StudentName.Split(" ");
        //            var firstName = name.First();
        //            var lastName = name.Last();
        //            searchStudentViewModel.StudentName = null;
        //            if (searchStudentViewModel.StudentName == null)
        //            {
        //                var a = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Where(x => x.TenantId == searchStudentViewModel.TenantId && x.SchoolId == searchStudentViewModel.SchoolId && x.FirstGivenName.StartsWith(firstName.ToString()) && x.LastFamilyName.StartsWith(lastName.ToString()));
        //                transactionIQ = transactionIQ.Concat(a).Distinct();

        //            }
        //            else
        //            {
        //                var b = transactionIQ.Where(x => x.TenantId == searchStudentViewModel.TenantId && x.SchoolId == searchStudentViewModel.SchoolId && x.FirstGivenName.StartsWith(searchStudentViewModel.StudentName) || x.LastFamilyName.StartsWith(searchStudentViewModel.StudentName));
        //                transactionIQ = transactionIQ.Concat(b).Distinct();

        //            }
        //        }

        //        int totalCount = transactionIQ.Count();

        //        var xyz = transactionIQ.Select(e => new SearchStudentForView
        //        {
        //            TenantId = e.TenantId,
        //            SchoolId = e.SchoolId,
        //            StudentId = e.StudentId,
        //            StudentName = e.FirstGivenName +" " + e.LastFamilyName,
        //            AlternateId = e.AlternateId,
        //            GradeId = e.StudentEnrollment.FirstOrDefault().GradeId,
        //            SectionId = e.SectionId,
        //            FirstLanguageId = e.FirstLanguageId,
        //        }).Skip((searchStudentViewModel.PageNumber - 1) * searchStudentViewModel.PageSize).Take(searchStudentViewModel.PageSize);

        //        searchStudentView.searchStudentForView = xyz.ToList();


        //    }
        //    catch (Exception es)
        //    {
        //        searchStudentView._failure = true;
        //        searchStudentView._message = es.Message;
        //    }
        //    return searchStudentView;
        //}

        /// <summary>
        /// Search Student List For Reenroll
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StudentListModel SearchStudentListForReenroll(PageResult pageResult)
        {            
            StudentListModel studentListModel = new StudentListModel();
            List<StudentMaster> Student = new List<StudentMaster>();
            IQueryable<StudentMaster> transactionIQ = null;
            try
            {
                     var studentDataList = this.context?.StudentMaster.Include(x => x.StudentEnrollment).Where(x=> (pageResult.SchoolId >0) ? x.SchoolId== pageResult.SchoolId && x.TenantId==pageResult.TenantId && x.IsActive==false : x.TenantId == pageResult.TenantId && x.IsActive == false).AsNoTracking().Select(e => new StudentMaster
                    {
                        TenantId = e.TenantId,
                        SchoolId = e.SchoolId,
                        StudentId = e.StudentId,
                        FirstGivenName = e.FirstGivenName,
                        MiddleName = e.MiddleName,
                        LastFamilyName = e.LastFamilyName,
                        AlternateId = e.AlternateId,
                        StudentInternalId = e.StudentInternalId,
                        MobilePhone = e.MobilePhone,
                        HomePhone = e.HomePhone,
                        PersonalEmail = e.PersonalEmail,
                        SchoolEmail = e.SchoolEmail,
                        StudentGuid = e.StudentGuid,
                        AdmissionNumber = e.AdmissionNumber,
                        RollNumber = e.RollNumber,
                        Dob = e.Dob,
                        Gender = e.Gender,
                        Race = e.Race,
                        Ethnicity = e.Ethnicity,
                        MaritalStatus = e.MaritalStatus,
                        CountryOfBirth = e.CountryOfBirth,
                        Nationality = e.Nationality,
                        FirstLanguage = e.FirstLanguage,
                        SecondLanguage = e.SecondLanguage,
                        ThirdLanguage = e.ThirdLanguage,
                        HomeAddressLineOne = e.HomeAddressLineOne,
                        HomeAddressLineTwo = e.HomeAddressLineTwo,
                        HomeAddressCity = e.HomeAddressCity,
                        HomeAddressCountry = e.HomeAddressCountry,
                        HomeAddressState = e.HomeAddressState,
                        HomeAddressZip = e.HomeAddressZip,
                        BusNo = e.BusNo,
                        FirstLanguageId = e.FirstLanguageId,
                        SectionId = e.SectionId,
                        StudentEnrollment = e.StudentEnrollment.Where(d => d.IsActive == false).OrderByDescending(a => a.EnrollmentDate).Select(s => new StudentEnrollment
                        {
                            EnrollmentDate = s.EnrollmentDate,
                            GradeLevelTitle = s.GradeLevelTitle,
                            TenantId = s.TenantId,
                            SchoolId = s.SchoolId,
                            StudentId = s.StudentId,
                            EnrollmentId = s.EnrollmentId,
                            StudentGuid = s.StudentGuid,
                            GradeId = s.GradeId,
                            ExitDate = s.ExitDate,
                            ExitCode = s.ExitCode
                        }).ToList()
                    }).ToList();                
                
                if (studentDataList.Count>0)
                {                    
                    Guid studentGuidData = new Guid();
                    foreach (var studentData in studentDataList)
                    {
                        if (studentData.StudentGuid!= studentGuidData)
                        {
                            var checkEnrolledStudent = this.context?.StudentMaster.FirstOrDefault(c => c.StudentGuid == studentData.StudentGuid && c.IsActive == true);

                            if (checkEnrolledStudent==null)
                            {
                                Student.Add(studentData);
                                studentGuidData = studentData.StudentGuid;
                            }
                            else
                            {
                                studentGuidData = studentData.StudentGuid;
                            }                            
                        }                        
                    }
                }

                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = Student.AsQueryable();
                }
                else
                {
                    string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        transactionIQ = Student.AsQueryable().Where(x => x.FirstGivenName != null && x.FirstGivenName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.MiddleName != null && x.MiddleName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.LastFamilyName != null && x.LastFamilyName.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.StudentInternalId != null && x.StudentInternalId.ToLower().Contains(Columnvalue.ToLower()) ||
                                                                    x.MobilePhone != null && x.MobilePhone.Contains(Columnvalue) ||
                                                                    x.PersonalEmail != null && x.PersonalEmail.Contains(Columnvalue));

                        //for StudentEnrollment Searching

                        var studentEnrollmentFilter = Student.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.ToList().Count > 0 ? x.StudentEnrollment.FirstOrDefault().GradeLevelTitle.ToLower().Contains(Columnvalue.ToLower()) : string.Empty.Contains(Columnvalue)).AsQueryable();

                        if (studentEnrollmentFilter.ToList().Count > 0)
                        {
                            transactionIQ = transactionIQ.AsNoTracking().ToList().Concat(studentEnrollmentFilter).AsQueryable();
                            //transactionIQ = gradeLevelFilter;
                        }
                    }
                    else
                    {
                        if (pageResult.FilterParams.Any(x => x.ColumnName.ToLower() == "enrollmentdate" || x.ColumnName.ToLower() == "exitdate" || x.ColumnName.ToLower() == "exitcode"))
                        {
                            
                            var enrollmentData = Student.AsQueryable();
                            foreach (var filterParam in pageResult.FilterParams)
                            {
                                if (filterParam.ColumnName.ToLower() == "enrollmentdate" || filterParam.ColumnName.ToLower() == "exitdate" || filterParam.ColumnName.ToLower() == "exitcode")
                                {
                                    var columnName = filterParam.ColumnName;
                                    var filterValue = filterParam.FilterValue;

                                    if (filterValue != null)
                                    {
                                        if (columnName.ToLower() == "enrollmentdate")
                                        {
                                            enrollmentData = enrollmentData.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().EnrollmentDate == Convert.ToDateTime(filterValue)).AsQueryable();
                                        }
                                        if (columnName.ToLower() == "exitdate")
                                        {
                                            enrollmentData = enrollmentData.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().ExitDate == Convert.ToDateTime(filterValue)).AsQueryable();
                                        }
                                        if (columnName.ToLower() == "exitcode")
                                        {
                                            enrollmentData = enrollmentData.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().ExitCode.ToLower() == filterValue.ToString().ToLower()).AsQueryable();
                                        }
                                    }
                                }
                            }
                            
                            pageResult.FilterParams.RemoveAll(x => x.ColumnName.ToLower() == "enrollmentdate" || x.ColumnName.ToLower() == "exitdate" || x.ColumnName.ToLower() == "exitcode");

                            if (pageResult.FilterParams.Count > 0)
                            {
                                transactionIQ = Utility.FilteredData(pageResult.FilterParams, enrollmentData).AsQueryable();
                            }
                            else
                            {
                                transactionIQ = enrollmentData;
                            }
                        }

                        //    if (pageResult.FilterParams.Any(x => x.ColumnName.ToLower() == "enrollmentdate"))
                        //    {
                        //        var filterValue = Convert.ToDateTime(pageResult.FilterParams.Where(x => x.ColumnName.ToLower() == "enrollmentdate").Select(x => x.FilterValue).FirstOrDefault());

                        //        var studentEnrollmentData = Student.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().EnrollmentDate == filterValue).AsQueryable();
                        //        var indexValue = pageResult.FilterParams.FindIndex(x => x.ColumnName.ToLower() == "enrollmentdate");
                        //        pageResult.FilterParams.RemoveAt(indexValue);

                        //        if (studentEnrollmentData.ToList().Count() > 0)
                        //        {
                        //            transactionIQ = studentEnrollmentData.AsNoTracking().ToList().AsQueryable();

                        //        }
                        //    }
                        //    if (pageResult.FilterParams.Any(x => x.ColumnName.ToLower() == "exitdate"))
                        //    {
                        //        var filterValue = Convert.ToDateTime(pageResult.FilterParams.Where(x => x.ColumnName.ToLower() == "exitdate").Select(x => x.FilterValue).FirstOrDefault());

                        //        var studentExitDate = Student.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().ExitDate == filterValue).AsQueryable();
                        //        var indexValue = pageResult.FilterParams.FindIndex(x => x.ColumnName.ToLower() == "exitdate");
                        //        pageResult.FilterParams.RemoveAt(indexValue);

                        //        if (studentExitDate.ToList().Count() > 0)
                        //        {
                        //            transactionIQ = studentExitDate.AsNoTracking().ToList().AsQueryable();

                        //        }
                        //    }
                        //    if (pageResult.FilterParams.Any(x => x.ColumnName.ToLower() == "exitcode"))
                        //    {
                        //        var filterValue = pageResult.FilterParams.Where(x => x.ColumnName.ToLower() == "exitcode").Select(x => x.FilterValue).FirstOrDefault();

                        //        var studentEnrollmentCode = Student.AsQueryable().AsNoTracking().ToList().Where(x => x.StudentEnrollment.FirstOrDefault().ExitCode.ToLower() == filterValue.ToLower()).AsQueryable();
                        //        var indexValue = pageResult.FilterParams.FindIndex(x => x.ColumnName.ToLower() == "exitcode");
                        //        pageResult.FilterParams.RemoveAt(indexValue);

                        //        if (studentEnrollmentCode.ToList().Count() > 0)
                        //        {
                        //            transactionIQ = studentEnrollmentCode.AsNoTracking().ToList().AsQueryable();
                        //        }
                        //    }
                        //    if (transactionIQ != null && pageResult.FilterParams.Count() > 0)
                        //    {
                        //        transactionIQ = Utility.FilteredData(pageResult.FilterParams, transactionIQ).AsQueryable();
                        //    }
                        //}
                        else
                        {
                            transactionIQ = Utility.FilteredData(pageResult.FilterParams, Student).AsQueryable();
                        }
                    }

                }                
                if (pageResult.SortingModel != null)
                {
                    switch (pageResult.SortingModel.SortColumn.ToLower())
                    {
                        //For GradeLevel Sorting
                        case "gradeleveltitle":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderBy(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().GradeLevelTitle : null).AsQueryable();
                            }
                            else
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderByDescending(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().GradeLevelTitle : null).AsQueryable();
                            }
                            break;

                        //For Student Enrollment Date Sorting
                        case "enrollmentdate":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderBy(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().EnrollmentDate : null).AsQueryable();
                            }
                            else
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderByDescending(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().EnrollmentDate : null).AsQueryable();
                            }
                            break;

                        //For Student Enrollment Exit Date Sorting
                        case "exitdate":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderBy(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().ExitDate : null).AsQueryable();
                            }
                            else
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderByDescending(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().ExitDate : null).AsQueryable();
                            }
                            break;

                        //For Student Enrollment Exit Code Sorting
                        case "exitcode":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderBy(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().ExitCode : null).AsQueryable();
                            }
                            else
                            {

                                transactionIQ = transactionIQ.AsNoTracking().ToList().OrderByDescending(a => a.StudentEnrollment.Count > 0 ? a.StudentEnrollment.FirstOrDefault().ExitCode : null).AsQueryable();
                            }
                            break;

                        default:
                            transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                            break;
                    }

                }
                
                if (transactionIQ != null)
                {
                    //transactionIQ = transactionIQ.Distinct();
                    int totalCount = transactionIQ.AsNoTracking().ToList().Count();
                    if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                    {
                        transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                    }
                    studentListModel.studentMaster = transactionIQ.ToList();
                    studentListModel.TotalCount = totalCount;
                }
                else
                {
                    //studentListModel.studentMaster = null;
                    studentListModel.TotalCount = 0;
                }


                studentListModel.TenantId = pageResult.TenantId;
                studentListModel.SchoolId = pageResult.SchoolId;               
                studentListModel.PageNumber = pageResult.PageNumber;
                studentListModel._pageSize = pageResult.PageSize;
                studentListModel._tenantName = pageResult._tenantName;
                studentListModel._token = pageResult._token;
                studentListModel._failure = false;
            }
            catch (Exception es)
            {
                studentListModel._message = es.Message;
                studentListModel._failure = true;
                studentListModel._tenantName = pageResult._tenantName;
                studentListModel._token = pageResult._token;
            }
            return studentListModel;
        }
        /// <summary>
        /// Re enrollment For Student
        /// </summary>
        /// <param name="studentListModel"></param>
        /// <returns></returns>
        public StudentListModel ReenrollmentForStudent(StudentListModel studentListModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    if (studentListModel.studentMaster.Count > 0)
                    {
                        int? calenderId = null;
                        var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == studentListModel.TenantId && x.SchoolId == studentListModel.SchoolId && x.AcademicYear.ToString() == studentListModel.AcademicYear && x.DefaultCalender == true);

                        if (defaultCalender != null)
                        {
                            calenderId = defaultCalender.CalenderId;
                        }

                        var enrollmenttitle = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == studentListModel.TenantId && x.SchoolId == studentListModel.SchoolId && x.EnrollmentCode == studentListModel.EnrollmentCode)?.Title;

                        foreach (var studentData in studentListModel.studentMaster)
                        {                           
                            var activeEnrollment = this.context?.StudentEnrollment.Where(x => x.StudentGuid == studentData.StudentGuid && x.IsActive == true).FirstOrDefault();

                            this.context?.StudentEnrollment.Where(x => x.StudentGuid == studentData.StudentGuid && x.IsActive == true).ToList().ForEach(x => { x.IsActive = false; x.ExitCode = activeEnrollment.EnrollmentCode; x.ExitDate = studentListModel.EnrollmentDate; });
                            
                            int? EnrollmentId = 1;
                            //EnrollmentId = Utility.GetMaxPK(this.context, new Func<StudentEnrollment, int>(x => x.EnrollmentId));

                            var studentEnrollmentData = this.context?.StudentEnrollment.Where(x => x.StudentGuid == studentData.StudentGuid).OrderByDescending(x => x.EnrollmentId).FirstOrDefault();

                            if (studentEnrollmentData != null)
                            {
                                EnrollmentId = studentEnrollmentData.EnrollmentId + 1;
                            }

                            var existingStudentData = this.context?.StudentMaster.FirstOrDefault(s => s.SchoolId == studentListModel.SchoolId && s.TenantId == studentListModel.TenantId && s.StudentGuid == studentData.StudentGuid);

                            if (existingStudentData != null)
                            {
                                existingStudentData.IsActive = true;                                
                                
                                var StudentEnrollmentData = new StudentEnrollment()
                                {
                                    TenantId = studentData.TenantId,
                                    SchoolId = (int)studentListModel.SchoolId,
                                    StudentId = existingStudentData.StudentId,
                                    EnrollmentId = (int)EnrollmentId,
                                    EnrollmentCode = enrollmenttitle,
                                    EnrollmentDate = studentListModel.EnrollmentDate,
                                    GradeLevelTitle = studentListModel.GradeLevelTitle,
                                    GradeId = studentListModel.GradeId,
                                    LastUpdated = DateTime.UtcNow,
                                    RollingOption = "Next Grade at Current School",
                                    StudentGuid = studentData.StudentGuid,
                                    IsActive = true,
                                    SchoolName = this.context?.SchoolMaster.FirstOrDefault(x => x.SchoolId == studentListModel.SchoolId)?.SchoolName,
                                    UpdatedBy=studentListModel.UpdatedBy,
                                    CalenderId= calenderId
                                };
                                this.context?.StudentEnrollment.Add(StudentEnrollmentData);
                            }
                            else
                            {

                                var studentInfo = this.context?.StudentMaster.FirstOrDefault(x => x.StudentGuid == studentData.StudentGuid);

                                int? MasterStudentId = 0;

                                var studentId = this.context?.StudentMaster.Where(x => x.SchoolId == studentListModel.SchoolId && x.TenantId == studentListModel.TenantId).OrderByDescending(x => x.StudentId).FirstOrDefault();

                                if (studentId != null)
                                {
                                    MasterStudentId = studentId.StudentId + 1;
                                }
                                else
                                {
                                    MasterStudentId = 1;
                                }

                                var StudentMasterData = new StudentMaster()
                                {
                                    TenantId = studentInfo.TenantId,
                                    SchoolId = (int)studentListModel.SchoolId,
                                    StudentId = (int)MasterStudentId,
                                    AlternateId = studentInfo.AlternateId,
                                    DistrictId = studentInfo.DistrictId,
                                    StateId = studentInfo.StateId,
                                    AdmissionNumber = studentInfo.AdmissionNumber,
                                    RollNumber = studentInfo.RollNumber,
                                    Salutation = studentInfo.Salutation,
                                    FirstGivenName = studentInfo.FirstGivenName,
                                    MiddleName = studentInfo.MiddleName,
                                    LastFamilyName = studentInfo.LastFamilyName,
                                    Suffix = studentInfo.Suffix,
                                    PreferredName = studentInfo.PreferredName,
                                    PreviousName = studentInfo.PreviousName,
                                    SocialSecurityNumber = studentInfo.SocialSecurityNumber,
                                    OtherGovtIssuedNumber = studentInfo.OtherGovtIssuedNumber,
                                    StudentPhoto = studentInfo.StudentPhoto,
                                    Dob = studentInfo.Dob,
                                    Gender = studentInfo.Gender,
                                    Race = studentInfo.Race,
                                    Ethnicity = studentInfo.Ethnicity,
                                    MaritalStatus = studentInfo.MaritalStatus,
                                    CountryOfBirth = studentInfo.CountryOfBirth,
                                    Nationality = studentInfo.Nationality,
                                    FirstLanguageId = studentInfo.FirstLanguageId,
                                    SecondLanguageId = studentInfo.SecondLanguageId,
                                    ThirdLanguageId = studentInfo.ThirdLanguageId,
                                    HomePhone = studentInfo.HomePhone,
                                    MobilePhone = studentInfo.MobilePhone,
                                    PersonalEmail = studentInfo.PersonalEmail,
                                    SchoolEmail = studentInfo.SchoolEmail,
                                    Twitter = studentInfo.Twitter,
                                    Facebook = studentInfo.Facebook,
                                    Instagram = studentInfo.Instagram,
                                    Youtube = studentInfo.Youtube,
                                    Linkedin = studentInfo.Linkedin,
                                    HomeAddressLineOne = studentInfo.HomeAddressLineOne,
                                    HomeAddressLineTwo = studentInfo.HomeAddressLineTwo,
                                    HomeAddressCountry = studentInfo.HomeAddressCountry,
                                    HomeAddressState = studentInfo.HomeAddressState,
                                    HomeAddressCity = studentInfo.HomeAddressCity,
                                    HomeAddressZip = studentInfo.HomeAddressZip,
                                    BusNo = studentInfo.BusNo,
                                    SchoolBusPickUp = studentInfo.SchoolBusPickUp,
                                    SchoolBusDropOff = studentInfo.SchoolBusDropOff,
                                    MailingAddressSameToHome = studentInfo.MailingAddressSameToHome,
                                    MailingAddressLineOne = studentInfo.MailingAddressLineOne,
                                    MailingAddressLineTwo = studentInfo.MailingAddressLineTwo,
                                    MailingAddressCountry = studentInfo.MailingAddressCountry,
                                    MailingAddressState = studentInfo.MailingAddressState,
                                    MailingAddressCity = studentInfo.MailingAddressCity,
                                    MailingAddressZip = studentInfo.MailingAddressZip,
                                    StudentPortalId = studentInfo.StudentPortalId,
                                    AlertDescription = studentInfo.AlertDescription,
                                    CriticalAlert = studentInfo.CriticalAlert,
                                    Dentist = studentInfo.Dentist,
                                    DentistPhone = studentInfo.DentistPhone,
                                    InsuranceCompany = studentInfo.InsuranceCompany,
                                    InsuranceCompanyPhone = studentInfo.InsuranceCompanyPhone,
                                    MedicalFacility = studentInfo.MedicalFacility,
                                    MedicalFacilityPhone = studentInfo.MedicalFacilityPhone,
                                    PolicyHolder = studentInfo.PolicyHolder,
                                    PolicyNumber = studentInfo.PolicyNumber,
                                    PrimaryCarePhysician = studentInfo.PrimaryCarePhysician,
                                    PrimaryCarePhysicianPhone = studentInfo.PrimaryCarePhysicianPhone,
                                    Vision = studentInfo.Vision,
                                    VisionPhone = studentInfo.VisionPhone,
                                    Associationship = studentInfo.Associationship,
                                    EconomicDisadvantage = studentInfo.EconomicDisadvantage,
                                    Eligibility504 = studentInfo.Eligibility504,
                                    EstimatedGradDate = studentInfo.EstimatedGradDate,
                                    FreeLunchEligibility = studentInfo.FreeLunchEligibility,
                                    LepIndicator = studentInfo.LepIndicator,
                                    SectionId = null,
                                    SpecialEducationIndicator = studentInfo.SpecialEducationIndicator,
                                    StudentInternalId = studentInfo.StudentInternalId,
                                    LastUpdated = DateTime.UtcNow,
                                    UpdatedBy = studentInfo.UpdatedBy,
                                    EnrollmentType = "Internal",
                                    IsActive = true,
                                    StudentGuid = studentData.StudentGuid

                                };
                                this.context?.StudentMaster.Add(StudentMasterData);

                                var StudentEnrollmentData = new StudentEnrollment()
                                {
                                    TenantId = studentData.TenantId,
                                    SchoolId = (int)studentListModel.SchoolId,
                                    StudentId = (int)MasterStudentId,
                                    EnrollmentId = (int)EnrollmentId,
                                    EnrollmentCode = enrollmenttitle,
                                    EnrollmentDate = studentListModel.EnrollmentDate,
                                    GradeLevelTitle = studentListModel.GradeLevelTitle,
                                    LastUpdated = DateTime.UtcNow,
                                    RollingOption = "Next Grade at Current School",
                                    StudentGuid = studentData.StudentGuid,
                                    IsActive = true,
                                    SchoolName = this.context?.SchoolMaster.FirstOrDefault(x => x.SchoolId == studentListModel.SchoolId)?.SchoolName,
                                    UpdatedBy=studentListModel.UpdatedBy,
                                    GradeId= studentListModel.GradeId,
                                    CalenderId=calenderId
                                };
                                this.context?.StudentEnrollment.Add(StudentEnrollmentData);

                                //Student Protal Access
                                if (studentData.StudentPortalId != null)
                                {
                                    var userMasterData = this.context?.UserMaster.FirstOrDefault(x => x.EmailAddress == studentData.StudentPortalId && x.TenantId == studentData.TenantId);
                                    if (userMasterData != null)
                                    {
                                        userMasterData.IsActive = false;
                                        
                                        UserMaster userMaster = new UserMaster();
                                        userMaster.TenantId = studentData.TenantId;
                                        userMaster.SchoolId = (int)studentListModel.SchoolId;
                                        userMaster.UserId = (int)MasterStudentId;
                                        userMaster.Name = userMasterData.Name;
                                        userMaster.EmailAddress = userMasterData.EmailAddress;
                                        userMaster.PasswordHash = userMasterData.PasswordHash;
                                        userMaster.LangId = userMasterData.LangId;
                                        var membershipsId = this.context?.Membership.Where(x => x.SchoolId == (int)studentListModel.SchoolId && x.TenantId == studentListModel.TenantId && x.Profile == "Student").Select(x => x.MembershipId).FirstOrDefault();
                                        userMaster.MembershipId = (int)membershipsId;
                                        userMaster.LastUpdated = DateTime.UtcNow;
                                        userMaster.UpdatedBy = studentListModel.UpdatedBy;
                                        userMaster.IsActive = true;
                                        this.context?.UserMaster.Add(userMaster);
                                    }
                                }
                            }
                            this.context?.SaveChanges();
                        }
                        //this.context?.SaveChanges();
                        transaction.Commit();
                        studentListModel._failure = false;
                        studentListModel._message = "Student Re-enrollment Added Successfully";
                    }
                    else
                    {
                        studentListModel._failure = true;
                        studentListModel._message = "Atleast Select One Student";
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentListModel._message = es.Message;
                    studentListModel._failure = true;
                }
                return studentListModel;
            }
        }

        /// <summary>
        /// Add Student List
        /// </summary>
        /// <param name="studentListAddViewModel"></param>
        /// <returns></returns>
        public StudentListAddViewModel AddStudentList(StudentListAddViewModel studentListAddViewModel)
        {
            StudentListAddViewModel studentListAdd = new StudentListAddViewModel();

            studentListAdd._tenantName = studentListAddViewModel._tenantName;
            studentListAdd._token = studentListAddViewModel._token;
            studentListAdd._userName = studentListAddViewModel._userName;

            if (studentListAddViewModel.studentAddViewModelList.Count > 0)
            {
                studentListAdd._failure = false;
                studentListAdd._message = "Student Added Successfully";
                int? MasterStudentId = 1;

                var studentData = this.context?.StudentMaster.Where(x => x.SchoolId == studentListAddViewModel.SchoolId && x.TenantId == studentListAddViewModel.TenantId).OrderByDescending(x => x.StudentId).FirstOrDefault();

                if (studentData != null)
                {
                    MasterStudentId = studentData.StudentId + 1;
                }
                int? indexNo = -1;
                foreach (var student in studentListAddViewModel.studentAddViewModelList)
                {
                    indexNo++;
                    //UserMaster userMaster = new UserMaster();
                    var StudentEnrollmentData = new StudentEnrollment();
                    using (var transaction = this.context.Database.BeginTransaction())
                    {
                        try
                        {
                            student.studentMaster.TenantId = studentListAddViewModel.TenantId;
                            student.studentMaster.SchoolId = studentListAddViewModel.SchoolId;
                            student.studentMaster.StudentId = (int)MasterStudentId;
                            Guid GuidId = Guid.NewGuid();
                            var GuidIdExist = this.context?.StudentMaster.FirstOrDefault(x => x.StudentGuid == GuidId);

                            if (GuidIdExist != null)
                            {
                                studentListAdd.ConflictIndexNo = studentListAdd.ConflictIndexNo != null ? studentListAdd.ConflictIndexNo + "," + indexNo.ToString() : indexNo.ToString();
                                studentListAdd.studentAddViewModelList.Add(student);
                                studentListAdd._failure = true;
                                studentListAdd._message = "Student Rejected Due to Data Error";
                                continue;
                            }

                            student.studentMaster.StudentGuid = GuidId;
                            student.studentMaster.IsActive = true;
                            student.studentMaster.EnrollmentType = "Internal";

                            if (!string.IsNullOrEmpty(student.studentMaster.StudentInternalId))
                            {
                                bool checkInternalID = CheckInternalID(student.studentMaster.TenantId, student.studentMaster.StudentInternalId, student.studentMaster.SchoolId);
                                if (checkInternalID == false)
                                {
                                    studentListAdd.ConflictIndexNo = studentListAdd.ConflictIndexNo != null ? studentListAdd.ConflictIndexNo + "," + indexNo.ToString() : indexNo.ToString();
                                    studentListAdd.studentAddViewModelList.Add(student);
                                    studentListAdd._failure = true;
                                    studentListAdd._message = "Student Rejected Due to Data Error";
                                    continue;
                                }
                            }
                            else
                            {
                                student.studentMaster.StudentInternalId = MasterStudentId.ToString();
                            }

                            if (student.studentMaster.FirstLanguageId != null)
                            {
                                var firstLanguageId = this.context?.Language.Where(x => x.Locale.ToLower() == student.studentMaster.FirstLanguageId.ToString().ToLower()).Select(x => x.LangId).FirstOrDefault();

                                student.studentMaster.FirstLanguageId = firstLanguageId != null ? firstLanguageId : null;
                            }

                            if (student.studentMaster.SecondLanguageId != null)
                            {
                                var secondLanguageId = this.context?.Language.Where(x => x.Locale.ToLower() == student.studentMaster.SecondLanguageId.ToString().ToLower()).Select(x => x.LangId).FirstOrDefault();

                                student.studentMaster.SecondLanguageId = secondLanguageId != null ? secondLanguageId : null;
                            }

                            if (student.studentMaster.ThirdLanguageId != null)
                            {
                                var thirdLanguageId = this.context?.Language.Where(x => x.Locale.ToLower() == student.studentMaster.ThirdLanguageId.ToString().ToLower()).Select(x => x.LangId).FirstOrDefault();

                                    student.studentMaster.ThirdLanguageId = thirdLanguageId != null ? thirdLanguageId : null;
                            }

                            if (student.studentMaster.CountryOfBirth != null)
                            {
                                var countryOfBirthId = this.context?.Country.Where(x => x.Name.ToLower() == student.studentMaster.CountryOfBirth.ToString().ToLower()).Select(x => x.Id).FirstOrDefault();

                                student.studentMaster.CountryOfBirth = countryOfBirthId != null ? countryOfBirthId : null;
                            }

                            if (student.studentMaster.Nationality != null)
                            {
                                var nationalityId = this.context?.Country.Where(x => x.Name.ToLower() == student.studentMaster.Nationality.ToString().ToLower()).Select(x => x.Id).FirstOrDefault();

                                student.studentMaster.Nationality = nationalityId != null ? nationalityId : null;
                            }

                            if (student.studentMaster.SectionId != null)
                            {
                                var sectionId = this.context?.Sections.Where(x => x.Name.ToLower() == student.studentMaster.SectionId.ToString().ToLower()).Select(x => x.SectionId).FirstOrDefault();
 
                                student.studentMaster.SectionId = sectionId != null ? sectionId : null;
                            }


                            var schoolName = this.context?.SchoolMaster.Where(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId).Select(s => s.SchoolName).FirstOrDefault();

                            //Insert data into Enrollment table
                            int? calenderId = null;
                            string enrollmentCode = null;

                            var defaultCalender = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.AcademicYear.ToString() == student.AcademicYear && x.DefaultCalender == true);

                            if (defaultCalender != null)
                            {
                                calenderId = defaultCalender.CalenderId;
                            }

                            var enrollmentType = this.context?.StudentEnrollmentCode.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Type.ToLower() == "Add".ToLower());

                            if (enrollmentType != null)
                            {
                                enrollmentCode = enrollmentType.Title;
                            }
                            if(student.CurrentGradeLevel!=null)
                            {
                                var gradeLevelData = this.context?.Gradelevels.FirstOrDefault(x => x.SchoolId == student.studentMaster.SchoolId && x.TenantId == student.studentMaster.TenantId && x.Title.ToLower() == student.CurrentGradeLevel.ToLower());

                                if(gradeLevelData!=null)
                                {
                                    StudentEnrollmentData = new StudentEnrollment() { TenantId = student.studentMaster.TenantId, SchoolId = student.studentMaster.SchoolId, StudentId = student.studentMaster.StudentId, EnrollmentId = 1, SchoolName = schoolName, RollingOption = "Next grade at current school", EnrollmentCode = enrollmentCode, CalenderId = calenderId, GradeLevelTitle =student.CurrentGradeLevel, EnrollmentDate = DateTime.UtcNow, StudentGuid = GuidId, IsActive = true, GradeId = gradeLevelData.GradeId };
                                }
                                else
                                {
                                    studentListAdd.ConflictIndexNo = studentListAdd.ConflictIndexNo != null ? studentListAdd.ConflictIndexNo + "," + indexNo.ToString() : indexNo.ToString();
                                    studentListAdd.studentAddViewModelList.Add(student);
                                    studentListAdd._failure = true;
                                    studentListAdd._message = "Student Rejected Due to Data Error";
                                    continue;
                                }
                            }
                            else
                            {
                                var gradeLevel = this.context?.Gradelevels.Where(x => x.SchoolId == student.studentMaster.SchoolId).OrderBy(x => x.GradeId).FirstOrDefault();


                                int? gradeId = null;
                                if (gradeLevel != null)
                                {
                                    gradeId = gradeLevel.GradeId;
                                }

                                StudentEnrollmentData = new StudentEnrollment() { TenantId = student.studentMaster.TenantId, SchoolId = student.studentMaster.SchoolId, StudentId = student.studentMaster.StudentId, EnrollmentId = 1, SchoolName = schoolName, RollingOption = "Next grade at current school", EnrollmentCode = enrollmentCode, CalenderId = calenderId, GradeLevelTitle = (gradeLevel != null) ? gradeLevel.Title : null, EnrollmentDate = DateTime.UtcNow, StudentGuid = GuidId, IsActive = true, GradeId = gradeId };
                            }
                         

                            //Add student portal access
                            if (!string.IsNullOrWhiteSpace(student.PasswordHash) && !string.IsNullOrWhiteSpace(student.LoginEmail))
                            {
                                UserMaster userMaster = new UserMaster();

                                var decrypted = Utility.Decrypt(student.PasswordHash);
                                string passwordHash = Utility.GetHashedPassword(decrypted);

                                var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.EmailAddress == student.LoginEmail);

                                if (loginInfo == null)
                                {
                                    var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == student.studentMaster.TenantId && x.SchoolId == student.studentMaster.SchoolId && x.Profile == "Student");

                                    userMaster.SchoolId = student.studentMaster.SchoolId;
                                    userMaster.TenantId = student.studentMaster.TenantId;
                                    userMaster.UserId = student.studentMaster.StudentId;
                                    userMaster.LangId = 1;
                                    userMaster.MembershipId = membership.MembershipId;
                                    userMaster.EmailAddress = student.LoginEmail;
                                    userMaster.PasswordHash = passwordHash;
                                    userMaster.Name = student.studentMaster.FirstGivenName;
                                    userMaster.LastUpdated = DateTime.UtcNow;
                                    userMaster.IsActive = student.PortalAccess;
                                    student.studentMaster.StudentPortalId = student.LoginEmail;
                                    this.context?.UserMaster.Add(userMaster);
                                    this.context?.SaveChanges();
                                }
                                else
                                {
                                         studentListAdd.ConflictIndexNo = studentListAdd.ConflictIndexNo != null ? studentListAdd.ConflictIndexNo + "," + indexNo.ToString() : indexNo.ToString();
                                    studentListAdd.studentAddViewModelList.Add(student);
                                    studentListAdd._failure = true;
                                    studentListAdd._message = "Student Rejected Due to Data Error";
                                    continue;
                                }
                            }

                            this.context?.StudentMaster.Add(student.studentMaster);
                            this.context?.StudentEnrollment.Add(StudentEnrollmentData);
                            this.context?.SaveChanges();

                            if (student.fieldsCategoryList != null && student.fieldsCategoryList.ToList().Count > 0)
                            {
                                //var fieldsCategory = student.fieldsCategoryList.FirstOrDefault(x => x.CategoryId == student.SelectedCategoryId);
                                //if (fieldsCategory != null)
                                //{
                                foreach (var fieldsCategory in student.fieldsCategoryList.ToList())
                                {
                                    foreach (var customFields in fieldsCategory.CustomFields.ToList())
                                    {
                                        if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                        {
                                            customFields.CustomFieldsValue.FirstOrDefault().Module = "Student";
                                            customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                            customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                            customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                            customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                            customFields.CustomFieldsValue.FirstOrDefault().SchoolId = student.studentMaster.SchoolId;
                                            customFields.CustomFieldsValue.FirstOrDefault().TargetId = student.studentMaster.StudentId;
                                            this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                            this.context?.SaveChanges();
                                        }

                                    }
                                }
                                //}
                            }
                            transaction.Commit();
                            MasterStudentId++;
                        }

                        catch (Exception es)
                        {
                            transaction.Rollback();
                            studentListAdd.studentAddViewModelList.Add(student);
                            studentListAdd.ConflictIndexNo = studentListAdd.ConflictIndexNo != null ? studentListAdd.ConflictIndexNo + "," + indexNo.ToString() : indexNo.ToString();
                            this.context?.StudentMaster.Remove(student.studentMaster);
                            this.context?.StudentEnrollment.Remove(StudentEnrollmentData);
                            //this.context?.UserMaster.Remove(userMaster);
                            studentListAdd._failure = true;
                            studentListAdd._message = "Student Rejected Due to Data Error";
                            continue;
                        }
                    }
                }
            }
            else
            {
                studentListAdd._failure = true;
                studentListAdd._message = "Please Import Student";
            }
            return studentListAdd;
        }

        /// <summary>
        /// View Transcript For Student
        /// </summary>
        /// <param name="transcriptViewModel"></param>
        /// <returns></returns>
        public TranscriptViewModel TranscriptViewForStudent(TranscriptViewModel transcriptViewModel)
        {
            TranscriptViewModel transcriptView = new TranscriptViewModel();
            try
            {
                transcriptView.TenantId = transcriptViewModel.TenantId;
                transcriptView.SchoolId = transcriptViewModel.SchoolId;
                transcriptView._tenantName = transcriptViewModel._tenantName;
                transcriptView._token = transcriptViewModel._token;
                transcriptView._userName = transcriptViewModel._userName;
        

                if (transcriptViewModel.StudentsDetailsForTranscriptViewModelList.Count > 0)
                {
                    var schoolData = this.context?.SchoolMaster.Include(x => x.SchoolDetail).FirstOrDefault(x => x.TenantId == transcriptViewModel.TenantId && x.SchoolId == transcriptViewModel.SchoolId);

                    if (schoolData != null)
                    {
                        transcriptView.SchoolName = schoolData.SchoolName;
                        transcriptView.StreetAddress1 = schoolData.StreetAddress1;
                        transcriptView.StreetAddress2 = schoolData.StreetAddress2;
                        transcriptView.State = schoolData.State;
                        transcriptView.City = schoolData.City;
                        transcriptView.District = schoolData.District;
                        transcriptView.Zip = schoolData.Zip;
                        transcriptView.SchoolPicture = transcriptViewModel.SchoolLogo != null ? schoolData.SchoolDetail.FirstOrDefault().SchoolLogo : null;
                    }

                    if(transcriptViewModel.GradeLagend==true)
                    {
                        var gradeDetails = this.context?.Grade.Where(x => x.TenantId == transcriptViewModel.TenantId && x.SchoolId == transcriptViewModel.SchoolId).ToList();
                        transcriptView.gradeList = gradeDetails;
                    }

                    foreach (var student in transcriptViewModel.StudentsDetailsForTranscriptViewModelList)
                    {                                            
                        decimal? totalCreditAttempeted = 0.0m;
                        decimal? totalCreditEarned = 0.0m;
                        decimal? cumulativeGPValue = 0.0m;
                        decimal? cumulativeCreditHours = 0.0m;

                        var StudentsDetailsForTranscript = new StudentsDetailsForTranscriptViewModel();

                        var studentData = this.context?.StudentMaster.Include(x => x.StudentEnrollment).FirstOrDefault(x => x.TenantId == transcriptViewModel.TenantId && x.SchoolId == transcriptViewModel.SchoolId && x.StudentId == student.StudentId);

                        if (studentData != null)
                        {
                            StudentsDetailsForTranscript.StudentGuid = studentData.StudentGuid;
                            StudentsDetailsForTranscript.StudentId = studentData.StudentId;
                            StudentsDetailsForTranscript.StudentInternalId = studentData.StudentInternalId;
                            StudentsDetailsForTranscript.FirstGivenName = studentData.FirstGivenName;
                            StudentsDetailsForTranscript.MiddleName = studentData.MiddleName;
                            StudentsDetailsForTranscript.LastFamilyName = studentData.LastFamilyName;
                            StudentsDetailsForTranscript.StudentPhoto = transcriptViewModel.StudentPhoto != null ? studentData.StudentPhoto : null;
                            StudentsDetailsForTranscript.HomeAddressLineOne = studentData.HomeAddressLineOne;
                            StudentsDetailsForTranscript.HomeAddressLineTwo = studentData.HomeAddressLineTwo;
                            StudentsDetailsForTranscript.HomeAddressState = studentData.HomeAddressState;
                            StudentsDetailsForTranscript.HomeAddressCity = studentData.HomeAddressCity;
                            StudentsDetailsForTranscript.HomeAddressCountry = studentData.HomeAddressCountry;
                            StudentsDetailsForTranscript.HomeAddressZip = studentData.HomeAddressZip;
                           
                            var gradeIds = transcriptViewModel.GradeLavels.Split(",");

                            foreach (var grade in gradeIds.ToList())
                            {
                               
                                var GradeDetailsForTranscript = new GradeDetailsForTranscriptViewModel();

                                var studentDataWithCurrentGrade = studentData.StudentEnrollment.Where(x => x.GradeId == Convert.ToInt32(grade)).FirstOrDefault();

                                if (studentDataWithCurrentGrade != null)
                                {
                                    var calenderData = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == transcriptViewModel.TenantId && x.SchoolId == studentDataWithCurrentGrade.SchoolId && x.CalenderId == studentDataWithCurrentGrade.CalenderId);

                                    if(calenderData!=null)
                                    {
                                        GradeDetailsForTranscript.SchoolYear = calenderData.StartDate + "-" + calenderData.EndDate;
                                    }

                                    GradeDetailsForTranscript.GradeId = studentDataWithCurrentGrade.GradeId;
                                    GradeDetailsForTranscript.GradeTitle = studentDataWithCurrentGrade.GradeLevelTitle;
                                    GradeDetailsForTranscript.SchoolName = studentDataWithCurrentGrade.SchoolName;

                                    decimal? gPValue = 0.0m;
                                    decimal? gPAValue = 0.0m;
                                    decimal? creditAttemped = 0.0m;
                                    decimal? creditEarned = 0.0m;

                                    var reportCardData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeStandard).Where(x => x.TenantId == transcriptViewModel.TenantId && x.StudentId == student.StudentId && x.GradeId == Convert.ToInt32(grade)).ToList();
                                    if (reportCardData.Count > 0)
                                    {
                                        foreach (var reportCard in reportCardData)
                                        {
                                            
                                            var ReportCardDetailsForTranscript = new ReportCardDetailsForTranscriptViewModel();
                                            var courseSectionData = this.context.CourseSection.Include(x => x.Course).FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.CourseId == reportCard.CourseId && x.CourseSectionId == reportCard.CourseSectionId);

                                            if (courseSectionData != null)
                                            {
                                                ReportCardDetailsForTranscript.CourseCode = courseSectionData.Course.CourseShortName;
                                                ReportCardDetailsForTranscript.CourseSectionName = courseSectionData.CourseSectionName;
                                                ReportCardDetailsForTranscript.CreditHours = courseSectionData.CreditHours;
                                                ReportCardDetailsForTranscript.Grade = reportCard.GradeObtained;

                                                var gradeData = this.context?.Grade.FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.GradeId == reportCard.StudentFinalGradeStandard.FirstOrDefault().GradeObtained);

                                                if (gradeData != null)
                                                {
                                                    if (gradeData.Title == "F" || gradeData.Title == "Inc")
                                                    {
                                                        ReportCardDetailsForTranscript.CreditEarned = 0;

                                                        gPValue = ReportCardDetailsForTranscript.CreditEarned * gradeData.UnweightedGpValue;
                                                    }
                                                    else
                                                    {
                                                        ReportCardDetailsForTranscript.CreditEarned = courseSectionData.CreditHours;
                                                        
                                                        gPValue = courseSectionData.IsWeightedCourse != true ? ReportCardDetailsForTranscript.CreditEarned * gradeData.UnweightedGpValue : ReportCardDetailsForTranscript.CreditEarned * gradeData.WeightedGpValue;
                                                    }
                                                }

                                                ReportCardDetailsForTranscript.GPValue = gPValue; //gpValue=CreditEarned*(WeightedGpValue or UnweightedGpValue)
                                                creditAttemped += ReportCardDetailsForTranscript.CreditHours;
                                                creditEarned += ReportCardDetailsForTranscript.CreditEarned;
                                                gPAValue += ReportCardDetailsForTranscript.GPValue;

                                                GradeDetailsForTranscript.reportCardDetailsForTranscriptViewModel.Add(ReportCardDetailsForTranscript);
                                            }
                                        }
                                    }
                                    GradeDetailsForTranscript.CreditAttemped = creditAttemped; // Σ CreditHours of course sections
                                    GradeDetailsForTranscript.CreditEarned = creditEarned; // Σ CreditEarned of course sections
                                    GradeDetailsForTranscript.GPA = gPAValue / creditEarned; // Σ gpValue of course sections / Σ CreditEarned of course sections
                                    totalCreditEarned += creditEarned;
                                    totalCreditAttempeted += creditAttemped;
                                    cumulativeGPValue += gPAValue;
                                    cumulativeCreditHours += creditAttemped;
                                    StudentsDetailsForTranscript.gradeDetailsForTranscriptViewModel.Add(GradeDetailsForTranscript);
                                }
                            }
                            StudentsDetailsForTranscript.CumulativeGPA = cumulativeGPValue / cumulativeCreditHours;  // Σ gpValue of all course sections / Σ CreditHours of all course sections
                            StudentsDetailsForTranscript.TotalCreditAttempeted = totalCreditAttempeted; //Σ CreditAttemped 
                            StudentsDetailsForTranscript.TotalCreditEarned = totalCreditEarned;  //Σ CreditEarned

                            transcriptView.StudentsDetailsForTranscriptViewModelList.Add(StudentsDetailsForTranscript);
                        }
                    }
                }
                else
                {
                    transcriptView._failure = true;
                    transcriptView._message = "Select Student Please";
                }
            }
            catch (Exception es)
            {
                transcriptView._failure = false;
                transcriptView._message = es.Message;
            }
            return transcriptView;
        }

        /// <summary>
        /// Add Transcript For Students
        /// </summary>
        /// <param name="transcriptAddViewModel"></param>
        /// <returns></returns>
        public TranscriptAddViewModel AddTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptView = new TranscriptAddViewModel();
            try
            {
                int i = 0;
                long? ide = 1;
                if (transcriptAddViewModel.studentListForTranscript.Count > 0)
                {
                    foreach (var student in transcriptAddViewModel.studentListForTranscript)
                    {
                        List<StudentTranscriptMaster> studentTranscriptMasterList = new List<StudentTranscriptMaster>();
                        List<StudentTranscriptDetail> studentTranscriptDetailsList = new List<StudentTranscriptDetail>();
                        decimal? totalCreditAttempeted = 0.0m;
                        decimal? totalCreditEarned = 0.0m;
                        decimal? cumulativeGPValue = 0.0m;
                        decimal? cumulativeCreditHours = 0.0m;

                        List<StudentTranscriptDetail> studentTranscriptDetails = new List<StudentTranscriptDetail>();

                        var existingStudentTranscriptMasterData = this.context?.StudentTranscriptMaster.Where(x => x.SchoolId == transcriptAddViewModel.SchoolId && x.TenantId == transcriptAddViewModel.TenantId && x.StudentId == student.StudentId).ToList();

                        if (existingStudentTranscriptMasterData.Count>0)
                        {
                            var existingStudentTranscriptDetailsData = this.context?.StudentTranscriptDetail.Where(x => x.SchoolId == transcriptAddViewModel.SchoolId && x.TenantId == transcriptAddViewModel.TenantId && x.StudentId == student.StudentId).ToList();
                            if (existingStudentTranscriptDetailsData.Count > 0)
                            {
                                this.context?.StudentTranscriptDetail.RemoveRange(existingStudentTranscriptDetailsData);
                            }
                            this.context?.StudentTranscriptMaster.RemoveRange(existingStudentTranscriptMasterData);
                            this.context.SaveChanges();
                        }
                        if (i == 0)
                        {
                            var idData = this.context?.StudentTranscriptDetail.Where(x => x.TenantId == transcriptAddViewModel.TenantId && x.SchoolId == transcriptAddViewModel.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                            if (idData != null)
                            {
                                ide = idData.Id + 1;
                            }
                        }

                        var studentData = this.context?.StudentMaster.Include(x => x.StudentEnrollment).FirstOrDefault(x => x.TenantId == transcriptAddViewModel.TenantId && x.SchoolId == transcriptAddViewModel.SchoolId && x.StudentId == student.StudentId);

                        if (studentData != null)
                        {
                            var gradeIds = transcriptAddViewModel.GradeLavels.Split(",");

                            foreach (var grade in gradeIds.ToList())
                            {
                                decimal? gPValue = 0.0m;
                                decimal? sumOfGPValue = 0.0m;
                                decimal? creditAttemped = 0.0m;
                                decimal? creditEarned = 0.0m;
                                var calenderData = new SchoolCalendars();
                                decimal? GPA = 0.0m;
                                var studentDataWithCurrentGrade = studentData.StudentEnrollment.Where(x => x.GradeId == Convert.ToInt32(grade)).FirstOrDefault();

                                if (studentDataWithCurrentGrade != null)
                                {
                                    calenderData = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == transcriptAddViewModel.TenantId && x.SchoolId == studentDataWithCurrentGrade.SchoolId && x.CalenderId == studentDataWithCurrentGrade.CalenderId);

                                    var reportCardData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeStandard).Where(x => x.TenantId == transcriptAddViewModel.TenantId && x.StudentId == student.StudentId && x.GradeId == Convert.ToInt32(grade)).ToList();

                                    if (reportCardData.Count > 0)
                                    {
                                        foreach (var reportCard in reportCardData)
                                        {
                                            var gradeData = new Grade();

                                            var courseSectionData = this.context.CourseSection.Include(x => x.Course).FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.CourseId == reportCard.CourseId && x.CourseSectionId == reportCard.CourseSectionId);

                                            if (courseSectionData != null)
                                            {
                                                gradeData = this.context?.Grade.FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.Title.ToLower() == reportCard.GradeObtained.ToLower()&& x.GradeScaleId == reportCard.GradeScaleId);
                                                if (gradeData != null)
                                                {
                                                    gPValue = courseSectionData.IsWeightedCourse != true ? courseSectionData.CreditHours * gradeData.UnweightedGpValue : courseSectionData.CreditHours * gradeData.WeightedGpValue;

                                                }
                                            }
                                            creditAttemped += courseSectionData.CreditHours;
                                            creditEarned += courseSectionData.CreditHours;
                                            sumOfGPValue += gPValue;

                                            var studentTranscriptDetail = new StudentTranscriptDetail()
                                            {
                                                Id = (long)ide,
                                                TenantId = (Guid)transcriptAddViewModel.TenantId,
                                                SchoolId = (int)transcriptAddViewModel.SchoolId,
                                                StudentId = student.StudentId,
                                                CourseCode = courseSectionData.Course.CourseShortName,
                                                CourseName = courseSectionData.CourseSectionName,
                                                CreditHours = courseSectionData.CreditHours,
                                                CreditEarned = courseSectionData.CreditHours,
                                                GpValue = gPValue,
                                                Grade = reportCard.GradeObtained,
                                                GradeTitle= studentDataWithCurrentGrade.GradeLevelTitle,
                                                CreatedBy = transcriptAddViewModel.CreatedBy,
                                                CreatedOn = DateTime.UtcNow

                                            };
                                            studentTranscriptDetailsList.Add(studentTranscriptDetail);
                                            ide++;

                                        }
                                        this.context?.StudentTranscriptDetail.AddRange(studentTranscriptDetailsList);
                                        GPA = sumOfGPValue / creditEarned;
                                        totalCreditEarned += creditEarned;
                                        totalCreditAttempeted += creditAttemped;
                                        cumulativeGPValue += sumOfGPValue;
                                        cumulativeCreditHours += creditAttemped;
                                    }

                                    var CumulativeGPA = cumulativeGPValue / cumulativeCreditHours;

                                    var studentTranscriptMaster = new StudentTranscriptMaster
                                    {
                                        TenantId = (Guid)transcriptAddViewModel.TenantId,
                                        SchoolId = (int)transcriptAddViewModel.SchoolId,
                                        StudentId = (int)student.StudentId,
                                        StudentInternalId = studentData.StudentInternalId,
                                        CumulativeGpa = CumulativeGPA,
                                        TotalCreditAttempted = totalCreditAttempeted,
                                        TotalCreditEarned = totalCreditEarned,
                                        GeneratedOn = DateTime.UtcNow,
                                        CreatedBy = transcriptAddViewModel.CreatedBy,
                                        CreatedOn = DateTime.UtcNow,
                                        SchoolYear = calenderData.AcademicYear.ToString(),
                                        SchoolName = studentDataWithCurrentGrade.SchoolName,
                                        GradeTitle = studentDataWithCurrentGrade.GradeLevelTitle,
                                        TotalGradeCreditEarned = creditEarned,
                                        CreditAttempted = creditAttemped,
                                        Gpa = GPA,
                                    };
                                    studentTranscriptMasterList.Add(studentTranscriptMaster);
                                }
                            }
                        }
                        this.context?.StudentTranscriptMaster.AddRange(studentTranscriptMasterList);
                        i++;
                    }
                    this.context?.SaveChanges();
                    transcriptAddViewModel._message = "Added Successfully";
                }
                else
                {
                    transcriptAddViewModel._failure = true;
                    transcriptAddViewModel._message = "Select Student Please";
                }
            }
            catch (Exception es)
            {
                transcriptAddViewModel._failure = false;
                transcriptAddViewModel._message = es.Message;
            }
            return transcriptAddViewModel;
        }

        /// <summary>
        /// Generate Pdf Transcript For Student
        /// </summary>
        /// <param name="transcriptAddViewModel"></param>
        /// <returns></returns>
        public async Task<TranscriptAddViewModel> GenerateTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptView = new TranscriptAddViewModel();
            try
            {
                transcriptView.TenantId = transcriptAddViewModel.TenantId;
                transcriptView.SchoolId = transcriptAddViewModel.SchoolId;
                transcriptView._tenantName = transcriptAddViewModel._tenantName;
                transcriptView._userName = transcriptAddViewModel._userName;
                string base64 = null;
                object data = new object();

                List<object> transcriptList = new List<object>();
                List<object> teacherCommentList = new List<object>();

                foreach (var student in transcriptAddViewModel.studentListForTranscript)
                {
                    var studentData = this.context?.StudentMaster.FirstOrDefault(x => x.SchoolId == transcriptAddViewModel.SchoolId && x.TenantId == transcriptAddViewModel.TenantId && x.StudentId == student.StudentId);

                    var schoolData = this.context?.SchoolMaster.Include(m => m.SchoolDetail).FirstOrDefault(x => x.SchoolId == transcriptAddViewModel.SchoolId && x.TenantId == transcriptAddViewModel.TenantId);

                    var studentTranscriptData = this.context?.StudentTranscriptMaster.Include(x => x.StudentTranscriptDetail).Where(x => x.SchoolId == transcriptAddViewModel.SchoolId && x.TenantId == transcriptAddViewModel.TenantId && x.StudentId == student.StudentId).ToList();

                    var gradeData = this.context?.Grade.Where(x => x.TenantId == transcriptAddViewModel.TenantId && x.SchoolId == transcriptAddViewModel.SchoolId).ToList();

                    if (studentData != null && schoolData != null && studentTranscriptData.Count > 0)
                    {
                        List<object> transcriptDetailsList = new List<object>();
                        studentData.StudentPhoto = transcriptAddViewModel.StudentPhoto == true ? studentData.StudentPhoto : null;
                        string studentDob = studentData.Dob.HasValue == true ? studentData.Dob.Value.ToShortDateString() : null;
                        foreach (var studentTranscript in studentTranscriptData)
                        {
                            var studentTranscriptDetailsData = studentTranscript.StudentTranscriptDetail.Where(x => x.GradeTitle == studentTranscript.GradeTitle).ToList();

                            object gardeLevelWiseData = new
                            {
                                studentTranscript.GradeTitle,
                                studentTranscript.SchoolName,
                                studentTranscript.SchoolYear,
                                studentTranscript.CreditAttempted,
                                studentTranscript.TotalGradeCreditEarned,
                                studentTranscript.Gpa,
                                Details = studentTranscriptDetailsData
                            };
                            transcriptDetailsList.Add(gardeLevelWiseData);
                        }

                        object transcript = new
                        {
                            SchoolData = schoolData,
                            nameOfPrincipal = schoolData.SchoolDetail != null ? schoolData.SchoolDetail.FirstOrDefault().NameOfPrincipal : null,
                            TasnscriptdetailsData = transcriptDetailsList,
                            StudentData = studentData,
                            StudentDob= studentDob,
                            cumulativeGpa = studentTranscriptData.LastOrDefault().CumulativeGpa,
                            totalCreditEarned = studentTranscriptData.LastOrDefault().TotalCreditEarned,
                            totalCreditAttempted = studentTranscriptData.LastOrDefault().TotalCreditAttempted,
                            GradeDetails = transcriptAddViewModel.GradeLagend == true ? gradeData : null,
                        };
                        transcriptList.Add(transcript);
                    }
                }
                if (transcriptList != null)
                {
                    data = new
                    {
                        TotalData = transcriptList
                    };
                }

                GenerateTranscript _transcript = new GenerateTranscript();
                var message = await _transcript.Generate(data);

                if (message == "success")
                {
                    using (var fileStream = new FileStream(@"ReportCard\\StudentTranscript.pdf", FileMode.Open))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            byte[] bytes = memoryStream.ToArray();
                            base64 = Convert.ToBase64String(bytes);
                            fileStream.Close();
                        }
                    }
                    transcriptView.TranscriptPdf = base64;
                }
                else
                {
                    transcriptView._message = "Problem occur!!! Prlease Try Again";
                    transcriptView._failure = true;
                }
            }
            catch (Exception es)
            {
                transcriptView._message = es.Message;
                transcriptView._failure = true;

            }
            return transcriptView;
        }

        /// <summary>
        /// Add Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel AddStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            try
            {
                int ide = 1;
                var studentMedicalAlertData = this.context?.StudentMedicalAlert.Where(x => x.TenantId == studentMedicalAlertAddViewModel.studentMedicalAlert.TenantId && x.SchoolId == studentMedicalAlertAddViewModel.studentMedicalAlert.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (studentMedicalAlertData != null)
                {
                    ide = studentMedicalAlertData.Id + 1;
                }
                studentMedicalAlertAddViewModel.studentMedicalAlert.Id = ide;
                studentMedicalAlertAddViewModel.studentMedicalAlert.CreatedOn = DateTime.UtcNow;
                this.context?.StudentMedicalAlert.Add(studentMedicalAlertAddViewModel.studentMedicalAlert);
                this.context.SaveChanges();
                studentMedicalAlertAddViewModel._failure = false;
                studentMedicalAlertAddViewModel._message = "StudentMedicalAlert Added Successfully";
            }
            catch (Exception es)
            {
                studentMedicalAlertAddViewModel._message = es.Message;
                studentMedicalAlertAddViewModel._failure = true;
            }
            return studentMedicalAlertAddViewModel;
        }

        /// <summary>
        /// Update Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel UpdateStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            try
            {
                var studentMedicalAlertData = this.context?.StudentMedicalAlert.FirstOrDefault(x => x.TenantId == studentMedicalAlertAddViewModel.studentMedicalAlert.TenantId && x.SchoolId == studentMedicalAlertAddViewModel.studentMedicalAlert.SchoolId && x.StudentId == studentMedicalAlertAddViewModel.studentMedicalAlert.StudentId && x.Id == studentMedicalAlertAddViewModel.studentMedicalAlert.Id);

                if (studentMedicalAlertData != null)
                {
                    studentMedicalAlertAddViewModel.studentMedicalAlert.CreatedOn = studentMedicalAlertData.CreatedOn;
                    studentMedicalAlertAddViewModel.studentMedicalAlert.CreatedBy = studentMedicalAlertData.CreatedBy;
                    studentMedicalAlertAddViewModel.studentMedicalAlert.UpdatedOn = DateTime.UtcNow;
                    this.context.Entry(studentMedicalAlertData).CurrentValues.SetValues(studentMedicalAlertAddViewModel.studentMedicalAlert);
                    this.context.SaveChanges();
                    studentMedicalAlertAddViewModel._failure = false;
                    studentMedicalAlertAddViewModel._message = "StudentMedicalAlert Updated Successfully";
                }
                else
                {
                    studentMedicalAlertAddViewModel._message = NORECORDFOUND;
                    studentMedicalAlertAddViewModel._failure = true;
                }

            }
            catch (Exception es)
            {
                studentMedicalAlertAddViewModel._message = es.Message;
                studentMedicalAlertAddViewModel._failure = true;
            }
            return studentMedicalAlertAddViewModel;
        }

        /// <summary>
        /// Delete Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel DeleteStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            try
            {
                var studentMedicalAlertData = this.context?.StudentMedicalAlert.FirstOrDefault(x => x.TenantId == studentMedicalAlertAddViewModel.studentMedicalAlert.TenantId && x.SchoolId == studentMedicalAlertAddViewModel.studentMedicalAlert.SchoolId && x.StudentId == studentMedicalAlertAddViewModel.studentMedicalAlert.StudentId && x.Id == studentMedicalAlertAddViewModel.studentMedicalAlert.Id);

                if (studentMedicalAlertData != null)
                {
                    this.context?.StudentMedicalAlert.Remove(studentMedicalAlertData);
                    this.context.SaveChanges();
                    studentMedicalAlertAddViewModel._failure = false;
                    studentMedicalAlertAddViewModel._message = "StudentMedicalAlert Deleted Successfully";
                }
                else
                {
                    studentMedicalAlertAddViewModel._message = NORECORDFOUND;
                    studentMedicalAlertAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalAlertAddViewModel._message = es.Message;
                studentMedicalAlertAddViewModel._failure = true;
            }
            return studentMedicalAlertAddViewModel;
        }

        /// <summary>
        /// Add Student Medical Note
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel AddStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            try
            {
                int ide = 1;
                var studentMedicalNoteData = this.context?.StudentMedicalNote.Where(x => x.TenantId == studentMedicalNoteAddViewModel.studentMedicalNote.TenantId && x.SchoolId == studentMedicalNoteAddViewModel.studentMedicalNote.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (studentMedicalNoteData != null)
                {
                    ide = studentMedicalNoteData.Id + 1;
                }
                studentMedicalNoteAddViewModel.studentMedicalNote.Id = ide;
                studentMedicalNoteAddViewModel.studentMedicalNote.CreatedOn = DateTime.UtcNow;
                this.context?.StudentMedicalNote.Add(studentMedicalNoteAddViewModel.studentMedicalNote);
                this.context.SaveChanges();
                studentMedicalNoteAddViewModel._failure = false;
                studentMedicalNoteAddViewModel._message = "StudentMedicalNote Added Successfully";
            }
            catch (Exception es)
            {
                studentMedicalNoteAddViewModel._message = es.Message;
                studentMedicalNoteAddViewModel._failure = true;
            }
            return studentMedicalNoteAddViewModel;
        }

        /// <summary>
        /// Update Student Medical Note
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel UpdateStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            try
            {
                var studentMedicalNoteData = this.context?.StudentMedicalNote.FirstOrDefault(x => x.TenantId == studentMedicalNoteAddViewModel.studentMedicalNote.TenantId && x.SchoolId == studentMedicalNoteAddViewModel.studentMedicalNote.SchoolId && x.StudentId == studentMedicalNoteAddViewModel.studentMedicalNote.StudentId && x.Id == studentMedicalNoteAddViewModel.studentMedicalNote.Id);

                if (studentMedicalNoteData != null)
                {
                    studentMedicalNoteAddViewModel.studentMedicalNote.CreatedOn = studentMedicalNoteData.CreatedOn;
                    studentMedicalNoteAddViewModel.studentMedicalNote.CreatedBy = studentMedicalNoteData.CreatedBy;
                    studentMedicalNoteAddViewModel.studentMedicalNote.UpdatedOn = DateTime.UtcNow;
                    this.context.Entry(studentMedicalNoteData).CurrentValues.SetValues(studentMedicalNoteAddViewModel.studentMedicalNote);
                    this.context.SaveChanges();
                    studentMedicalNoteAddViewModel._failure = false;
                    studentMedicalNoteAddViewModel._message = "StudentMedicalNote Updated Successfully";
                }
                else
                {
                    studentMedicalNoteAddViewModel._message = NORECORDFOUND;
                    studentMedicalNoteAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalNoteAddViewModel._message = es.Message;
                studentMedicalNoteAddViewModel._failure = true;
            }
            return studentMedicalNoteAddViewModel;
        }

        /// <summary>
        /// Delete Student Medical Note
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel DeleteStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            try
            {
                var studentMedicalNoteData = this.context?.StudentMedicalNote.FirstOrDefault(x => x.TenantId == studentMedicalNoteAddViewModel.studentMedicalNote.TenantId && x.SchoolId == studentMedicalNoteAddViewModel.studentMedicalNote.SchoolId && x.StudentId == studentMedicalNoteAddViewModel.studentMedicalNote.StudentId && x.Id == studentMedicalNoteAddViewModel.studentMedicalNote.Id);

                if (studentMedicalNoteData != null)
                {
                    this.context?.StudentMedicalNote.Remove(studentMedicalNoteData);
                    this.context.SaveChanges();
                    studentMedicalNoteAddViewModel._failure = false;
                    studentMedicalNoteAddViewModel._message = "StudentMedicalNote Deleted Successfully";
                }
                else
                {
                    studentMedicalNoteAddViewModel._message = NORECORDFOUND;
                    studentMedicalNoteAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalNoteAddViewModel._message = es.Message;
                studentMedicalNoteAddViewModel._failure = true;
            }
            return studentMedicalNoteAddViewModel;
        }

        /// <summary>
        /// Add Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel AddStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            try
            {
                int ide = 1;
                var studentMedicalImmunizationData = this.context?.StudentMedicalImmunization.Where(x => x.TenantId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.TenantId && x.SchoolId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (studentMedicalImmunizationData != null)
                {
                    ide = studentMedicalImmunizationData.Id + 1;
                }
                studentMedicalImmunizationAddViewModel.studentMedicalImmunization.Id = ide;
                studentMedicalImmunizationAddViewModel.studentMedicalImmunization.CreatedOn = DateTime.UtcNow;
                this.context?.StudentMedicalImmunization.Add(studentMedicalImmunizationAddViewModel.studentMedicalImmunization);
                this.context.SaveChanges();
                studentMedicalImmunizationAddViewModel._failure = false;
                studentMedicalImmunizationAddViewModel._message = "StudentMedicalImmunization Added Successfully";
            }
            catch (Exception es)
            {
                studentMedicalImmunizationAddViewModel._message = es.Message;
                studentMedicalImmunizationAddViewModel._failure = true;
            }
            return studentMedicalImmunizationAddViewModel;
        }

        /// <summary>
        /// Update Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel UpdateStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            try
            {
                var studentMedicalImmunizationData = this.context?.StudentMedicalImmunization.FirstOrDefault(x => x.TenantId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.TenantId && x.SchoolId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.SchoolId && x.StudentId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.StudentId && x.Id == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.Id);

                if (studentMedicalImmunizationData != null)
                {
                    studentMedicalImmunizationAddViewModel.studentMedicalImmunization.CreatedOn = studentMedicalImmunizationData.CreatedOn;
                    studentMedicalImmunizationAddViewModel.studentMedicalImmunization.CreatedBy = studentMedicalImmunizationData.CreatedBy;
                    studentMedicalImmunizationAddViewModel.studentMedicalImmunization.UpdatedOn = DateTime.UtcNow;
                    this.context.Entry(studentMedicalImmunizationData).CurrentValues.SetValues(studentMedicalImmunizationAddViewModel.studentMedicalImmunization);
                    this.context.SaveChanges();
                    studentMedicalImmunizationAddViewModel._failure = false;
                    studentMedicalImmunizationAddViewModel._message = "StudentMedicalImmunization Updated Successfully";
                }
                else
                {
                    studentMedicalImmunizationAddViewModel._message = NORECORDFOUND;
                    studentMedicalImmunizationAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalImmunizationAddViewModel._message = es.Message;
                studentMedicalImmunizationAddViewModel._failure = true;
            }
            return studentMedicalImmunizationAddViewModel;
        }

        /// <summary>
        /// Delete Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel DeleteStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            try
            {
                var studentMedicalImmunizationData = this.context?.StudentMedicalImmunization.FirstOrDefault(x => x.TenantId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.TenantId && x.SchoolId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.SchoolId && x.StudentId == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.StudentId && x.Id == studentMedicalImmunizationAddViewModel.studentMedicalImmunization.Id);

                if (studentMedicalImmunizationData != null)
                {
                    this.context?.StudentMedicalImmunization.Remove(studentMedicalImmunizationData);
                    this.context.SaveChanges();
                    studentMedicalImmunizationAddViewModel._failure = false;
                    studentMedicalImmunizationAddViewModel._message = "StudentMedicalImmunization Deleted Successfully";
                }
                else
                {
                    studentMedicalImmunizationAddViewModel._message = NORECORDFOUND;
                    studentMedicalImmunizationAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalImmunizationAddViewModel._message = es.Message;
                studentMedicalImmunizationAddViewModel._failure = true;
            }
            return studentMedicalImmunizationAddViewModel;
        }

        /// <summary>
        /// Add Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel AddStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            try
            {
                int ide = 1;
                var studentMedicalNurseVisitData = this.context?.StudentMedicalNurseVisit.Where(x => x.TenantId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.TenantId && x.SchoolId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (studentMedicalNurseVisitData != null)
                {
                    ide = studentMedicalNurseVisitData.Id + 1;
                }
                studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.Id = ide;
                studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.CreatedOn = DateTime.UtcNow;
                this.context?.StudentMedicalNurseVisit.Add(studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit);
                this.context.SaveChanges();
                studentMedicalNurseVisitAddViewModel._failure = false;
                studentMedicalNurseVisitAddViewModel._message = "StudentMedicalNurseVisit Added Successfully";
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitAddViewModel._message = es.Message;
                studentMedicalNurseVisitAddViewModel._failure = true;
            }
            return studentMedicalNurseVisitAddViewModel;
        }

        /// <summary>
        /// Update Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel UpdateStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            try
            {
                var studentMedicalNurseVisitData = this.context?.StudentMedicalNurseVisit.FirstOrDefault(x => x.TenantId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.TenantId && x.SchoolId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.SchoolId && x.StudentId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.StudentId && x.Id == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.Id);

                if (studentMedicalNurseVisitData != null)
                {
                    studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.CreatedOn = studentMedicalNurseVisitData.CreatedOn;
                    studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.CreatedBy = studentMedicalNurseVisitData.CreatedBy;
                    studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.UpdatedOn = DateTime.UtcNow;
                    this.context.Entry(studentMedicalNurseVisitData).CurrentValues.SetValues(studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit);
                    this.context.SaveChanges();
                    studentMedicalNurseVisitAddViewModel._failure = false;
                    studentMedicalNurseVisitAddViewModel._message = "StudentMedicalNurseVisit Updated Successfully";
                }
                else
                {
                    studentMedicalNurseVisitAddViewModel._message = NORECORDFOUND;
                    studentMedicalNurseVisitAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitAddViewModel._message = es.Message;
                studentMedicalNurseVisitAddViewModel._failure = true;
            }
            return studentMedicalNurseVisitAddViewModel;
        }

        /// <summary>
        /// Delete Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel DeleteStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            try
            {
                var studentMedicalNurseVisitData = this.context?.StudentMedicalNurseVisit.FirstOrDefault(x => x.TenantId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.TenantId && x.SchoolId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.SchoolId && x.StudentId == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.StudentId && x.Id == studentMedicalNurseVisitAddViewModel.studentMedicalNurseVisit.Id);

                if (studentMedicalNurseVisitData != null)
                {
                    this.context?.StudentMedicalNurseVisit.Remove(studentMedicalNurseVisitData);
                    this.context.SaveChanges();
                    studentMedicalNurseVisitAddViewModel._failure = false;
                    studentMedicalNurseVisitAddViewModel._message = "StudentMedicalNurseVisit Deleted Successfully";
                }
                else
                {
                    studentMedicalNurseVisitAddViewModel._message = NORECORDFOUND;
                    studentMedicalNurseVisitAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitAddViewModel._message = es.Message;
                studentMedicalNurseVisitAddViewModel._failure = true;
            }
            return studentMedicalNurseVisitAddViewModel;
        }

        /// <summary>
        /// Add Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel AddStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {

                    int ide = 1;
                    var studentMedicalProviderData = this.context?.StudentMedicalProvider.Where(x => x.TenantId == studentMedicalProviderAddViewModel.studentMedicalProvider.TenantId && x.SchoolId == studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (studentMedicalProviderData != null)
                    {
                        ide = studentMedicalProviderData.Id + 1;
                    }
                    studentMedicalProviderAddViewModel.studentMedicalProvider.Id = ide;
                    studentMedicalProviderAddViewModel.studentMedicalProvider.CreatedOn = DateTime.UtcNow;
                    this.context?.StudentMedicalProvider.Add(studentMedicalProviderAddViewModel.studentMedicalProvider);
                    this.context.SaveChanges();

                    if (studentMedicalProviderAddViewModel.fieldsCategoryList != null && studentMedicalProviderAddViewModel.fieldsCategoryList.ToList().Count > 0)
                    {
                        var fieldsCategory = studentMedicalProviderAddViewModel.fieldsCategoryList.FirstOrDefault(x => x.CategoryId == studentMedicalProviderAddViewModel.SelectedCategoryId);
                        if (fieldsCategory != null)
                        {
                            foreach (var customFields in fieldsCategory.CustomFields.ToList())
                            {
                                if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                {
                                    customFields.CustomFieldsValue.FirstOrDefault().Module = "Student";
                                    customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                    customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                    customFields.CustomFieldsValue.FirstOrDefault().SchoolId = studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId;
                                    customFields.CustomFieldsValue.FirstOrDefault().TargetId = studentMedicalProviderAddViewModel.studentMedicalProvider.StudentId;
                                    this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                    this.context?.SaveChanges();
                                }
                            }
                        }
                    }

                    studentMedicalProviderAddViewModel._failure = false;
                    studentMedicalProviderAddViewModel._message = "StudentMedicalProvider Added Successfully";
                    transaction.Commit();
                }

                catch (Exception es)
                {
                    transaction.Rollback();
                    studentMedicalProviderAddViewModel._message = es.Message;
                    studentMedicalProviderAddViewModel._failure = true;
                }
            }
            return studentMedicalProviderAddViewModel;
        }

        /// <summary>
        /// Update Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel UpdateStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var studentMedicalProviderData = this.context?.StudentMedicalProvider.FirstOrDefault(x => x.TenantId == studentMedicalProviderAddViewModel.studentMedicalProvider.TenantId && x.SchoolId == studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId && x.StudentId == studentMedicalProviderAddViewModel.studentMedicalProvider.StudentId && x.Id == studentMedicalProviderAddViewModel.studentMedicalProvider.Id);

                    if (studentMedicalProviderData != null)
                    {
                        studentMedicalProviderAddViewModel.studentMedicalProvider.CreatedOn = studentMedicalProviderData.CreatedOn;
                        studentMedicalProviderAddViewModel.studentMedicalProvider.CreatedBy = studentMedicalProviderData.CreatedBy;
                        studentMedicalProviderAddViewModel.studentMedicalProvider.UpdatedOn = DateTime.UtcNow;
                        this.context.Entry(studentMedicalProviderData).CurrentValues.SetValues(studentMedicalProviderAddViewModel.studentMedicalProvider);
                        this.context.SaveChanges();

                        if (studentMedicalProviderAddViewModel.fieldsCategoryList != null && studentMedicalProviderAddViewModel.fieldsCategoryList.ToList().Count > 0)
                        {
                            var fieldsCategory = studentMedicalProviderAddViewModel.fieldsCategoryList.FirstOrDefault(x => x.CategoryId == studentMedicalProviderAddViewModel.SelectedCategoryId);
                            if (fieldsCategory != null)
                            {
                                foreach (var customFields in fieldsCategory.CustomFields.ToList())
                                {
                                    var customFieldValueData = this.context?.CustomFieldsValue.FirstOrDefault(x => x.TenantId == studentMedicalProviderAddViewModel.studentMedicalProvider.TenantId && x.SchoolId == studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId && x.CategoryId == customFields.CategoryId && x.FieldId == customFields.FieldId && x.Module == "Student" && x.TargetId == studentMedicalProviderAddViewModel.studentMedicalProvider.StudentId);
                                    if (customFieldValueData != null)
                                    {
                                        this.context?.CustomFieldsValue.RemoveRange(customFieldValueData);
                                    }
                                    if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                    {
                                        customFields.CustomFieldsValue.FirstOrDefault().Module = "Student";
                                        customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                        customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                        customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                        customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                        customFields.CustomFieldsValue.FirstOrDefault().SchoolId = studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId;
                                        customFields.CustomFieldsValue.FirstOrDefault().TargetId = studentMedicalProviderAddViewModel.studentMedicalProvider.StudentId;
                                        this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                        this.context?.SaveChanges();
                                    }
                                }
                            }
                        }
                        studentMedicalProviderAddViewModel._failure = false;
                        studentMedicalProviderAddViewModel._message = "StudentMedicalProvider Updated Successfully";
                        transaction.Commit();
                    }
                    else
                    {
                        studentMedicalProviderAddViewModel._message = NORECORDFOUND;
                        studentMedicalProviderAddViewModel._failure = true;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentMedicalProviderAddViewModel._message = es.Message;
                    studentMedicalProviderAddViewModel._failure = true;
                }
            }
            return studentMedicalProviderAddViewModel;
        }

        /// <summary>
        /// Delete Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel DeleteStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            try
            {
                var studentMedicalProviderData = this.context?.StudentMedicalProvider.FirstOrDefault(x => x.TenantId == studentMedicalProviderAddViewModel.studentMedicalProvider.TenantId && x.SchoolId == studentMedicalProviderAddViewModel.studentMedicalProvider.SchoolId && x.StudentId == studentMedicalProviderAddViewModel.studentMedicalProvider.StudentId && x.Id == studentMedicalProviderAddViewModel.studentMedicalProvider.Id);

                if (studentMedicalProviderData != null)
                {
                    this.context?.StudentMedicalProvider.Remove(studentMedicalProviderData);
                    this.context.SaveChanges();
                    studentMedicalProviderAddViewModel._failure = false;
                    studentMedicalProviderAddViewModel._message = "StudentMedicalProvider Deleted Successfully";
                }
                else
                {
                    studentMedicalProviderAddViewModel._message = NORECORDFOUND;
                    studentMedicalProviderAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalProviderAddViewModel._message = es.Message;
                studentMedicalProviderAddViewModel._failure = true;
            }
            return studentMedicalProviderAddViewModel;
        }

        /// <summary>
        /// Get All Student Medical Info
        /// </summary>
        /// <param name="studentMedicalInfoViewModel"></param>
        /// <returns></returns>
        public StudentMedicalInfoViewModel GetAllStudentMedicalInfo(StudentMedicalInfoViewModel studentMedicalInfoViewModel)
        {
            StudentMedicalInfoViewModel studentMedicalInfoList = new StudentMedicalInfoViewModel();
            try
            {
                studentMedicalInfoList.TenantId = studentMedicalInfoViewModel.TenantId;
                studentMedicalInfoList.SchoolId = studentMedicalInfoViewModel.SchoolId;
                studentMedicalInfoList.StudentId = studentMedicalInfoViewModel.StudentId;
                studentMedicalInfoList._tenantName = studentMedicalInfoViewModel._tenantName;
                studentMedicalInfoList._userName = studentMedicalInfoViewModel._userName;

                var studentData = this.context?.StudentMaster.Include(x => x.StudentMedicalAlert).Include(x => x.StudentMedicalNote).Include(x => x.StudentMedicalImmunization).Include(x => x.StudentMedicalNurseVisit).Include(x => x.StudentMedicalProvider).FirstOrDefault(x => x.TenantId == studentMedicalInfoViewModel.TenantId && x.SchoolId == studentMedicalInfoViewModel.SchoolId && x.StudentId == studentMedicalInfoViewModel.StudentId);

                if (studentData != null)
                {
                    studentMedicalInfoList.studentMedicalAlertList = studentData.StudentMedicalAlert.ToList();
                    studentMedicalInfoList.studentMedicalNoteList = studentData.StudentMedicalNote.ToList();
                    studentMedicalInfoList.studentMedicalImmunizationList = studentData.StudentMedicalImmunization.ToList();
                    studentMedicalInfoList.studentMedicalNurseVisitList = studentData.StudentMedicalNurseVisit.ToList();
                    studentMedicalInfoList.studentMedicalProviderList = studentData.StudentMedicalProvider.ToList();

                    studentMedicalInfoList.studentMedicalAlertList.ForEach(x => x.StudentMaster = null);
                    studentMedicalInfoList.studentMedicalNoteList.ForEach(x => x.StudentMaster = null);
                    studentMedicalInfoList.studentMedicalImmunizationList.ForEach(x => x.StudentMaster = null);
                    studentMedicalInfoList.studentMedicalNurseVisitList.ForEach(x => x.StudentMaster = null);
                    studentMedicalInfoList.studentMedicalProviderList.ForEach(x => x.StudentMaster = null);

                    var fieldsCategories = this.context?.FieldsCategory.Where(x => x.TenantId == studentMedicalInfoViewModel.TenantId && x.SchoolId == studentMedicalInfoViewModel.SchoolId && x.Module == "Student").OrderByDescending(x => x.IsSystemCategory).ThenBy(x => x.SortOrder)
                       .Select(y => new FieldsCategory
                       {
                           TenantId = y.TenantId,
                           SchoolId = y.SchoolId,
                           CategoryId = y.CategoryId,
                           IsSystemCategory = y.IsSystemCategory,
                           Search = y.Search,
                           Title = y.Title,
                           Module = y.Module,
                           SortOrder = y.SortOrder,
                           Required = y.Required,
                           Hide = y.Hide,
                           LastUpdate = y.LastUpdate,
                           UpdatedBy = y.UpdatedBy,
                           CustomFields = y.CustomFields.Where(x => x.SystemField != true).Select(z => new CustomFields
                           {
                               TenantId = z.TenantId,
                               SchoolId = z.SchoolId,
                               CategoryId = z.CategoryId,
                               FieldId = z.FieldId,
                               Module = z.Module,
                               Type = z.Type,
                               Search = z.Search,
                               Title = z.Title,
                               SortOrder = z.SortOrder,
                               SelectOptions = z.SelectOptions,
                               SystemField = z.SystemField,
                               Required = z.Required,
                               DefaultSelection = z.DefaultSelection,
                               LastUpdate = z.LastUpdate,
                               UpdatedBy = z.UpdatedBy,
                               CustomFieldsValue = z.CustomFieldsValue.Where(w => w.TargetId == studentMedicalInfoViewModel.StudentId).ToList()
                           }).OrderByDescending(x => x.SystemField).ThenBy(x => x.SortOrder).ToList()
                       }).ToList();

                    studentMedicalInfoList.fieldsCategoryList = fieldsCategories;
                }
                else
                {
                    studentMedicalInfoList._message = NORECORDFOUND;
                    studentMedicalInfoList._failure = true;
                }
            }
            catch (Exception es)
            {
                studentMedicalInfoList._message = es.Message;
                studentMedicalInfoList._failure = true;
            }
            return studentMedicalInfoList;
        }
    }
}



