import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionExpireAlertComponent } from '../session-expire/session-expire-alert/session-expire-alert.component';
import { MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [SessionExpireAlertComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    TranslateModule,
    MatProgressSpinnerModule,
    MatIconModule
  ]
})
export class SessionExpireModule { }
