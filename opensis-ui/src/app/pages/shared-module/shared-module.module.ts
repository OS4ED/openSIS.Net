import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSpinnerOverlayComponent } from './mat-spinner-overlay/mat-spinner-overlay.component';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { ProfileImageComponent } from './profile-image/profile-image.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { IconModule } from '@visurel/iconify-angular';
import { PhoneMaskDirective } from './directives/phone-mask.directive';
import { EmtyBooleanCheckPipe } from './user-define-pipe/emty-boolean-check-pipe';
import { EmtyValueCheckPipe } from './user-define-pipe/emty-value-check.pipe';
import {EmtyNumberCheckPipe} from './user-define-pipe/emty-number-check.pipe';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { MatCardModule } from '@angular/material/card';
import { TranslateModule } from '@ngx-translate/core';
import {InvalidControlScrollDirective} from './user-defined-directives/invalid-control-scroll.directive';
import {EmtyBooleanCheckReversePipe} from './user-define-pipe/emty-boolean-check.reverse.pipe';
import { SafePipe } from './user-define-pipe/safeHtml.pipe';
import { TransformDateTimePipe } from './user-define-pipe/transform-datetime-pipe';
import {EmailvalidatorDirective} from './user-defined-directives/emailvalidator.directive';
import {PhonevalidatorDirective} from './user-defined-directives/phonevalidator.directive';
import { AgePipe } from './user-define-pipe/age-calculator.pipe';
import { TransformTimePipe } from './user-define-pipe/transfrom-time.pipe';
import { EvenOddPipe } from './user-define-pipe/even-odd.pipe';
import { WeekDayPipe } from './user-define-pipe/number-to-week-day.pipe';
import { NgForFilterPipe } from './user-define-pipe/course-section-ngFor-div-filter.pipe';
import { Transform24to12Pipe } from './user-define-pipe/transform-24to12.pipe';

@NgModule({
  declarations: [MatSpinnerOverlayComponent, ProfileImageComponent,PhoneMaskDirective,EmtyBooleanCheckPipe,EmtyBooleanCheckReversePipe,
    EmtyValueCheckPipe,EmtyNumberCheckPipe, ConfirmDialogComponent,InvalidControlScrollDirective,TransformDateTimePipe,TransformTimePipe,EmailvalidatorDirective,
    PhonevalidatorDirective,SafePipe,AgePipe,EvenOddPipe,WeekDayPipe,NgForFilterPipe,Transform24to12Pipe],
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    ImageCropperModule,
    MatSnackBarModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    IconModule,
    MatCardModule,
    TranslateModule,
    
  ],
  exports:[MatSpinnerOverlayComponent, ProfileImageComponent,PhoneMaskDirective,EmtyBooleanCheckPipe,EmtyValueCheckPipe,EmtyNumberCheckPipe,InvalidControlScrollDirective,
    EmtyBooleanCheckReversePipe,TransformDateTimePipe,TransformTimePipe,EmailvalidatorDirective,PhonevalidatorDirective,SafePipe,AgePipe,EvenOddPipe,WeekDayPipe,NgForFilterPipe,Transform24to12Pipe]
})
export class SharedModuleModule { }
