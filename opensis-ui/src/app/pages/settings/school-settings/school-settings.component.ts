import { Component, OnInit } from '@angular/core';
import { RolePermissionListViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';

@Component({
  selector: 'vex-school-settings',
  templateUrl: './school-settings.component.html',
  styleUrls: ['./school-settings.component.scss'],
  animations: [
    fadeInRight400ms
  ]
})
export class SchoolSettingsComponent implements OnInit {
  pages=[];
  schoolSettings=true;
  pageTitle:string;
  pageId: string;

  constructor(private cryptoService:CryptoService) { }

  ngOnInit(): void {
    let permissions:RolePermissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    let settingIndex = permissions?.permissionList?.findIndex((item) => {
      return item.permissionGroup?.permissionGroupId == 12
    });

    let schoolMenu= permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory.findIndex((item)=>{
      return item.permissionCategoryId==22;
    });
    permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[schoolMenu].permissionSubcategory.map((option)=>{
      if(option.rolePermission[0].canView){
        this.pages.push(option.title);
      }
    }) 

    let availablePageId=localStorage.getItem("pageId");
    if(availablePageId==null || !this.pages.includes(availablePageId)){
      for(let item of permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[schoolMenu].permissionSubcategory){
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
