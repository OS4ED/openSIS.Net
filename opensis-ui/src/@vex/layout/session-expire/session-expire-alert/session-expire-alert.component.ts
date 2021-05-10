import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserViewModel } from '../../../../app/models/user.model';
import { SessionService } from '../../../services/session.service';
import { interval, Subscription } from 'rxjs';
import * as jwt_decode from 'jwt-decode';
import { DefaultValuesService } from '../../../../app/common/default-values.service';

@Component({
  selector: 'vex-session-expire-alert',
  templateUrl: './session-expire-alert.component.html',
  styleUrls: ['./session-expire-alert.component.scss']
})
export class SessionExpireAlertComponent implements OnInit, OnDestroy {

  count: number = 30;
  searchTimer: Subscription;
  sessionRenewLoader: boolean = false;
  decoded;
  constructor(private router: Router,
    public dialogRef: MatDialogRef<SessionExpireAlertComponent>,
    @Inject(MAT_DIALOG_DATA) public data,
    private sessionService: SessionService,
    public translateService: TranslateService,
    private defaultValueService:DefaultValuesService,
    private dialog: MatDialog) {
    translateService.use('en');
  }

  ngOnInit() {
    let tokenExpired=false;
    this.decoded = JSON.parse(JSON.stringify(jwt_decode.default(sessionStorage.getItem('token'))));

    const source = interval(1000);
    this.searchTimer = source.subscribe(() => {
      if(tokenExpired) {
        this.logout();
        return;
      }

      if (this.count > 1) {
        this.count--;
        tokenExpired = this.checkToken()
      }
      else if (this.count === 1) {
        this.dialogRef.close(null);
        this.dialog.closeAll();
      }
    });

  }

  checkToken(){
    const tokenExpired = Date.now() > (this.decoded.exp * 1000-120000);
    return tokenExpired;
  }

  logout() {
    sessionStorage.clear();
    localStorage.clear();
    sessionStorage.setItem('tenant',this.defaultValueService.getDefaultTenant());
    let a =sessionStorage.setItem('tenant',this.defaultValueService.getDefaultTenant());
    console.log(a);
    this.dialog.closeAll();
    this.router.navigateByUrl('/');
  }

  continue() {
    this.sessionRenewLoader = true;
    const loginViewModel: UserViewModel = new UserViewModel();
    this.sessionService.RefreshToken(loginViewModel).subscribe(res => {
      this.sessionRenewLoader = false;
      this.dialogRef.close(res._token);
    });
  }

  onCloseModal() {
    this.dialogRef.close();
  }

  ngOnDestroy() {
    this.searchTimer.unsubscribe()
  }

}
