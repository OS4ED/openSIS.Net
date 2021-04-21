import { Component, Input, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LayoutService } from 'src/@vex/services/layout.service';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import icGeneralInfo from '@iconify/icons-ic/outline-account-circle';
import icAddress from '@iconify/icons-ic/outline-location-on';
import icAccessInfo from '@iconify/icons-ic/outline-lock-open';
import { ImageCropperService } from '../../../services/image-cropper.service';
import { SchoolCreate } from '../../../enums/school-create.enum';
import { ParentInfoService } from '../../../services/parent-info.service';
import { AddParentInfoModel } from '../../../models/parent-info.model';
import { takeUntil } from 'rxjs/operators';
import { RolePermissionListViewModel } from '../../../models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-edit-parent',
  templateUrl: './edit-parent.component.html',
  styleUrls: ['./edit-parent.component.scss'],
  animations: [  
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditParentComponent implements OnInit {
  pageStatus:string="View Parent";
  showAddressInfo:boolean= false;
  showGeneralInfo: boolean= false;
  icGeneralInfo = icGeneralInfo;
  icAddress = icAddress;
  icAccessInfo = icAccessInfo;
  destroySubject$: Subject<void> = new Subject();
  pageId:string;
  parentCreate = SchoolCreate;
  parentId: number;
  @Input() parentCreateMode: SchoolCreate = SchoolCreate.VIEW;
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  parentTitle;
  responseImage: string;
  
  enableCropTool = true;
  constructor(private layoutService: LayoutService,
    private parentInfoService:ParentInfoService,
    private imageCropperService:ImageCropperService,
    private cryptoService: CryptoService) {
    this.layoutService.collapseSidenav();
    this.imageCropperService.getCroppedEvent().pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.parentInfoService.setParentImage(res[1]);
    });
    this.parentInfoService.modeToUpdate.pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      if(res==this.parentCreate.EDIT){
        this.pageStatus="Edit Parent"
      }else if(res==this.parentCreate.VIEW){
        this.pageStatus="View Parent"
      }
    });
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    
    if(this.permissionListViewModel.permissionList[3].permissionGroup.permissionCategory[0].rolePermission[0].canView){
      this.showGeneralInfo= true;
      this.pageId="General Info";
    }
    else if(this.permissionListViewModel.permissionList[3].permissionGroup.permissionCategory[1].rolePermission[0].canView){
      this.showAddressInfo= true;
      this.pageId="Address Info";
    }

    this.parentCreateMode = this.parentCreate.VIEW;
    this.parentId = this.parentInfoService.getParentId();   
    this.getParentDetailsUsingId();
  }
  

  getParentDetailsUsingId(){
    this.addParentInfoModel.parentInfo.parentId = this.parentId;
    this.parentInfoService.viewParentInfo(this.addParentInfoModel).subscribe(data => {
      this.addParentInfoModel = data;
      this.parentInfoService.sendDetails(this.addParentInfoModel);
      this.parentTitle = this.addParentInfoModel.parentInfo.salutation + " " +this.addParentInfoModel.parentInfo.firstname + " " + this.addParentInfoModel.parentInfo.middlename+ " " + this.addParentInfoModel.parentInfo.lastname;
    
      this.responseImage = this.addParentInfoModel.parentInfo.parentPhoto;     
      this.parentInfoService.setParentImage(this.responseImage);
      
    });
  }

  showPage(pageId) {    
    localStorage.setItem("pageId",pageId); 
    this.pageId=localStorage.getItem("pageId");
  }

  ngOnDestroy() {
    this.parentInfoService.setParentImage(null);
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
