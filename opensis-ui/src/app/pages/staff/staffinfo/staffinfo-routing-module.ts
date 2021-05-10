import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StaffinfoComponent } from './staffinfo.component';
import { AddStaffComponent } from '../add-staff/add-staff.component';
import { StaffDataImportComponent } from '../staff-data-import/staff-data-import.component';


const routes: Routes = [
  {
    path: '',
    component: StaffinfoComponent
  },
  {
    path: 'staff-bulk-import',
    component: StaffDataImportComponent
  },
  {
    path: 'add-staff',
    component: AddStaffComponent
  },
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffinfoRoutingModule {
}
