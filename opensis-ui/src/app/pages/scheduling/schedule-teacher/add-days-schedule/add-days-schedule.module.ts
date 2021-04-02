import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { TranslateModule } from '@ngx-translate/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { AddDaysScheduleComponent } from './add-days-schedule.component';
import {SharedModuleModule} from '../../../shared-module/shared-module.module';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';


@NgModule({
  declarations: [AddDaysScheduleComponent],
  imports: [
    CommonModule,
    MatDividerModule,
    MatDialogModule,
    MatIconModule,
    IconModule,
    TranslateModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatNativeDateModule,
    SharedModuleModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ]
})
export class AddDaysScheduleModule { }
