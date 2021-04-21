import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { 
  GradeAddViewModel, 
  GradeDragDropModel, 
  GradeScaleAddViewModel, 
  GradeScaleListView, 
  EffortGradeScaleListModel, 
  EffortGradeScaleModel, 
  GetAllEffortGradeScaleListModel,
   UpdateEffortGradeScaleSortOrderModel ,
   SchoolSpecificStandarModel,
   GradeStandardSubjectCourseListModel,
   GetAllSchoolSpecificListModel,
   CheckStandardRefNoModel, 
   EffortGradeLibraryCategoryListView,
   EffortGradeLibraryCategoryAddViewModel,
   EffortGradeLlibraryDragDropModel,
   EffortGradeLibraryCategoryItemAddViewModel,
   HonorRollAddViewModel,
   HonorRollListModel} from '../models/grades.model';
   import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class GradesService {

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  addEffortGradeScale(effortGradeScale:EffortGradeScaleModel){
    effortGradeScale= this.defaultValuesService.getAllMandatoryVariable(effortGradeScale);
    effortGradeScale.effortGradeScale.schoolId = this.defaultValuesService.getSchoolID();
    effortGradeScale.effortGradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + effortGradeScale._tenantName + "/Grade/addEffortGradeScale";
    return this.http.post<EffortGradeScaleModel>(apiurl, effortGradeScale)
  }

  updateEffortGradeScale(effortGradeScale:EffortGradeScaleModel){
    effortGradeScale= this.defaultValuesService.getAllMandatoryVariable(effortGradeScale);
    effortGradeScale.effortGradeScale.schoolId = this.defaultValuesService.getSchoolID();
    effortGradeScale.effortGradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + effortGradeScale._tenantName + "/Grade/updateEffortGradeScale";
    return this.http.put<EffortGradeScaleModel>(apiurl, effortGradeScale)
  }

  deleteEffortGradeScale(effortGradeScale:EffortGradeScaleModel){
    effortGradeScale= this.defaultValuesService.getAllMandatoryVariable(effortGradeScale);
    effortGradeScale.effortGradeScale.schoolId = this.defaultValuesService.getSchoolID();
    effortGradeScale.effortGradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + effortGradeScale._tenantName + "/Grade/deleteEffortGradeScale";
    return this.http.post<EffortGradeScaleModel>(apiurl, effortGradeScale)
  }

  getAllEffortGradeScaleList(effortGradeScale:GetAllEffortGradeScaleListModel){
    effortGradeScale= this.defaultValuesService.getAllMandatoryVariable(effortGradeScale);
    let apiurl = this.apiUrl + effortGradeScale._tenantName + "/Grade/getAllEffortGradeScaleList";
    return this.http.post<EffortGradeScaleListModel>(apiurl, effortGradeScale)
  }


  updateEffortGradeScaleSortOrder(effortGradeScaleSort:UpdateEffortGradeScaleSortOrderModel){
    effortGradeScaleSort= this.defaultValuesService.getAllMandatoryVariable(effortGradeScaleSort);
    let apiurl = this.apiUrl + effortGradeScaleSort._tenantName + "/Grade/updateEffortGradeScaleSortOrder";
    return this.http.put<UpdateEffortGradeScaleSortOrderModel>(apiurl, effortGradeScaleSort)
  }

  getAllGradeScaleList(obj: GradeScaleListView){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/getAllGradeScaleList";   
    return this.http.post<GradeScaleListView>(apiurl,obj)
  }
  addGradeScale(obj:GradeScaleAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.gradeScale.schoolId = this.defaultValuesService.getSchoolID();
    obj.gradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/addGradeScale";
    return this.http.post<GradeScaleAddViewModel>(apiurl,obj)
  }
  updateGradeScale(obj:GradeScaleAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.gradeScale.schoolId = this.defaultValuesService.getSchoolID();
    obj.gradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateGradeScale";
    return this.http.put<GradeScaleAddViewModel>(apiurl,obj)
  }
  deleteGradeScale(obj:GradeScaleAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.gradeScale.schoolId = this.defaultValuesService.getSchoolID();
    obj.gradeScale.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/deleteGradeScale";
    return this.http.post<GradeScaleAddViewModel>(apiurl,obj)
  }
  deleteGrade(obj:GradeAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.grade.schoolId = this.defaultValuesService.getSchoolID();
    obj.grade.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/deleteGrade";
    return this.http.post<GradeAddViewModel>(apiurl,obj)
  }
  addGrade(obj:GradeAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.grade.schoolId = this.defaultValuesService.getSchoolID();
    obj.grade.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/addGrade";
    return this.http.post<GradeAddViewModel>(apiurl,obj)
  }
  updateGrade(obj:GradeAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.grade.schoolId = this.defaultValuesService.getSchoolID();
    obj.grade.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateGrade";
    return this.http.put<GradeAddViewModel>(apiurl,obj)
  }
  updateGradeSortOrder(obj:GradeDragDropModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateGradeSortOrder";
    return this.http.put<GradeDragDropModel>(apiurl,obj)
  }

  // School Specific Standards
  addGradeUsStandard(schoolSpecificStandard:SchoolSpecificStandarModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    schoolSpecificStandard.gradeUsStandard.tenantId = this.defaultValuesService.getTenantID();
    schoolSpecificStandard.gradeUsStandard.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/addGradeUsStandard";
    return this.http.post<SchoolSpecificStandarModel>(apiurl,schoolSpecificStandard)
  }
  updateGradeUsStandard(schoolSpecificStandard:SchoolSpecificStandarModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    schoolSpecificStandard.gradeUsStandard.tenantId = this.defaultValuesService.getTenantID();
    schoolSpecificStandard.gradeUsStandard.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/updateGradeUsStandard";
    return this.http.put<SchoolSpecificStandarModel>(apiurl,schoolSpecificStandard)
  }
  deleteGradeUsStandard(schoolSpecificStandard:SchoolSpecificStandarModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    schoolSpecificStandard.gradeUsStandard.tenantId = this.defaultValuesService.getTenantID();
    schoolSpecificStandard.gradeUsStandard.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/deleteGradeUsStandard";
    return this.http.post<SchoolSpecificStandarModel>(apiurl,schoolSpecificStandard)
  }
  
  getAllGradeUsStandardList(schoolSpecificStandard:GetAllSchoolSpecificListModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/getAllGradeUsStandardList";
    return this.http.post<GetAllSchoolSpecificListModel>(apiurl,schoolSpecificStandard)
  }

  getAllSubjectStandardList(schoolSpecificStandard:GradeStandardSubjectCourseListModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/getAllSubjectStandardList";
    return this.http.post<GradeStandardSubjectCourseListModel>(apiurl,schoolSpecificStandard)
  }

  getAllCourseStandardList(schoolSpecificStandard:GradeStandardSubjectCourseListModel){
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName+"/Grade/getAllCourseStandardList";
    return this.http.post<GradeStandardSubjectCourseListModel>(apiurl,schoolSpecificStandard)
  }
  checkStandardRefNo(schoolSpecificStandard: CheckStandardRefNoModel) {
    schoolSpecificStandard = this.defaultValuesService.getAllMandatoryVariable(schoolSpecificStandard);
    let apiurl = this.apiUrl + schoolSpecificStandard._tenantName + "/Grade/checkStandardRefNo";
    return this.http.post<CheckStandardRefNoModel>(apiurl, schoolSpecificStandard)
  }


  getAllEffortGradeLlibraryCategoryList(obj:EffortGradeLibraryCategoryListView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/getAllEffortGradeLlibraryCategoryList";
    return this.http.post<EffortGradeLibraryCategoryListView>(apiurl,obj)
  }
  deleteEffortGradeLibraryCategoryItem(obj:EffortGradeLibraryCategoryItemAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategoryItem.schoolId = this.defaultValuesService.getSchoolID();
    obj.effortGradeLibraryCategoryItem.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/deleteEffortGradeLibraryCategoryItem";
    return this.http.post<EffortGradeLibraryCategoryItemAddViewModel>(apiurl,obj)
  }
  deleteEffortGradeLibraryCategory(obj: EffortGradeLibraryCategoryAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategory.schoolId = this.defaultValuesService.getSchoolID();
    obj.effortGradeLibraryCategory.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/deleteEffortGradeLibraryCategory";
    return this.http.post<EffortGradeLibraryCategoryAddViewModel>(apiurl,obj)
  }
  addEffortGradeLibraryCategoryItem(obj:EffortGradeLibraryCategoryItemAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategoryItem.tenantId = this.defaultValuesService.getTenantID();
    obj.effortGradeLibraryCategoryItem.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/addEffortGradeLibraryCategoryItem";
    return this.http.post<EffortGradeLibraryCategoryItemAddViewModel>(apiurl,obj)
  }
  updateEffortGradeLibraryCategoryItem(obj:EffortGradeLibraryCategoryItemAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategoryItem.schoolId = this.defaultValuesService.getSchoolID();
    obj.effortGradeLibraryCategoryItem.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateEffortGradeLibraryCategoryItem";
    return this.http.put<EffortGradeLibraryCategoryItemAddViewModel>(apiurl,obj)
  }
  addEffortGradeLibraryCategory(obj:EffortGradeLibraryCategoryAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategory.schoolId = this.defaultValuesService.getSchoolID();
    obj.effortGradeLibraryCategory.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/addEffortGradeLibraryCategory";
    return this.http.post<EffortGradeLibraryCategoryAddViewModel>(apiurl,obj)
  }
  updateEffortGradeLibraryCategory(obj:EffortGradeLibraryCategoryAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.effortGradeLibraryCategory.tenantId = this.defaultValuesService.getTenantID();
    obj.effortGradeLibraryCategory.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateEffortGradeLibraryCategory";
    return this.http.put<EffortGradeLibraryCategoryAddViewModel>(apiurl,obj)
  }
  updateEffortGradeLlibraryCategorySortOrder(obj:EffortGradeLlibraryDragDropModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateEffortGradeLlibraryCategorySortOrder";
    return this.http.put<EffortGradeLlibraryDragDropModel>(apiurl,obj)
  }


  //honor setup
  addHonorRoll(obj:HonorRollAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.honorRolls.schoolId = this.defaultValuesService.getSchoolID();
    obj.honorRolls.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/addHonorRoll";
    return this.http.post<HonorRollAddViewModel>(apiurl,obj);
  }
  updateHonorRoll(obj:HonorRollAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.honorRolls.schoolId = this.defaultValuesService.getSchoolID();
    obj.honorRolls.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/updateHonorRoll";
    return this.http.put<HonorRollAddViewModel>(apiurl,obj);
  }
  deleteHonorRoll(obj:HonorRollAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.honorRolls.schoolId = this.defaultValuesService.getSchoolID();
    obj.honorRolls.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/deleteHonorRoll";
    return this.http.post<HonorRollAddViewModel>(apiurl,obj);
  }
  getAllHonorRollList(obj:HonorRollListModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+"/Grade/getAllHonorRollList";
    return this.http.post<HonorRollListModel>(apiurl,obj)
  }
}