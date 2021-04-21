import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { StudentAttendanceService } from '../../../../../services/student-attendance.service';
import { AddUpdateStudentAttendanceModel, GetAllStudentAttendanceListModel, StudentAttendanceModel } from '../../../../../models/take-attendance-list.model';
import { AddCommentsComponent } from './add-comments/add-comments.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentScheduleService } from '../../../../../services/student-schedule.service';
import { GetAllAttendanceCodeModel } from '../../../../../models/attendance-code.model';
import { AttendanceCodeService } from '../../../../../services/attendance-code.service';
import {SharedFunction} from '../../../../shared/shared-function'
import { LoaderService } from '../../../../../services/loader.service';
import { ScheduleStudentListViewModel } from '../../../../../models/student-schedule.model';
@Component({
  selector: 'vex-period-attendance',
  templateUrl: './period-attendance.component.html',
  styleUrls: ['./period-attendance.component.scss']
})
export class PeriodAttendanceComponent implements OnInit {
  pageStatus = "Teacher Function";
  portalAccess: boolean;
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
  constructor(
    private dialog: MatDialog,
    public translateService:TranslateService,
    private studentAttendanceService: StudentAttendanceService,
    private router: Router,
    private snackbar: MatSnackBar,
    private studentScheduleService:StudentScheduleService,
    private attendanceCodeService:AttendanceCodeService,
    private commonFunction: SharedFunction,
    private loaderService:LoaderService
    ) { 
    translateService.use('en');
    this.staffDetails = this.studentAttendanceService.getStaffDetails();
    Object.keys(this.staffDetails).length > 0 ? '' : this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance']);

    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
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
    this.dialog.open(AddCommentsComponent, {
      width: '500px',
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
