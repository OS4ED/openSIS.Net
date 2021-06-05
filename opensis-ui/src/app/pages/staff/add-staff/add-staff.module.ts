import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddStaffComponent } from './add-staff.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule } from '@angular/material/core';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModuleModule } from '../../shared-module/shared-module.module';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatExpansionModule } from '@angular/material/expansion';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
//import { MatButtonToggle, MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { StaffGeneralinfoComponent } from './staff-generalinfo/staff-generalinfo.component';
import { StaffSchoolinfoComponent } from './staff-schoolinfo/staff-schoolinfo.component';
import { StaffLogininfoComponent } from './staff-logininfo/staff-logininfo.component';
import { StaffAddressinfoComponent } from './staff-addressinfo/staff-addressinfo.component';
import { StaffCertificationinfoComponent } from './staff-certificationinfo/staff-certificationinfo.component';
import { ViewstaffCertificationinfoComponent } from './viewstaff-certificationinfo/viewstaff-certificationinfo.component';
import { CustomFieldModule } from '../../../../../src/app/common/custom-field/custom-field.module';
import { CustomFieldWithoutFormModule } from '../../../../../src/app/common/custom-field-without-form/custom-field-without-form.module';
import { ViewStaffGeneralinfoComponent } from './view-staff-generalinfo/view-staff-generalinfo.component';
import { ViewStaffAddressinfoComponent } from './view-staff-addressinfo/view-staff-addressinfo.component';
import { AddStaffRoutingModule } from './add-staff-routing-module';
import { MatDialogModule } from '@angular/material/dialog';



@NgModule({
  declarations: [
    AddStaffComponent,
    StaffGeneralinfoComponent,
    StaffSchoolinfoComponent,
    StaffLogininfoComponent,
    StaffAddressinfoComponent,
    StaffCertificationinfoComponent,
    ViewstaffCertificationinfoComponent,
    ViewStaffGeneralinfoComponent,
    ViewStaffAddressinfoComponent
  ],
  imports: [
    CommonModule,
    AddStaffRoutingModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatRadioModule,
    MatNativeDateModule,
    IconModule,
    MatCardModule,
    MatSnackBarModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    MatSidenavModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    TranslateModule,
    // BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModuleModule,
    MatCheckboxModule,
    MatExpansionModule,
    ScrollbarModule,
    FlexLayoutModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    //MatButtonToggle,
    //MatButtonToggleModule,
    MatTableModule,
    MatTooltipModule,
    MatSlideToggleModule,
    CustomFieldModule,
    CustomFieldWithoutFormModule,
    MatDialogModule
  ]
})
export class AddStaffModule { }
