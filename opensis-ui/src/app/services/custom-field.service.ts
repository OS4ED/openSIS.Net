import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import {CustomFieldAddView, CustomFieldDragDropModel, CustomFieldListViewModel} from '../models/custom-field.model';
import {FieldsCategoryAddView, FieldsCategoryListView} from '../models/fields-category.model';
@Injectable({
  providedIn: 'root'
})
export class CustomFieldService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }
  getAllCustomField(obj: CustomFieldListViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/getAllCustomField';
    return this.http.post<CustomFieldListViewModel>(apiurl, obj);
  }
  deleteCustomField(obj: CustomFieldAddView) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.customFields.schoolId = this.defaultValuesService.getSchoolID();
    obj.customFields.tenantId = this.defaultValuesService.getTenantID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/deleteCustomField';
    return this.http.post<CustomFieldAddView>(apiurl, obj);
  }
  updateCustomField(obj: CustomFieldAddView) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.customFields.schoolId = this.defaultValuesService.getSchoolID();
    obj.customFields.tenantId = this.defaultValuesService.getTenantID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/updateCustomField';
    return this.http.put<CustomFieldAddView>(apiurl, obj);
  }
  addCustomField(obj: CustomFieldAddView) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.customFields.schoolId = this.defaultValuesService.getSchoolID();
    obj.customFields.tenantId = this.defaultValuesService.getTenantID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/addCustomField';
    return this.http.post<CustomFieldAddView>(apiurl, obj);
  }

  addFieldsCategory(obj: FieldsCategoryAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.fieldsCategory.tenantId = this.defaultValuesService.getTenantID();
    obj.fieldsCategory.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/addFieldsCategory' ;
    return this.http.post<FieldsCategoryAddView>(apiurl, obj);
  }
  updateFieldsCategory(obj: FieldsCategoryAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.fieldsCategory.tenantId = this.defaultValuesService.getTenantID();
    obj.fieldsCategory.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/updateFieldsCategory' ;
    return this.http.put<FieldsCategoryAddView>(apiurl, obj);
  }
  deleteFieldsCategory(obj: FieldsCategoryAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.fieldsCategory.tenantId = this.defaultValuesService.getTenantID();
    obj.fieldsCategory.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/deleteFieldsCategory' ;
    return this.http.post<FieldsCategoryAddView>(apiurl, obj);
  }
  getAllFieldsCategory(obj: FieldsCategoryListView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/getAllFieldsCategory' ;
    return this.http.post<FieldsCategoryListView>(apiurl, obj);
  }
  updateCustomFieldSortOrder(obj: CustomFieldDragDropModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/CustomField/updateCustomFieldSortOrder' ;
    return this.http.put<CustomFieldDragDropModel>(apiurl, obj);
  }
}
