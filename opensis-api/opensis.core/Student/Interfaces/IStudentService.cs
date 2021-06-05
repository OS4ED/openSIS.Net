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

using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace opensis.core.Student.Interfaces
{
    public interface IStudentService
    {
        public StudentAddViewModel SaveStudent(StudentAddViewModel student);
        public StudentAddViewModel UpdateStudent(StudentAddViewModel student);
        public LoginInfoAddModel AddStudentLoginInfo(LoginInfoAddModel login);
        public StudentListModel GetAllStudentList(PageResult pageResult);

        public StudentDocumentAddViewModel SaveStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);
        public StudentDocumentAddViewModel UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);
        public StudentDocumentListViewModel GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentListViewModel);
        public StudentDocumentAddViewModel DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel);

        public StudentCommentAddViewModel SaveStudentComment(StudentCommentAddViewModel studentCommentAddViewModel);
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
        public StudentEnrollmentListViewModel GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListModel);
        public StudentEnrollmentListModel UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel);
        public StudentAddViewModel AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel);
        public StudentListModel SearchStudentListForReenroll(PageResult pageResult);
        public StudentListModel ReenrollmentForStudent(StudentListModel studentListModel);
        ///public SearchStudentViewModel SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel);
        public StudentListAddViewModel AddStudentList(StudentListAddViewModel studentListAddViewModel);
        public TranscriptViewModel TranscriptViewForStudent(TranscriptViewModel transcriptViewModel);
        public TranscriptAddViewModel AddTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel); 
        public Task<TranscriptAddViewModel> GenerateTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel);
        public StudentMedicalAlertAddViewModel AddStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel);
        public StudentMedicalAlertAddViewModel UpdateStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel);
        public StudentMedicalAlertAddViewModel DeleteStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel);
        public StudentMedicalNoteAddViewModel AddStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel);
        public StudentMedicalNoteAddViewModel UpdateStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel);
        public StudentMedicalNoteAddViewModel DeleteStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel);
        public StudentMedicalImmunizationAddViewModel AddStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel);
        public StudentMedicalImmunizationAddViewModel UpdateStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel);
        public StudentMedicalImmunizationAddViewModel DeleteStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel);
        public StudentMedicalNurseVisitAddViewModel AddStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel);
        public StudentMedicalNurseVisitAddViewModel UpdateStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel);
        public StudentMedicalNurseVisitAddViewModel DeleteStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel);
        public StudentMedicalProviderAddViewModel AddStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel);
        public StudentMedicalProviderAddViewModel UpdateStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel);
        public StudentMedicalProviderAddViewModel DeleteStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel);
        public StudentMedicalInfoViewModel GetAllStudentMedicalInfo(StudentMedicalInfoViewModel studentMedicalInfoViewModel);
    }
}
