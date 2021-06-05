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

import { Component, Input, OnInit } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LayoutService } from 'src/@vex/services/layout.service';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import icGeneralInfo from '@iconify/icons-ic/outline-account-circle';
import icAddress from '@iconify/icons-ic/outline-location-on';
import icAccessInfo from '@iconify/icons-ic/outline-lock-open';
import { ImageCropperService } from '../../../services/image-cropper.service';
import { SchoolCreate } from '../../../enums/school-create.enum';
import { ParentInfoService } from '../../../services/parent-info.service';
import { AddParentInfoModel } from '../../../models/parent-info.model';
import { takeUntil } from 'rxjs/operators';
import { RolePermissionListViewModel } from '../../../models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';
import { Router } from '@angular/router';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'vex-edit-parent',
  templateUrl: './edit-parent.component.html',
  styleUrls: ['./edit-parent.component.scss'],
  animations: [  
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditParentComponent implements OnInit {
  pageStatus:string="View Parent";
  showAddressInfo:boolean= false;
  showGeneralInfo: boolean= false;
  icGeneralInfo = icGeneralInfo;
  icAddress = icAddress;
  icAccessInfo = icAccessInfo;
  secondarySidebar = 0;
  destroySubject$: Subject<void> = new Subject();
  pageId:string;
  parentCreate = SchoolCreate;
  parentId: number;
  parentCreateMode: SchoolCreate = SchoolCreate.VIEW;
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  parentTitle='';
  responseImage: string;
  loading:boolean;
  enableCropTool = true;
  constructor(private layoutService: LayoutService,
    private parentInfoService:ParentInfoService,
    private imageCropperService:ImageCropperService,
    private cryptoService: CryptoService,
    private router: Router,
    private loaderService:LoaderService) {
    this.layoutService.collapseSidenav();
    this.imageCropperService.getCroppedEvent().pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.parentInfoService.setParentImage(res[1]);
    });
    this.parentInfoService.modeToUpdate.pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      if(res==this.parentCreate.EDIT){
        this.pageStatus="Edit Parent"
      }else if(res==this.parentCreate.VIEW){
        this.pageStatus="View Parent"
      }
    });
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionListViewModel.permissionList[3].permissionGroup.permissionCategory.forEach(category => {
      if (category.rolePermission[0].canView){
        if (category.title === 'General Info'){
          this.showGeneralInfo = true;
        }
        else if (category.title === 'Address info'){
          this.showAddressInfo = true;
        }
      }
    });
    if (this.permissionListViewModel.permissionList[3].permissionGroup.permissionCategory[0].rolePermission[0].canView){
      this.pageId = 'General Info';
      this.checkCurrentCategoryAndRoute();
    }
    else if (this.permissionListViewModel.permissionList[3].permissionGroup.permissionCategory[1].rolePermission[0].canView){
      this.pageId = 'Address Info';
      this.checkCurrentCategoryAndRoute();
    }

    this.parentCreateMode = this.parentCreate.VIEW;
    this.parentInfoService.setParentCreateMode(this.parentCreateMode);
    this.parentId = this.parentInfoService.getParentId();   
    this.getParentDetailsUsingId();
  }
  

  getParentDetailsUsingId(){
    this.addParentInfoModel.parentInfo.parentId = this.parentId;
    this.parentInfoService.viewParentInfo(this.addParentInfoModel).subscribe(data => {
      this.addParentInfoModel = data;
      this.parentInfoService.sendDetails(this.addParentInfoModel);
      this.parentTitle = (this.addParentInfoModel.parentInfo.salutation?this.addParentInfoModel.parentInfo.salutation+' ':'')+''+
       this.addParentInfoModel.parentInfo.firstname+' '+
       (this.addParentInfoModel.parentInfo.middlename?' '+this.addParentInfoModel.parentInfo.middlename+' ':'')+''+
       this.addParentInfoModel.parentInfo.lastname;
      this.responseImage = this.addParentInfoModel.parentInfo.parentPhoto;     
      this.parentInfoService.setParentImage(this.responseImage);
      this.parentInfoService.setParentDetailsForViewAndEdit(this.addParentInfoModel);
      
    });
    
  }

  toggleSecondarySidebar() {
    if(this.secondarySidebar === 0){
      this.secondarySidebar = 1;
    } else {
      this.secondarySidebar = 0;
    }
  }

  showPage(pageId) {    
    localStorage.setItem("pageId",pageId); 
    this.pageId=localStorage.getItem("pageId");
    this.secondarySidebar = 0; // Close secondary sidenav in mobile view
    this.checkCurrentCategoryAndRoute();
  }

  checkCurrentCategoryAndRoute() {
    if(this.pageId === 'General Info') {
      this.router.navigate(['/school', 'parents', 'parent-generalinfo']);
    } else if(this.pageId === 'Address Info') {
      this.router.navigate(['/school', 'parents', 'parent-addressinfo']);
    }
  }

  ngOnDestroy() {
    this.parentInfoService.setParentImage(null);
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
