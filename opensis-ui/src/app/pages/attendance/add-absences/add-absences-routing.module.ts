import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddAbsencesComponent } from './add-absences.component';

const routes: Routes = [
  {
    path: '',
    component: AddAbsencesComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AddAbsencesRoutingModule { }
