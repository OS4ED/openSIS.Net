import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomFieldComponent } from 'src/app/common/custom-field/custom-field.component';
import { AddSchoolComponent } from './add-school.component';
import { GeneralInfoComponent } from './general-info/general-info.component';
import { WashInfoComponent } from './wash-info/wash-info.component';


const routes: Routes = [
 {
     path: '',
     component: AddSchoolComponent,
     children: [
       {
         path: 'generalinfo',
         component: GeneralInfoComponent
       },
       {
        path: 'washinfo',
        component: WashInfoComponent
      },
      {
        path:'custom/:type',
        component: CustomFieldComponent
      }
     ]
 },
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AddSchoolRoutingModule {
}
