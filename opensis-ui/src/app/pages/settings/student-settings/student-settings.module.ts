import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentSettingsComponent } from './student-settings.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
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
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {  RouterModule } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { EnrollmentCodesComponent } from '../../student/enrollment-codes/enrollment-codes.component';
import { StudentFieldsComponent } from '../../student/student-fields/student-fields.component';
// import { RoomsComponent } from '../../school/rooms/rooms.component';
// import { SchoolFieldsComponent } from '../../school/school-fields/school-fields.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import { SharedModuleModule } from '../../../pages/shared-module/shared-module.module';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import {DragDropModule } from '@angular/cdk/drag-drop';
import { StudentDataImportModule } from '../../student/student-data-import/student-data-import.module';

@NgModule({
  declarations: [StudentSettingsComponent, EnrollmentCodesComponent, StudentFieldsComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
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
    MatNativeDateModule,
    TranslateModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    SharedModuleModule,
    MatDialogModule,
    MatMenuModule,
    MatSortModule,
    MatCheckboxModule,
    MatButtonToggleModule,
    DragDropModule,
    StudentDataImportModule
  ]
})
export class StudentSettingsModule { }
