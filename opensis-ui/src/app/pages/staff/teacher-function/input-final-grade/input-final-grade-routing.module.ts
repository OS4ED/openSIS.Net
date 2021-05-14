import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { GradeDetailsComponent } from "./grade-details/grade-details.component";
import { InputFinalGradeComponent } from "./input-final-grade.component";


const routes: Routes = [
  {
    path: "",
    component: InputFinalGradeComponent,
  },
  {
    path: "grade-details",
    component: GradeDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InputFinalGradeRoutingModule {}
