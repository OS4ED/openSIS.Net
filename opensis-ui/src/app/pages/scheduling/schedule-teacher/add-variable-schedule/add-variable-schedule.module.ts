import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { AddVariableScheduleComponent } from './add-variable-schedule.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { IconModule } from '@visurel/iconify-angular';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [AddVariableScheduleComponent],
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
    MatNativeDateModule
  ]
})
export class AddVariableScheduleModule { }
