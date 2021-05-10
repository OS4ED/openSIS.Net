import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'
import { CommonModule } from '@angular/common';
import { AddSchoolComponent } from './add-school.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import{GeneralInfoComponent} from '../add-school/general-info/general-info.component';
import{WashInfoComponent} from '../add-school/wash-info/wash-info.component';
import { TranslateModule } from '@ngx-translate/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModuleModule } from '../../shared-module/shared-module.module';
import { CustomFieldModule } from "../../../common/custom-field/custom-field.module";
import { CustomFieldWithoutFormModule } from "../../../common/custom-field-without-form/custom-field-without-form.module";
import { ViewGeneralInfoComponent } from './view-general-info/view-general-info.component';
import { ViewWashInfoComponent } from './view-wash-info/view-wash-info.component';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { AddCopySchoolComponent } from './add-copy-school/add-copy-school.component';
import { MatDividerModule } from '@angular/material/divider';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { SuccessCopySchoolComponent } from './success-copy-school/success-copy-school.component';


@NgModule({
  declarations: [AddSchoolComponent,GeneralInfoComponent,WashInfoComponent, ViewGeneralInfoComponent, ViewWashInfoComponent, AddCopySchoolComponent, SuccessCopySchoolComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatCheckboxModule,
    MatSelectModule,
    IconModule,
    MatButtonModule,   
    MatCardModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatFormFieldModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    TranslateModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModuleModule,
    CustomFieldModule,
    CustomFieldWithoutFormModule,
    ScrollbarModule,
    MatDividerModule,
    MatDialogModule,
    MatSlideToggleModule
  ]
})
export class AddSchoolModule { }
