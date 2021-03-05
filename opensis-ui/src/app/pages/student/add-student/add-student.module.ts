import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddStudentComponent } from './add-student.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule } from '@angular/material/core';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { StudentGeneralinfoComponent } from '../add-student/student-generalinfo/student-generalinfo.component';
import { StudentAddressandcontactsComponent } from '../add-student/student-addressandcontacts/student-addressandcontacts.component';
import { StudentEnrollmentinfoComponent } from '../add-student/student-enrollmentinfo/student-enrollmentinfo.component';
import { StudentFamilyinfoComponent } from '../add-student/student-familyinfo/student-familyinfo.component';
import { StudentLogininfoComponent } from '../add-student/student-logininfo/student-logininfo.component';
import { StudentMedicalinfoComponent } from '../add-student/student-medicalinfo/student-medicalinfo.component';
import { StudentCommentsComponent } from '../add-student/student-comments/student-comments.component';
import { StudentDocumentsComponent } from '../add-student/student-documents/student-documents.component';
import { StudentContactsComponent } from '../add-student/student-familyinfo/student-contacts/student-contacts.component';
import { SiblingsinfoComponent } from '../add-student/student-familyinfo/siblingsinfo/siblingsinfo.component';
import { TranslateModule } from '@ngx-translate/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModuleModule } from '../../shared-module/shared-module.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatExpansionModule } from '@angular/material/expansion';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ViewstudentLogininfoComponent } from './viewstudent-logininfo/viewstudent-logininfo.component';
import { CustomFieldModule } from '../../../../../src/app/common/custom-field/custom-field.module';
import { CustomFieldWithoutFormModule } from '../../../../../src/app/common/custom-field-without-form/custom-field-without-form.module';
import {MatTabsModule} from '@angular/material/tabs';
import { ViewStudentGeneralinfoComponent } from './view-student-generalinfo/view-student-generalinfo.component';
import { ViewStudentAddressandcontactsComponent } from './view-student-addressandcontacts/view-student-addressandcontacts.component';
import { DatePipe } from '@angular/common';
import { StudentCourseScheduleComponent } from '../add-student/student-course-schedule/student-course-schedule.component';
import { AddAssignCourseComponent } from './student-course-schedule/add-assign-course/add-assign-course.component';
import { MatDividerModule } from '@angular/material/divider';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

@NgModule({
  declarations: [
    AddStudentComponent,
    StudentGeneralinfoComponent,
    StudentAddressandcontactsComponent,
    StudentEnrollmentinfoComponent,
    StudentFamilyinfoComponent,
    StudentLogininfoComponent,
    StudentMedicalinfoComponent,
    StudentCommentsComponent,
    StudentDocumentsComponent,
    StudentContactsComponent,
    SiblingsinfoComponent,
    ViewstudentLogininfoComponent,
    ViewStudentGeneralinfoComponent,
    ViewStudentAddressandcontactsComponent,
    StudentCourseScheduleComponent,
    AddAssignCourseComponent
  ],
  imports: [
    CommonModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    IconModule,
    MatButtonModule,   
    MatCardModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatFormFieldModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    MatDatepickerModule,
    MatRadioModule,
    MatNativeDateModule,
    TranslateModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModuleModule,
    MatCheckboxModule,
    MatExpansionModule,
    ScrollbarModule,
    FlexLayoutModule,
    NgxDropzoneModule,
    MatTableModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonToggleModule,
    MatTooltipModule,
    MatSlideToggleModule,
    CustomFieldModule,
    CustomFieldWithoutFormModule,
    MatTabsModule,
    MatDividerModule
  ],
  providers: [
    DatePipe
  ]
})
export class AddStudentModule { }
