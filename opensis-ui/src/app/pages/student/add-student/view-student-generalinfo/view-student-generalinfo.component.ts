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
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { StudentAddModel } from '../../../../models/student.model';
import { SharedFunction } from '../../../shared/shared-function';
import { TranslateService } from '@ngx-translate/core';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatDialog } from '@angular/material/dialog';
import { ResetPasswordComponent } from 'src/app/pages/shared-module/reset-password/reset-password.component';

@Component({
  selector: 'vex-view-student-generalinfo',
  templateUrl: './view-student-generalinfo.component.html',
  styleUrls: ['./view-student-generalinfo.component.scss'],
  animations: [
    stagger60ms
  ]
})
export class ViewStudentGeneralinfoComponent implements OnInit {


  studentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  @Input() categoryId;
  @Input() studentViewDetails: StudentAddModel;
  module = 'Student';
  @Input() nameOfMiscValues;
  constructor(private commonFunction: SharedFunction,
              public translateService: TranslateService,
              private dialog: MatDialog
  ) {
    translateService.use('en');
   }

  ngOnInit(): void {
  }

  openResetPassword() {
    this.dialog.open(ResetPasswordComponent, {
      width: '500px',
      data: { userId: this.studentViewDetails.studentMaster.studentId, emailAddress: this.studentViewDetails.studentMaster.studentPortalId }
    });
  }

  getAge(birthDate) {
    return this.commonFunction.getAge(birthDate);
  }

}
