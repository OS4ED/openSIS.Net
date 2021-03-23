import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StaffDataImportComponent } from './staff-data-import.component';

const routes: Routes = [
  {
    path: '',
    component: StaffDataImportComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffDataImportRoutingModule { }
