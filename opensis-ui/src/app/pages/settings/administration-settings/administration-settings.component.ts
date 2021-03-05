import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import icAdd from '@iconify/icons-ic/baseline-add';
import icSuperadmin from '@iconify/icons-ic/baseline-admin-panel-settings';
import icAdmin from '@iconify/icons-ic/baseline-account-box';
import icTeacher from '@iconify/icons-ic/twotone-person';
import icHomeroomTeacher from '@iconify/icons-ic/twotone-account-circle';
import icParent from '@iconify/icons-ic/twotone-escalator-warning';
import icStudent from '@iconify/icons-ic/baseline-face';
import icMoreVert from '@iconify/icons-ic/more-vert';
import { TranslateService } from '@ngx-translate/core';
import { EditCustomProfileComponent } from '../../administration/access-control/edit-custom-profile/edit-custom-profile.component';
import { RollBasedAccessService } from '../../../services/rollBasedAccess.service';
import { RolePermissionListViewModel,PermissionGroupListViewModel } from '../../../models/rollBasedAccessModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MembershipService } from '../../../services/membership.service';
import { GetAllMembersList, Membership } from '../../../models/membershipModel';
@Component({
  selector: 'vex-administration-settings',
  templateUrl: './administration-settings.component.html',
  styleUrls: ['./administration-settings.component.scss'],
  animations: [
    fadeInRight400ms
  ]
})
export class AdministrationSettingsComponent implements OnInit {
  pages=['Profiles and Permissions']
  administrationSettings=true;
  pageTitle = 'Profiles and Permissions';
  pageId: string = '';

  icAdd = icAdd;
  icSuperadmin = icSuperadmin;
  icAdmin = icAdmin;
  icTeacher = icTeacher;
  icHomeroomTeacher = icHomeroomTeacher;
  icParent = icParent;
  icStudent = icStudent;
  icMoreVert = icMoreVert;
  rolePermissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroupListViewModel:PermissionGroupListViewModel= new PermissionGroupListViewModel();
  memberList=[];
  selectedMemeber = 1; 
  permissionList=[];
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  constructor(
    public translateService:TranslateService, private dialog: MatDialog,
    public rollBasedAccessService:RollBasedAccessService,private snackbar: MatSnackBar,
    public membershipService:MembershipService) {
    translateService.use('en');
  }

  ngOnInit(): void {
    
    this.getAllMembership();
    this.getRolePermission(this.selectedMemeber);
    this.pageId = localStorage.getItem("pageId");
  }

  getSelectedPage(pageId){
    this.pageId = pageId;
    localStorage.setItem("pageId", pageId);
  }

  goToAdd() {
    this.dialog.open(EditCustomProfileComponent, {
      width: '500px'
    });
  }

  getAllMembership(){
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Role Permission List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res._message === "NO RECORD FOUND") {
            if (res.getAllMemberList == null) {
              this.memberList=null;
            }

          } else {
            this.snackbar.open('Role Permission List failed. ' + res._message, 'LOL THANKS', {
              duration: 10000
            });
          }

        }
        else {
         this.selectedMemeber = res.getAllMemberList[0].membershipId;         
         res.getAllMemberList.map(val=>{
            var obj={};
            obj['profile'] = val.profile
            obj['membershipId'] = val.membershipId
            var icon = this.getIcon(val.profile);
            if(icon !== undefined){
              if(val.profile == "Teacher" || val.profile == "Homeroom Teacher"){
                obj["className"]=icon;
                obj["icon"] = icon;
              
              }else{
                obj["className"]="mr-3 align-middle";
                obj['icon']= icon
              }           
            }else{
              obj["className"]="mr-3 align-middle";
              obj['icon']= icAdmin
            }
            
            this.memberList.push(obj)
         })
         
        }
      }
    })
  }
  getIcon(val){
    switch(val) { 
      case "Super Administrator": { 
        return icSuperadmin
        
      } 
      case "Administrator": { 
        return icAdmin
        
      } 
      case "Admin Assistant": { 
        return icAdmin
       
      } 
      case "Teacher": { 
        return "icon-teacher mr-3 align-middle"
       
      } 
      case "Homeroom Teacher": { 
        return "icon-homeroom-teacher mr-3 align-middle"
       
      } 
      case "Parent": { 
        return icParent
      
      } 
      case "Student": { 
        return icStudent
       
      }      
    } 
  }
  getRolePermission(memberId){  
    this.rolePermissionListViewModel.membershipId = memberId;
    this.rollBasedAccessService.getAllRolePermission(this.rolePermissionListViewModel).subscribe(
      (res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Role Permission List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res._message === "NO RECORD FOUND") {
            if (res.permissionList == null) {
              this.permissionList=null;
            }

          } else {
            this.snackbar.open('Role Permission List failed. ' + res._message, 'LOL THANKS', {
              duration: 10000
            });
          }

        }
        else {
         this.permissionList = res.permissionList;        
         this.rollBasedAccessService.send(this.permissionList);         
         this.selectedMemeber = memberId;
         this.rollBasedAccessService.sendSelectedMember(this.selectedMemeber);
        }
      }
    })
  }




}
