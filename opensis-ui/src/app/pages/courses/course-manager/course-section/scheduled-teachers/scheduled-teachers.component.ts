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

import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import icEmail from '@iconify/icons-ic/twotone-email';
import { ScheduledStaffForCourseSection } from '../../../../../models/course-section.model';
import { CourseSectionService } from '../../../../../services/course-section.service';

@Component({
  selector: 'vex-scheduled-teachers',
  templateUrl: './scheduled-teachers.component.html',
  styleUrls: ['./scheduled-teachers.component.scss']
})
export class ScheduledTeachersComponent implements OnInit {
  icEmail = icEmail;
  @Input() courseSectionDetails;
  constructor(private courseSectionService:CourseSectionService,
    private snackbar:MatSnackBar) { } 
  scheduledTeacher:ScheduledStaffForCourseSection = new ScheduledStaffForCourseSection();
  
  ngOnChanges(changes:SimpleChanges){
        this.getScheduledTeachers();
  }
  ngOnInit(): void {
  }

  getScheduledTeachers(){
    this.scheduledTeacher.courseId=this.courseSectionDetails.courseSection.courseId;
    this.scheduledTeacher.courseSectionId=this.courseSectionDetails.courseSection.courseSectionId;

    this.courseSectionService.getAllStaffScheduleInCourseSection(this.scheduledTeacher).subscribe((res)=>{
      if (typeof (res) == 'undefined') {       
        this.snackbar.open('Teacher Schedule Failed ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) { 
            if(res.courseSectionsList==null){
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
              this.scheduledTeacher.courseSectionsList=[]
            }else{
              this.scheduledTeacher.courseSectionsList=[]
            }
        } else {
          this.scheduledTeacher.courseSectionsList=res.courseSectionsList;
          this.scheduledTeacher.courseSectionsList[0].staffCoursesectionSchedule.forEach((item)=>{
            item.staffMaster.staffEmail=item.staffMaster.loginEmailAddress?item.staffMaster.loginEmailAddress
                            :item.staffMaster.personalEmail?item.staffMaster.personalEmail
                            :item.staffMaster.schoolEmail?item.staffMaster.schoolEmail:null
          })
        }
      }
    })
  }

}
