import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder,FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditorChangeContent, EditorChangeSelection } from 'ngx-quill';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentCommentsAddView } from '../../../../../models/studentCommentsModel';
import {StudentService} from '../../../../../services/student.service'

@Component({
  selector: 'vex-edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditCommentComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  body: string = null;
  commentTitle: string;
  buttonType: string;
  currentCommnetID: any;
  studentCommentsAddView:StudentCommentsAddView=new StudentCommentsAddView()
  constructor(
    private dialogRef: MatDialogRef<EditCommentComponent>, 
    @Inject(MAT_DIALOG_DATA) public data:any,
    private fb: FormBuilder, 
    private snackbar:MatSnackBar,
    private studentService:StudentService,
    public translateService:TranslateService) {
      this.form= fb.group({
        commentId:[0],
        Body:['',[Validators.required]]
      })
      if(data.information==null){
        
        this.commentTitle="addComment";
        this.currentCommnetID=0;
        this.buttonType="submit";
      }
      else{
        this.studentCommentsAddView.studentComments=data.information;
        this.currentCommnetID=data.information.commentId;
        this.commentTitle="editComment";
        this.buttonType="update";
        this.form.controls.commentId.patchValue(data.information.commentId)
        this.form.controls.Body.patchValue(data.information.comment)
      }
     }

  ngOnInit(): void {
  }
  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
    if (event.source == 'user') {
      this.body = document.querySelector(".ql-editor").innerHTML;
    }
  }
  submit(){
    if(this.form.controls.Body.hasError("required")){
      this.form.controls.Body.markAllAsTouched()
    }
    if(this.form.valid){
      if(this.form.controls.commentId.value==0){
        this.studentCommentsAddView.studentComments.studentId=this.data.studentId;
        this.studentCommentsAddView.studentComments.commentId=this.currentCommnetID;
        this.studentCommentsAddView.studentComments.comment= this.form.controls.Body.value;
        
        this.studentService.addStudentComment(this.studentCommentsAddView).subscribe(
          (res:StudentCommentsAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('Comment failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(res._message, '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        )
      }
      else{
        this.studentCommentsAddView.studentComments.studentId=this.data.studentId;
        this.studentCommentsAddView.studentComments.commentId=this.currentCommnetID;
        this.studentCommentsAddView.studentComments.comment= this.form.controls.Body.value;
        this.studentService.updateStudentComment(this.studentCommentsAddView).subscribe(
          (res:StudentCommentsAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('Comment failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(res._message, '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        )
      }
    }
  }
}
