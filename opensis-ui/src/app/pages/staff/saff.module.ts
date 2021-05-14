import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SecondaryToolbarModule } from 'src/@vex/components/secondary-toolbar/secondary-toolbar.module';
import { TranslateModule } from '@ngx-translate/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDividerModule } from '@angular/material/divider';
import { MatSortModule } from '@angular/material/sort';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { StaffRoutingModule } from './staff-routing-module';
import { BreadcrumbsModule } from 'src/@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from 'src/@vex/components/page-layout/page-layout.module';
import { ContainerModule } from 'src/@vex/directives/container/container.module';
import { ScrollbarModule } from 'src/@vex/components/scrollbar/scrollbar.module';
import { SharedModuleModule } from '../shared-module/shared-module.module';
import { GradeDetailsModule } from './teacher-function/input-final-grade/grade-details/grade-details.module';
import { EffortGradeDetailsModule } from './teacher-function/input-effort-grades/effort-grade-details/effort-grade-details.module';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StaffRoutingModule,
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
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    SharedModuleModule
  ],
})
export class StaffModule { }
