import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeacherDashboardRoutingModule } from './teacher-dashboard-routing.module';
import { TeacherDashboardComponent } from './teacher-dashboard.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IconModule } from '@visurel/iconify-angular';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { MatButtonModule } from '@angular/material/button';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { SharedModuleModule } from '../../shared-module/shared-module.module';

@NgModule({
  declarations: [TeacherDashboardComponent],
  imports: [
    CommonModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    TeacherDashboardRoutingModule,
    FlexLayoutModule,
    MatIconModule,
    MatTooltipModule,
    IconModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    MatButtonModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    MatExpansionModule,
    MatDividerModule,
    SharedModuleModule
  ]
})
export class TeacherDashboardModule { }
