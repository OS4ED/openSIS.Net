import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-comments',
  templateUrl: './add-comments.component.html',
  styleUrls: ['./add-comments.component.scss']
})
export class AddCommentsComponent implements OnInit {
  icClose = icClose;
  comments:string;
  actionButtonTitle='submit'
  headerTitle='addCommentTo'
  constructor(
    private dialogRef: MatDialogRef<AddCommentsComponent>,
     public translateService:TranslateService,
     @Inject(MAT_DIALOG_DATA) public data
     ) { 
    translateService.use('en');
      this.comments=data.comments
      if(this.comments){
        this.actionButtonTitle='update'
        this.headerTitle='updateCommentTo'
      }
  }

  ngOnInit(): void {
  }

  addOrUpdateComments() {
    this.dialogRef.close({comments:this.comments,submit:true});
  }

}
