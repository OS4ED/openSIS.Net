import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddCourseSectionComponent } from './add-course-section.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { IconModule } from '@visurel/iconify-angular';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { TranslateModule } from '@ngx-translate/core';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import {MatTooltipModule} from '@angular/material/tooltip';



@NgModule({
  declarations: [AddCourseSectionComponent],
  imports: [
    CommonModule,
    MatTableModule,
    MatSelectModule,
    MatInputModule,
    MatDividerModule,
    TranslateModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatButtonModule,
    IconModule,
    MatDatepickerModule,
    FlexLayoutModule,
    MatIconModule,
    MatDialogModule,
    MatTooltipModule
  ]
})
export class AddCourseSectionModule { }
