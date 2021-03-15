import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icRemove from '@iconify/icons-ic/remove-circle';
import icAdd from '@iconify/icons-ic/baseline-add';
import { MatDialog } from '@angular/material/dialog';
import { AddSiblingComponent } from '../add-sibling/add-sibling.component';
import { ViewSiblingComponent } from '../view-sibling/view-sibling.component';
import { StudentService } from '../../../../../services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentSiblingAssociation, StudentViewSibling } from '../../../../../models/studentModel';
import { ConfirmDialogComponent } from '../../../../shared-module/confirm-dialog/confirm-dialog.component';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../../models/rollBasedAccessModel';
import { CryptoService } from '../../../../../services/Crypto.service';

@Component({
  selector: 'vex-siblingsinfo',
  templateUrl: './siblingsinfo.component.html',
  styleUrls: ['./siblingsinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class SiblingsinfoComponent implements OnInit {
  icEdit = icEdit;
  icRemove = icRemove;
  icAdd = icAdd;

  removeStudentSibling:StudentSiblingAssociation = new StudentSiblingAssociation();
  studentViewSibling:StudentViewSibling=new StudentViewSibling();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  viewPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(private fb: FormBuilder,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private cryptoService:CryptoService,
    private studentService:StudentService,
    private snackbar: MatSnackBar) { }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 6);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.viewPermission = permissionSubCategory.rolePermission[0].canView;

    this.getAllSiblings()
  }

  openAddNew() {
    this.dialog.open(AddSiblingComponent, {
      width: '800px',
      disableClose: true
    }).afterClosed().subscribe((res)=>{
      if(res){
        this.getAllSiblings();
      }
    });
  }

  openViewDetails(siblingDetails) {
    this.dialog.open(ViewSiblingComponent,{
      data:{
        siblingDetails:siblingDetails,
      }, 
      width: '800px'
    });
  }

  getAllSiblings(){
    this.studentViewSibling.studentId=this.studentService.getStudentId();
    this.studentService.viewSibling(this.studentViewSibling).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Siblings failed to fetch. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
            if(res.studentMaster==null){
              this.studentViewSibling.studentMaster=null;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else{
              this.studentViewSibling.studentMaster=null;
            }
        } else {  
          this.studentViewSibling.studentMaster=res.studentMaster;
        }
      }
    });
  }
  confirmDelete(siblingDetails)
  { 
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete your association, "+siblingDetails.firstGivenName+"."}
    });
    
    dialogRef.afterClosed().subscribe(dialogResult => {      
      if(dialogResult){
        this.removeSibling(siblingDetails);
      }
    });
  }

  removeSibling(siblingDetails){
    this.removeStudentSibling.studentMaster.studentId=siblingDetails.studentId;
    this.removeStudentSibling.studentMaster.schoolId=siblingDetails.schoolId;
    this.removeStudentSibling.studentId=this.studentService.getStudentId();
    this.studentService.removeSibling(this.removeStudentSibling).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Sibling is failed to remove.' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {  
          this.getAllSiblings();
          this.snackbar.open(res._message,'', {
            duration: 10000
          });
        }
      }
    })
  }

}
