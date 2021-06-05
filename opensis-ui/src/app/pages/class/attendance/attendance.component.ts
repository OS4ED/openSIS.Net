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

import { Component, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import { MAT_DATE_FORMATS } from "@angular/material/core";
import { MatDialog } from "@angular/material/dialog";
import { TranslateService } from "@ngx-translate/core";
import * as _moment from 'moment';
import { fadeInRight400ms } from "../../../../@vex/animations/fade-in-right.animation";
import { fadeInUp400ms } from "../../../../@vex/animations/fade-in-up.animation";
import { stagger60ms } from "../../../../@vex/animations/stagger.animation";
import { AttendanceDetails } from "../../../models/attendance-details.model";
import { AddTeacherCommentsComponent } from "./add-teacher-comments/add-teacher-comments.component";
import {default as _rollupMoment} from 'moment';
const moment = _rollupMoment || _moment;

const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: "vex-attendance",
  templateUrl: "./attendance.component.html",
  styleUrls: ["./attendance.component.scss"],
  animations: [fadeInRight400ms, stagger60ms, fadeInUp400ms],
  providers: [
    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})

export class AttendanceComponent implements OnInit {
  
  currentComponent:string;
  date = new FormControl(moment());

  attendanceDetails: AttendanceDetails[] = [
    { id: 1, students: "Arthur Boucher", attendanceCodes: "-", comments: "-" },
    { id: 2, students: "Dany Anderson", attendanceCodes: "-", comments: "-" },
    { id: 3, students: "Justin Aponte", attendanceCodes: "-", comments: "-" },
    { id: 4, students: "Julie Davis", attendanceCodes: "-", comments: "-" },
    { id: 5, students: "Javier Holmes", attendanceCodes: "-", comments: "-" },
    { id: 6, students: "Roman Loafer", attendanceCodes: "-", comments: "-" },
    { id: 7, students: "Laura Paiva", attendanceCodes: "-", comments: "-" },
    { id: 8, students: "Jesse Hayes", attendanceCodes: "-", comments: "-" },
    { id: 9, students: "Taylor Hart", attendanceCodes: "-", comments: "-" },
    { id: 10, students: "Casey West", attendanceCodes: "-", comments: "-" },
    { id: 11, students: "Kerry Meadows", attendanceCodes: "-", comments: "-" },
    { id: 12, students: "Sydney English", attendanceCodes: "-", comments: "-" },
    { id: 13, students: "Harper Duncan", attendanceCodes: "-", comments: "-" },
    { id: 14, students: "Carol Mcpherson", attendanceCodes: "-", comments: "-"},
    { id: 15, students: "Coolin Parkar", attendanceCodes: "-", comments: "-" }
  ];
  displayedColumns: string[] = ["students", "attendanceCodes", "comments"];

  constructor(public translateService: TranslateService, private dialog: MatDialog) {
    translateService.use("en");
  }

  ngOnInit(): void {
    this.currentComponent = 'takeAttendance';
  }

  changeComponent(step) {
    this.currentComponent = step;
  }

  addComments(){
    this.dialog.open(AddTeacherCommentsComponent, {
      width: '500px'
    });
  }
}
