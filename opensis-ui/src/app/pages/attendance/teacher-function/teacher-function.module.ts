import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherFunctionRoutingModule } from './teacher-function-routing.module';
import { TeacherFunctionComponent } from './teacher-function.component';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SecondaryToolbarModule } from 'src/@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { TranslateModule } from '@ngx-translate/core';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDividerModule } from '@angular/material/divider';
import { InputFinalGradeComponent } from './input-final-grade/input-final-grade.component';
import { InputEffortGradesComponent } from './input-effort-grades/input-effort-grades.component';
import { EffortGradeDetailsModule } from './input-effort-grades/effort-grade-details/effort-grade-details.module';
import { GradeDetailsModule } from './input-final-grade/grade-details/grade-details.module';
import { MatSortModule } from '@angular/material/sort';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [TeacherFunctionComponent, InputFinalGradeComponent, InputEffortGradesComponent],
  imports: [
    CommonModule,
    TeacherFunctionRoutingModule,
    MatIconModule,
    IconModule,  
    MatCardModule,
    MatSidenavModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    ScrollbarModule,
    FlexLayoutModule,
    MatTableModule,
    MatPaginatorModule,
    MatDividerModule,
    GradeDetailsModule,
    EffortGradeDetailsModule,
    MatSortModule,
    FormsModule,
    ReactiveFormsModule
  ],
})
export class TeacherFunctionModule { }
