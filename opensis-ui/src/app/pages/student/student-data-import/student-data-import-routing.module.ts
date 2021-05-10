import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentDataImportComponent } from './student-data-import.component';

const routes: Routes = [
  {
    path: '',
    component: StudentDataImportComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentDataImportRoutingModule { }
