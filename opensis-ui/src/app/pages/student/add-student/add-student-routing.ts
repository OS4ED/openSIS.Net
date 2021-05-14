import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomFieldComponent } from '../../../common/custom-field/custom-field.component';
import { AddStudentComponent } from './add-student.component';
import { StudentAddressandcontactsComponent } from './student-addressandcontacts/student-addressandcontacts.component';
import { StudentAttendanceComponent } from './student-attendance/student-attendance.component';
import { StudentCommentsComponent } from './student-comments/student-comments.component';
import { StudentCourseScheduleComponent } from './student-course-schedule/student-course-schedule.component';
import { StudentDocumentsComponent } from './student-documents/student-documents.component';
import { StudentEnrollmentinfoComponent } from './student-enrollmentinfo/student-enrollmentinfo.component';
import { StudentFamilyinfoComponent } from './student-familyinfo/student-familyinfo.component';
import { StudentGeneralinfoComponent } from './student-generalinfo/student-generalinfo.component';
import { StudentMedicalinfoComponent } from './student-medicalinfo/student-medicalinfo.component';
import { StudentReportCardComponent } from './student-report-card/student-report-card.component';
import { StudentTranscriptComponent } from './student-transcript/student-transcript.component';

const routes: Routes = [
  {
    path:'',
    component: AddStudentComponent,
    children: [
      {
        path:'student-generalinfo',
        component: StudentGeneralinfoComponent
      },
      {
        path:'student-address-contact',
        component: StudentAddressandcontactsComponent
      },
      {
        path:'student-enrollmentinfo',
        component: StudentEnrollmentinfoComponent
      },
      {
        path:'student-familyinfo',
        component: StudentFamilyinfoComponent
      },
      {
        path:'student-medicalinfo',
        component: StudentMedicalinfoComponent
      },
      {
        path:'student-comments',
        component: StudentCommentsComponent
      },
      {
        path:'student-documents',
        component: StudentDocumentsComponent
      },
      {
        path:'student-course-schedule',
        component: StudentCourseScheduleComponent
      },
      {
        path:'student-attendance',
        component: StudentAttendanceComponent
      },
      {
        path:'student-transcript',
        component: StudentTranscriptComponent
      },
      {
        path:'student-report-card',
        component: StudentReportCardComponent
      },
      {
        path:'custom/:type',
        component: CustomFieldComponent
      }
    ]
},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AddStudentRoutingModule {
}
