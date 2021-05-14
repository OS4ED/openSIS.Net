import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MissingAttendanceListComponent } from './attendance/missing-attendance-list/missing-attendance-list.component';
import { ClassComponent } from './class.component';

const routes: Routes = [
  {
    path: '',
    component: ClassComponent
  },
  {
    path: "attendance/missing-attendance-list",
    component: MissingAttendanceListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClassRoutingModule { }
