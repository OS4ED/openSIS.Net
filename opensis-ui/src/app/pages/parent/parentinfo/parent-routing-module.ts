import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParentinfoComponent } from './parentinfo.component';
import { EditParentComponent } from '../edit-parent/edit-parent.component';


const routes: Routes = [
 {
     path:'',
     component: ParentinfoComponent
 },
  {
    path:'',
    loadChildren: () => import('../edit-parent/edit-parent.module').then(m => m.EditParentModule),
   }, 
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ParentRoutingModule {
}
