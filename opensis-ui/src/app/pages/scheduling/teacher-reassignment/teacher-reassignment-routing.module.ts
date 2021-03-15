import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeacherReassignmentComponent } from './teacher-reassignment.component';

const routes: Routes = [
  {
    path: '',
    component: TeacherReassignmentComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherReassignmentRoutingModule { }
