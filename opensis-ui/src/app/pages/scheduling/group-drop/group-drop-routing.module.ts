import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GroupDropComponent } from './group-drop.component';



const routes: Routes = [
  {
    path: '',
    component: GroupDropComponent
  },
  
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GroupDropRoutingModule {
}
