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

import { TitleCasePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { RolePermissionListViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';
import { LoaderService } from 'src/app/services/loader.service';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { InputEffortGradesComponent } from './input-effort-grades/input-effort-grades.component';
import { InputFinalGradeComponent } from './input-final-grade/input-final-grade.component';
import { TakeAttendanceComponent } from './take-attendance/take-attendance.component';
@Component({
  selector: 'vex-teacher-function',
  templateUrl: './teacher-function.component.html',
  styleUrls: ['./teacher-function.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ],
  providers: [TitleCasePipe]

})

export class TeacherFunctionComponent implements OnInit {
  pageStatus;
  parentPageStatus = [];
  routerArray = [];
  activeMenu;
  secondarySidebar = 0;

  //currentCategory: number = 1;
  gradeComponent: any;
  gradeComponentListCat = [InputFinalGradeComponent, InputEffortGradesComponent];
  takeAttendance = [TakeAttendanceComponent]
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup;
  permissionCategoryForTeacherFunctions;
  loading:boolean;

  constructor(
    public translateService:TranslateService,
    private router: Router,
    private cryptoService: CryptoService,
    private titlecasePipe: TitleCasePipe,
    private loaderService:LoaderService,
    ) { 
    translateService.use('en');

    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });

    router.events.subscribe((val) => {
      this.parentPageStatus = [];
      
      if(val instanceof NavigationEnd) {
      this.routerArray = val.url.split('/');
      this.routerArray.forEach((value, index)=>{
        if(index > 2 && index < this.routerArray.length - 1) {
          this.parentPageStatus.push(this.titlecasePipe.transform(value.replace(/-/g, " ")));
        }
      })
      if(this.parentPageStatus.length === 0 ) {
        this.parentPageStatus = [this.titlecasePipe.transform(this.routerArray[this.routerArray.length - 2].replace(/-/g, " "))];
      }
      this.pageStatus = this.titlecasePipe.transform(this.routerArray[this.routerArray.length - 1].replace(/-/g, " "));
      }
      if(this.router.url == '/school/staff/teacher-functions/input-final-grade'){
        this.activeMenu = 'inputFinalGrade';
      } 
      if(this.router.url == '/school/staff/teacher-functions/input-effort-grade'){
        this.activeMenu = 'inputEffortGrade';
      } 
      if(this.router.url == '/school/staff/teacher-functions/take-attendance'){
        this.activeMenu = 'takeAttendance';
      }
      this.secondarySidebar = 0;
  });
  }

  ngOnInit(): void {
    // this.gradeComponent = this.gradeComponentListCat[0];
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    this.permissionCategoryForTeacherFunctions = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 11);
    this.checkPermissionAndRoute();
  }

  toggleSecondarySidebar() {
    if(this.secondarySidebar === 0){
      this.secondarySidebar = 1;
    } else {
      this.secondarySidebar = 0;
    }
  }

  changeCategory(step:number = 0){
    this.gradeComponent = this.gradeComponentListCat[step];
  }

  checkPermissionAndRoute() {
    this.permissionCategoryForTeacherFunctions.permissionSubcategory.map((item)=>{
      if(item.permissionSubcategoryId === 50 && !item.rolePermission[0].canView) {
        this.router.navigateByUrl('/school/staff/teacher-functions/input-effort-grade');
      } else if(item.permissionSubcategoryId === 51 && !item.rolePermission[0].canView) {
        this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance']);
      } else if(item.permissionSubcategoryId === 52 && !item.rolePermission[0].canView) {
        this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance']);
      } else if(item.permissionSubcategoryId === 53 && !item.rolePermission[0].canView) {
        this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance']);
      } else if(item.permissionSubcategoryId === 54 && !item.rolePermission[0].canView) {
        this.router.navigate(['/school', 'staff', 'teacher-functions']);
      } else if(item.permissionSubcategoryId === 55 && !item.rolePermission[0].canView) {
        this.router.navigate(['/school', 'staff', 'teacher-functions']);
      }
    })
  }

  checkViewEnableOrNot(id): boolean {
    return this.permissionCategoryForTeacherFunctions.permissionSubcategory.find(x=> x.permissionSubcategoryId === id).rolePermission[0].canView;
  }
}
