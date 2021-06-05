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
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { GetAllAttendanceCodeModel } from '../../../../models/attendance-code.model';
import { ScheduleStudentListViewModel } from '../../../../models/student-schedule.model';
import { AddUpdateStudentAttendanceModel, GetAllStudentAttendanceListModel, StudentAttendanceModel } from '../../../../models/take-attendance-list.model';
import { AttendanceDetails } from '../../../../models/attendance-details.model';
import { AddTeacherCommentsComponent } from './add-teacher-comments/add-teacher-comments.component';
import { StudentAttendanceService } from 'src/app/services/student-attendance.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentScheduleService } from 'src/app/services/student-schedule.service';
import { AttendanceCodeService } from 'src/app/services/attendance-code.service';
import { SharedFunction } from 'src/app/pages/shared/shared-function';
import { LayoutService } from 'src/@vex/services/layout.service';

@Component({
  selector: 'vex-take-attendance',
  templateUrl: './take-attendance.component.html',
  styleUrls: ['./take-attendance.component.scss']
})
export class TakeAttendanceComponent implements OnInit {

  pageStatus = "Teacher Function";
  public portalAccess: boolean;
  

  displayedColumns: string[] = ['students', 'attendanceCodes', 'comments'];
  staffDetails;
  showShortName:boolean=false;
  getAllAttendanceCodeModel:GetAllAttendanceCodeModel=new GetAllAttendanceCodeModel()
  scheduleStudentListViewModel: ScheduleStudentListViewModel = new ScheduleStudentListViewModel()
  addUpdateStudentAttendanceModel : AddUpdateStudentAttendanceModel = new AddUpdateStudentAttendanceModel();
  isAttendanceDateToday=true;
  studentAttendanceList: GetAllStudentAttendanceListModel= new GetAllStudentAttendanceListModel();
  actionButtonTitle:string = 'submit';
  loading:boolean;

  constructor( private dialog: MatDialog,
    public translateService:TranslateService,
    private studentAttendanceService: StudentAttendanceService,
    private router: Router,
    private snackbar: MatSnackBar,
    private studentScheduleService:StudentScheduleService,
    private attendanceCodeService:AttendanceCodeService,
    private commonFunction: SharedFunction,
    private layoutService: LayoutService,) { 
    translateService.use('en');
    this.staffDetails = this.studentAttendanceService.getStaffDetails();
    console.log("staffDetails",this.staffDetails);
    Object.keys(this.staffDetails).length > 0 ? '' : this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance']);
    this.staffDetails.attendanceDate = this.commonFunction.formatDateSaveWithoutTime(this.staffDetails.attendanceDate)
   
   }

  ngOnInit(): void {
    this.getScheduledStudentList();
    this.isToday();
  }
  isToday(){
    let inputDate = new Date(this.staffDetails.attendanceDate);
    let todaysDate = new Date();
     if(inputDate.setHours(0,0,0,0) != todaysDate.setHours(0,0,0,0)) {
       this.isAttendanceDateToday=false
     }
   }
 
   getScheduledStudentList(){
     this.scheduleStudentListViewModel.sortingModel=null;
     this.scheduleStudentListViewModel.courseSectionId=this.staffDetails.courseSectionId;
     this.scheduleStudentListViewModel.pageNumber=0;
     this.scheduleStudentListViewModel.pageSize = 0;
     this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res)=>{
       if (res._failure) {
         if(res.scheduleStudentForView==null){
           this.snackbar.open(res._message, '', {
             duration: 10000
           });
           this.scheduleStudentListViewModel.scheduleStudentForView=[];
         }else{
           this.scheduleStudentListViewModel.scheduleStudentForView=res.scheduleStudentForView;
         }
        
       } else {
         this.scheduleStudentListViewModel.scheduleStudentForView=res.scheduleStudentForView;
         this.getAllAttendanceCode();
 
       }
     })
   }
 
   getAllAttendanceCode() {
     this.getAllAttendanceCodeModel.attendanceCategoryId=this.staffDetails.attendanceCategoryId;
     this.attendanceCodeService.getAllAttendanceCode(this.getAllAttendanceCodeModel).subscribe((res)=>{
       if(res._failure){
         if(res.attendanceCodeList===null){
           this.getAllAttendanceCodeModel.attendanceCodeList=[];
           this.snackbar.open('' + res._message, '', {
             duration: 10000
           });
         } else{
           this.getAllAttendanceCodeModel.attendanceCodeList=[];
         }
       }else{
         this.getAllAttendanceCodeModel.attendanceCodeList=res.attendanceCodeList;
         this.scheduleStudentListViewModel.scheduleStudentForView.map((item,i)=>{
           this.initializeDefaultValues(item,i);
           if(this.scheduleStudentListViewModel.scheduleStudentForView.length!=i+1){
             this.addUpdateStudentAttendanceModel.studentAttendance.push(new StudentAttendanceModel());
           }
         });
         this.getStudentAttendanceList();
 
       }
     })
     }
 
     getStudentAttendanceList(){
       this.studentAttendanceList = {...this.setDefaultDataInStudentAttendance(this.studentAttendanceList)}
       this.studentAttendanceService.getAllStudentAttendanceList(this.studentAttendanceList).subscribe((res)=>{
         if (typeof (res) == 'undefined') {
           this.studentAttendanceList.studentAttendance = [];
         }
         else {
           if (res._failure) {
             if (res.studentAttendance == null) {
               // this.snackbar.open(res._message, '', {
               //   duration: 5000
               // });
             this.studentAttendanceList.studentAttendance = [];
             this.updateStudentAttendanceList();
 
             } else {
            this.studentAttendanceList.studentAttendance = res.studentAttendance;
            this.updateStudentAttendanceList();  
             }
           } else {
             this.studentAttendanceList.studentAttendance = res.studentAttendance;
             this.updateStudentAttendanceList();
           }
     
         }
       })
     }
 
      
     initializeDefaultValues(item,i){
       this.addUpdateStudentAttendanceModel.studentAttendance[i].studentId=item.studentId;
       this.addUpdateStudentAttendanceModel.studentAttendance[i].attendanceCategoryId=this.staffDetails.attendanceCategoryId;
       this.addUpdateStudentAttendanceModel.studentAttendance[i].attendanceDate=this.staffDetails.attendanceDate;
       this.addUpdateStudentAttendanceModel.studentAttendance[i].blockId=this.staffDetails.blockId;
 
       this.addUpdateStudentAttendanceModel.studentAttendance[i].attendanceCode=this.getAllAttendanceCodeModel.attendanceCodeList[0]?.attendanceCode1.toString();
       this.addUpdateStudentAttendanceModel.studentAttendance[i].comments='';
 
     }
   
     updateStudentAttendanceList(){
       for(let studentAttendance of this.studentAttendanceList.studentAttendance){
         this.addUpdateStudentAttendanceModel.studentAttendance.forEach((addUpdateStudentAttendance)=>{
           if(addUpdateStudentAttendance.studentId==studentAttendance.studentId){
             addUpdateStudentAttendance.attendanceCode=studentAttendance.attendanceCode.toString();
             addUpdateStudentAttendance.comments=studentAttendance.comments;
             this.actionButtonTitle='update';
           }
         })
       }      
     }
 
 
   addComments(index){
     let studentName=this.scheduleStudentListViewModel.scheduleStudentForView[index].firstGivenName+' '+this.scheduleStudentListViewModel.scheduleStudentForView[index].lastFamilyName
     this.dialog.open(AddTeacherCommentsComponent, {
       width: '700px',
       data: {studentName,comments: this.addUpdateStudentAttendanceModel.studentAttendance[index].comments}
     }).afterClosed().subscribe((res)=>{
       if(!res?.submit){
         return
       }
       if(this.addUpdateStudentAttendanceModel.studentAttendance[index].comments || res.comments?.trim()) {
         this.addUpdateStudentAttendanceModel.studentAttendance[index].comments = res.comments.trim();
       }
     });
   }
 
   addUpdateStudentAttendance() {
     this.addUpdateStudentAttendanceModel={...this.setDefaultDataInStudentAttendance(this.addUpdateStudentAttendanceModel)};
       this.studentAttendanceService.addUpdateStudentAttendance(this.addUpdateStudentAttendanceModel).subscribe((res)=>{
         if (res._failure) {
             this.snackbar.open(res._message, '', {
               duration: 10000
             });
         } else {
           this.snackbar.open(res._message, '', {
             duration: 10000
           });          
         }
       })
     
   }
 
   setDefaultDataInStudentAttendance(attendanceModel){
     attendanceModel.courseId=this.staffDetails.courseId;
     attendanceModel.courseSectionId=this.staffDetails.courseSectionId;
     attendanceModel.attendanceDate=this.staffDetails.attendanceDate;
     attendanceModel.periodId=this.staffDetails.periodId;
     attendanceModel.staffId=this.staffDetails.staffId;
     return attendanceModel;
   }
}
