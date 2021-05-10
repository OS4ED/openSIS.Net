import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RecalculateDailyAttendanceComponent } from './recalculate-daily-attendance.component';

const routes: Routes = [
  {
    path: '',
    component: RecalculateDailyAttendanceComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RecalculateDailyAttendanceRoutingModule { }
