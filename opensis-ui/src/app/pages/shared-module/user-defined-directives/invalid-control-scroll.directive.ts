import {
    Directive,
    HostListener,
    ElementRef
  } from "@angular/core";
  import { FormGroupDirective, NgForm } from "@angular/forms";
  
  @Directive({
    selector: "[appInvalidControlScroll]"
  })
  export class InvalidControlScrollDirective {
   
    constructor(
      private el: ElementRef,
      private formGroupDir: NgForm
    ) {}
  
    @HostListener("ngSubmit") onSubmit() {
      if (this.formGroupDir.control.invalid) {
        this.scrollToFirstInvalidControl();
      }
    }

    scrollToFirstInvalidControl() {
    const firstInvalidControl: HTMLElement = this.el.nativeElement.querySelector(
      'mat-select.ng-invalid, textarea.ng-invalid, input.ng-invalid .custom-scroll'
    );
      firstInvalidControl.scrollIntoView({ behavior: 'smooth',block: 'center' });
  }
  }