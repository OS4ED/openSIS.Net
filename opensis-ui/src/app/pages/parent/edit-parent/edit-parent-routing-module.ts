import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditParentComponent } from './edit-parent.component';
import { EditparentAddressinfoComponent } from './editparent-addressinfo/editparent-addressinfo.component';
import { EditparentGeneralinfoComponent } from './editparent-generalinfo/editparent-generalinfo.component';

const routes: Routes = [
  {
    path: '',
    component: EditParentComponent,
    children: [
      {
        path: 'parent-generalinfo',
        component: EditparentGeneralinfoComponent
      },
      {
        path: 'parent-addressinfo',
        component: EditparentAddressinfoComponent
      },
]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditParentRoutingModule {
}
