import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CourseSectionComponent } from "./course-section/course-section.component";
import { PeriodAttendanceComponent } from "./period-attendance/period-attendance.component";
import { TakeAttendanceComponent } from "./take-attendance.component";

const routes: Routes = [
  {
    path: "",
    component: TakeAttendanceComponent,
  },
  {
    path: "course-section",
    component: CourseSectionComponent,
  },
  {
    path: "period-attendance",
    component: PeriodAttendanceComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TakeAttendanceRoutingModule {}
