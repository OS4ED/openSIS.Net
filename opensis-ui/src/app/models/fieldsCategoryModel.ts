
import { CommonField } from "../models/commonField";
import {CustomFieldModel} from "../models/customFieldModel";
export class FieldsCategoryModel {
    tenantId: string;
    schoolId: number;
    categoryId: number;
    isSystemCategory: boolean;
    search: boolean;
    title: string;
    module: string;
    sortOrder: number;
    required: boolean;
    hide: boolean;
    lastUpdate: string;
    updatedBy: string;
    customFields:[CustomFieldModel]
    constructor(){
        this.tenantId=sessionStorage.getItem('tenantId');
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
        this.categoryId=0;
        this.isSystemCategory=false;
        this.search=false;
        this.title="";
        this.module="";
        this.sortOrder=0;
        this.required=false;
        this.hide=false;
        this.lastUpdate=null;
        this.updatedBy=null;
        this.customFields=null;
    }
}

export class FieldsCategoryAddView extends CommonField{
    fieldsCategory:FieldsCategoryModel
    constructor(){
        super();
        this._tenantName=sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
        this.fieldsCategory=new FieldsCategoryModel()
    }
}
export class FieldsCategoryListView extends CommonField{
    public fieldsCategoryList:[FieldsCategoryModel];
    public tenantId:string;
    public schoolId:number;
    public module:string;
    constructor(){
        super();
        this.module="";
        this._tenantName=sessionStorage.getItem("tenant");
        this._token=sessionStorage.getItem("token");
        this.tenantId=sessionStorage.getItem('tenantId');
        this.schoolId=+sessionStorage.getItem('selectedSchoolId');
    }
}