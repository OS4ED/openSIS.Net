import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SessionExpireAlertComponent } from '../session-expire/session-expire-alert/session-expire-alert.component';
import { MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [SessionExpireAlertComponent],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    TranslateModule
  ]
})
export class SessionExpireModule { }
