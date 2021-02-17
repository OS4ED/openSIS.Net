import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CalendarModule as AngularCalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { IconModule } from '@visurel/iconify-angular';
import { MatMenuModule } from '@angular/material/menu';
import { TranslateModule } from '@ngx-translate/core';
import { EditCourseSectionComponent } from './edit-course-section.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MatRadioModule } from '@angular/material/radio';
import { FixedSchedulingComponent } from './fixed-scheduling/fixed-scheduling.component';
import { VariableSchedulingComponent } from './variable-scheduling/variable-scheduling.component';
import { CalendarDaysComponent } from './calendar-days/calendar-days.component';
import { RotatingSchedulingComponent } from './rotating-scheduling/rotating-scheduling.component';
import { SharedModuleModule } from '../../../../pages/shared-module/shared-module.module';

@NgModule({
  declarations: [EditCourseSectionComponent, FixedSchedulingComponent, VariableSchedulingComponent, CalendarDaysComponent, RotatingSchedulingComponent],
  imports: [
    CommonModule,
    AngularCalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    IconModule,
    FlexLayoutModule,
    MatInputModule,
    MatDividerModule,
    MatMenuModule,
    MatFormFieldModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    TranslateModule,
    MatSelectModule,
    FormsModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatRadioModule,
    SharedModuleModule
  ]
})
export class EditCourseSectionModule { }
