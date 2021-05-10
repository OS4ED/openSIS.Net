import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef, AfterViewChecked, AfterContentChecked } from '@angular/core';
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
import { RollBasedAccessService } from '../../../services/roll-based-access.service';
import { RolePermissionListViewModel, PermissionGroupListViewModel } from '../../../models/roll-based-access.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MembershipService } from '../../../services/membership.service';
import { GetAllMembersList, Membership } from '../../../models/membership.model';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { AddMembershipModel } from '../../../models/membership.model';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { LoaderService } from '../../../services/loader.service';
import { LayoutService } from '../../../../@vex/services/layout.service';
import { CryptoService } from '../../../services/Crypto.service';
@Component({
  selector: 'vex-administration-settings',
  templateUrl: './administration-settings.component.html',
  styleUrls: ['./administration-settings.component.scss'],
  animations: [
    fadeInRight400ms
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdministrationSettingsComponent implements OnInit, AfterContentChecked {
  pages = []
  administrationSettings = true;
  pageTitle: string;
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
  permissionGroupListViewModel: PermissionGroupListViewModel = new PermissionGroupListViewModel();
  memberList = [];
  selectedMemeber;
  permissionList = [];
  memberDetails = [];
  selectedMemeberProfile;
  selectedDescription;
  destroySubject$: Subject<void> = new Subject();
  loading: boolean;
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  addMembershipModel: AddMembershipModel = new AddMembershipModel();
  customMembershipDeleted = false;
  constructor(
    public translateService: TranslateService, private dialog: MatDialog,
    public rollBasedAccessService: RollBasedAccessService, private snackbar: MatSnackBar,
    public membershipService: MembershipService,
    private cryptoService: CryptoService,
    private cdr: ChangeDetectorRef,
    private loaderService: LoaderService,
    private layoutService: LayoutService) {
    translateService.use('en');
    this.layoutService.collapseSidenav();

  }

  ngOnInit(): void {
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
    let permissions: RolePermissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    let settingIndex = permissions?.permissionList?.findIndex((item) => {
      return item.permissionGroup?.permissionGroupId == 12
    });

    let administrationMenu = permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory.findIndex((item) => {
      return item.permissionCategoryId == 27;
    });
    permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[administrationMenu].permissionSubcategory.map((option) => {
      if (option.rolePermission[0].canView) {
        this.pages.push(option.title);
      }
    })
    let availablePageId = localStorage.getItem("pageId");
    if (availablePageId == null || !this.pages.includes(availablePageId)) {
      for (let item of permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[administrationMenu].permissionSubcategory) {
        if (item.rolePermission[0].canView) {
          localStorage.setItem("pageId", item.title);
          break;
        }
      }
    }
    this.pageId = localStorage.getItem("pageId");

    this.getAllMembership();
    this.getRolePermission(this.selectedMemeber, this.selectedMemeberProfile, this.selectedDescription);
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
  }
  getSelectedPage(pageId) {
    this.pageId = pageId;
    localStorage.setItem("pageId", pageId);
  }

  goToAdd() {
    this.dialog.open(EditCustomProfileComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data) {
        this.getAllMembership();
      }
    });
  }
  editMember(editDetails) {
    this.dialog.open(EditCustomProfileComponent, {
      data: editDetails,
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data) {
        this.getAllMembership();
        const obj = {
          memberId: this.selectedMemeber,
          memberTitle: data.profile,
          memberDescription: data.description
        };
        this.rollBasedAccessService.sendSelectedMember(obj);
      }
    });
  }
  deleteMember(deleteDetails) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete " + deleteDetails.profile + "."
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.customMembershipDeleted = true;
        this.deleteProfile(deleteDetails.membershipId);
      }
    });
  }
  deleteProfile(id) {
    this.addMembershipModel.membership.membershipId = id;
    this.membershipService.deleteMembership(this.addMembershipModel).subscribe(
      (res) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Member Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.getAllMembership();
          }
        }
      }
    )
  }
  getAllMembership() {
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Membership List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.getAllMemberList == null) {
            this.memberList = [];
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.memberList = [];
          }

        }
        else {
          this.memberList = [];
          res.getAllMemberList.map(val => {
            let obj = {};
            obj['profile'] = val.profile
            obj['membershipId'] = val.membershipId
            obj['description'] = val.description
            obj['isSystem'] = val.isSystem
            obj['profileType'] = val.profileType
            if (val.membershipId === +sessionStorage.getItem("userMembershipID")) {
              this.selectedDescription = val.description;
            }
            let icon = this.getIcon(val.profile);
            if (icon !== undefined) {
              if (val.profile == "Teacher" || val.profile == "Homeroom Teacher") {
                obj["className"] = icon;
                obj["icon"] = null;

              } else {
                obj["className"] = "mr-3 align-middle";
                obj['icon'] = icon
              }
            } else {
              obj["className"] = "mr-3 align-middle";
              obj['icon'] = icAdmin
            }

            this.memberList.push(obj)

          });
          let membershipIdFound = false;
          for (let member of this.memberList) {
            if (member.membershipId == +this.selectedMemeber) {
              membershipIdFound = true;
              break;
            }
          }
          if (!membershipIdFound) {
            this.selectedMemeber = sessionStorage.getItem("userMembershipID");
            this.selectedMemeberProfile = sessionStorage.getItem("membershipName");
            this.getRolePermission(this.selectedMemeber, this.selectedMemeberProfile, this.selectedDescription);
          }



          // if (this.customMembershipDeleted) {
          //   this.getRolePermission(this.selectedMemeber, this.selectedMemeberProfile, this.selectedDescription);
          //   this.customMembershipDeleted = false;
          // }

        }
      }
    })
  }
  getIcon(val) {
    switch (val) {
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
  getRolePermission(memberId, memberProfile, description) {
    if (memberId == this.rolePermissionListViewModel.membershipId) {
      return
    }
    if (memberId) {
      this.rolePermissionListViewModel.membershipId = memberId;
    } else {
      this.rolePermissionListViewModel.membershipId = +sessionStorage.getItem("userMembershipID");
    }
    this.rollBasedAccessService.getAllRolePermission(this.rolePermissionListViewModel).subscribe(
      (res) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Role Permission List failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res.permissionList == null) {
              this.permissionList = [];
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
            }
            else {
              this.permissionList = [];
            }
          }
          else {
            if (memberId) {
              this.selectedMemeber = memberId;
            }
            if (memberProfile) {
              this.selectedMemeberProfile = memberProfile;
            }
            if (description) {
              this.selectedDescription = description;
            }
            this.permissionList = res.permissionList;
            delete this.permissionList[0]

            let permissionList = this.permissionList.filter(x => x != null)
            this.rollBasedAccessService.send(permissionList);


            let obj = {};
            obj['memberId'] = this.selectedMemeber;
            obj['memberTitle'] = this.selectedMemeberProfile;
            obj['memberDescription'] = this.selectedDescription;

            this.rollBasedAccessService.sendSelectedMember(obj);
          }
        }
      })
  }




}
