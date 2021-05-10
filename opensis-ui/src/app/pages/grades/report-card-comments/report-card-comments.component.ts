import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import { GetAllCourseListModel } from '../../../models/course-manager.model';
import { CourseManagerService } from '../../../services/course-manager.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReportCardService } from '../../../services/report-card.service';
import { DefaultValuesService } from '../../../common/default-values.service';
import { AddCourseCommentCategoryModel, CommentModel, DeleteCourseCommentCategoryModel, DistinctGetAllReportCardModel, DistinctReportCardModel, GetAllCourseCommentCategoryModel, ReportCardCommentModel, UpdateSortOrderForCourseCommentCategoryModel } from '../../../models/report-card.model';
import { CdkDragDrop } from "@angular/cdk/drag-drop";
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'vex-report-card-comments',
  templateUrl: './report-card-comments.component.html',
  styleUrls: ['./report-card-comments.component.scss']
})
export class ReportCardCommentsComponent implements OnInit {
  icEdit = icEdit;
  icDelete = icDelete;

  selectedCategoryIndex = -1;
  getAllCourseListModel: GetAllCourseListModel = new GetAllCourseListModel();
  getAllCommentsWithUniqueCategory: DistinctGetAllReportCardModel = new DistinctGetAllReportCardModel();
  selectedCoursesForCategory = [];
  allCommentsWithCategoryInAdd: DistinctGetAllReportCardModel = new DistinctGetAllReportCardModel();
  commentListBasedOnSelectedCategory=[];
  newComment: string;
  loading: boolean;
  selectedCourseId=null;
  constructor(
    public translateService: TranslateService,
    private courseManagerService: CourseManagerService,
    private snackbar: MatSnackBar,
    private reportCardService: ReportCardService,
    private defaultValuesService: DefaultValuesService,
    private dialog: MatDialog,
    private loaderService:LoaderService) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.getAllCourse();
    this.getAllCourseCommentCategories();
  }

  onCourseChange(index){
    this.selectedCategoryIndex=index;
    this.newComment='';
    if(this.selectedCategoryIndex!==-1){
      this.selectedCourseId=this.getAllCommentsWithUniqueCategory.courseCommentCategories[this.selectedCategoryIndex].courseId;

      this.commentListBasedOnSelectedCategory= JSON.parse(JSON.stringify(this.getAllCommentsWithUniqueCategory.courseCommentCategories[this.selectedCategoryIndex].comments)) ;
      this.commentListBasedOnSelectedCategory.sort((a, b) =>a.sortOrder < b.sortOrder ? -1 : 1 )
    }else{
      this.selectedCourseId=null;
      let nullCourseCategoryIndex = this.getAllCommentsWithUniqueCategory.courseCommentCategories.findIndex(item=>item.courseId==null);
      if(nullCourseCategoryIndex!==-1){
        this.commentListBasedOnSelectedCategory=JSON.parse(JSON.stringify(this.getAllCommentsWithUniqueCategory.courseCommentCategories[nullCourseCategoryIndex].comments));
      this.commentListBasedOnSelectedCategory.sort((a, b) =>a.sortOrder < b.sortOrder ? -1 : 1 )
      }else{
        this.commentListBasedOnSelectedCategory=[];
      }
    }
  }

  getAllCourse() {

    this.courseManagerService.GetAllCourseList(this.getAllCourseListModel).subscribe(res => {
      if (res) {
        if (res._failure) {
          if (res.courseViewModelList === null) {
            this.getAllCourseListModel.courseViewModelList = [];
            this.snackbar.open(res._message, '', {
              duration: 1000
            });
          } else {
            this.getAllCourseListModel.courseViewModelList = res.courseViewModelList;
          }
        } else {
          this.getAllCourseListModel.courseViewModelList = res.courseViewModelList;
        }
      } else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  getAllCourseCommentCategories(){
    this.getAllCommentsWithUniqueCategory.courseCommentCategories=[];
    this.selectedCoursesForCategory=[];
    const getAllCommentsWithCategory: GetAllCourseCommentCategoryModel = new GetAllCourseCommentCategoryModel();
    this.reportCardService.getAllCourseCommentCategory(getAllCommentsWithCategory).subscribe((res)=>{
      if(res){
        if(res._failure){
          
          if(res.courseCommentCategories){
            this.renderComments(res);
          }else{
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.getAllCommentsWithUniqueCategory.courseCommentCategories=[];
            this.commentListBasedOnSelectedCategory=[];
          }
        }else{
          this.renderComments(res);
        }
      }else{
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  renderComments(res){
    res.courseCommentCategories.map((item)=>{
      this.createDatasetForView(item,item.courseId)
  })
  this.getAllCommentsWithUniqueCategory.courseCommentCategories.map((item)=>{
    this.selectedCoursesForCategory.push(item.courseId);
  });

  this.getAllCommentsWithUniqueCategory.courseCommentCategories.sort((a, b) =>a.courseId < b.courseId ? -1 : 1 );
  let index=this.getAllCommentsWithUniqueCategory.courseCommentCategories.findIndex(item=>item.courseId===this.selectedCourseId);
  if(index!==-1 && index!==0){
    this.onCourseChange(index);
  }else{
    this.onCourseChange(-1);
  }
  // if(this.selectedCategoryIndex<=this.getAllCommentsWithUniqueCategory.courseCommentCategories.length-1){
    
  //   this.onCourseChange(this.selectedCategoryIndex);
  // }else{
  //   this.onCourseChange(-1);
  // }
  }

  createDatasetForView(item,courseId){
    let existingCatIndex:number;
    if(courseId){
      existingCatIndex = this.getAllCommentsWithUniqueCategory.courseCommentCategories.findIndex(eachCat=>eachCat.courseId===item.courseId);
    }else{
      existingCatIndex = this.getAllCommentsWithUniqueCategory.courseCommentCategories.findIndex(eachCat=>eachCat.courseId===null);
    }
    if(existingCatIndex!==-1){
      let comment: CommentModel[] = [];
      comment.push({
        courseId:item.courseId,
        courseName:item.courseName,
        applicableAllCourses:item.applicableAllCourses,
        courseCommentId:item.courseCommentId,
        comment:item.comments,
        takeInput:false,
        sortOrder:item.sortOrder
      })
      this.getAllCommentsWithUniqueCategory.courseCommentCategories[existingCatIndex].comments.push(...comment)
    }else{
      let distinctReportCard: DistinctReportCardModel = new DistinctReportCardModel();
      let comment: CommentModel[] = [];
      comment.push({
        courseId:item.courseId,
        courseName:item.courseName,
        applicableAllCourses:item.applicableAllCourses,
        courseCommentId:item.courseCommentId,
        comment:item.comments,
        takeInput:false,
        sortOrder:item.sortOrder
      })
      distinctReportCard={
        static:false,

        courseCommentId:item.courseCommentId,
        courseId: item.courseId,
        courseName: item.courseName,
        applicableAllCourses:item.applicableAllCourses,
        comments:comment,
        sortOrder: item.sortOrder
      }
      this.getAllCommentsWithUniqueCategory.courseCommentCategories.push(distinctReportCard);
    }
  }

  onCourseSelect(courseDetails){
    this.selectedCoursesForCategory.push(courseDetails.courseId);
    let courseCategory: DistinctReportCardModel = new DistinctReportCardModel();
    this.defaultValuesService.getAllMandatoryVariable(courseCategory);
    courseCategory.static=true;
    courseCategory.courseId=courseDetails.courseId;
    courseCategory.courseName=courseDetails.courseTitle;
    courseCategory.comments=[];
    courseCategory.createdBy=this.defaultValuesService.getEmailId();
    courseCategory.updatedBy=this.defaultValuesService.getEmailId();
    this.getAllCommentsWithUniqueCategory.courseCommentCategories.push(courseCategory);
    this.selectedCategoryIndex=this.getAllCommentsWithUniqueCategory.courseCommentCategories.length-1;
    this.selectedCourseId=this.getAllCommentsWithUniqueCategory.courseCommentCategories[this.selectedCategoryIndex].courseId;
    this.onCourseChange(this.selectedCategoryIndex);
  }

  confirmDeleteGradeScale(details,type,index?) {
    // If type is 0, then its Category neither its a Comment.
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
        title: 'Are you sure?',
        message: `You are about to delete a ${type===0?'Comment Category':'Comment'}.`
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        if(type===0){
          this.onDeleteCategory(details,index)
        }else{
          this.onCommentDelete(details)
        }
      }
    });
  }

  onCommentDelete(commentDetails){
    this.deleteCategoryFromServer(commentDetails,commentDetails.courseCommentId)
  }

  onDeleteCategory(courseDetails,index){
    if(courseDetails.static){
      this.getAllCommentsWithUniqueCategory.courseCommentCategories.splice(index,1);
      this.selectedCoursesForCategory=this.selectedCoursesForCategory.filter((item)=>item!==courseDetails.courseId);
    }else{
      this.deleteCategoryFromServer(courseDetails);
    }
  }

  deleteCategoryFromServer(courseDetails,commentId=null){
    const deleteCategory: DeleteCourseCommentCategoryModel = new DeleteCourseCommentCategoryModel();
    deleteCategory.courseId=courseDetails.courseId;
    if(commentId){
    deleteCategory.courseCommentId=commentId;
    }
    this.reportCardService.deleteCourseCommentCategory(deleteCategory).subscribe((res)=>{
      if(res){
        if(res._failure){
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        }else{
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          this.getAllCourseCommentCategories();
        }
      }else{
      this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    })
  }

  onCommentEdit(commentDetails,index){
    this.commentListBasedOnSelectedCategory[index].takeInput = true;
  }

  addComments(){
    let addCommentWithCategoryModel: AddCourseCommentCategoryModel = new AddCourseCommentCategoryModel();
    this.commentListBasedOnSelectedCategory.map((item)=>{

      if(this.selectedCategoryIndex!==-1){
        let currentCategoryDetails=this.getAllCommentsWithUniqueCategory.courseCommentCategories[this.selectedCategoryIndex]
        let courseCategory: ReportCardCommentModel = new ReportCardCommentModel();
          this.defaultValuesService.getAllMandatoryVariable(courseCategory);
          courseCategory.courseId=currentCategoryDetails.courseId;
          courseCategory.courseName=currentCategoryDetails.courseName;
          courseCategory.courseCommentId=item.courseCommentId;
          courseCategory.comments=item.comment;
          courseCategory.createdBy=this.defaultValuesService.getEmailId();
          courseCategory.updatedBy=this.defaultValuesService.getEmailId();
          courseCategory.applicableAllCourses=false;
          addCommentWithCategoryModel.courseCommentCategory.push(courseCategory)

      }else{
        let courseCategory: ReportCardCommentModel = new ReportCardCommentModel();
          this.defaultValuesService.getAllMandatoryVariable(courseCategory);
          courseCategory.courseId=item.courseId;
          courseCategory.courseName=item.courseName;
          courseCategory.courseCommentId=item.courseCommentId;
          courseCategory.comments=item.comment;
          courseCategory.createdBy=this.defaultValuesService.getEmailId();
          courseCategory.updatedBy=this.defaultValuesService.getEmailId();
          courseCategory.applicableAllCourses=true;
          addCommentWithCategoryModel.courseCommentCategory.push(courseCategory)
      }
    })

    if(this.newComment.trim()){
      addCommentWithCategoryModel=this.pushNewCommentIfAny(addCommentWithCategoryModel);
    }

    this.addCommentsWithCategory(addCommentWithCategoryModel);
  }

  pushNewCommentIfAny(addCommentWithCategoryModel){

    if(this.selectedCategoryIndex!==-1){
      let currentCategoryDetails=this.getAllCommentsWithUniqueCategory.courseCommentCategories[this.selectedCategoryIndex]
      let courseCategory: ReportCardCommentModel = new ReportCardCommentModel();
        this.defaultValuesService.getAllMandatoryVariable(courseCategory);
        courseCategory.courseId=currentCategoryDetails.courseId;
        courseCategory.courseName=currentCategoryDetails.courseName;
        courseCategory.courseCommentId=0;
        courseCategory.comments=this.newComment;
        courseCategory.createdBy=this.defaultValuesService.getEmailId();
        courseCategory.updatedBy=this.defaultValuesService.getEmailId();
        courseCategory.applicableAllCourses=false;
        addCommentWithCategoryModel.courseCommentCategory.push(courseCategory)
    }else{
      let courseCategory: ReportCardCommentModel = new ReportCardCommentModel();
      this.defaultValuesService.getAllMandatoryVariable(courseCategory);
      courseCategory.courseId=null;
      courseCategory.courseName='allCourses';
      courseCategory.courseCommentId=0;
      courseCategory.comments=this.newComment;
      courseCategory.createdBy=this.defaultValuesService.getEmailId();
      courseCategory.updatedBy=this.defaultValuesService.getEmailId();
      courseCategory.applicableAllCourses=true;
      addCommentWithCategoryModel.courseCommentCategory.push(courseCategory)
    }
    return addCommentWithCategoryModel;
  }

  addCommentsWithCategory(addCommentWithCategoryModel){

    this.reportCardService.addCourseCommentCategory(addCommentWithCategoryModel).subscribe((res)=>{
      if(res){
        if(res._failure){
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        }else{
          this.newComment='';
          this.getAllCourseCommentCategories();
        }
      }else{
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    })
  }
  

  onDrop(event: CdkDragDrop<string[]>){
    const updateCommentsSortOrder: UpdateSortOrderForCourseCommentCategoryModel = new UpdateSortOrderForCourseCommentCategoryModel();
    updateCommentsSortOrder.courseId = this.commentListBasedOnSelectedCategory[0].courseId;
    updateCommentsSortOrder.currentSortOrder = this.commentListBasedOnSelectedCategory[event.currentIndex].sortOrder;
    updateCommentsSortOrder.previousSortOrder = this.commentListBasedOnSelectedCategory[event.previousIndex].sortOrder
    this.reportCardService.updateSortOrderForCourseCommentCategory(updateCommentsSortOrder).subscribe(
      (res) => {
       if(res){
          if(res._failure){
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }else{
            this.getAllCourseCommentCategories();
          }
       }else{
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
       }
      }
    );
  }

  


}
