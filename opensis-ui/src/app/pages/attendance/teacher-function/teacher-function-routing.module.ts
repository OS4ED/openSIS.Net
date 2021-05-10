import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TakeAttendanceComponent } from './take-attendance/take-attendance.component';
import { TeacherFunctionComponent } from './teacher-function.component';
import { InputFinalGradeComponent } from './input-final-grade/input-final-grade.component';
import { InputEffortGradesComponent } from './input-effort-grades/input-effort-grades.component';
import { GradeDetailsComponent } from './input-final-grade/grade-details/grade-details.component';
import { EffortGradeDetailsComponent } from './input-effort-grades/effort-grade-details/effort-grade-details.component';

const routes: Routes = [
  {
    path: "",
    component: TeacherFunctionComponent,
    children: [
      {
        path: "input-final-grade",
        component: InputFinalGradeComponent,

      },
      {
        path: "grade-details",
        component: GradeDetailsComponent,
      },
      {
        path: "effort-grade-details",
        component: EffortGradeDetailsComponent,
      },
      
      {
        path: "input-effort-grade",
        component: InputEffortGradesComponent,
      },
      {
        path: 'take-attendance',
        loadChildren: () => import('./take-attendance/take-attendance.module').then(m => m.TakeAttendanceModule),
        // canActivate: [AuthGuard]            
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeacherFunctionRoutingModule { }
