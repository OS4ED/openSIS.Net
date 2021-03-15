import { Component, OnInit } from '@angular/core';
import icArrowDropDown from '@iconify/icons-ic/arrow-drop-down';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import icMoreVert from '@iconify/icons-ic/more-vert';
import icMenu from '@iconify/icons-ic/menu';
import icAdd from '@iconify/icons-ic/add';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icClose from '@iconify/icons-ic/close';
import icInfo from '@iconify/icons-ic/info';
import { MatDialog } from '@angular/material/dialog';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { EditMarkingPeriodComponent } from '../marking-periods/edit-marking-period/edit-marking-period.component';
import { MarkingPeriodService } from '../../../services/marking-period.service';
import { MarkingPeriodListModel } from '../../../models/markingPeriodModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MarkingPeriodAddModel, SemesterAddModel, QuarterAddModel, ProgressPeriodAddModel } from '../../../models/markingPeriodModel';
import { SharedFunction } from '../../shared/shared-function';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { LayoutService } from 'src/@vex/services/layout.service';
import { RollBasedAccessService } from '../../../services/rollBasedAccess.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/rollBasedAccessModel';
import { CryptoService } from '../../../services/Crypto.service';
@Component({
  selector: 'vex-marking-periods',
  templateUrl: './marking-periods.component.html',
  styleUrls: ['./marking-periods.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class MarkingPeriodsComponent implements OnInit {
  icArrowDropDown = icArrowDropDown;
  icEdit = icEdit;
  icCheckBox = icCheckBox;
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  icMoreVert = icMoreVert;
  icMenu = icMenu;
  icInfo = icInfo;
  icAdd = icAdd;
  icClose = icClose;
  icDelete = icDelete;
  menuOpen = false;
  markingPeriodListModel: MarkingPeriodListModel = new MarkingPeriodListModel();
  markingPeriodAddModel: MarkingPeriodAddModel = new MarkingPeriodAddModel();
  semesterAddModel: SemesterAddModel = new SemesterAddModel();
  quarterAddModel: QuarterAddModel = new QuarterAddModel();
  progressPeriodAddModel: ProgressPeriodAddModel = new ProgressPeriodAddModel();
  list: any = [];
  viewFirstChild;
  doesGrades = false;
  doesExam = false;
  doesComments = false;
  loading;
  academicYear;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  zeroIndexOfelement: boolean = false;
  constructor(private dialog: MatDialog,
              private markingPeriodService: MarkingPeriodService,
              private snackbar: MatSnackBar,
              private commonFunction: SharedFunction,
              private loaderService: LoaderService,
              public rollBasedAccessService: RollBasedAccessService,
              public translateService: TranslateService,private layoutService: LayoutService,
              private cryptoService: CryptoService   ) {  
      if (localStorage.getItem('collapseValue') !== null){
        if ( localStorage.getItem('collapseValue') === 'false'){
          this.layoutService.expandSidenav();
        }else{
          this.layoutService.collapseSidenav();
        } 
      }else{
        this.layoutService.expandSidenav();
      }   
      translateService.use('en');
      this.loaderService.isLoading.subscribe((val) => {
        this.loading = val;
      });
  }
  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 2);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;

    this.academicYear = +sessionStorage.getItem('academicyear');

    this.getMarkingPeriod();
  }



  viewDetails(details, data) {

    let elem = document.querySelectorAll('.commonClass');
    elem.forEach(val => {
      val.setAttribute('style', 'background-color:white');
      val.setAttribute('style', 'color:black');
      val.setAttribute('class', 'tree-node card flex shadow-none border-solid border commonClass');
    });
    data.parentElement.style.backgroundColor = '#1763b3';
    data.parentElement.style.color = 'white';
    data.parentElement.style.fontWeight = 'bold';
    this.viewFirstChild = details;
    this.viewFirstChild.startDate = this.commonFunction.formatDate(this.viewFirstChild.startDate);
    this.viewFirstChild.endDate = this.commonFunction.formatDate(this.viewFirstChild.endDate);
    this.viewFirstChild.postStartDate = this.commonFunction.formatDate(this.viewFirstChild.postStartDate);
    this.viewFirstChild.postEndDate = this.commonFunction.formatDate(this.viewFirstChild.postEndDate);
    if (this.viewFirstChild.doesGrades) {
      this.doesGrades = true;
    } else {
      this.doesGrades = false;
    }
    if (this.viewFirstChild.doesExam) {
      this.doesExam = true;
    } else {
      this.doesExam = false;
    }
    if (this.viewFirstChild.doesComments) {
      this.doesComments = true;
    } else {
      this.doesComments = false;
    }
  }
  getMarkingPeriod() {

    this.markingPeriodService.GetMarkingPeriod(this.markingPeriodListModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('General Info Updation failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
            if (data.schoolYearsView == null) {
              this.viewFirstChild = [];
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            }
            else {
              this.list = data.schoolYearsView;
              this.viewFirstMarkingPeriodChild();
            }
        } else {
          this.list = data.schoolYearsView;
          this.viewFirstMarkingPeriodChild();
        }
      }

    })
  }

  viewFirstMarkingPeriodChild() {
    if (this.list.length > 0) {
      this.list.forEach((value, index) => {
        if (index === 0) {
          this.viewFirstChild = value;
        }
      });
      this.viewFirstChild.startDate = this.commonFunction.formatDate(this.viewFirstChild.startDate);
      this.viewFirstChild.endDate = this.commonFunction.formatDate(this.viewFirstChild.endDate);
      this.viewFirstChild.postStartDate = this.commonFunction.formatDate(this.viewFirstChild.postStartDate);
      this.viewFirstChild.postEndDate = this.commonFunction.formatDate(this.viewFirstChild.postEndDate);
      if (this.viewFirstChild.doesGrades) {
        this.doesGrades = true;
      } else {
        this.doesGrades = false;
      }
      if (this.viewFirstChild.doesExam) {
        this.doesExam = true;
      } else {
        this.doesExam = false;
      }
      if (this.viewFirstChild.doesComments) {
        this.doesComments = true;
      } else {
        this.doesComments = false;
      }
    } else {
      this.viewFirstChild = [];
    }
  }

  editItem(editDetails) {
    this.dialog.open(EditMarkingPeriodComponent, {
      width: '600px',
      data: {
        editDetails: editDetails,
        isAdd: false,
        isEdit: true,
        fullData: this.list
      }
    }).afterClosed().subscribe((data) => {
      if (data) {

        this.getMarkingPeriod();
      }
    });
  }
  confirmDelete(deleteDetails) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete " + deleteDetails.title + "."
      }
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.deleteItem(deleteDetails);
      }
    });
  }
  deleteItem(deleteDetails) {
    if (deleteDetails.isParent) {
      this.markingPeriodAddModel.tableSchoolYears.markingPeriodId = deleteDetails.markingPeriodId;
      this.markingPeriodService.DeleteSchoolYear(this.markingPeriodAddModel).subscribe(data => {
        if (typeof (data) == 'undefined') {
          this.snackbar.open('School Year Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (data._failure) {
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
          } else {
            this.markingPeriodService.getCurrentYear(true);
            this.snackbar.open(data._message, '', {
              duration: 10000
            }).afterOpened().subscribe(data => {
              this.getMarkingPeriod();
            });
          }
        }

      })
    } else {
      if (deleteDetails.yearId > 0) {
        this.semesterAddModel.tableSemesters.markingPeriodId = deleteDetails.markingPeriodId;
        this.markingPeriodService.DeleteSemester(this.semesterAddModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('School Semester Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {
              this.markingPeriodService.getCurrentYear(true);
              this.snackbar.open(data._message, '', {
                duration: 10000
              }).afterOpened().subscribe(data => {
                this.getMarkingPeriod();
              });

            }
          }

        })
      } else if (deleteDetails.semesterId > 0) {
        this.quarterAddModel.tableQuarter.markingPeriodId = deleteDetails.markingPeriodId;
        this.markingPeriodService.DeleteQuarter(this.quarterAddModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('School Quarter Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {
              this.markingPeriodService.getCurrentYear(true);
              this.snackbar.open(data._message, '', {
                duration: 10000
              }).afterOpened().subscribe(data => {
                this.getMarkingPeriod();
              });
            }
          }

        })
      } else if (deleteDetails.quarterId > 0) {
        this.progressPeriodAddModel.tableProgressPeriods.markingPeriodId = deleteDetails.markingPeriodId;
        this.markingPeriodService.DeleteProgressPeriod(this.progressPeriodAddModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('School Progress Period Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {
              this.markingPeriodService.getCurrentYear(true);
              this.snackbar.open(data._message, '', {
                duration: 10000
              }).afterOpened().subscribe(data => {
                this.getMarkingPeriod();
              });

            }
          }

        })
      }
    }

  }




  openAddNew() {
    this.dialog.open(EditMarkingPeriodComponent, {
      data: null,
      width: '600px'
    }).afterClosed().subscribe((data) => {
      if (data[0]) {
        this.markingPeriodListModel.academicYear = +data[1];
        this.getMarkingPeriod();
      }
    });
  }
  addChildren(details) {
    this.dialog.open(EditMarkingPeriodComponent, {
      width: '600px',
      data: {
        details: details,
        isAdd: true,
        isEdit: false
      }
    }).afterClosed().subscribe((data) => {
      if (data) {
        this.getMarkingPeriod();
      }
    });
  }
  setData() {
    this.menuOpen = false;
  }

  openMenu() {
    this.menuOpen = true;
  }

}
