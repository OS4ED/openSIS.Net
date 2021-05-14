import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomFieldComponent } from '../../../common/custom-field/custom-field.component';
import { AddStaffComponent } from './add-staff.component';
import { StaffAddressinfoComponent } from './staff-addressinfo/staff-addressinfo.component';
import { StaffCertificationinfoComponent } from './staff-certificationinfo/staff-certificationinfo.component';
import { StaffGeneralinfoComponent } from './staff-generalinfo/staff-generalinfo.component';
import { StaffSchoolinfoComponent } from './staff-schoolinfo/staff-schoolinfo.component';


const routes: Routes = [
  {
    path: '',
    component: AddStaffComponent,
    children: [
  {
    path: 'staff-generalinfo',
    component: StaffGeneralinfoComponent
  },
  {
    path: 'staff-schoolinfo',
    component: StaffSchoolinfoComponent
  },
  {
    path: 'staff-addressinfo',
    component: StaffAddressinfoComponent
  },
  {
    path: 'staff-certificationinfo',
    component: StaffCertificationinfoComponent
  },
  {
    path: 'custom/:type',
    component: CustomFieldComponent
  },
]
}];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AddStaffRoutingModule {
}
