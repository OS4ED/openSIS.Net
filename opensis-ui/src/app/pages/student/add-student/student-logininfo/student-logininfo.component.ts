import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import icVisibility from '@iconify/icons-ic/twotone-visibility';
import icVisibilityOff from '@iconify/icons-ic/twotone-visibility-off';
import icEdit from '@iconify/icons-ic/twotone-edit';
import { SchoolCreate } from '../../../../enums/school-create.enum';

@Component({
  selector: 'vex-student-logininfo',
  templateUrl: './student-logininfo.component.html',
  styleUrls: ['./student-logininfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms

  ],
})
export class StudentLogininfoComponent implements OnInit {
  StudentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  icEdit = icEdit;
  form: FormGroup;
  icVisibility = icVisibility;
  icVisibilityOff = icVisibilityOff;
  inputType = 'password';
  visible = false;

  constructor(private fb: FormBuilder, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {

    this.form = this.fb.group({});
  }

  toggleVisibility() {
    if (this.visible) {
      this.inputType = 'password';
      this.visible = false;
      this.cd.markForCheck();
    } else {
      this.inputType = 'text';
      this.visible = true;
      this.cd.markForCheck();
    }
  }

}
