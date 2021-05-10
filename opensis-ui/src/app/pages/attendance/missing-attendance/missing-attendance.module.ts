import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { MissingAttendanceRoutingModule } from "./missing-attendance-routing.module";
import { MissingAttendanceComponent } from "./missing-attendance.component";
import { MissingAttendanceDetailsComponent } from "./missing-attendance-details/missing-attendance-details.component";
import { TakeAttendanceComponent } from "./take-attendance/take-attendance.component";
import { SecondaryToolbarModule } from "../../../../@vex/components/secondary-toolbar/secondary-toolbar.module";
import { BreadcrumbsModule } from "../../../../@vex/components/breadcrumbs/breadcrumbs.module";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { PageLayoutModule } from "../../../../@vex/components/page-layout/page-layout.module";
import { ContainerModule } from "../../../../@vex/directives/container/container.module";
import { TranslateModule } from "@ngx-translate/core";
import { MatDividerModule } from "@angular/material/divider";
import { MatInputModule } from "@angular/material/input";
import { MatDialogModule } from "@angular/material/dialog";
import { MatSelectModule } from "@angular/material/select";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatMenuModule } from "@angular/material/menu";
import { MatCardModule } from "@angular/material/card";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatTableModule } from "@angular/material/table";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatSortModule } from "@angular/material/sort";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { IconModule } from "@visurel/iconify-angular";
import { RouterModule } from "@angular/router";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { FlexLayoutModule } from "@angular/flex-layout";
import { AddTeacherCommentsComponent } from "./take-attendance/add-teacher-comments/add-teacher-comments.component";

@NgModule({
  declarations: [
    MissingAttendanceComponent,
    MissingAttendanceDetailsComponent,
    TakeAttendanceComponent,
    AddTeacherCommentsComponent
  ],
  imports: [
    CommonModule,
    MissingAttendanceRoutingModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    MatIconModule,
    MatButtonModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    MatDividerModule,
    MatInputModule,
    MatDialogModule,
    MatSelectModule,
    MatCheckboxModule,
    MatMenuModule,
    MatCardModule,
    MatPaginatorModule,
    MatTableModule,
    MatButtonToggleModule,
    MatSortModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    IconModule,
    RouterModule,
    MatSlideToggleModule,
    FlexLayoutModule
  ],
})
export class MissingAttendanceModule {}
