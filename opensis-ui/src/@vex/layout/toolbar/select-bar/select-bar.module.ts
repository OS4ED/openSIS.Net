import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SelectBarComponent } from './select-bar.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatRippleModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [SelectBarComponent],
  imports: [
    CommonModule,
    NgxMatSelectSearchModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatRippleModule,
    MatSelectModule,
    MatFormFieldModule,
    ReactiveFormsModule
  ],
 
  exports: [
    MatButtonModule,    
    SelectBarComponent
  ]
})
export class SelectBarModule { }
