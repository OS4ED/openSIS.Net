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

import { Component, OnInit } from '@angular/core';
import { RolePermissionListViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';

@Component({
  selector: 'vex-attendance-settings',
  templateUrl: './attendance-settings.component.html',
  styleUrls: ['./attendance-settings.component.scss'],
  animations: [
    fadeInRight400ms
  ]
})
export class AttendanceSettingsComponent implements OnInit {
  pages=[]
  attendanceSettings=true;
  pageTitle;
  pageId: string;

  constructor(private cryptoService:CryptoService) { }

  ngOnInit(): void {
    let permissions:RolePermissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    let settingIndex = permissions?.permissionList?.findIndex((item) => {
      return item.permissionGroup?.permissionGroupId == 12
    });
  
    let attendanceMenu= permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory.findIndex((item)=>{
      return item.permissionCategoryId==25;
    });
    permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[attendanceMenu].permissionSubcategory.map((option)=>{
      if(option.rolePermission[0].canView){
        this.pages.push(option.title);
      }
    });

    let availablePageId=localStorage.getItem("pageId");
    if(availablePageId==null || !this.pages.includes(availablePageId)){
      for(let item of permissions?.permissionList[settingIndex]?.permissionGroup.permissionCategory[attendanceMenu].permissionSubcategory){
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
