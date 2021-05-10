import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecalculateDailyAttendanceRoutingModule } from './recalculate-daily-attendance-routing.module';
import { RecalculateDailyAttendanceComponent } from './recalculate-daily-attendance.component';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { TranslateModule } from '@ngx-translate/core';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [RecalculateDailyAttendanceComponent],
  imports: [
    CommonModule,
    RecalculateDailyAttendanceRoutingModule,
    MatIconModule,
    IconModule,  
    MatCardModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    ScrollbarModule,
    FlexLayoutModule,
    MatDividerModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatInputModule,
    MatButtonModule,
  ]
})
export class RecalculateDailyAttendanceModule { }
