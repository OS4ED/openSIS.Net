import { Component, OnInit, ViewChild } from '@angular/core';
import { LayoutService } from 'src/@vex/services/layout.service';
import icExpandAll from '@iconify/icons-ic/unfold-more';
import icCollapseAll from '@iconify/icons-ic/unfold-less';
import icExpand from '@iconify/icons-ic/expand-more';
import icCollapse from '@iconify/icons-ic/expand-less';
import { MatAccordion } from '@angular/material/expansion';
import { RollBasedAccessService } from '../../../services/roll-based-access.service';
import { RolePermissionListViewModel, RolePermissionViewModel, PermissionCategory, RolePermission, PermissionSubCategory, PermissionGroupListViewModel } from '../../../models/roll-based-access.model';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CryptoService } from '../../../services/Crypto.service';
import { NavigationService } from 'src/@vex/services/navigation.service';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { DefaultValuesService } from '../../../common/default-values.service';

@Component({
  selector: 'vex-access-control',
  templateUrl: './access-control.component.html',
  styleUrls: ['./access-control.component.scss']
})
export class AccessControlComponent implements OnInit {

  @ViewChild(MatAccordion) accordion: MatAccordion;
  @ViewChild('form') currentForm: NgForm;
  icExpandAll = icExpandAll;
  icExpand = icExpand;
  icCollapseAll = icCollapseAll;
  icCollapse = icCollapse;
  permissionList = [];
  permissionGroupList = [];
  allCanViewCheckFlag = false;
  memberId;
  memberProfile;
  memberDetails = [];
  memberDescription;
  rolePermissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroupListViewModel: PermissionGroupListViewModel = new PermissionGroupListViewModel();
  constructor(
    private layoutService: LayoutService,
    private rollBasedAccessService: RollBasedAccessService,
    private snackbar: MatSnackBar,
    private defaultValuesService: DefaultValuesService,
    private cryptoService: CryptoService,
    private navigationService: NavigationService
  ) {
   

    this.rollBasedAccessService.currentpermissionList.subscribe(receiveddata => {
      this.permissionList = receiveddata;
      this.permissionGroupListViewModel.permissionGroupList = [];
      this.rolePermissionListViewModel.permissionList = [];

      this.permissionList = this.permissionList.filter((item) => {
        return item.permissionGroup.rolePermission?.length > 0;
      });
      this.permissionList = this.permissionList.map((item) => {
        item.permissionGroup.permissionCategory = item.permissionGroup.permissionCategory.filter((cat) => {
          return cat.rolePermission.length > 0;
        });
        return item;
      });

      this.permissionList.map((val, index) => {
        this.permissionGroupListViewModel.permissionGroupList.push(val.permissionGroup);
        this.rolePermissionListViewModel.permissionList.push(new RolePermissionViewModel());
        val.permissionGroup.permissionCategory.map((val1, j) => {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.push(new PermissionCategory());
          val1.permissionSubcategory.map((val2, k) => {
            this.rolePermissionListViewModel.permissionList[index].
            permissionGroup.permissionCategory[j].permissionSubcategory.push(new PermissionSubCategory());


          });
        });
      });

      this.permissionList.map((val, index) => {

        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionGroupName =
        val.permissionGroup.permissionGroupName;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canView =
        val.permissionGroup.rolePermission[0].canView;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canEdit =
        val.permissionGroup.rolePermission[0].canEdit;
        val.permissionGroup.permissionCategory.map((val1, j) => {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].permissionCategoryName =
          val1.permissionCategoryName;
          if (val1.rolePermission[0] !== undefined) {
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView =
            val1.rolePermission[0].canView;
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit =
            val1.rolePermission[0].canEdit;
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].membershipId =
            this.memberId;
          }
          // if (Array.isArray(val1.permissionSubcategory) && val1.permissionSubcategory.length){
          val1.permissionSubcategory.map((val2, k) => {
            if (val2.rolePermission[0] !== undefined && this.rolePermissionListViewModel.permissionList[index].permissionGroup.
              permissionCategory[j].permissionSubcategory[k] !==undefined) {
              this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
              permissionSubcategory[k].permissionSubcategoryName = val2.permissionSubcategoryName;
              this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
              permissionSubcategory[k].rolePermission[0].canView = val2.rolePermission[0].canView;
              this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
              permissionSubcategory[k].rolePermission[0].canEdit = val2.rolePermission[0].canEdit;
              this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
              permissionSubcategory[k].rolePermission[0].membershipId = this.memberId;
            }
          });
          // }
        });


      });


      /* var getAllPermissionList = this.rolePermissionListViewModel.permissionList.filter(function(val) {
        return val.permissionGroup.hasOwnProperty('permissionGroupName')
      });

      var addPermissionList = this.permissionGroupListViewModel.permissionGroupList.filter(function(val) {
        return val.hasOwnProperty('permissionGroupName')
      });
      this.rolePermissionListViewModel.permissionList = getAllPermissionList;
      this.permissionGroupListViewModel.permissionGroupList=addPermissionList;  */

    });
  }

  ngOnInit(): void {
    this.rollBasedAccessService.selectedMember.subscribe(member => {
      this.memberId = member.memberId;
      this.memberProfile = member.memberTitle;
      this.memberDescription = member.memberDescription;
    });

  }

  getRolePermission(memberId) {
    if (memberId) {
      this.rolePermissionListViewModel.membershipId = memberId;
    } else {
      this.rolePermissionListViewModel.membershipId = +sessionStorage.getItem('userMembershipID');
    }
    this.rolePermissionListViewModel.permissionList.map((val, i) => {
      val.permissionGroup.permissionCategory.map((val1, j) => {
        delete this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].membershipId;
        val1.permissionSubcategory.map((val2, k) => {
          delete this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
          permissionSubcategory[k].rolePermission[0].membershipId;
        });
      });
    });
    this.rollBasedAccessService.getAllRolePermission(this.rolePermissionListViewModel).subscribe(
      (res) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Role Permission List failed. ' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res.permissionList == null) {
              this.permissionList = null;
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
            }
            else {
              this.permissionList = null;
            }
          }
          else {
            if (this.rolePermissionListViewModel.membershipId == +sessionStorage.getItem('userMembershipID')){
              localStorage.setItem('permissions', this.cryptoService.dataEncrypt(JSON.stringify(res)));
              this.rollBasedAccessService.changeAccessControl(true);
            }
            delete res.permissionList[0];
            const permissionList = this.permissionList.filter(x => x != null);
            this.permissionList = permissionList;

          }
        }
      });
  }

  changeParentCanView(e, val) {
    const index = this.rolePermissionListViewModel.permissionList.findIndex((x) => {
      return x.permissionGroup.permissionGroupName === val.permissionGroup.permissionGroupName;
    });
    this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.map((res, j) => {
      if (e.source.checked) {
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
        permissionSubcategory.map((res1, k) => {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory[k].rolePermission[0].canView = true;
        });
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canView = true;
      } else {
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = false;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
        permissionSubcategory.map((res1, k) => {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory[k].rolePermission[0].canView = false;
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory[k].rolePermission[0].canEdit = false;
        });

        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canView = false;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canEdit = false;

      }
    });
  }
  changeParentCanEdit(e, val) {
    const index = this.rolePermissionListViewModel.permissionList.findIndex((x) => {
      return x.permissionGroup.permissionGroupName === val.permissionGroup.permissionGroupName;
    });
    this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory.map((res, j) => {
      if (e.source.checked) {

        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canView = true;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = true;
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
        if (this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory !== undefined) {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory.map((res1, k) => {
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
            permissionSubcategory[k].rolePermission[0].canView = true;
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
            permissionSubcategory[k].rolePermission[0].canEdit = true;
          });
        }
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canEdit = true;
      } else {
        this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
        if (this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory !== undefined) {
          this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
          permissionSubcategory.map((res1, k) => {
            this.rolePermissionListViewModel.permissionList[index].permissionGroup.permissionCategory[j].
            permissionSubcategory[k].rolePermission[0].canEdit = false;
          });
        }

        this.rolePermissionListViewModel.permissionList[index].permissionGroup.rolePermission[0].canEdit = false;
      }
    });
  }
  changeCategoryCanEdit(e: MatSlideToggleChange, i, j) {
    if (e.source.checked) {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
      permissionSubcategory.map((res, k) => {
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
        permissionSubcategory[k].rolePermission[0].canView = true;
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
        permissionSubcategory[k].rolePermission[0].canEdit = true;
      });
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
      if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = true;
      }
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
    } else {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.map((res, k) => {
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
        permissionSubcategory[k].rolePermission[0].canEdit = false;
      });
    }


    const permissionCategoryLength = this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.length;
    if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[permissionCategoryLength - 1].hasOwnProperty('permissionCategoryName') == false){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.pop();
    }
    const permissionCategoryStateEdit = this.rolePermissionListViewModel.permissionList[i].
    permissionGroup.permissionCategory.every(function(permissionCategory){
        return permissionCategory.rolePermission[0].canEdit === e.source.checked ;
      });
    if (permissionCategoryStateEdit === true){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = e.source.checked;
        if (e.source.checked === true){
          this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
        }
      }

  }

  changeCategoryCanView(e: MatSlideToggleChange, i, j) {
    if (e.source.checked === false) {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.map((res, k) => {
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
        permissionSubcategory[k].rolePermission[0].canView = false;
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
        permissionSubcategory[k].rolePermission[0].canEdit = false;
      });
    } else {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.map((res, k) => {
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].
        rolePermission[0].canView = true;
      });
    }

    const permissionCategoryLength = this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.length;

    if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[permissionCategoryLength - 1].hasOwnProperty('permissionCategoryName') == false){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.pop();
    }
    const permissionCategoryStateView = this.rolePermissionListViewModel.permissionList[i].
    permissionGroup.permissionCategory.every(function(permissionCategory){
      return permissionCategory.rolePermission[0].canView === e.source.checked ;
    });
    if (permissionCategoryStateView === true){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = e.source.checked;
      if (e.source.checked === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = false;
      }
    }
  }

  changeSubCategoryCanView(i, j, k, e) {
    if (e.source.checked === false) {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].
      rolePermission[0].canEdit = false;
    } else {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
    }
    const permissionSubCategoryLength = this.rolePermissionListViewModel.permissionList[i].
    permissionGroup.permissionCategory[j].permissionSubcategory.length;

    if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
      permissionSubcategory[permissionSubCategoryLength - 1].hasOwnProperty('permissionSubcategoryName') === false){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.pop();
    }
    const permissionSubCategoryStateView = this.rolePermissionListViewModel.permissionList[i].
    permissionGroup.permissionCategory[j].permissionSubcategory.every(function(permissionSubCategory){
      return permissionSubCategory.rolePermission[0].canView === e.source.checked ;
    });
    if (permissionSubCategoryStateView === true){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = e.source.checked;
      if (e.source.checked === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = false;
      }
      const permissionCategoryLength = this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.length;
      if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.
        permissionCategory[permissionCategoryLength - 1].hasOwnProperty('permissionCategoryName') === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.pop();
      }
      const permissionCategoryStateView = this.rolePermissionListViewModel.permissionList[i].
      permissionGroup.permissionCategory.every(function(permissionCategory){
        return permissionCategory.rolePermission[0].canView === e.source.checked ;
      });
      if (permissionCategoryStateView === true){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = e.source.checked;
        if (e.source.checked === false){
          this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = false;
        }
      }
    }
  }

  changeSubCategoryCanEdit(i, j, k, e) {
    if (e.source.checked) {
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory[k].
      rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canView = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = true;
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
    }
    const permissionSubCategoryLength =
    this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.length;
    if (this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].
      permissionSubcategory[permissionSubCategoryLength - 1].hasOwnProperty('permissionSubcategoryName') === false){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].permissionSubcategory.pop();
    }
    const permissionSubCategoryStateEdit = this.rolePermissionListViewModel.permissionList[i].
    permissionGroup.permissionCategory[j].permissionSubcategory.every(function(permissionSubCategory){
      return permissionSubCategory.rolePermission[0].canEdit === e.source.checked ;
    });
    if (permissionSubCategoryStateEdit === true){
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory[j].rolePermission[0].canEdit = e.source.checked;

      const permissionCategoryLength = this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.length;
      if (this.rolePermissionListViewModel.permissionList[i].
        permissionGroup.permissionCategory[permissionCategoryLength - 1].hasOwnProperty('permissionCategoryName') === false){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.pop();
      }
      const permissionCategoryStateEdit =
      this.rolePermissionListViewModel.permissionList[i].permissionGroup.permissionCategory.every(function(permissionCategory){
        return permissionCategory.rolePermission[0].canEdit === e.source.checked ;
      });
      if (permissionCategoryStateEdit === true){
        this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canEdit = e.source.checked;
        if (e.source.checked === true){
          this.rolePermissionListViewModel.permissionList[i].permissionGroup.rolePermission[0].canView = true;
        }
      }
    }
  }

  submit() {
    this.rolePermissionListViewModel.permissionList.map((val, i) => {
      if (val.permissionGroup !== undefined) {

        if (this.permissionGroupListViewModel.permissionGroupList[i].rolePermission.length > 0) {

          this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canEdit =
          val.permissionGroup.rolePermission[0].canEdit;
          this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canView =
          val.permissionGroup.rolePermission[0].canView;
          if (this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canEdit) {
            this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canAdd = true;
          } else {
            this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canAdd = false;
          }
        }

        val.permissionGroup.permissionCategory.map((val, j) => {
          if (val.permissionCategoryName !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j] !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission.length > 0) {

            this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canEdit = val.rolePermission[0].canEdit;
            this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canView = val.rolePermission[0].canView;
            if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canEdit) {
              this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canAdd = true;
            } else {
              this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canAdd = false;
            }
            val.permissionSubcategory.map((val1, k) => {
              if (val1.permissionSubcategoryName !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k] !== undefined) {
                if (val1.rolePermission[0] !== undefined && this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0] !== undefined) {
                  this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit = val1.rolePermission[0].canEdit;
                  this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView = val1.rolePermission[0].canView;
                  if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit) {
                    this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canAdd = true;
                  } else {
                    this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canAdd = false;
                  }
                }
              }
            });
          }
        });
      }
    });

    this.permissionGroupListViewModel.permissionGroupList.map((val, i) => {
      let falseFlagCategoryCanEdit = false;
      let falseFlagCategoryCanView = false;
      if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory.length > 0) {
        this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory.map((val1, j) => {
          if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canEdit) {
            falseFlagCategoryCanEdit = true;
          }
          if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canView) {
            falseFlagCategoryCanView = true;
          }
          if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory.length > 0) {
            let falseFlagSubCategoryCanEdit = false;
            let falseFlagSubCategoryCanView = false;
            this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory.map((val2, k) => {
              if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canEdit) {
                falseFlagSubCategoryCanEdit = true;
              }
              if (this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].permissionSubcategory[k].rolePermission[0].canView) {
                falseFlagSubCategoryCanView = true;
              }
            });
            if (falseFlagSubCategoryCanEdit === false) {
              this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canEdit = false;
              this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canAdd = false;
            }
            if (falseFlagSubCategoryCanView === false) {
              this.permissionGroupListViewModel.permissionGroupList[i].permissionCategory[j].rolePermission[0].canView = false;
            }
          }
        });

        if (falseFlagCategoryCanEdit === false) {
          this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canEdit = false;
          this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canAdd = false;
        }
        if (falseFlagCategoryCanView === false) {
          this.permissionGroupListViewModel.permissionGroupList[i].rolePermission[0].canView = false;
        }
      }
    });





    this.rollBasedAccessService.updateRolePermission(this.permissionGroupListViewModel).subscribe(
      (res) => {
        if (res){
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.getRolePermission(this.memberId);
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('roleBaseAccessSubmissionFailed') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      });

  }



}
