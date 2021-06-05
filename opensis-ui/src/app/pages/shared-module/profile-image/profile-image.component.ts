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

import { Component, OnInit, ViewChild, TemplateRef, Input, OnDestroy, OnChanges } from '@angular/core';
import { ImageCroppedEvent, base64ToFile } from 'ngx-image-cropper';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import icClose from '@iconify/icons-ic/twotone-close';
import icUpload from '@iconify/icons-ic/cloud-upload';
import { ImageCropperService } from '../../../services/image-cropper.service';
import { SchoolService } from '../../../services/school.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { StaffService } from '../../../services/staff.service';
import { StudentService } from '../../../services/student.service';
import { ImageModel } from '../../../models/image-cropper.model';
import { SchoolCreate } from '../../../enums/school-create.enum';
import { ModuleIdentifier } from '../../../enums/module-identifier.enum';
import { StaffAddModel } from '../../../models/staff.model';
import { StudentAddModel } from '../../../models/student.model';
import { LoaderService } from '../../../services/loader.service';
import { SchoolAddViewModel } from '../../../models/school-master.model';
import { ParentInfoService } from '../../../services/parent-info.service';
import { AddParentInfoModel } from '../../../models/parent-info.model';
import { NgxImageCompressService } from 'ngx-image-compress';

@Component({
  selector: 'vex-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss'],
  animations: [
    fadeInRight400ms
  ]
})
export class ProfileImageComponent implements OnInit, OnDestroy {
  @ViewChild('mytemplate') mytemplate: TemplateRef<any>;

  icUpload = icUpload;
  icClose = icClose;
  modes = SchoolCreate;
  createMode: SchoolCreate;
  moduleIdentifier: ModuleIdentifier;
  modules = ModuleIdentifier;
  preview: string = '';
  originalFileName: string;
  imageChangedEvent = '';
  croppedImage = '';
  showCropTool: boolean = false;
  showCropperandButton: boolean;
  // afterConvertingBase64toFile;
  fileUploader: any;
  hideCropperToolButton: boolean = true;
  enableUpload: boolean;
  inputType: string = "file";
  destroySubject$: Subject<void> = new Subject();
  @Input() enableCropTool = true;
  @Input() customCss = 'rounded-full border-2 border-gray-light';
  @Input() responseImage;
  loading: boolean;
  staffAddModel: StaffAddModel = new StaffAddModel();
  studentAddModel: StudentAddModel = new StudentAddModel();
  schoolAddModel: SchoolAddViewModel = new SchoolAddViewModel();
  AddParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  constructor(private dialog: MatDialog,
    private imageCropperService: ImageCropperService,
    private snackbar: MatSnackBar,
    private schoolService: SchoolService,
    private staffService: StaffService,
    private parentService: ParentInfoService,
    private studentService: StudentService,
    private loaderService: LoaderService,
    private imageCompressService: NgxImageCompressService,
    ) {
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });

    this.imageCropperService.shareImageStatus.pipe(takeUntil(this.destroySubject$)).subscribe((message) => {
      if (message == "school") {
        this.preview = '';
        this.responseImage = this.schoolService.getSchoolCloneImage();
      }
      if (message == "staff") {
        this.preview = '';
        this.croppedImage = '';
        this.responseImage = this.staffService.getStaffCloneImage();
      }
      if (message == "student") {
        this.preview = '';
        this.croppedImage = '';
        this.responseImage = this.studentService.getStudentCloneImage();
      }
    });
  }

  ngOnInit(): void {
    this.imageCropperService.sharedMessage.pipe(takeUntil(this.destroySubject$)).subscribe((res: ImageModel) => {
      if (res.upload) {
        this.moduleIdentifier = res.module;
        this.createMode = res.mode;
        this.inputType = "file"
      } else {
        this.moduleIdentifier = res.module;
        this.createMode = res.mode;
        this.inputType = "none"
      }
    });
  }

  imageCropped(event: ImageCroppedEvent) {
    this.imageCompressService.compressFile(event.base64, -1, 75, 50).then(result => {
        this.croppedImage = result;
        let base64ImageSplit = this.croppedImage.split(',')
        let sendCropImage = (croppedImage) => {
          this.imageCropperService.sendCroppedEvent(croppedImage);
        }
        sendCropImage(base64ImageSplit);
      });
  }

  setImage() {
    this.showCropperandButton = true;
  }
  unsetImage() {
    this.hideCropperToolButton = !this.hideCropperToolButton;
  }

  imageLoaded() {
    // console.log("Image is loaded!");
  }
  cropperReady() {
    // cropper ready
  }
  loadImageFailed() {
    // console.log("Failed to Load!!")
  }

  uploadFile(event, fileUpload) {
    this.fileUploader = fileUpload;
    this.responseImage = null;
    this.hideCropperToolButton = true;
    // if (event.target.files[0]?.size > 307200) {
    //   this.snackbar.open('Warning: File must be less than 300kb', '', { duration: 10000 });
    // } else
     if ((event.target.files[0]?.type == "image/jpeg") ||
      (event.target.files[0]?.type == "image/jpg") ||
      (event.target.files[0]?.type == "image/png")) {
      this.originalFileName = event.target.files[0].name;
      this.showCropTool = false;
      let files = event.target.files;
      let _URL = window.URL || window.webkitURL;
      for (let i = 0; i < files.length; i++) {
        // let img = new Image();
        // img.onload = () => {
          if (this.enableCropTool) {
            this.callCropper(event);
          } else {
            this.callUncropper(event);
          }
        // }

        // img.src = event.target.result;
        // img.src = _URL.createObjectURL(files[i]);
      }
    } else {
      if (event.target.files[0]?.size > 0) {
        this.snackbar.open('Warning: Only jpg/jpeg/png will support', '', { duration: 10000 });
      }
    }
  }

  callCropper(event) {
    this.imageChangedEvent = event;
    this.showCropTool = true;
    this.openModal();
    return false;
  }

  callUncropper(event) {
    const file = (event.target as HTMLInputElement).files[0];
    if (file) {
      const reader = new FileReader();
    reader.onload = () => {
      this.resizeImage(reader.result, 256, 256).then(compressed => {
        this.imageCompressService.compressFile(compressed as string, -1, 75, 50).then(result => {
          this.preview = result ;
          this.handleReaderLoaded(result.split(',')[1]);
        });
      })
    }
      reader.readAsDataURL(file);
    }
  }

  resizeImage(src, newX, newY) {
    return new Promise((resolve, reject) => {
      const img = new Image();
      img.src = src;
      img.onload = () => {
        const elem = document.createElement('canvas');
        elem.width = newX;
        elem.height = newY;
        const ctx = elem.getContext('2d');
        ctx.drawImage(img, 0, 0, newX, newY);
        const data = ctx.canvas.toDataURL();
        resolve(data);
      }
      img.onerror = error => reject(error);
    })
  }

  handleReaderLoaded(e) {
    this.croppedImage = '';
    let sendImageData2 = (e) => {
      this.imageCropperService.sendUncroppedEvent(e);
      if (this.moduleIdentifier == this.modules.SCHOOL && this.createMode != this.modes.ADD) {
        this.updateSchoolImage();
      }
      this.fileUploader.value = null;
    }
    sendImageData2(e);
  }

  openModal() {
    let dialogRef = this.dialog.open(this.mytemplate, {
      width: '700px',
    });

    dialogRef.afterClosed().subscribe(result => {
      // Do Something after Dialog Closed
      this.fileUploader.value = null;

    });
  }

  onClose() {
    this.hideCropperToolButton = false;
    this.fileUploader.value = null;
    // this.cancelPhoto();
    // this.showCropperandButton=true;
    this.dialog.closeAll();
  }

  cancelPhoto() {
    if (this.moduleIdentifier == this.modules.STUDENT) {
      this.preview = '';
      this.croppedImage = '';
      this.responseImage = this.studentService.getStudentCloneImage();
      this.dialog.closeAll();
    } else if (this.moduleIdentifier == this.modules.STAFF) {
      this.preview = '';
      this.croppedImage = '';
      this.responseImage = this.staffService.getStaffCloneImage();
      this.dialog.closeAll();
    }
  }

  uploadPhotoDirectly() {
    if (this.moduleIdentifier == this.modules.STUDENT) {
      this.updateStudentImage();
    } else if (this.moduleIdentifier == this.modules.STAFF) {
      this.updateStaffImage();
    } else if (this.moduleIdentifier == this.modules.PARENT) {
      this.updateParentImage();
    }
  }

  updateSchoolImage() {
    this.schoolService.addUpdateSchoolLogo(this.schoolAddModel).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
          this.schoolService.setSchoolCloneImage(res.schoolMaster.schoolDetail[0].schoolLogo);
        }
      }
    });
  }

  updateStudentImage() {
    this.studentService.addUpdateStudentPhoto(this.studentAddModel).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
          this.studentService.setStudentCloneImage(res.studentMaster.studentPhoto);
          this.dialog.closeAll();
        }
      }
    });
  }

  updateStaffImage() {
    this.staffService.addUpdateStaffPhoto(this.staffAddModel).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
          this.staffService.setStaffCloneImage(res.staffMaster.staffPhoto);
          this.dialog.closeAll();
        }
      }
    });
  }

  updateParentImage() {
    this.parentService.addUpdateParentPhoto(this.AddParentInfoModel).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
          this.dialog.closeAll();
        }
      }
    });
  }


  ngOnDestroy(): void {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
