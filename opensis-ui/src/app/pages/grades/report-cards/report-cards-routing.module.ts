import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ReportCardsComponent } from "./report-cards.component";
const routes: Routes = [
  {
    path: "",
    component: ReportCardsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportCardsRoutingModule {}
