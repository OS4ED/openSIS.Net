import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TranscriptsRoutingModule } from './transcripts-routing.module';
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
import {  RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { TranscriptsComponent } from './transcripts.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SharedModuleModule } from '../../shared-module/shared-module.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatExpansionModule } from '@angular/material/expansion';
import { ReportCardsModule } from '../report-cards/report-cards.module';

@NgModule({
  declarations: [
    TranscriptsComponent
    ],
  imports: [
    CommonModule,
    TranscriptsRoutingModule,
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
    MatButtonToggleModule,
    DragDropModule,
    MatSlideToggleModule,
    MatDividerModule,
    NgxDocViewerModule,
    MatExpansionModule,
    MatProgressSpinnerModule,
    ReportCardsModule
  ]
})
export class TranscriptsModule { }
