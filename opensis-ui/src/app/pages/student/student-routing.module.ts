import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddStudentComponent } from './add-student/add-student.component';
import { StudentDataImportComponent } from './student-data-import/student-data-import.component';
import { StudentComponent } from './student.component';

const routes: Routes = [
  {
    path:'',
    component: StudentComponent,
    children: [
      {
        path:'student-data-import',
        component: StudentDataImportComponent
       }
    ]
},
{
  path:'',
  loadChildren: () => import('./add-student/add-student.module').then(m => m.AddStudentModule),
},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
