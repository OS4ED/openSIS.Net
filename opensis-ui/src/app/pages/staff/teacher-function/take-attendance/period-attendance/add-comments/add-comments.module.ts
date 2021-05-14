import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddCommentsComponent } from './add-comments.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { FlexLayoutModule } from '@angular/flex-layout';
import { TranslateModule } from '@ngx-translate/core';
import { MatDividerModule } from '@angular/material/divider';
import { IconModule } from '@visurel/iconify-angular';
import {MatButtonModule} from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [AddCommentsComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatIconModule,
    FlexLayoutModule,
    TranslateModule,
    MatDividerModule,
    IconModule,
    MatButtonModule,
    MatInputModule,
    FormsModule
  ]
})
export class AddCommentsModule { }
