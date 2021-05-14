import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { EffortGradeDetailsComponent } from "./effort-grade-details/effort-grade-details.component";
import { EffortGradeDetailsModule } from "./effort-grade-details/effort-grade-details.module";
import { InputEffortGradesComponent } from "./input-effort-grades.component";



const routes: Routes = [
  {
    path: "",
    component: InputEffortGradesComponent,
  },
  {
    path: "effort-grade-details",
    component: EffortGradeDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InputEffortGradesRoutingModule {}
