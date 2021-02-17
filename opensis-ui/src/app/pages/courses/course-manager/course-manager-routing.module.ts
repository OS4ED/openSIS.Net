import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CourseManagerComponent } from './course-manager.component';
import { CalendarDaysComponent } from './edit-course-section/calendar-days/calendar-days.component';
import { FixedSchedulingComponent } from './edit-course-section/fixed-scheduling/fixed-scheduling.component';
import { RotatingSchedulingComponent } from './edit-course-section/rotating-scheduling/rotating-scheduling.component';
import { VariableSchedulingComponent } from './edit-course-section/variable-scheduling/variable-scheduling.component';



const routes: Routes = [
  {
    path: '',
    component: CourseManagerComponent,
    children:[{
      path:  'variable-scheduling',
      component:  VariableSchedulingComponent
    },
    {
    path:  'fixed-scheduling',
    component:  FixedSchedulingComponent
    },
    {
      path:  'rotating-scheduling',
      component:  RotatingSchedulingComponent
    },
    {
      path:  'calendar-days',
      component:  CalendarDaysComponent
    }]

  },
   
      
           
  
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CourseManagerRoutingModule {
}
