import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-create-assignment',
  templateUrl: './create-assignment.component.html',
  styleUrls: ['./create-assignment.component.scss']
})
export class CreateAssignmentComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;

  constructor(private dialogRef: MatDialogRef<CreateAssignmentComponent>) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      'editor': new FormControl(null)
    })
  }

}
