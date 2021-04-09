import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GradeSettingsComponent } from './grade-settings.component';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {  RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { StandardGradeSetupComponent } from '../../grades/standard-grade-setup/standard-grade-setup.component';
import { UsCommonCoreStandardsComponent } from '../../grades/us-common-core-standards/us-common-core-standards.component';
import { SchoolSpecificStandardsComponent } from '../../grades/school-specific-standards/school-specific-standards.component';
import { EffortGradeSetupComponent } from '../../grades/effort-grade-setup/effort-grade-setup.component';
import { EffortGradeLibraryComponent } from '../../grades/effort-grade-library/effort-grade-library.component';
import { EffortGradeScaleComponent } from '../../grades/effort-grade-scale/effort-grade-scale.component';
import { ReportCardGradesComponent } from '../../grades/report-card-grades/report-card-grades.component';
import { HonorRollSetupComponent } from '../../grades/honor-roll-setup/honor-roll-setup.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { SharedModuleModule } from '../../../pages/shared-module/shared-module.module';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ReportCardCommentsComponent } from '../../grades/report-card-comments/report-card-comments.component';
import { StandardGradesComponent } from '../../grades/standard-grades/standard-grades.component';



@NgModule({
  declarations: [
    GradeSettingsComponent,
    StandardGradeSetupComponent,
    UsCommonCoreStandardsComponent,
    SchoolSpecificStandardsComponent,
    EffortGradeSetupComponent,
    EffortGradeLibraryComponent,
    EffortGradeScaleComponent,
    ReportCardGradesComponent,
    HonorRollSetupComponent,
    ReportCardCommentsComponent,
    StandardGradesComponent
  ],
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
    BrowserAnimationsModule,
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
    DragDropModule
  ]
})
export class GradeSettingsModule { }
