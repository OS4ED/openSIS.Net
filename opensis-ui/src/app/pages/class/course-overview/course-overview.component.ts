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
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/edit';
import icHowtoReg from '@iconify/icons-ic/outline-how-to-reg';
import { CourseSectionService } from '../../../services/course-section.service';
import { ClassRoomURLInCourseSectionModel, CourseSectionAddViewModel, GetAllCourseSectionModel } from 'src/app/models/course-section.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-course-overview',
  templateUrl: './course-overview.component.html',
  styleUrls: ['./course-overview.component.scss']
})
export class CourseOverviewComponent implements OnInit {

  icEdit = icEdit;
  icHowtoReg = icHowtoReg;
  showUrlInput = false;
  hideUrl = true;
  hidePassword = true;
  showPasswordInput = false;
  courseSectionId;
  selectedCourseSection;
  singleCourseSection;
  meatingDays;
  markingperiod;
  roomTitle;
  getAllCourseSectionModel: GetAllCourseSectionModel = new GetAllCourseSectionModel();
  courseSectionAddViewModel: CourseSectionAddViewModel = new CourseSectionAddViewModel();
  classRoomURLInCourseSectionModel: ClassRoomURLInCourseSectionModel = new ClassRoomURLInCourseSectionModel();
  periodTitle;
  weekArray = ['S', 'M', 'T', 'W', 'T', 'F', 'S'];
  name = 'subhajit';

  constructor(
    public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private courseSectionService: CourseSectionService) {
    translateService.use('en');
    this.courseSectionId = JSON.parse(localStorage.getItem('courseSectionId'));
    this.selectedCourseSection = JSON.parse(localStorage.getItem('selectedCourseSection'));
    this.meatingDays = this.selectedCourseSection.meetingDays.replace(/[0-9]/g, '');
    this.meatingDays = this.selectedCourseSection.meetingDays.replace(this.meatingDays, '').split('').map(
      x => {
        x = +x;
        return x;
    });
  }

  ngOnInit(): void {
    this.getAllCourseSection();
  }
  getAllCourseSection(){
    this.getAllCourseSectionModel.courseId = this.selectedCourseSection.courseId;
    this.getAllCourseSectionModel.academicYear = +sessionStorage.getItem('academicyear');
    this.courseSectionService.getAllCourseSection(this.getAllCourseSectionModel).subscribe(
      (res: GetAllCourseSectionModel) => {
        if (res){
          if (res._failure){
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
          else{
            this.singleCourseSection = res.getCourseSectionForView.find(couresesection =>
              couresesection.courseSection.courseSectionId === this.courseSectionId);
          }
        }
        else{
          this.snackbar.open( sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  addUrl() {
    this.hideUrl = false;
    this.hidePassword = false;
    this.showUrlInput = true;
    this.showPasswordInput = true;
  }
  getRoomTitle(courseSection) {
    if (courseSection.scheduleType === 'Fixed Schedule') {
      this.roomTitle = courseSection?.courseFixedSchedule?.rooms?.title;
    }
    return this.roomTitle;
  }
  getPeriodTitle(courseSection) {
    if (courseSection.scheduleType === 'Fixed Schedule') {
      this.periodTitle = courseSection?.courseFixedSchedule?.blockPeriod?.periodTitle;

    }
    return this.periodTitle;
  }
  findMarkingPeriodTitle(){
    if (this.singleCourseSection.courseSection.durationBasedOnPeriod){
      if (this.singleCourseSection.courseSection.quarters != null){
        this.markingperiod.mpTitle = this.singleCourseSection.courseSection.quarters.title;
        this.markingperiod.mpStartDate = this.singleCourseSection.courseSection.quarters.startDate;
        this.markingperiod.mpEndDate = this.singleCourseSection.courseSection.quarters.endDate;
      }else if (this.singleCourseSection.courseSection.schoolYears != null){
        this.markingperiod.mpTitle = this.singleCourseSection.courseSection.schoolYears.title;
        this.markingperiod.mpStartDate = this.singleCourseSection.courseSection.schoolYears.startDate;
        this.markingperiod.mpEndDate = this.singleCourseSection.courseSection.schoolYears.endDate;
      }else{
        this.markingperiod.mpTitle = this.singleCourseSection.courseSection.semesters.title;
        this.markingperiod.mpStartDate = this.singleCourseSection.courseSection.semesters.startDate;
        this.markingperiod.mpEndDate = this.singleCourseSection.courseSection.semesters.endDate;
      }
    }
  }
  addPassword() {
    this.hidePassword = false;
    this.showPasswordInput = true;
  }
  updateClassroomUrlandPassword(){
    this.classRoomURLInCourseSectionModel.courseSection.courseSectionId = this.singleCourseSection.courseSection.courseSectionId;
    this.classRoomURLInCourseSectionModel.courseSection.academicYear = this.singleCourseSection.courseSection.academicYear;
    this.classRoomURLInCourseSectionModel.courseSection.onlineClassRoom = this.singleCourseSection.courseSection.onlineClassRoom;
    this.classRoomURLInCourseSectionModel.courseSection.onlineClassroomUrl = this.singleCourseSection.courseSection.onlineClassroomUrl;
    this.classRoomURLInCourseSectionModel.courseSection.onlineClassroomPassword =
    this.singleCourseSection.courseSection.onlineClassroomPassword;
    this.classRoomURLInCourseSectionModel.courseSection.courseId = this.singleCourseSection.courseSection.courseId;
    this.courseSectionService.updateOnlineClassRoomURLInCourseSection(this.classRoomURLInCourseSectionModel).subscribe(
      (res) => {
        if (res){
          if (res._failure){
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
          else{
            this.hideUrl = true;
            this.hidePassword = true;
            this.showUrlInput = false;
            this.showPasswordInput = false;
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
        }
        else{
          this.snackbar.open( sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  copyInputMessage(inputElement){
    inputElement.select();
    document.execCommand('copy');
    inputElement.setSelectionRange(0, 0);
  }
}
