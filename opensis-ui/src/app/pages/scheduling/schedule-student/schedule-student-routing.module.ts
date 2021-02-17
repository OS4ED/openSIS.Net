import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleStudentComponent } from './schedule-student.component';



const routes: Routes = [
  {
    path: '',
    component: ScheduleStudentComponent
  },
  
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ScheduleStudentRoutingModule {
}
