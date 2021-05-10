using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IStudentRepository
    {
        public StudentAddViewModel AddStudent(StudentAddViewModel student);
        public StudentAddViewModel UpdateStudent(StudentAddViewModel student);
        public StudentListModel GetAllStudentList(PageResult pageResult);
        public LoginInfoAddModel AddStudentLoginInfo(LoginInfoAddModel login);

        public StudentDocumentAddViewModel AddStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);
        public StudentDocumentAddViewModel UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);
        public StudentDocumentListViewModel GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentListViewModel);
        public StudentDocumentAddViewModel DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);

        public StudentCommentAddViewModel AddStudentComment(StudentCommentAddViewModel studentCommentAddViewModel);
        public StudentCommentAddViewModel UpdateStudentComment(StudentCommentAddViewModel studentCommentAddViewModel);
        public StudentCommentListViewModel GetAllStudentCommentsList(StudentCommentListViewModel studentCommentListViewModel);
        public StudentCommentAddViewModel DeleteStudentComment(StudentCommentAddViewModel studentCommentAddViewModel);

        //public StudentAddViewModel ViewStudent(StudentAddViewModel student);
        public StudentAddViewModel ViewStudent(StudentAddViewModel student);
        //public StudentAddViewModel DeleteStudent(StudentAddViewModel student);

        public SiblingSearchForStudentListModel SearchSiblingForStudent(SiblingSearchForStudentListModel studentSiblingListViewModel);
        public SiblingAddUpdateForStudentModel AssociationSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel);
        public StudentListModel ViewAllSibling(StudentListModel studentListModel);
        public SiblingAddUpdateForStudentModel RemoveSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel);
        public CheckStudentInternalIdViewModel CheckStudentInternalId(CheckStudentInternalIdViewModel checkStudentInternalIdViewModel);
        public StudentEnrollmentListModel AddStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel);
        public StudentEnrollmentListViewModel GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListViewModel);
        public StudentEnrollmentListModel UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel);
        public StudentAddViewModel AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel);
        public StudentListModel SearchStudentListForReenroll(PageResult pageResult);
        public StudentListModel ReenrollmentForStudent(StudentListModel studentListModel);
        //public SearchStudentViewModel SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel);
        public StudentListAddViewModel AddStudentList(StudentListAddViewModel studentListAddViewModel);
        public TranscriptViewModel GenerateTranscriptForStudent(TranscriptViewModel transcriptViewModel);
    }
}

