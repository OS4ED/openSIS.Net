import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { AddCourseCommentCategoryModel, DeleteCourseCommentCategoryModel, GetAllCourseCommentCategoryModel, UpdateSortOrderForCourseCommentCategoryModel } from '../models/report-card.model';

@Injectable({
  providedIn: 'root'
})
export class ReportCardService {

  constructor(private defaultValuesService:DefaultValuesService,
    private http:HttpClient) { }
    apiUrl: string = environment.apiURL;


  addCourseCommentCategory(reportCardComment:AddCourseCommentCategoryModel){
    reportCardComment = this.defaultValuesService.getAllMandatoryVariable(reportCardComment);
    let apiurl = this.apiUrl + reportCardComment._tenantName+"/ReportCard/addCourseCommentCategory";
    return this.http.post<AddCourseCommentCategoryModel>(apiurl,reportCardComment)
  }

  deleteCourseCommentCategory(reportCardComment:DeleteCourseCommentCategoryModel){
    reportCardComment = this.defaultValuesService.getAllMandatoryVariable(reportCardComment);
    let apiurl = this.apiUrl + reportCardComment._tenantName+"/ReportCard/deleteCourseCommentCategory";
    return this.http.post<DeleteCourseCommentCategoryModel>(apiurl,reportCardComment)
  }

  updateSortOrderForCourseCommentCategory(reportCardComment:UpdateSortOrderForCourseCommentCategoryModel){
    reportCardComment = this.defaultValuesService.getAllMandatoryVariable(reportCardComment);
    let apiurl = this.apiUrl + reportCardComment._tenantName+"/ReportCard/updateSortOrderForCourseCommentCategory";
    return this.http.post<UpdateSortOrderForCourseCommentCategoryModel>(apiurl,reportCardComment)
  }

  getAllCourseCommentCategory(reportCardComment:GetAllCourseCommentCategoryModel){
    reportCardComment = this.defaultValuesService.getAllMandatoryVariable(reportCardComment);
    let apiurl = this.apiUrl + reportCardComment._tenantName+"/ReportCard/getAllCourseCommentCategory";
    return this.http.post<GetAllCourseCommentCategoryModel>(apiurl,reportCardComment)
  }

}
