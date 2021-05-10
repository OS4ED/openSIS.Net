import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MissingAttendanceDetailsComponent } from './missing-attendance-details/missing-attendance-details.component';
import { MissingAttendanceComponent } from './missing-attendance.component';
import { TakeAttendanceComponent } from './take-attendance/take-attendance.component';

const routes: Routes = [
  {
    path: "",
    component: MissingAttendanceComponent,
  },
  {
    path: "missing-attendance-details",
    component: MissingAttendanceDetailsComponent,
  },
  {
    path: "take-attendance",
    component: TakeAttendanceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MissingAttendanceRoutingModule { }
