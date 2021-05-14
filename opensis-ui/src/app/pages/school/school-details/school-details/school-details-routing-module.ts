import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SchoolDetailsComponent } from './school-details.component';
import { AddSchoolComponent } from '../../add-school/add-school.component';




const routes: Routes = [
  {
    path:'',
    component: SchoolDetailsComponent
},
{
 path:'',
 loadChildren: () => import('../../add-school/add-school.module').then(m => m.AddSchoolModule),
}, 
 
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SchoolDetailsRoutingModule {
}
