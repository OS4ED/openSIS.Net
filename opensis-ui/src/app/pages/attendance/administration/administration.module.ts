import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrationRoutingModule } from './administration-routing.module';
import { AdministrationComponent } from './administration.component';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { TranslateModule } from '@ngx-translate/core';
import { ScrollbarModule } from '../../../../@vex/components/scrollbar/scrollbar.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AddCommentsComponent } from './add-comments/add-comments.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { StudentAttendanceCommentComponent } from './student-attendance-comment/student-attendance-comment.component';
import { MatTooltipModule } from '@angular/material/tooltip';


@NgModule({
  declarations: [AdministrationComponent, AddCommentsComponent, StudentAttendanceCommentComponent],
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    MatIconModule,
    IconModule,  
    MatCardModule,
    MatSidenavModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    TranslateModule,
    ScrollbarModule,
    FlexLayoutModule,
    MatTableModule,
    MatPaginatorModule,
    MatDividerModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatTooltipModule
  ]
})
export class AdministrationModule { }
