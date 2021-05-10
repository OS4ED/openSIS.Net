import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TeacherDashboardComponent } from './teacher-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: TeacherDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherDashboardRoutingModule { }
