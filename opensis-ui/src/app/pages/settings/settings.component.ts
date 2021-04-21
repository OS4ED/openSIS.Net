import { Component, OnInit } from '@angular/core';
import icSchool from '@iconify/icons-ic/twotone-account-balance';
import icStudents from '@iconify/icons-ic/twotone-school';
import icUsers from '@iconify/icons-ic/twotone-people';
import icSchedule from '@iconify/icons-ic/twotone-date-range';
import icGrade from '@iconify/icons-ic/twotone-leaderboard';
import icAttendance from '@iconify/icons-ic/twotone-access-alarm';
import icListOfValues from '@iconify/icons-ic/baseline-format-list-bulleted';
import { TranslateService } from '@ngx-translate/core';
import { LayoutService } from 'src/@vex/services/layout.service';
import { NavigationService } from 'src/@vex/services/navigation.service';
import { CryptoService } from 'src/app/services/Crypto.service';
import { PermissionGroup, RolePermissionListViewModel } from '../../models/roll-based-access.model';
import { Router } from '@angular/router';
import { RollBasedAccessService } from '../../services/roll-based-access.service';
@Component({
  selector: 'vex-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  icSchool = icSchool;
  icStudents = icStudents;
  icUsers = icUsers;
  icSchedule = icSchedule;
  icGrade = icGrade;
  icAttendance = icAttendance;
  icListOfValues = icListOfValues;

  settingSubmenu:PermissionGroup;
  constructor(public translateService: TranslateService,
    private layoutService: LayoutService,
    private cryptoService: CryptoService,
    private navigationService: NavigationService,
    private roleBaseAccessService:RollBasedAccessService) {
    if (localStorage.getItem("collapseValue") !== null) {
      if (localStorage.getItem("collapseValue") === "false") {
        this.layoutService.expandSidenav();
      } else {
        this.layoutService.collapseSidenav();
      }
    } else {
      this.layoutService.expandSidenav();
    }
    translateService.use('en');
    this.navigationService.menuItems.subscribe((res)=>{
      if(res){
         this.renderSettingsMenu();
      }
    })
  }

  ngOnInit(): void {
  //  this.renderSettingsMenu();
  }

  renderSettingsMenu(){
    let permissions:RolePermissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    let settingIndex = permissions?.permissionList?.findIndex((item) => {
      return item.permissionGroup?.permissionGroupId == 12
    });
    this.settingSubmenu = permissions?.permissionList[settingIndex]?.permissionGroup;

    for(let item of this.settingSubmenu?.permissionCategory){
      item.icon=this.pickIcon(item.permissionCategoryName)
    }

    for(let item of this.settingSubmenu.permissionCategory[0].permissionSubcategory){
      if(item.rolePermission[0].canView){
        this.setPageId(item.title);
        break;
      }
    }
   
  }

  pickIcon(categoryName) {
    switch (categoryName) {
      case 'School':{
        return icSchool;
      }
      case 'Student':{
        return icStudents;
      }
      case 'Staff':{
        return icUsers;
      }
      case 'Attendance':{
        return icAttendance;
      }
      case 'Administration':{
        return icGrade;
      }
      case 'Grades':{
        return icGrade;
      }
      case 'List of Values':{
        return icListOfValues;
      }
    }
  }

  setPageId(pageId?) {
    localStorage.setItem("pageId", pageId);
  }

  setParentId(index){
    for(let item of this.settingSubmenu.permissionCategory[index].permissionSubcategory){
      if(item.rolePermission[0].canView){
        localStorage.setItem("pageId", item.title);
        break;
      }
    }
  }
}
