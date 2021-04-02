import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentReEnrollComponent } from './student-re-enroll.component';

const routes: Routes = [
  {
    path: '',
    component: StudentReEnrollComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentReEnrollRoutingModule { }
