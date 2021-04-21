import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {CustomFieldAddView, CustomFieldDragDropModel, CustomFieldListViewModel} from '../models/custom-field.model';
import {FieldsCategoryAddView, FieldsCategoryListView} from '../models/fields-category.model'
import { BlockAddViewModel, BlockListViewModel, BlockPeriodAddViewModel, BlockPeriodSortOrderViewModel } from '../models/school-period.model';
import { DefaultValuesService } from '../common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class SchoolPeriodService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  deleteBlockPeriod(obj: BlockPeriodAddViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Period/deleteBlockPeriod";
    return this.http.post<BlockPeriodAddViewModel>(apiurl, obj);
  }
  updateBlockPeriod(obj: BlockPeriodAddViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.blockPeriod.schoolId = this.defaultValuesService.getSchoolID();
    obj.blockPeriod.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Period/updateBlockPeriod";
    return this.http.put<BlockPeriodAddViewModel>(apiurl, obj)
  }
  addBlockPeriod(obj: BlockPeriodAddViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.blockPeriod.schoolId = this.defaultValuesService.getSchoolID();
    obj.blockPeriod.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + '/Period/addBlockPeriod';
    return this.http.post<BlockPeriodAddViewModel>(apiurl, obj);
  }

  addBlock(obj: BlockAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.block.tenantId = this.defaultValuesService.getTenantID();
    obj.block.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + obj._tenantName + '/Period/addBlock';
    return this.http.post<BlockAddViewModel>(apiurl, obj);
  }
  updateBlock(obj: BlockAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.block.tenantId = this.defaultValuesService.getTenantID();
    obj.block.schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + obj._tenantName + '/Period/updateBlock' ;
    return this.http.put<BlockAddViewModel>(apiurl, obj);
  }
  deleteBlock(obj: BlockAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Period/deleteBlock' ;
    return this.http.post<BlockAddViewModel>(apiurl, obj);
  }
  getAllBlockList(obj: BlockListViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Period/getAllBlockList' ;
    return this.http.post<BlockListViewModel>(apiurl,obj);
  }
  updateBlockPeriodSortOrder(obj: BlockPeriodSortOrderViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Period/updateBlockPeriodSortOrder' ;
    return this.http.put<BlockPeriodSortOrderViewModel>(apiurl, obj);
  }

  private blockPeriodList: any;
  setBlockPeriodList(value: any) {
    this.blockPeriodList = value;
  }
  getBlockPeriodList() {
    return this.blockPeriodList;
  }

}
