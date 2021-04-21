import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserViewModel } from '../../../../app/models/user.model';
import { SessionService } from '../../../services/session.service';

@Component({
  selector: 'vex-session-expire-alert',
  templateUrl: './session-expire-alert.component.html',
  styleUrls: ['./session-expire-alert.component.scss']
})
export class SessionExpireAlertComponent implements OnInit,OnDestroy {

  count: number = 30;
  searchTimer:any;

  constructor(private router: Router,
    public dialogRef: MatDialogRef<SessionExpireAlertComponent>,
      @Inject(MAT_DIALOG_DATA) public data,
      private sessionService:SessionService,
      public translateService: TranslateService,
      private dialog: MatDialog) {
        translateService.use('en');
     }

  ngOnInit() {
    
    this.searchTimer = setInterval(() => {
      if(this.count >1){
      this.count--;
      }
      else if(this.count ===1){
        this.dialogRef.close(null)
      }
    }, 1000);
  }

  logout() {
    sessionStorage.clear();
    localStorage.clear();
    this.dialog.closeAll();
    this.router.navigateByUrl('/');
  }

  continue() {
    const loginViewModel:UserViewModel=new UserViewModel();
    this.sessionService.RefreshToken(loginViewModel).subscribe(res =>{
      this.dialogRef.close(res._token);
    });
  }

  onCloseModal(){
    this.dialogRef.close();
  }

  ngOnDestroy(){
    clearInterval(this.searchTimer);
  }

}
