import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./staffinfo/staffinfo.module').then(m => m.StaffinfoModule),
  },
  {
    path: 'teacher-functions',
    loadChildren: () => import('./teacher-function/teacher-function.module').then(m => m.TeacherFunctionModule),
  },
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StaffRoutingModule {
}
