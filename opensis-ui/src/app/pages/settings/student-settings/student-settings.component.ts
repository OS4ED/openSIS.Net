import { Component, OnInit } from '@angular/core';
import { RolePermissionListViewModel } from 'src/app/models/rollBasedAccessModel';
import { CryptoService } from 'src/app/services/Crypto.service';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';

@Component({
  selector: 'vex-student-settings',
  templateUrl: './student-settings.component.html',
  styleUrls: ['./student-settings.component.scss'],
  animations: [
    fadeInRight400ms
  ]
})
export class StudentSettingsComponent implements OnInit {
  pages=[]
  studentSettings=true;
  pageTitle:string;
  pageId: string;

  constructor(private cryptoService:CryptoService) { }

  ngOnInit(): void {
    let permissions:RolePermissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    let settingIndex = permissions?.permissionList?.findIndex((item) => {
      return item.permissionGroup?.permissionGroupId == 12
    });

    let studentMenu= permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory.findIndex((item)=>{
      return item.permissionCategoryId==23;
    });
    permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[studentMenu].permissionSubcategory.map((option)=>{
      if(option.rolePermission[0].canView){
        this.pages.push(option.title);
      }
    })
    
    let availablePageId=localStorage.getItem("pageId");
    if(availablePageId==null || !this.pages.includes(availablePageId)){
      for(let item of permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[studentMenu].permissionSubcategory){
        if(item.rolePermission[0].canView){
          localStorage.setItem("pageId",item.title);
          break;
        }
      }
    }

   
    this.pageId = localStorage.getItem("pageId");
  }

  getSelectedPage(pageId){
    this.pageId = pageId;
    localStorage.setItem("pageId", pageId);
  }

}
