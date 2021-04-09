import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherReassignmentRoutingModule } from './teacher-reassignment-routing.module';
import { TeacherReassignmentComponent } from './teacher-reassignment.component';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TranslateModule } from '@ngx-translate/core';
import { SharedModuleModule } from '../../shared-module/shared-module.module';
import { MatRadioModule } from '@angular/material/radio';



@NgModule({
  declarations: [TeacherReassignmentComponent],
  imports: [
    CommonModule,
    TeacherReassignmentRoutingModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    MatIconModule,
    MatButtonModule,
    PageLayoutModule,
    ContainerModule,
    MatCheckboxModule,
    MatTooltipModule,
    TranslateModule,
    SharedModuleModule,
    MatRadioModule
    
  ]
})
export class TeacherReassignmentModule { }
