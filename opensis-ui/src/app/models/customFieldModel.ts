
import { CommonField } from "../models/commonField";
import { CustomFieldsValueModel } from './customFieldsValueModel';
export class CustomFieldModel {
    tenantId: string;
    schoolId: number;
    fieldId: number;
    module:string;
    type: string;
    search: boolean;
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
        this.tenantId=sessionStorage.getItem('tenantId');
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
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
        this._tenantName=sessionStorage.getItem("tenant");
        this._userName = sessionStorage.getItem("user");
        this._token=sessionStorage.getItem("token");
        this.customFields=new CustomFieldModel()
    }
}
export class CustomFieldListViewModel extends CommonField {
    public customFieldsList: [CustomFieldModel];
    public tenantId:string;
    public schoolId: number;
    constructor() {
        super();
        this._tenantName=sessionStorage.getItem("tenant")
        this._userName = sessionStorage.getItem("user");
        this.tenantId=sessionStorage.getItem("tenantId")
        this.schoolId=+sessionStorage.getItem("selectedSchoolId")
        this._token=sessionStorage.getItem("token")
    }
}
export class CustomFieldDragDropModel extends CommonField{
        
    schoolId:number;
    previousSortOrder: number;
    currentSortOrder:number;
    categoryId:number;
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem("tenant")
        this._userName = sessionStorage.getItem("user");
        this.schoolId=+sessionStorage.getItem("selectedSchoolId")
        this._token=sessionStorage.getItem("token")
        this.previousSortOrder=0;
        this.currentSortOrder=0;
        this.categoryId=0;
    }


}
