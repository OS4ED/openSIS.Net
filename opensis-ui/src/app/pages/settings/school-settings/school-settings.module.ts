import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SchoolSettingsComponent } from './school-settings.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { IconModule } from '@visurel/iconify-angular';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SecondaryToolbarModule } from '../../../../@vex/components/secondary-toolbar/secondary-toolbar.module';
import { BreadcrumbsModule } from '../../../../@vex/components/breadcrumbs/breadcrumbs.module';
import { PageLayoutModule } from '../../../../@vex/components/page-layout/page-layout.module';
import { ContainerModule } from '../../../../@vex/directives/container/container.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TranslateModule } from '@ngx-translate/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {  RouterModule } from '@angular/router';

import { MatTableModule } from '@angular/material/table';
import { GradeLevelsComponent } from '../../school/grade-levels/grade-levels.component';
import { SectionsComponent } from '../../school/sections/sections.component';
import { RoomsComponent } from '../../school/rooms/rooms.component';
import { PeriodsComponent } from '../../school/periods/periods.component';
import { PeriodsListComponent } from '../../school/periods/periods-list/periods-list.component';
import { SchoolFieldsComponent } from '../../school/school-fields/school-fields.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import { SharedModuleModule } from '../../../pages/shared-module/shared-module.module';
import {DragDropModule } from '@angular/cdk/drag-drop';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { PreferenceComponent } from '../../school/preference/preference.component';
import { MatDividerModule } from '@angular/material/divider';


@NgModule({
  declarations: [SchoolSettingsComponent,GradeLevelsComponent,SectionsComponent, RoomsComponent,SchoolFieldsComponent, PeriodsComponent, PeriodsListComponent, PreferenceComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    IconModule,
    MatButtonModule,   
    MatCardModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatFormFieldModule,
    SecondaryToolbarModule,
    BreadcrumbsModule,
    PageLayoutModule,
    ContainerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    TranslateModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    SharedModuleModule,
    MatDialogModule,
    MatMenuModule,
    MatSortModule,
    MatCheckboxModule,
    MatPaginatorModule,
    DragDropModule,
    MatSlideToggleModule,
    MatDividerModule
  ]
})
export class SchoolSettingsModule { }
