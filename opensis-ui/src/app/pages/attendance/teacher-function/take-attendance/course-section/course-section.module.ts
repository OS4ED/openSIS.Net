import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseSectionComponent } from './course-section.component';
import { SecondaryToolbarModule } from '../../../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PageLayoutModule } from '../../../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../../../@vex/directives/container/container.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { RouterModule } from '@angular/router';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { TruncatePipe } from '../../../../../pipe/truncate.pipe';
import { SharedModuleModule } from '../../../../shared-module/shared-module.module';



@NgModule({
  declarations: [
    CourseSectionComponent,
    TruncatePipe
  ],
  imports: [
    CommonModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    SecondaryToolbarModule,
    BreadcrumbsModule,
    MatIconModule,
    MatButtonModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    MatDividerModule,
    MatInputModule,
    RouterModule,
    MatCheckboxModule,
    SharedModuleModule
  ]
})
export class CourseSectionModule { }
