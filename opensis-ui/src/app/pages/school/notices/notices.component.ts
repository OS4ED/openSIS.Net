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

import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit } from '@angular/core';
import icArrowDropDown from '@iconify/icons-ic/arrow-drop-down';
import icAdd from '@iconify/icons-ic/add';
import { MatDialog } from '@angular/material/dialog';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { EditNoticeComponent } from '../notices/edit-notice/edit-notice.component';
import { NoticeService } from '../../../services/notice.service';
import { NoticeListViewModel } from '../../../models/notice.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { MembershipService } from '../../../services/membership.service';
import { GetAllMembersList } from '../../../models/membership.model';
import moment from 'moment';
import { LoaderService } from '../../../services/loader.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { LayoutService } from 'src/@vex/services/layout.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../services/roll-based-access.service';
import { CryptoService } from '../../../services/Crypto.service';
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'vex-notices',
  templateUrl: './notices.component.html',
  styleUrls: ['./notices.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class NoticesComponent implements OnInit, OnDestroy {

  @Input() afterClosed = new EventEmitter<boolean>();
  noticeListViewModel: NoticeListViewModel = new NoticeListViewModel();
  noticeList = [];
  icPreview = icArrowDropDown;
  icAdd = icAdd;
  activateOpenAddNew= true;
  recordFor: string;
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private dialog: MatDialog,
              public translateService: TranslateService,
              private noticeService: NoticeService,
              private membershipService: MembershipService,
              private snackbar: MatSnackBar,
              private loaderService: LoaderService,
              private cdr: ChangeDetectorRef,
              public rollBasedAccessService: RollBasedAccessService,
              private layoutService: LayoutService,
              private cryptoService: CryptoService) {
    translateService.use('en');
    if (localStorage.getItem('collapseValue') !== null){
      if ( localStorage.getItem('collapseValue') === 'false'){
        this.layoutService.expandSidenav();
      }else{
        this.layoutService.collapseSidenav();
      }
    }else{
      this.layoutService.expandSidenav();
    }
    this.loaderService.isLoading.subscribe((v) => {
      this.loading = v;
    });
    this.noticeService.currentNotice.pipe(takeUntil(this.destroySubject$)).subscribe(
      res => {
        if (res){
          this.showRecords('All');
        }
      }
    );

  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 4);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;

    this.showRecords('All');
    this.getMemberList();
  }
  ngAfterViewChecked(){
    this.cdr.detectChanges();
 }
  getMemberList(){
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe(
      (res) => {
        if (res){
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllMembersList = res;
          }

        }
        else{
          this.snackbar.open('No Member Found. ' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
    });
  }
  showRecords(event) {
    this.noticeService.getAllNotice(this.noticeListViewModel).subscribe(
      (res) => {
      this.noticeList = res.noticeList;
      if (event != 'All') {
        this.recordFor = event.target.innerHTML;
        const today = new Date();
        if (this.recordFor.toLowerCase() == 'today') {
          this.noticeList = res.noticeList?.filter(
            m => {
                const validFrom = moment(m.validFrom).format('DD-MM-YYYY').toString();
                const validFromarr = validFrom.split('-');
                const validFromDate = +validFromarr[0];
                const validFromMonth = +validFromarr[1];
                const validFromYear = +validFromarr[2];
                const validTo = moment(m.validTo).format('DD-MM-YYYY').toString();
                const validToarr = validTo.split('-');
                const validToDate = +validToarr[0];
                const validToMonth = +validToarr[1];
                const validToYear = +validToarr[2];
                const today = moment().format('DD-MM-YYYY').toString();
                const todayarr = today.split('-');
                const todayDate = +todayarr[0];
                const todayMonth = +todayarr[1];
                const todayYear = +todayarr[2];

                if (validFromYear <= todayYear && validToYear >= todayYear){
                  if (validFromMonth <= todayMonth && validToMonth >= todayMonth){
                    if (validFromDate <= todayDate && validToDate >= todayDate){
                      return m;
                    }
                  }
                }
            }
          );
        }
        else if (this.recordFor.toLowerCase() === 'upcoming') {
          this.noticeList =  res.noticeList?.filter(
            m => {

              const validFrom = moment(m.validFrom).format('DD-MM-YYYY').toString();
              const validFromarr = validFrom.split('-');
              const validFromDate = +validFromarr[0];
              const validFromMonth = +validFromarr[1];
              const validFromYear = +validFromarr[2];
              const today = moment().format('DD-MM-YYYY').toString();
              const todayarr = today.split('-');
              const todayDate = +todayarr[0];
              const todayMonth = +todayarr[1];
              const todayYear = +todayarr[2];
              if (validFromYear > todayYear){
                return m;
              }
              else if (validFromYear == todayYear){
                if (validFromMonth > todayMonth){
                  return m;
                }
                else if (validFromMonth == todayMonth){
                  if (validFromDate > todayDate){
                    return m;
                  }
                }
              }
            }
          );
        }
        else if (this.recordFor.toLowerCase() == 'past') {
          this.noticeList = res.noticeList?.filter(
            m => {

              const validTo = moment(m.validTo).format('DD-MM-YYYY').toString();
              const validToarr = validTo.split('-');
              const validToDate = +validToarr[0];
              const validToMonth = +validToarr[1];
              const validToYear = +validToarr[2];
              const today = moment().format('DD-MM-YYYY').toString();
              const todayarr = today.split('-');
              const todayDate = +todayarr[0];
              const todayMonth = +todayarr[1];
              const todayYear = +todayarr[2];
              if (validToYear < todayYear){
                return m;
              }
              else if (validToYear == todayYear){
                if (validToMonth < todayMonth){
                  return m;
                }
                else if (validToMonth == todayMonth){
                  if (validToDate < todayDate){
                    return m;
                  }
                }
              }
            }
          );
        }
      }
    });
  }

  openAddNew() {
      this.dialog.open(EditNoticeComponent, {
        data: {allMembers: this.getAllMembersList, membercount: this.getAllMembersList.getAllMemberList.length},
        width: '800px'
      }).afterClosed().subscribe(
        res => {
          if (res){
            this.showRecords('All');
          }
        }
      );
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
    this.noticeService.changeNotice(false);
  }


}
