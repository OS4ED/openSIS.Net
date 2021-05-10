import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AttendanceDetails } from '../../../../models/attendance-details.model';
import { AddTeacherCommentsComponent } from './add-teacher-comments/add-teacher-comments.component';

@Component({
  selector: 'vex-take-attendance',
  templateUrl: './take-attendance.component.html',
  styleUrls: ['./take-attendance.component.scss']
})
export class TakeAttendanceComponent implements OnInit {

  pageStatus = "Teacher Function";
  public portalAccess: boolean;
  

  attendanceDetails: AttendanceDetails[] = [
    {id: 1, students: 'Arthur Boucher', attendanceCodes: '-', comments: '-'},
    {id: 2, students: 'Dany Anderson', attendanceCodes: '-', comments: '-'},
    {id: 3, students: 'Justin Aponte', attendanceCodes: '-', comments: '-'},
    {id: 4, students: 'Julie Davis', attendanceCodes: '-', comments: '-'},
    {id: 5, students: 'Javier Holmes', attendanceCodes: '-', comments: '-'},
    {id: 6, students: 'Roman Loafer', attendanceCodes: '-', comments: '-'},
    {id: 7, students: 'Laura Paiva', attendanceCodes: '-', comments: '-'},
    {id: 8, students: 'Jesse Hayes', attendanceCodes: '-', comments: '-'},
    {id: 9, students: 'Taylor Hart', attendanceCodes: '-', comments: '-'},
    {id: 10, students: 'Casey West', attendanceCodes: '-', comments: '-'},
    {id: 11, students: 'Kerry Meadows', attendanceCodes: '-', comments: '-'},
    {id: 12, students: 'Sydney English', attendanceCodes: '-', comments: '-'},
    {id: 13, students: 'Harper Duncan', attendanceCodes: '-', comments: '-'},
    {id: 14, students: 'Carol Mcpherson', attendanceCodes: '-', comments: '-'},
    {id: 15, students: 'Coolin Parkar', attendanceCodes: '-', comments: '-'},
    
  ];
  displayedColumns: string[] = ['students', 'attendanceCodes', 'comments'];

  constructor(private dialog: MatDialog, public translateService:TranslateService) { 
    translateService.use('en');
   }

  ngOnInit(): void {
  }

  addComments(){
    this.dialog.open(AddTeacherCommentsComponent, {
      width: '500px'
    });
  }
}
