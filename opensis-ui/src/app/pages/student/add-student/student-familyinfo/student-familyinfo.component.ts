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

import { Component, OnInit , Input} from '@angular/core';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/baseline-add';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { StudentService } from '../../../../services/student.service';
@Component({
  selector: 'vex-student-familyinfo',
  templateUrl: './student-familyinfo.component.html',
  styleUrls: ['./student-familyinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StudentFamilyinfoComponent implements OnInit {
  @Input() studentCreateMode: SchoolCreate;
  @Input() studentDetailsForViewAndEdit;
  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  currentTab: string;
  studentDetailsForViewAndEditData;
  constructor(public translateService: TranslateService,
    private studentService: StudentService) {
  }

  ngOnInit(): void {
    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
    });
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.studentDetailsForViewAndEdit = res;
    });
    this.studentDetailsForViewAndEditData = this.studentDetailsForViewAndEdit;
    this.currentTab = 'contacts';
  }
  changeTab(status){
    this.currentTab = status;
  }


}
