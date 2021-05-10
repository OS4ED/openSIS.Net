
import { CommonField } from "./common-field.model";
import { CustomFieldsValueModel } from './custom-fields-value.model';
export class CustomFieldModel {
    tenantId: string;
    schoolId: number;
    fieldId: number;
    module:string;
    type: string;
    search: boolean;
    fieldName:string;
    title: string;
    sortOrder: number;
    selectOptions: string;
    categoryId: number;
    systemField: boolean;
    required: boolean;
    defaultSelection: string;
    hide: boolean;
    lastUpdate: string;
    updatedBy: string;
    customFieldsValue :CustomFieldsValueModel[]
    constructor(){
        this.fieldId=0;
        this.type="";
        this.search=true;
        this.title="";
        this.module="";
        this.sortOrder=0;
        this.selectOptions="";
        this.categoryId=0;
        this.systemField=false;
        this.required=false;
        this.defaultSelection="";
        this.hide=false;
        this.lastUpdate=null;
        this.updatedBy=sessionStorage.getItem('email');
        this.customFieldsValue =[];
    }
  }
export class CustomFieldAddView extends CommonField{
    customFields:CustomFieldModel
    constructor(){
        super();
        this.customFields=new CustomFieldModel()
    }
}
export class CustomFieldListViewModel extends CommonField {
    public customFieldsList: [CustomFieldModel];
    public tenantId:string;
    public schoolId: number;
}
export class CustomFieldDragDropModel extends CommonField{
        
    schoolId:number;
    previousSortOrder: number;
    currentSortOrder:number;
    categoryId:number;
    constructor(){
        super();
        this.previousSortOrder=0;
        this.currentSortOrder=0;
        this.categoryId=0;
    }


}
