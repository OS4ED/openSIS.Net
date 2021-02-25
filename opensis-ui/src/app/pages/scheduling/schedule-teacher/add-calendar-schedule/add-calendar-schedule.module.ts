import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddCalendarScheduleComponent } from './add-calendar-schedule.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { TranslateModule } from '@ngx-translate/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';



@NgModule({
  declarations: [AddCalendarScheduleComponent],
  imports: [
    CommonModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    MatDividerModule,
    MatDialogModule,
    MatIconModule,
    IconModule,
    TranslateModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatNativeDateModule
  ]
})
export class AddCalendarScheduleModule { }
