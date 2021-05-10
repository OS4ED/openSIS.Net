import { Component, Input, OnInit } from '@angular/core';
import icPreview from '@iconify/icons-ic/round-preview';
import icPeople from '@iconify/icons-ic/twotone-people';
import icMoreVert from '@iconify/icons-ic/more-vert';
import { NoticeDeleteModel } from '../../../../models/notice-delete.model';
import { NoticeService } from '../../../../services/notice.service';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NoticeAddViewModel, NoticeListViewModel } from '../../../../models/notice.model';
import { MatDialog } from '@angular/material/dialog';
import { EditNoticeComponent } from '../edit-notice/edit-notice.component';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../../../../app/pages/shared-module/confirm-dialog/confirm-dialog.component';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';
@Component({
  selector: 'vex-notice-cards',
  templateUrl: './notice-cards.component.html',
  styleUrls: ['./notice-cards.component.scss']
})
export class NoticeCardsComponent implements OnInit {
  noticeListViewModel: NoticeListViewModel = new NoticeListViewModel();
  noticeaddViewModel: NoticeAddViewModel = new NoticeAddViewModel();
  icPreview = icPreview;
  icPeople = icPeople;
  icMoreVert = icMoreVert;
  showMember = true;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  noticeDeleteModel = new NoticeDeleteModel();
  @Input() title: string;
  @Input() notice;
  @Input() noticeId: number;
  @Input() imageUrl: string;
  @Input() visibleFrom: string;
  @Input() visibleTo: number;
  @Input() getAllMembersList;

  constructor(private dialog: MatDialog,
              private noticeService: NoticeService,
              public translateService: TranslateService,
              public rollBasedAccessService: RollBasedAccessService,
              private Activeroute: ActivatedRoute,
              private snackbar: MatSnackBar,
              private cryptoService: CryptoService,
              private defaultValuesService: DefaultValuesService) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 4);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;

    if (this.visibleTo.toString() === ''){
      this.showMember = false;
    }
  }


  deleteNoticeConfirm(id) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') + this.title}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if(dialogResult){
        this.deleteNotice(id);
      }
   });
  }

deleteNotice(id){
  this.noticeDeleteModel.NoticeId= +id;
  this.noticeService.deleteNotice(this.noticeDeleteModel).subscribe(
    (res) => {
    if (res._failure) {
      this.snackbar.open( res._message, '', {
        duration: 10000
      });
    } else {
      //  this.noticeService.getAllNotice(this.noticeListViewModel).subscribe((res) => {
      //    this.noticeListViewModel = res;
      //  });
      this.snackbar.open(res._message, '', {
        duration: 10000
      });
      this.noticeService.changeNotice(true)      
    }
  });
}

    
  editNotice(noticeId: number) 
  {
    this.dialog.open(EditNoticeComponent, {
      data: {allMembers:this.getAllMembersList,notice:this.notice,membercount:this.getAllMembersList.getAllMemberList.length},
      width: '800px'
    })    
  }
}
