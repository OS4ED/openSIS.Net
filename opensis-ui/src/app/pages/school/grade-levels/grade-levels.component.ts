/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/twotone-search';
import icAdd from '@iconify/icons-ic/twotone-add';
import icFilterList from '@iconify/icons-ic/twotone-filter-list';
import { EditGradeLevelsComponent } from '../grade-levels/edit-grade-levels/edit-grade-levels.component';
import { TranslateService } from '@ngx-translate/core';
import { AddGradeLevelModel, GelAllGradeEquivalencyModel, GetAllGradeLevelsModel } from '../../../models/grade-level.model';
import { MatTableDataSource } from '@angular/material/table';
import { GradeLevelService } from '../../../services/grade-level.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from '../../../services/loader.service';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';
import { CommonService } from '../../../services/common.service';
import { AgeRangeList, EducationalStage } from '../../../models/common.model';

@Component({
  selector: 'vex-grade-levels',
  templateUrl: './grade-levels.component.html',
  styleUrls: ['./grade-levels.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class GradeLevelsComponent implements OnInit {
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort

  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icAdd = icAdd;
  icFilterList = icFilterList;
  getAllGradeLevels: GetAllGradeLevelsModel = new GetAllGradeLevelsModel();
  gradeLevelList: MatTableDataSource<GetAllGradeLevelsModel>;
  getGradeEquivalencyList: GelAllGradeEquivalencyModel = new GelAllGradeEquivalencyModel();
  ageRangeList:AgeRangeList = new AgeRangeList();
  educationalStageList:EducationalStage = new EducationalStage();
  sendGradeLevelsToDialog: [];
  editMode: boolean;
  sendDetailsToEditComponent: [];
  loading: boolean = false;
  searchKey: string;
  deleteGradeLevelData: AddGradeLevelModel = new AddGradeLevelModel();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  columns = [
    { label: 'Title', property: 'title', type: 'text', visible: true },
    { label: 'Short Name', property: 'shortName', type: 'text', visible: true },
    { label: 'Sort Order', property: 'sortOrder', type: 'number', visible: true },
    { label: 'Grade Level Equivalency', property: 'gradeLevelEquivalency', type: 'text', visible: true },
   // { label: 'Age Range', property: 'ageRange', type: 'text', visible: false},
  //  { label: 'Educational Stage', property: 'educationalStage', type: 'text', visible: false},
    { label: 'Next Grade', property: 'nextGrade', type: 'text', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];
  constructor(private dialog: MatDialog,
    public translateService: TranslateService,
    private gradeLevelService: GradeLevelService,
    private loaderService: LoaderService,
    private snackbar: MatSnackBar,
    private cryptoService: CryptoService,
    private commonService:CommonService) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }
  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 22);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 18);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getGradeEquivalency();
    this.getAgeRangeList();
    this.getEducationalStageList();
    this.getAllGradeLevel();
  }

 
  getGradeEquivalency() {
    this.gradeLevelService.getAllGradeEquivalency(this.getGradeEquivalencyList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Equivalency List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.gradeEquivalencyList==null) {
            this.getGradeEquivalencyList= new GelAllGradeEquivalencyModel();
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } else {
            this.getGradeEquivalencyList= new GelAllGradeEquivalencyModel();
          }
        }
        else {
          this.getGradeEquivalencyList = res;

        }
      }
    })
  }
  getAgeRangeList(){
    this.commonService.getAllGradeAgeRange(this.ageRangeList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Age Range List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.gradeAgeRangeList==null) {
            this.ageRangeList= new AgeRangeList();
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } else {
            this.ageRangeList= new AgeRangeList();
          }
        }
        else {
          this.ageRangeList = res;
        }
      }
    })
  }
  getEducationalStageList(){
    this.commonService.getAllGradeEducationalStage(this.educationalStageList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Educational Stage List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.gradeEducationalStageList==null) {
            this.educationalStageList= new EducationalStage();
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } else {
            this.educationalStageList= new EducationalStage();
          }
        }
        else {
          this.educationalStageList = res;
        }
      }
    })
  }

  openAddNew() {
    this.editMode = false;
    this.dialog.open(EditGradeLevelsComponent, {
      data: {
        editMode: this.editMode,
        gradeLevels: this.sendGradeLevelsToDialog,
        gradeLevelEquivalencyList: this.getGradeEquivalencyList,
        ageRangeList: this.ageRangeList,
        educationalStage:this.educationalStageList
      },
      width: '600px'
    }).afterClosed().subscribe((res) => {
      if (res) {
        this.getAllGradeLevel();
      }
    });
  }

  openEdit(editDetails) {
    this.editMode = true;
    this.dialog.open(EditGradeLevelsComponent, {
      data: {
        editMode: this.editMode,
        editDetails: editDetails,
        gradeLevels: this.sendGradeLevelsToDialog,
        gradeLevelEquivalencyList: this.getGradeEquivalencyList,
        ageRangeList: this.ageRangeList,
        educationalStage:this.educationalStageList
      },
      width: '600px'
    }).afterClosed().subscribe((res) => {
      if (res) {
        this.getAllGradeLevel();
      }
    });
  }


  getAllGradeLevel() {
    this.getAllGradeLevels.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getAllGradeLevels._tenantName = sessionStorage.getItem("tenant");
    this.getAllGradeLevels._token = sessionStorage.getItem("token");
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevels).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Level List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.tableGradelevelList === null) {
            this.gradeLevelList = new MatTableDataSource([]);
            this.gradeLevelList.sort = this.sort;
            this.sendGradeLevelsToDialog = [];
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          } else {
            this.gradeLevelList = new MatTableDataSource([]);
            this.gradeLevelList.sort = this.sort;
            this.sendGradeLevelsToDialog = [];
          }

        }
        else {
          this.gradeLevelList = new MatTableDataSource(res.tableGradelevelList);
          this.gradeLevelList.sort = this.sort;
          this.sendGradeLevelsToDialog = res.tableGradelevelList;
        }
      }
    })
  }

  onSearchClear() {
    this.searchKey = "";
    this.applyFilter();
  }

  applyFilter() {
    this.gradeLevelList.filter = this.searchKey.trim().toLowerCase()
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  confirmDelete(deleteDetails) {
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete " + deleteDetails.title + "."
      }
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if (dialogResult) {
        this.deleteGradeLevel(deleteDetails);
      }
    });
  }

  deleteGradeLevel(deleteDetails) {
    this.deleteGradeLevelData.tblGradelevel.schoolId = deleteDetails.schoolId;
    this.deleteGradeLevelData.tblGradelevel.gradeId = deleteDetails.gradeId;
    this.deleteGradeLevelData._tenantName = sessionStorage.getItem("tenant");
    this.deleteGradeLevelData._token = sessionStorage.getItem("token");
    this.gradeLevelService.deleteGradelevel(this.deleteGradeLevelData).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Level Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      } else if (res._failure) {
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open('' + res._message, '', {
          duration: 10000
        });
        this.getAllGradeLevel();
      }
    })
  }
}
