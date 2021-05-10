import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentComponent } from './student.component';
import { AddStudentComponent } from '../add-student/add-student.component';
import { StudentDataImportComponent } from '../student-data-import/student-data-import.component';


const routes: Routes = [
 {
     path:'',
     component: StudentComponent
 },
 {path:'student-generalinfo',component:AddStudentComponent},
 {
  path:'student-data-import',
  component: StudentDataImportComponent
}
 
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule {
}
