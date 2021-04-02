import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScheduleTeacherComponent } from './schedule-teacher.component';
import { ScheduleTeacherRoutingModule } from './schedule-teacher-routing.module';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {SharedModuleModule} from '../../shared-module/shared-module.module';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@NgModule({
  declarations: [ScheduleTeacherComponent],
  imports: [
    CommonModule,
    ScheduleTeacherRoutingModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    MatIconModule,
    MatButtonModule,
    PageLayoutModule,
    ContainerModule,
    IconModule,
    MatCardModule,
    MatSnackBarModule,
    MatCheckboxModule,
    MatMenuModule,
    TranslateModule,
    MatTooltipModule,
    MatSlideToggleModule,
    SharedModuleModule,
    MatProgressSpinnerModule
  ]
})
export class ScheduleTeacherModule { }
