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

import { Component, OnInit, Input } from '@angular/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { Router} from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditReportCardGradeComponent } from '../edit-report-card-grade/edit-report-card-grade.component';

@Component({
  selector: 'vex-report-card-grade-list',
  templateUrl: './report-card-grade-list.component.html',
  styleUrls: ['./report-card-grade-list.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class ReportCardGradeListComponent implements OnInit {

  @Input()
  columns = [
    { label: 'ID', property: 'id', type: 'number', visible: true },
    { label: 'Title', property: 'title', type: 'text', visible: true },
    { label: 'Short Name', property: 'short_name', type: 'text', visible: true },
    { label: 'Start Time', property: 'start_time', type: 'text', visible: true },
    { label: 'End Time', property: 'end_time', type: 'text', visible: true },
    { label: 'Length', property: 'length', type: 'number', visible: true },
    { label: 'action', property: 'action', type: 'text', visible: true }
  ];

  ReportCardGradesModelList;

  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  loading:Boolean;

  constructor(private router: Router,private dialog: MatDialog,public translateService:TranslateService) {
    translateService.use('en');
    this.ReportCardGradesModelList = [
      {id: 1, title: 'Daily Attendance', short_name: 'DA', start_time: '10:00 AM', end_time: '10:50 AM', length: 50},
      {id: 2, title: 'Period 2', short_name: 'P2', start_time: '11:00 AM', end_time: '11:50 AM', length: 50},
      {id: 3, title: 'Period 3', short_name: 'P3', start_time: '12:00 PM', end_time: '12:50 PM', length: 50},
      {id: 4, title: 'Lunch', short_name: 'L', start_time: '12:50 PM', end_time: '02:00 PM', length: 70},
      {id: 5, title: 'Period 4', short_name: 'P4', start_time: '02:00 PM', end_time: '02:50 PM', length: 50},
      {id: 6, title: 'Period 5', short_name: 'P5', start_time: '03:00 PM', end_time: '03:50 PM', length: 50},
      {id: 7, title: 'Period 6', short_name: 'P6', start_time: '04:00 PM', end_time: '04:50 PM', length: 50},
    ]
  }

  ngOnInit(): void {
  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  goToAddPeriod() {
    this.dialog.open(EditReportCardGradeComponent, {
      width: '500px'
    });
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

}
